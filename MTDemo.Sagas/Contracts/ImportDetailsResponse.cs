namespace MTDemo.Sagas.Contracts
{
	public class ImportDetailsResponse
	{
		public Guid SurveyImportId { get; set; }
		public string[] QuestionIds { get; set; }
		public string[] ConditionIds { get; set; }
	}
}
