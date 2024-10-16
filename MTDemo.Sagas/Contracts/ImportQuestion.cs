using MTDemo.Sagas.SurveyImport;

namespace MTDemo.Sagas.Contracts
{
	public class ImportQuestionCommand
	{
		public Guid SurveyImportId { get; set; }
		public Question Question { get; set; }
	}
}
