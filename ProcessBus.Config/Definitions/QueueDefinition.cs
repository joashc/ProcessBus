namespace ProcessBus.Config.Definitions
{
    public class QueueDefinition : IMessageTransport
    {
        public QueueDefinition(string path)
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
