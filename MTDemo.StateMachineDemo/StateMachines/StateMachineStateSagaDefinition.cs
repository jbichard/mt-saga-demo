namespace Company.StateMachines
{
    using MassTransit;

    public class StateMachineStateSagaDefinition :
        SagaDefinition<StateMachineState>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<StateMachineState> sagaConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

            endpointConfigurator.UseInMemoryOutbox(context);
        }
    }
}