using System;
using LanguageExt;

namespace ProcessBus.Config.Definitions
{
    public class RawConfig
    {
        public RawConfig(Lst<string> transports, Lst<Tuple<string, string>>  forwards)
        {
            Transports = transports;
            Forwards = forwards;
        }

        public Lst<string> Transports { get; }
        public Lst<Tuple<string, string>> Forwards { get; }
    }
}
