namespace TmdbBully.Managers
{
	public class FetchManager
	{
		private readonly string _apiKey;

		public FetchManager(string apiKey)
		{
			_apiKey = apiKey;
		}

		public async Task Process()
		{
			await FetchAllData();
		}

		#region Fetching

		private async Task FetchAllData()
		{
			var tasks = new List<Task>();

			for (int i = 1; i < 7; i++)
			{
				tasks.Add(FetchPage(i));
			}

			await Task.WhenAll(tasks);
		}

		private async Task FetchPage(int page)
		{
			using var httpClient = new HttpClient();
			var client = new ApiManager(httpClient, _apiKey);
			var response = await client.GetTopRatedAsync(page);

			foreach (var movie in response.Results)
			{
				// Each could run at the same time, but TMDB blocks the requests
				// due to the high amount in a short period of time
				await FetchMovieDetailsAndCredits(movie.Id ?? 0);
			}
		}

		private async Task FetchMovieDetailsAndCredits(int id)
		{
			using var httpClient = new HttpClient();
			var client = new ApiManager(httpClient, _apiKey);

			var movieDetailsAndCredits = await client.GetMovieDetailsAndCreditsAsync(id);
			var directorCrew = movieDetailsAndCredits.Credits.Crew.FirstOrDefault(c => c.Job == "Director");
			var directorPerson = await client.GetPersonDetailsAsync(directorCrew?.Id ?? 0);
		}

		#endregion
	}
}
