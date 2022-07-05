using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TmdbBully.Managers;

namespace TmdbBully.Models
{
	[Table("Movie")]
	public partial class Movie
	{
		[Key]
		public long UID { get; set; }
		public string Title { get; set; }
		public int? Length { get; set; }
		public string GenreIds { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public string Overview { get; set; }
		public string PosterUrl { get; set; }
		public int? TmdbId { get; set; }
		public double? TmdbVoteAverage { get; set; }
		public int? TmdbVoteCount { get; set; }
		public int? DirectorId { get; set; }

		public Movie()
		{

		}

		public Movie(MovieDetailsAndCreditsResponse response, Director director)
		{
			Title = response.Title;
			Length = response.Runtime;
			GenreIds = string.Join(",", response.Genres.Select(g => g.Id).ToList());
			ReleaseDate = response.Release_date?.DateTime;
			Overview = response.Overview;
			PosterUrl = response.Poster_path;
			TmdbId = response.Id;
			TmdbVoteAverage = response.Vote_average;
			TmdbVoteCount = response.Vote_count;
			DirectorId = director.TmdbId;
		}

		public string TmdbUrl()
		{
			return $"https://www.themoviedb.org/movie/{TmdbId}";
		}
	}
}
