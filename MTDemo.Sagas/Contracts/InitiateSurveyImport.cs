using MassTransit;

namespace MTDemo.Sagas.Contracts
{
	public class InitiateSurveyImport
	{
		public Guid SurveyImportId { get; set; }
	}
}
