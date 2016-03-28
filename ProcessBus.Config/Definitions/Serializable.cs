using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessBus.Config.Definitions
{
    public class SerializableDefinition
    {
        public List<SerializableTransport> Transports { get; set; }
    }

    public class SerializableTransport
    {
        public SerializableTransport()
        {
            ForwardTo = new List<string>();
            ForwardFrom = new List<string>();
            Path = String.Empty;
        }
        public string Path { get; set; }
        public List<string> ForwardTo { get; set; }
        public List<string> ForwardFrom { get; set; }
    }
}
