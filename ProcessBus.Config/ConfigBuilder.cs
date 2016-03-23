using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using ProcessBus.Config.Definitions;
using ProcessBus.Config.Errors;
using static LanguageExt.Prelude;

namespace ProcessBus.Config
{
    public class ConfigBuilder
    {
        public TransportBuilder Transport(string path)
        {
            return new TransportBuilder(path);
        }
    }

    public class TransportBuilder
    {
        private string _currentTransport;
        private Lst<Tuple<string, string>> _forwards;
        private Lst<string> _transports;

        public TransportBuilder(string currentTransport)
        {
            _currentTransport = currentTransport;
            _forwards = Lst<Tuple<string, string>>.Empty;
            _transports = Lst<string>.Empty;
            _transports = _transports.Add(currentTransport);
        }

        public TransportBuilder Transport(string path)
        {
            _transports = _transports.Add(path);
            _currentTransport = path;
            return this;
        }

        public TransportBuilder Forward(string forwardTo)
        {
            _forwards = _forwards.Add(Tuple(_currentTransport, forwardTo));
            return this;
        }

        public Either<IConfigError, RoutingDefinition> Build()
        {
            return ConfigParser.Parse(new RawConfig(_transports, _forwards));
        }
    }
}
