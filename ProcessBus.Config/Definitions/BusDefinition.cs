namespace ProcessBus.Config.Definitions
{
    public class BusDefinition : IMessageTransport
    {
        public BusDefinition(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public bool Equals(IMessageTransport other)
        {
            return other.Path == Path;
        }
    }
}
