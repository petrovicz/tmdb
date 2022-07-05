using Microsoft.AspNetCore.Mvc;

namespace TmdbBully.Controllers
{
	[ApiController]
	public class TmdbController : ControllerBase
	{
		private readonly ILogger<TmdbController> _logger;

		public TmdbController(ILogger<TmdbController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[Route("fetch")]
		public string Fetch()
		{
			return "Fetching top 120 movies...";
		}

		[HttpGet]
		[Route("schedule")]
		public string Schedule(int seconds = 10)
		{
			return $"Scheduling a fetch every {seconds} seconds...";
		}
	}
}