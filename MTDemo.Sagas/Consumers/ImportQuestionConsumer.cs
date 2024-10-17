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
			var question = surveyState.Questions.First(q => q.QuestionId == context.Message.Question.QuestionId);
			//if (IsPrime(question.Sequence))
			//{
			//	throw new InvalidDataException("Prime number questions are not allowed!");
			//}
			question.IsImported = true;
			question.Error = IsPrime(question.Sequence) ? "Prime number questions are not allowed!" : null;
			await dbContext.SaveChangesAsync();

			bool? isSuccessful = null;
			if (surveyState.Questions.All(q => q.IsImported))
			{
				isSuccessful = !surveyState.Questions.Any(q => q.Error != null);
			}
			logger.LogDebug("[{surveyImportId}] Question {questionId} import successful", context.Message.SurveyImportId, context.Message.Question.QuestionId);

			await context.Publish<QuestionImported>(new()
			{
				SurveyImportId = context.Message.SurveyImportId,
				QuestionId = context.Message.Question.QuestionId,
				IsSuccessful = isSuccessful
			});
		}

		// Just assumed this would be in Math...
		private static bool IsPrime(int n) => (n > 1) && Enumerable.Range(1, n).Where(x => n % x == 0).SequenceEqual([1, n ]);
	}
}
