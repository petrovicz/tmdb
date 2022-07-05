using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TmdbBully.Managers;

namespace TmdbBully.Models
{
	[Table("Genre")]
	public partial class Genre
	{
		[Key]
		public long UID { get; set; }
		public int? TmdbId { get; set; }
		public string Name { get; set; }

		public Genre()
		{

		}

		public Genre(Genres g)
		{
			TmdbId = g.Id;
			Name = g.Name;
		}
	}
}
