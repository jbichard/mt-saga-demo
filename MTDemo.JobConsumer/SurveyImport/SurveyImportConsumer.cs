using MassTransit;

namespace MTDemo.JobConsumer.SurveyImport
{
	public class SurveyImportConsumer : IJobConsumer<SurveyImportJob>
	{
		public async Task Run(JobContext<SurveyImportJob> context)
		{
			await context.NotifyStarted();
			var survey = await Mocks.GetSurvey(context.Job.SurveyImportId);
			foreach (var question in survey.Questions)
			{
				// Process question
				await Task.Delay(new Random(DateTime.UtcNow.Microsecond).Next(100, 10000));
				Console.WriteLine($"Survey import id {context.Job.SurveyImportId}; job id {context.JobId}; imported question {question.QuestionId}");
			}
			await context.NotifyCompleted();
		}
	}
}
