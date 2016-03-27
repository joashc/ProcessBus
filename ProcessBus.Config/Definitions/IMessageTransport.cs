using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessBus.Config.Definitions
{
    public interface IMessageTransport : IEquatable<IMessageTransport>, IComparable
    {
        string Path { get; }
    }
}
