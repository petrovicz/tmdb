using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TmdbBully.Models
{
	[Table("FetchProcess")]
	public partial class FetchProcess
	{
		[Key]
		public long UID { get; set; }
		public DateTime Timestamp { get; set; }
		public string MovieIds { get; set; }

		public FetchProcess()
		{

		}

		public FetchProcess(DateTime timestamp)
		{
			Timestamp = timestamp;
		}
	}
}
