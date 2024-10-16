using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.SagaStateMachines;

namespace MTDemo.Sagas.Persistence
{
	public class SurveyImportStateMap : SagaClassMap<SurveyImportSagaState>
	{
		protected override void Configure(EntityTypeBuilder<SurveyImportSagaState> entity, ModelBuilder model)
		{
			entity.Property(x => x.CurrentState).HasMaxLength(64);

			// If using Optimistic concurrency, otherwise remove this property
			entity.Property(x => x.RowVersion).IsRowVersion();
		}
	}
}
