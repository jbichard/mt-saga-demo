using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
namespace MTDemo.Sagas.SagaStateMachines
{
	public class SurveyImportSagaState : SagaStateMachineInstance
	{
		// No need for key attribute - SagaClassMap does this
		public Guid CorrelationId { get; set; } = NewId.NextGuid();
		public string CurrentState { get; set; } = null!;

		public DateTime? ImportStartDate { get; set; }
		public DateTime? SurveyImportDate { get; set; }
		public DateTime? SurveyPublishDate { get; set; }
		public DateTime? ImportEndDate { get; set; }
		public bool Success { get; set; }

		// If using Optimistic concurrency, this property is required
		public byte[]? RowVersion { get; set; }
	}
}
