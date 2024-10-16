namespace MTDemo.Sagas.Contracts
{
	public class QuestionImported
	{
		public Guid SurveyImportId { get; set; }
		public string QuestionId { get; set; }
		public bool AllQuestionsComplete { get; set; }
	}
}
