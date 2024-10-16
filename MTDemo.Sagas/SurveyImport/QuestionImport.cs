using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MTDemo.Sagas.SurveyImport
{
	[PrimaryKey(nameof(SurveyImportId), nameof(QuestionId))]
	public class QuestionImport
	{
		public string QuestionId { get; set; }
		public bool IsImported { get; set; } = false;
		public string? Error { get; set; }

		public Guid SurveyImportId { get; set; }
		public virtual SurveyImport SurveyImport { get; set; }
	}
}
