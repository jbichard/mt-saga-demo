using MassTransit;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.Contracts;
using MTDemo.Sagas.Persistence;
using MTDemo.Sagas.SurveyImport;

namespace MTDemo.Sagas.Consumers
{
	public class ImportQuestionsConsumer(SurveyImportStateDbContext DbContext) : IConsumer<ImportQuestionsCommand>
	{
		public async Task Consume(ConsumeContext<ImportQuestionsCommand> context)
		{
			var survey = await DbContext.SurveyImports.Where(x => x.SurveyImportId == context.Message.SurveyImportId).Include(x => x.Questions).FirstOrDefaultAsync();
			foreach (var question in survey?.Questions ?? [])
			{
				await context.Publish<ImportQuestionCommand>(new { context.Message.SurveyImportId, Question = new Question() { QuestionId = question.QuestionId } });
			}
		}
	}
}
