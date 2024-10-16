using MassTransit;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.Contracts;
using MTDemo.Sagas.Persistence;

namespace MTDemo.Sagas.SagaStateMachines
{
	public class SurveyImportSaga : MassTransitStateMachine<SurveyImportSagaState>
	{
		public Event<InitiateSurveyImport> InitiateSurveyImport { get; private set; } = null!;
		public Event<QuestionImported> QuestionImported { get; private set; } = null!;

		public State Initiated { get; private set; } = null!;
		public State ImportingQuestions { get; private set; } = null!;
		public State QuestionsImported { get; private set; } = null!;

		public SurveyImportSaga(ILogger<SurveyImportSaga> logger)
		{
			InstanceState(x => x.CurrentState);
			GlobalTopology.Send.UseCorrelationId<InitiateSurveyImport>(x => x.SurveyImportId);
			GlobalTopology.Send.UseCorrelationId<QuestionImported>(x => x.SurveyImportId);
			Event(() => InitiateSurveyImport);
			Event(() => QuestionImported);

			Initially(
				When(InitiateSurveyImport)
				.Then(x =>
				{
					logger.LogDebug("[{surveyImportId}]: Saga created", x.Message.SurveyImportId);
					x.Saga.ImportStartDate = DateTime.UtcNow;
				})
				.TransitionTo(ImportingQuestions)
				.Publish(x => new ImportQuestionsCommand() { SurveyImportId = x.Message.SurveyImportId })
			);

			During(ImportingQuestions,
				When(QuestionImported)
				.Then(x =>
				{
					logger.LogDebug("[{surveyImportId}]: Saga received Question Imported event - {questionId}", x.Message.SurveyImportId, x.Message.QuestionId);
				})
				.If(
					x => x.Message.AllQuestionsComplete,
					x => {
						return x
							.TransitionTo(QuestionsImported)
							.Then(y =>
							{
								y.Saga.ImportEndDate = DateTime.UtcNow;

								logger.LogDebug("[{surveyImportId}]: Saga completed", y.Message.SurveyImportId);
							});
					}
				)
			);
		}
	}
}
