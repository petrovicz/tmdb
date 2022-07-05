using Quartz;
using TmdbBully.Context;
using TmdbBully.Managers;
using TmdbBully.Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TmdbContext>();
builder.Services.AddScoped<IFetchManager, FetchManager>();

builder.Services.AddQuartz(q =>
{
	q.UseMicrosoftDependencyInjectionJobFactory();
});

builder.Services.AddQuartzHostedService(options =>
{
	options.WaitForJobsToComplete = true;
});

builder.Services.AddTransient<FetchJob>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
