namespace MTDemo.Sagas.Contracts
{
	public class ResumeImport
	{
		public Guid SurveyImportId { get; set; }
		public bool? IsQuestionImportSuccessful { get; set; }
	}
}
