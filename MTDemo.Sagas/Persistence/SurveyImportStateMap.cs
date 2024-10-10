using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.SagaStateMachines;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MTDemo.Sagas.Persistence
{
	public class SurveyImportStateMap : SagaClassMap<SurveyImportState>
	{
		protected override void Configure(EntityTypeBuilder<SurveyImportState> entity, ModelBuilder model)
		{
			var boolConverter = new ValueConverter<bool, int>(v => v ? 1 : 0, v => (v == 1));

			entity.Property(x => x.CurrentState).HasMaxLength(64);

			// If using Optimistic concurrency, otherwise remove this property
			entity.Property(x => x.RowVersion).IsRowVersion();

			model.Entity<QuestionImportState>(x =>
			{
				x.HasKey(x => x.QuestionId);
				x.Property(x => x.QuestionId).HasMaxLength(64);
				x.Property(x => x.Error);
				x.Property(x => x.IsImported);
			});
			model.Entity<ConditionImportState>(x =>
			{
				x.HasKey(x => x.ConditionId);
				x.Property(x => x.ConditionId).HasMaxLength(64);
				x.Property(x => x.Error);
				x.Property(x => x.IsImported);
			});

			entity.HasMany(x => x.QuestionImportStates)
				.WithOne(x => x.SurveyImportState)
				.HasForeignKey(x => x.CorrelationId)
				.HasPrincipalKey(x => x.CorrelationId);
			entity.HasMany(x => x.ConditionImportStates)
				.WithOne(x => x.SurveyImportState)
				.HasForeignKey(x => x.CorrelationId)
				.HasPrincipalKey(x => x.CorrelationId);
		}
	}
}
