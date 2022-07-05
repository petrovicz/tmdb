using TmdbBully.Context;
using TmdbBully.Models;

namespace TmdbBully.Managers
{
	public interface IFetchManager
	{
		Task Process();
	}

	public class FetchManager : IFetchManager
	{
		private readonly IConfiguration _configuration;
		private readonly TmdbContext _tmdbContext;
		private readonly string _apiKey;

		public List<MovieFetchResult> MovieFetchResults { get; set; }
		public FetchProcess FetchProcess { get; set; }

		public FetchManager(IConfiguration configuration, TmdbContext tmdbContext)
		{
			_configuration = configuration;
			_apiKey = _configuration["ApiKey"];
			FetchProcess = new FetchProcess(DateTime.Now);
			_tmdbContext = tmdbContext;
		}

		public async Task Process()
		{
			await FetchAllData();
			await SaveFetchedData();
		}

		#region Fetching

		private async Task FetchAllData()
		{
			var tasks = new List<Task<List<MovieFetchResult>>>();

			for (int i = 1; i < 7; i++)
			{
				tasks.Add(FetchPage(i));
			}

			MovieFetchResults = (await Task.WhenAll(tasks)).SelectMany(r => r).ToList();
		}

		private async Task<List<MovieFetchResult>> FetchPage(int page)
		{
			using var httpClient = new HttpClient();
			var client = new ApiManager(httpClient, _apiKey);
			var response = await client.GetTopRatedAsync(page);
			var movieFetchResults = new List<MovieFetchResult>();

			foreach (var movie in response.Results)
			{
				// Each could run at the same time, but TMDB blocks the requests
				// due to the high amount in a short period of time
				var movieFetchResult = await FetchMovieDetailsAndCredits(movie.Id ?? 0);
				movieFetchResults.Add(movieFetchResult);
			}

			return movieFetchResults;
		}

		private async Task<MovieFetchResult> FetchMovieDetailsAndCredits(int id)
		{
			using var httpClient = new HttpClient();
			var client = new ApiManager(httpClient, _apiKey);

			var movieDetailsAndCredits = await client.GetMovieDetailsAndCreditsAsync(id);
			var directorCrew = movieDetailsAndCredits.Credits.Crew.FirstOrDefault(c => c.Job == "Director");
			var directorPerson = await client.GetPersonDetailsAsync(directorCrew?.Id ?? 0);

			var director = new Director(directorPerson);
			var movie = new Movie(movieDetailsAndCredits, director);
			var genres = movieDetailsAndCredits.Genres.Select(g => new Genre(g)).ToList();

			var result = new MovieFetchResult
			{
				Movie = movie,
				Director = director,
				Genres = genres
			};

			return result;
		}

		#endregion

		#region Saving

		private async Task SaveFetchedData()
		{
			var movies = MovieFetchResults.Select(m => m.Movie).ToList();
			var directors = MovieFetchResults.Select(m => m.Director).DistinctBy(d => d.TmdbId).ToList();
			var genres = MovieFetchResults.SelectMany(m => m.Genres).DistinctBy(g => g.TmdbId).ToList();

			var movieIds = movies.Select(m => m.TmdbId).ToList();
			var directorIds = directors.Select(d => d.TmdbId).ToList();
			var genreIds = genres.Select(g => g.TmdbId).ToList();

			FetchProcess.MovieIds = string.Join(",", movieIds);

			var existingMovieIds = _tmdbContext.Movies.Where(m => movieIds.Contains(m.TmdbId)).Select(m => m.TmdbId).ToList();
			var existingDirectorIds = _tmdbContext.Directors.Where(d => directorIds.Contains(d.TmdbId)).Select(d => d.TmdbId).ToList();
			var existingGenreIds = _tmdbContext.Genres.Where(g => genreIds.Contains(g.TmdbId)).Select(g => g.TmdbId).ToList();

			movies.RemoveAll(m => existingMovieIds.Contains(m.TmdbId));
			directors.RemoveAll(d => existingDirectorIds.Contains(d.TmdbId));
			genres.RemoveAll(g => existingGenreIds.Contains(g.TmdbId));

			var t1 = _tmdbContext.AddRangeAsync(movies);
			var t2 = _tmdbContext.AddRangeAsync(directors);
			var t3 = _tmdbContext.AddRangeAsync(genres);
			var t4 = _tmdbContext.AddRangeAsync(FetchProcess);

			await Task.WhenAll(t1, t2, t3, t4);
			await _tmdbContext.SaveChangesAsync();
		}

		#endregion
	}

	public class MovieFetchResult
	{
		public Movie Movie { get; set; }
		public Director Director { get; set; }
		public List<Genre> Genres { get; set; }
	}
}
