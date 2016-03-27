using System;
using System.Collections;
using System.Collections.Generic;
using LanguageExt;
using System.Linq;
using LanguageExt.Trans.Linq;
using ProcessBus.Config.Definitions;
using ProcessBus.Config.Errors;
using static ProcessBus.Config.Validation.ValidateTopology;
using static LanguageExt.Prelude;

namespace ProcessBus.Config
{
    public static class ConfigParser
    {
        private static Option<Lst<T>> SequenceOption<T>(Lst<Option<T>> list)
        {
            if (list.Any(a => a.IsNone)) return Option<Lst<T>>.None;
            return Some(list.Select(a => a.ToArray()[0]));
        }

        private static Either<IConfigError, RoutingDefinition> ConfigToDefinition(RawConfig config)
        {
            var transports = config.Transports.Select(t => (IMessageTransport) new BusDefinition(t));
            var forwards = SequenceOption(config.Forwards.Select(f =>
            {
                return
                    from bus in transports.Find(t => t.Path == f.Item1)
                    from to in transports.Find(t => t.Path == f.Item2)
                    select new ForwardingDefinition((BusDefinition) bus, to);
            }));

            var toMap = fun((Map<IMessageTransport, Lst<IMessageTransport>> map, IEnumerable<Tuple<IMessageTransport, IMessageTransport>> tuples) =>
                {
                    foreach (var tuple in tuples)
                    {
                        var key = tuple.Item1;
                        var val = tuple.Item2;
                        map = map.AddOrUpdate(
                            key,
                            list => list.Add(val),
                            List(val));
                    }
                    return map;
                });

            var forwardMap = from someForwards in forwards
                let tuples = someForwards.Select(f => Tuple(f.ForwardFrom, f.ForwardTo))
                select toMap(LanguageExt.Map<IMessageTransport, Lst<IMessageTransport>>.Empty, tuples);

            var definition = 
                from someForward in forwards
                from someMap in forwardMap
                select new RoutingDefinition
                {
                    Transports = transports,
                    Forwarding = someForward,
                    ForwardMap = someMap
                };

            return definition.Match(
                Right<IConfigError, RoutingDefinition>,
                () => Left <IConfigError, RoutingDefinition>(new ParseError())
            );
        }

        public static Either<IConfigError, RoutingDefinition> Parse(RawConfig config)
        {
            return CheckForwardsExist(config)
                    .Bind(ConfigToDefinition)
                    .Bind(CheckForwardingCyclicity)
                    .Bind(CheckDuplicateTransports)
                    .Bind(CheckSelfForwarding);
        }

    }
}
