using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.SagaStateMachines;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MTDemo.Sagas.Persistence
{
	public class SurveyImportStateMap : SagaClassMap<SurveyImportSagaState>
	{
		protected override void Configure(EntityTypeBuilder<SurveyImportSagaState> entity, ModelBuilder model)
		{
			var boolConverter = new ValueConverter<bool, int>(v => v ? 1 : 0, v => (v == 1));
			entity.Property(x => x.CurrentState).HasMaxLength(64);

			// If using Optimistic concurrency, otherwise remove this property
			entity.Property(x => x.RowVersion).IsRowVersion();

			entity.Property(x => x.Success).HasColumnType("NUMBER(1)").HasConversion(boolConverter);
		}
	}
}
