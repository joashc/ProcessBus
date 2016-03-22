using ProcessBus.Config.Definitions;

namespace ProcessBus.Config.Errors
{
    public class DuplicateTransportError : IConfigError
    {
        public DuplicateTransportError(IMessageTransport transport)
        {
            DuplicateTransport = transport;
        }

        public string Name { get; } = "Duplicate transport";
        public string Message => $"Transport \"{DuplicateTransport.Path}\" appears more than once in the configuration.";
        public IMessageTransport DuplicateTransport { get; }
    }
}
