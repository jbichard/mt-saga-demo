using MassTransit;

namespace MTDemo.Sagas.Contracts
{
	public record InitiateSurveyImport(Guid SurveyImportId);
}
