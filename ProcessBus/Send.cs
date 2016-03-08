using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Process;

namespace ProcessBus
{
    public static class Send
    {
        public static Unit send<T>(string transport, T message)
        {
            return Unit.Default;
        }
    }
}
