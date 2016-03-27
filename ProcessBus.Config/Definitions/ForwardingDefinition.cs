namespace ProcessBus.Config.Definitions
{
    public class ForwardingDefinition
    {
        public ForwardingDefinition(IMessageTransport forwardFrom, IMessageTransport forwardTo)
        {
            ForwardFrom = forwardFrom;
            ForwardTo = forwardTo;
        }

        public IMessageTransport ForwardFrom { get; private set; }
        public IMessageTransport ForwardTo { get; private set; }
    }
}
