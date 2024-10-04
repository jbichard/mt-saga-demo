using MassTransit;
using System.Runtime.InteropServices;
namespace MTDemo.Sagas.SagaStateMachines
{
	public class SurveyImportState : SagaStateMachineInstance
	{
		public Guid CorrelationId { get; set; }
		public string CurrentState { get; set; } = null!;

		public Guid SurveyImportId { get; set; }
		public DateTime? ImportStartDate { get; set; }

		public QuestionImportState[] QuestionImportStates { get; set; } = [];
		public ConditionImportState[] ConditionImportStates { get; set; } = [];

		public DateTime? SurveyImportDate { get; set; }
		public DateTime? SurveyPublishDate { get; set; }

		public Guid? GetImportDetailsRequestId { get; set; }
		

		// If using Optimistic concurrency, this property is required
		public byte[] RowVersion { get; set; }
	}

	public class QuestionImportState
	{
		public Guid CorrelationId { get; set; }
		public string QuestionId { get; set; } = null!;
		public bool IsImported { get; set; }
		public string Error { get; set; } = null!;

		public SurveyImportState SurveyImportState;
	}

	public class ConditionImportState
	{
		public Guid CorrelationId { get; set; }
		public string ConditionId { get; set; } = null!;
		public bool IsImported { get; set; }
		public string Error { get; set; } = null!;

		public SurveyImportState SurveyImportState;
	}
}
