using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MTDemo.Sagas.SurveyImport;

namespace MTDemo.Sagas.Persistence
{
	public class SurveyImportStateDbContext(DbContextOptions options) : DbContext(options)
	{
		public DbSet<SurveyImport.SurveyImport> SurveyImports { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); 
			var boolConverter = new ValueConverter<bool, int>(v => v ? 1 : 0, v => (v == 1));
			modelBuilder.Entity<QuestionImport>(entity =>
			{
				entity.Property(x => x.IsImported).HasColumnType("NUMBER(1)").HasConversion(boolConverter);
			});
		}
	}
}
