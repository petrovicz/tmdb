using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TmdbBully.Managers;

namespace TmdbBully.Models
{
	[Table("Director")]
	public partial class Director
	{
		[Key]
		public long UID { get; set; }
		public string Name { get; set; }
		public int? TmdbId { get; set; }
		public string Biography { get; set; }
		public DateTime? Birthday { get; set; }

		public Director()
		{

		}

		public Director(PersonDetailsResponse response)
		{
			Name = response.Name;
			TmdbId = response.Id;
			Biography = response.Biography;
			Birthday = string.IsNullOrWhiteSpace(response.Birthday) ? null : DateTime.Parse(response.Birthday);
		}
	}
}
