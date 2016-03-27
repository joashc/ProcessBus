using LanguageExt;

namespace ProcessBus.Config.Definitions
{
    public class RoutingDefinition
    {
        public Lst<IMessageTransport> Transports { get; set; }
        public Lst<ForwardingDefinition> Forwarding { get; set; }
        public Map<IMessageTransport, Lst<IMessageTransport>> ForwardMap { get; set; }
    }
}
