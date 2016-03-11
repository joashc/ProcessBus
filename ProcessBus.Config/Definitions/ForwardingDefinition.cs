namespace ProcessBus.Config.Definitions
{
    public class ForwardingDefinition
    {
        public ForwardingDefinition(BusDefinition fromBus, IMessageTransport forwardTo)
        {
            Bus = fromBus;
            ForwardTo = forwardTo;
        }

        public BusDefinition Bus { get; private set; }
        public IMessageTransport ForwardTo { get; private set; }
    }
}
