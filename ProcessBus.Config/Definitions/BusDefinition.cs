using System;
using static System.String;

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

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var transport = obj as IMessageTransport;
            if (transport == null) throw new ArgumentException("Object is not an IMessageTransport");
            return Compare(Path, transport.Path, StringComparison.Ordinal);
        }
    }
}
