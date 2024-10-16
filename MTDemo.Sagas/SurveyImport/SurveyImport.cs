using System.ComponentModel.DataAnnotations;

namespace MTDemo.Sagas.SurveyImport
{
	public class SurveyImport
	{
		[Key]
		public Guid SurveyImportId { get; set; }
		public string SurveyData { get; set; }
		public virtual List<QuestionImport> Questions { get; set; } = [];
	}
}
