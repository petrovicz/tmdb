using Microsoft.AspNetCore.Mvc;
using Quartz;
using TmdbBully.Context;
using TmdbBully.Managers;
using TmdbBully.Models;
using TmdbBully.Quartz;

namespace TmdbBully.Controllers
{
	[ApiController]
	public class TmdbController : ControllerBase
	{
		private readonly IFetchManager _fetchManager;
		private readonly ISchedulerFactory _schedulerFactory;
		private readonly TmdbContext _tmdbContext;

		public TmdbController(IFetchManager fetchManager, ISchedulerFactory schedulerFactory, TmdbContext tmdbContext)
		{
			_fetchManager = fetchManager;
			_schedulerFactory = schedulerFactory;
			_tmdbContext = tmdbContext;
		}

		#region Actions

		[HttpPost]
		[Route("fetch")]
		public async Task<string> Fetch()
		{
			await _fetchManager.Process();

			return "Fetched top 120 movies...";
		}

		[HttpPost]
		[Route("schedule")]
		public async Task<string> Schedule(int seconds = 10)
		{
			var scheduler = await _schedulerFactory.GetScheduler();

			await scheduler.ScheduleJob(
				JobBuilder.Create<FetchJob>().Build(),
				TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInSeconds(seconds).RepeatForever()).Build()
			);

			return $"Scheduled a fetch for every {seconds} seconds...";
		}

		#endregion

		#region Get Methods

		[HttpGet]
		[Route("processes")]
		public List<FetchProcess> Processes()
		{
			return _tmdbContext.FetchProcesses.ToList();
		}

		[HttpGet]
		[Route("movies")]
		public List<Movie> Movies()
		{
			return _tmdbContext.Movies.ToList();
		}

		[HttpGet]
		[Route("directors")]
		public List<Director> Directors()
		{
			return _tmdbContext.Directors.ToList();
		}

		[HttpGet]
		[Route("genres")]
		public List<Genre> Genres()
		{
			return _tmdbContext.Genres.ToList();
		}

		#endregion
	}
}
