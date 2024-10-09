using MassTransit;
using MassTransit.Configuration;
using MassTransit.Contracts.JobService;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTDemo.JobConsumer;
using MTDemo.JobConsumer.SurveyImport;
using Serilog;
using Serilog.Events;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
	.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
	.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilog();
builder.Services.AddDbContext<JobServiceSagaDbContext>(optionsBuilder =>
{
	optionsBuilder.UseOracle("DATA SOURCE=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=aspen)));USER ID=mt_job;PASSWORD=mt_job", m =>
	{
		m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
		m.MigrationsHistoryTable($"__{nameof(JobServiceSagaDbContext)}");
	});
});

builder.Services.AddMassTransit(x =>
{
	x.AddDelayedMessageScheduler();
	x.AddConsumer<SurveyImportConsumer>(cfg =>
	{
		cfg.Options<JobOptions<SurveyImportJob>>(options => options
			.SetJobTimeout(TimeSpan.FromMinutes(30))
			.SetConcurrentJobLimit(10));
	});

	x.SetJobConsumerOptions();
	x.AddJobSagaStateMachines(options => options.FinalizeCompleted = false)
		.EntityFrameworkRepository(r =>
		{
			r.ExistingDbContext<JobServiceSagaDbContext>();
			r.LockStatementProvider = new OracleLockStatementProvider();
		});

	x.SetKebabCaseEndpointNameFormatter();

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.UseDelayedMessageScheduler();
		cfg.Host(host: "localhost", port: 5672, virtualHost: "/test_vhost", hostConfigurator =>
		{
			hostConfigurator.Username("cr360_test_user");
			hostConfigurator.Password("cr360_test_user");
		}
		);
		cfg.Durable = true;
		cfg.ConfigureEndpoints(context);
	});
});

var app = builder.Build();

app.MapGet("/", () =>
{
	return "Hello World!";
});

app.MapPost("/import", async (HttpContext context, [FromServices] IBusControl bus) =>
{
	var jobId = Guid.NewGuid();
	var surveyImportId = Guid.NewGuid();
	await bus.Publish<SubmitJob<SurveyImportJob>>(new
	{
		JobId = jobId,
		Job = new SurveyImportJob(SurveyImportId: surveyImportId)
	});
	return jobId;
});

app.Run();
