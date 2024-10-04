namespace MTDemo.Sagas.Contracts
{
	public class ImportDetailsResponse
	{
		public Guid SurveyImportId;
		public Guid CorrelationId;
		public string[] QuestionIds;
		public string[] ConditionIds;
	}
}
