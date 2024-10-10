using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.Consumers;
using MTDemo.Sagas.Contracts;
using MTDemo.Sagas.Persistence;
using MTDemo.Sagas.SagaStateMachines;
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

builder.Services.AddMassTransit(x =>
{
	x.AddSagaStateMachine<SurveyImportSaga, SurveyImportState>()
	 .EntityFrameworkRepository(r =>
		{
			r.ConcurrencyMode = ConcurrencyMode.Optimistic; // or use Optimistic, which requires RowVersion

			r.AddDbContext<DbContext, SurveyImportSagaDbContext>((provider, builder) =>
			{
				builder.UseNpgsql("Server=localhost;Port=5432;user id=admin;password=root;database=mt_db;", m =>
				{
					m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
					m.MigrationsHistoryTable($"__{nameof(SurveyImportSagaDbContext)}");
				});
			});
		});

	x.AddConsumer<GetImportDetailsConsumer>();

	x.UsingRabbitMq((context, cfg) =>
	{
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

app.MapPost("/import", async (HttpContext context, [FromServices]IBusControl bus) =>
{
	await bus.Publish<InitiateSurveyImport>(new(SurveyImportId: Guid.NewGuid()));
});

app.Run();
