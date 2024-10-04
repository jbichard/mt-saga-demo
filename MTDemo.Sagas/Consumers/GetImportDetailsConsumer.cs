using MassTransit;
using MassTransit.Transports;
using MTDemo.Sagas.Contracts;

namespace MTDemo.Sagas.Consumers
{
	public class GetImportDetailsConsumer : IConsumer<GetImportDetails>
	{
		public async Task Consume(ConsumeContext<GetImportDetails> context)
		{
			// Call survey import cache/repository
			await Task.Delay(2000);

			// Process survey import data and extract question and condition ids
			var sendEnpoint = await context.GetSendEndpoint(new Uri("queue:SurveyImportState"));
			await sendEnpoint.Send<ImportDetailsResponse>(new()
			{
				SurveyImportId = context.Message.SurveyImportId,
				QuestionIds = ["Q1", "Q2", "Q3"],
				ConditionIds = ["C1", "C2", "C3"]
			});
		}
	}
}
