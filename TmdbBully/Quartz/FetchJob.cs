using Quartz;
using TmdbBully.Managers;

namespace TmdbBully.Quartz
{
	public class FetchJob : IJob
	{
		private readonly IFetchManager _fetchManager;

		public FetchJob(IFetchManager fetchManager)
		{
			_fetchManager = fetchManager;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			await _fetchManager.Process();
		}
	}
}
