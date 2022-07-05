using Microsoft.AspNetCore.Mvc;
using TmdbBully.Managers;

namespace TmdbBully.Controllers
{
	[ApiController]
	public class TmdbController : ControllerBase
	{
		private readonly ILogger<TmdbController> _logger;
		private readonly IConfiguration _configuration;

		public TmdbController(ILogger<TmdbController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}

		[HttpGet]
		[Route("fetch")]
		public string Fetch()
		{
			var fetchManager = new FetchManager(_configuration["ApiKey"]);
			Task.Run(() => fetchManager.Process());

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