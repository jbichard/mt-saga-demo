namespace Company.StateMachines
{
    using Contracts;
    using MassTransit;

    public class StateMachineStateMachine :
        MassTransitStateMachine<StateMachineState> 
    {
        public StateMachineStateMachine()
        {
            InstanceState(x => x.CurrentState, Created);

            Event(() => StateMachineEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(StateMachineEvent)
                    .Then(context => context.Saga.Value = context.Message.Value)
                    .TransitionTo(Created)
            );

            SetCompletedWhenFinalized();
        }

        public State Created { get; private set; }

        public Event<StateMachineEvent> StateMachineEvent { get; private set; }
    }
}