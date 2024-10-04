using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace MTDemo.Sagas.Persistence
{
    public class SurveyImportSagaDbContext(DbContextOptions options) : SagaDbContext(options)
    {
		protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new SurveyImportStateMap(); }
        }
    }
}
