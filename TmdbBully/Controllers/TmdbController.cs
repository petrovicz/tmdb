using Microsoft.AspNetCore.Mvc;
using TmdbBully.Context;
using TmdbBully.Managers;
using TmdbBully.Models;

namespace TmdbBully.Controllers
{
	[ApiController]
	public class TmdbController : ControllerBase
	{
		private readonly ILogger<TmdbController> _logger;
		private readonly IConfiguration _configuration;
		private readonly string _dbConnectionString;

		public TmdbController(ILogger<TmdbController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
			_dbConnectionString = _configuration.GetConnectionString("DbConnectionString");
		}

		#region Actions

		[HttpPost]
		[Route("fetch")]
		public string Fetch()
		{
			var fetchManager = new FetchManager(_configuration["ApiKey"], _dbConnectionString);
			Task.Run(() => fetchManager.Process());

			return "Fetching top 120 movies...";
		}

		[HttpPost]
		[Route("schedule")]
		public string Schedule(int seconds = 10)
		{
			return $"Scheduling a fetch every {seconds} seconds...";
		}

		#endregion

		#region Get Methods

		[HttpGet]
		[Route("processes")]
		public List<FetchProcess> Processes()
		{
			using var context = new TmdbContext(_dbConnectionString);
			return context.FetchProcesses.ToList();
		}

		[HttpGet]
		[Route("movies")]
		public List<Movie> Movies()
		{
			using var context = new TmdbContext(_dbConnectionString);
			return context.Movies.ToList();
		}

		[HttpGet]
		[Route("directors")]
		public List<Director> Directors()
		{
			using var context = new TmdbContext(_dbConnectionString);
			return context.Directors.ToList();
		}

		[HttpGet]
		[Route("genres")]
		public List<Genre> Genres()
		{
			using var context = new TmdbContext(_dbConnectionString);
			return context.Genres.ToList();
		}

		#endregion
	}
}
