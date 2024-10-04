using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.Consumers;
using MTDemo.Sagas.Contracts;
using MTDemo.Sagas.Persistence;
using MTDemo.Sagas.SagaStateMachines;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{

	x.AddSagaStateMachine<SurveyImportSaga, SurveyImportState>()
	 .EntityFrameworkRepository(r =>
		{
			r.ConcurrencyMode = ConcurrencyMode.Optimistic; // or use Optimistic, which requires RowVersion

			r.AddDbContext<DbContext, SurveyImportSagaDbContext>((provider, builder) =>
			{
				builder.UseOracle("DATA SOURCE=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=aspen)));USER ID=mt_demo;PASSWORD=mt_demo", m =>
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

app.MapGet("/", () =>
{
	return "Hello World!";
});

app.MapPost("/import", async (HttpContext context, [FromServices]IBusControl bus) =>
{
	await bus.Publish<InitiateSurveyImport>(new(SurveyImportId: Guid.NewGuid()));
});

app.Run();
