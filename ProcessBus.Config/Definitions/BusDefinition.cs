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

        public override bool Equals(object obj)
        {
            if (!(obj is IMessageTransport)) return false;
            return ((IMessageTransport) obj).Path == Path;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}
