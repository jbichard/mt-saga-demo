using MassTransit;
using MTDemo.Sagas.Contracts;

namespace MTDemo.Sagas.SagaStateMachines
{
	public class SurveyImportSaga : MassTransitStateMachine<SurveyImportState>
	{
		public Event<InitiateSurveyImport> InitiateSurveyImport { get; private set; } = null!;

		public Event<ImportDetailsResponse> ImportDetailsResponse { get; private set; } = null!;

		public State Initiated { get; private set; } = null!;
		public State GettingImportDetails { get; private set; } = null!;
		public State ImportingQuestions { get; private set; } = null!;

		public SurveyImportSaga()
		{
			InstanceState(x => x.CurrentState);
			Event(() => InitiateSurveyImport, x => x.CorrelateById(context => context.Message.SurveyImportId));
			Event(() => ImportDetailsResponse, x => {
				x.CorrelateById(context => context.Message.SurveyImportId);
			});

			Initially(
				When(InitiateSurveyImport)
				.Then(x =>
				{
					x.Saga.SurveyImportId = x.Message.SurveyImportId;
					x.Saga.ImportStartDate = DateTime.UtcNow;
				})
				.TransitionTo(GettingImportDetails)
				.PublishAsync(x => x.Init<GetImportDetails>( new { x.Saga.SurveyImportId }))
			);

			During(GettingImportDetails,
				When(ImportDetailsResponse)
				.Then(x =>
				{
					x.Saga.QuestionImportStates = x.Message.QuestionIds.Select(id => new QuestionImportState() { QuestionId = id }).ToArray();
					x.Saga.ConditionImportStates = x.Message.ConditionIds.Select(id => new ConditionImportState() { ConditionId = id }).ToArray();
				})
				//.Send(ImportQuestionsCommand)
				.TransitionTo(ImportingQuestions)
			);
		}
	}
}
