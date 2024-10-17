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
		public Event<ResumeImport> ResumeImport { get; private set; } = null!;

		public State ImportInitiated { get; private set; } = null!;
		public State ImportingQuestions { get; private set; } = null!;
		public State ImportComplete { get; private set; } = null!;

		public SurveyImportSaga(ILogger<SurveyImportSaga> logger)
		{
			InstanceState(x => x.CurrentState);
			GlobalTopology.Send.UseCorrelationId<InitiateSurveyImport>(x => x.SurveyImportId);
			GlobalTopology.Send.UseCorrelationId<QuestionImported>(x => x.SurveyImportId);
			GlobalTopology.Send.UseCorrelationId<ResumeImport>(x => x.SurveyImportId);
			Event(() => InitiateSurveyImport);
			Event(() => QuestionImported);
			Event(() => ResumeImport);

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
					x => x.Message.IsSuccessful.HasValue,
					x => {
						return x
							.TransitionTo(ImportComplete)
							.Then(y =>
							{
								y.Saga.ImportEndDate = DateTime.UtcNow;
								y.Saga.Success = y.Message.IsSuccessful!.Value;

								logger.LogDebug("[{surveyImportId}]: Saga {outcome}", y.Message.SurveyImportId, y.Message.IsSuccessful!.Value ? "finished" : "failed");
							});
					}
				)
			);

			During(ImportingQuestions,
				When(ResumeImport)
				.Then(x =>
				{
					logger.LogDebug("[{surveyImportId}]: Resume import", x.Message.SurveyImportId);
				})
				.If(
					x => x.Message.IsQuestionImportSuccessful.HasValue,
					x =>
					{
						return x
							.TransitionTo(ImportComplete)
							.Then(y =>
							{
								y.Saga.ImportEndDate = DateTime.UtcNow;
								y.Saga.Success = y.Message.IsQuestionImportSuccessful!.Value;

								logger.LogDebug("[{surveyImportId}]: Saga {outcome}", y.Message.SurveyImportId, y.Message.IsQuestionImportSuccessful!.Value ? "finished" : "failed");
							});
					}
				)
			);
		}
	}
}
