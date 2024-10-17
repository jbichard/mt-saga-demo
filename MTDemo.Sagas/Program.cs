using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.Consumers;
using MTDemo.Sagas.Contracts;
using MTDemo.Sagas.Persistence;
using MTDemo.Sagas.SagaStateMachines;
using MTDemo.Sagas.SurveyImport;
using Serilog;
using Serilog.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => 
	lc.ReadFrom.Configuration(ctx.Configuration)
		.WriteTo.Console()
		.MinimumLevel.Debug()
		.MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
		.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
		.MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
		.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
		.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
		.Enrich.FromLogContext()
);

builder.Services.AddDbContext<SurveyImportStateDbContext>((provider, builder) =>
	{
		builder.UseNpgsql("Server=localhost;Port=5432;user id=admin;password=root;database=mt_db;", m =>
		{
			m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
			m.MigrationsHistoryTable($"__{nameof(SurveyImportStateDbContext)}");
		});
	});

builder.Services.AddLogging();

builder.Services.AddMassTransit(x =>
{
	x.AddSagaStateMachine<SurveyImportSaga, SurveyImportSagaState>()
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

	x.AddConsumer<ImportQuestionConsumer>(x => x.ConcurrentMessageLimit = 5);
	x.AddConsumer<ImportQuestionsConsumer>();

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(host: "localhost", port: 5672, virtualHost: "/test_vhost", hostConfigurator =>
		{
			hostConfigurator.Username("cr360_test_user");
			hostConfigurator.Password("cr360_test_user");
			hostConfigurator.ConfigureBatchPublish(x => x.Enabled = true);
		}
		);
		cfg.Durable = true;
		cfg.ConfigureEndpoints(context);
	});
});

var app = builder.Build();

app.MapPost("/imports", 
	async (
		HttpContext context,
		[FromServices]IBusControl bus,
		[FromServices]SurveyImportStateDbContext db,
		[FromServices]ILogger<Program> logger
	) =>
	{
		var surveyImportId = NewId.NextGuid();
		var questions = new List<QuestionImport>();
		for (var i = 1; i <= 10; i++)
		{
			questions.Add(new() { QuestionId = Guid.NewGuid().ToString(), Sequence = i });
		}

		db.SurveyImports.Add(new SurveyImport()
		{
			SurveyImportId = surveyImportId,
			SurveyData = "dummy data",
			Questions = questions
		});
		await db.SaveChangesAsync();
		await bus.Publish<InitiateSurveyImport>(new() { SurveyImportId = surveyImportId });
		logger.LogDebug("Survey import initiated: {SurveyImportId}", surveyImportId);
		return surveyImportId;
	}
);

app.MapGet("/imports/{surveyImportId}", 
	async (
		HttpContext context,
		[FromServices]SurveyImportStateDbContext db,
		Guid surveyImportId
	) =>
	{
		var survey = await db.SurveyImports
			.Where(x => x.SurveyImportId == surveyImportId)
			.Include(x => x.Questions)
			.FirstOrDefaultAsync();
		return survey;
	}
);

app.MapPost("/imports/{surveyImportId}/resume",
	async (
		HttpContext context,
		[FromServices] IBusControl bus,
		[FromServices] SurveyImportStateDbContext db,
		[FromServices] ILogger<Program> logger,
		Guid surveyImportId
	) =>
	{
		var survey = await db.SurveyImports
			.Where(x => x.SurveyImportId == surveyImportId)
			.Include(x => x.Questions)
			.FirstOrDefaultAsync();

		bool? questionImportStatus = null;
		if (!survey!.Questions.Any(q => !q.IsImported))
		{
			questionImportStatus = survey.Questions.All(q => q.Error == null);
		}

		await bus.Publish<ResumeImport>(new() { SurveyImportId = surveyImportId, IsQuestionImportSuccessful = questionImportStatus });
		logger.LogDebug("Survey import initiated: {SurveyImportId}", surveyImportId);
	}
);

app.Run();
