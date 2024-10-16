using MassTransit;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.Contracts;
using MTDemo.Sagas.Persistence;

namespace MTDemo.Sagas.Consumers
{
	public class ImportQuestionConsumer(SurveyImportStateDbContext dbContext, ILogger<ImportQuestionConsumer> logger) : IConsumer<ImportQuestionCommand>
	{
		public async Task Consume(ConsumeContext<ImportQuestionCommand> context)
		{
			// Call API to import question
			logger.LogDebug("[{surveyImportId}] Importing question {questionId}", context.Message.SurveyImportId, context.Message.Question.QuestionId);
			await Task.Delay(new Random(DateTime.UtcNow.Microsecond).Next(1500, 2500));

			var surveyState = await dbContext.SurveyImports.Include(x => x.Questions).FirstAsync(si => si.SurveyImportId == context.Message.SurveyImportId);
			surveyState.Questions.First(q => q.QuestionId == context.Message.Question.QuestionId).IsImported = true;
			await dbContext.SaveChangesAsync();

			var allQuestionsComplete = surveyState.Questions.All(q => q.IsImported);
			logger.LogDebug("[{surveyImportId}] Question {questionId} import successful", context.Message.SurveyImportId, context.Message.Question.QuestionId);

			await context.Publish<QuestionImported>(new()
			{
				SurveyImportId = context.Message.SurveyImportId,
				QuestionId = context.Message.Question.QuestionId,
				AllQuestionsComplete = allQuestionsComplete
			});
		}
	}
}
