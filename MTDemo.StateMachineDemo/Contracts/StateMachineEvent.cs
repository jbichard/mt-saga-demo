namespace Contracts
{
    using System;

    public record StateMachineEvent
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }
}