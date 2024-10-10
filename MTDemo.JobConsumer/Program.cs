using MassTransit;
using MassTransit.Contracts.JobService;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
	optionsBuilder.UseNpgsql("Server=localhost;Port=5432;user id=admin;password=root;database=mt_db;", m =>
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
	//x.SetInMemorySagaRepositoryProvider();
	x.AddJobSagaStateMachines(options => options.FinalizeCompleted = false)
		.EntityFrameworkRepository(r =>
		{
			r.ExistingDbContext<JobServiceSagaDbContext>();
			r.UsePostgres();
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

app.MapPost("/import", async (HttpContext context, [FromServices] IRequestClient<SurveyImportJob> client) =>
{
	var jobId = Guid.NewGuid();
	var surveyImportId = Guid.NewGuid();
	var response = await client.GetResponse<JobSubmissionAccepted>(new
	{
		surveyImportId
	});
	return response.Message.JobId;
});

app.Run();
