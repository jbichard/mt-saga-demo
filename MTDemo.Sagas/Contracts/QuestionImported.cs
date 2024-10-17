namespace MTDemo.Sagas.Contracts
{
	public class QuestionImported
	{
		public Guid SurveyImportId { get; set; }
		public string QuestionId { get; set; }
		// Should only be set once all questions have been imported
		public bool? IsSuccessful { get; set; }
	}
}
