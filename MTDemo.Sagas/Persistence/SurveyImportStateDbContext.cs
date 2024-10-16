using Microsoft.EntityFrameworkCore;
using MTDemo.Sagas.SurveyImport;

namespace MTDemo.Sagas.Persistence
{
	public class SurveyImportStateDbContext(DbContextOptions options) : DbContext(options)
	{
		public DbSet<SurveyImport.SurveyImport> SurveyImports { get; set; }
	}
}
