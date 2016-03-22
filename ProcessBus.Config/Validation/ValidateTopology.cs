using ProcessBus.Config.Definitions;
using TopologicalSort;
using LanguageExt;
using ProcessBus.Config.Errors;
using static ProcessBus.Config.Validation.TopologyValidation;
using static LanguageExt.Prelude;

namespace ProcessBus.Config.Validation
{
    public static class ValidateTopology
    {
        // _routingDefinitionToGraph :: (Graph g) => RoutingDefinition -> g
        private static Graph<string> _routingDefinitionToGraph(RoutingDefinition def)
        {
            var verticies = def.Transports.Select(t => t.Path);
            var edges = def.Forwarding.Select(f => new Edge<string>(f.Bus.Path, f.ForwardTo.Path));
            return new Graph<string>(verticies, edges);
        }

        // CheckDuplicateTransports :: RoutingDefinition -> Either ConfigError RoutingDefinition
        public static Either<IConfigError, RoutingDefinition> CheckDuplicateTransports(RoutingDefinition def)
        {
            var duplicateTransport =  from _ 
                                      in CheckDuplicates(def.Transports)
                                      select def;
            return duplicateTransport.Match(
                _ => Right<IConfigError, RoutingDefinition>(def),
                transport => Left<IConfigError, RoutingDefinition>(new DuplicateTransportError(transport))
            );
        }

        // CheckSelfForwards :: RoutingDefinition -> Either ConfigError RoutingDefinition
        public static Either<IConfigError, RoutingDefinition> CheckSelfForwarding(RoutingDefinition def)
        {
            var selfForward = from _ 
                              in CheckSelfForwards(_routingDefinitionToGraph(def))
                              select def;

            return selfForward.Match(
                Right<IConfigError, RoutingDefinition>,
                transport => Left<IConfigError, RoutingDefinition>(new SelfForwardError(transport))
            );
        }

        // CheckForwardingCyclicity :: RoutingDefinition -> Either ConfigError RoutingDefinition
        public static Either<IConfigError, RoutingDefinition> CheckForwardingCyclicity(RoutingDefinition def)
        {
            var cyclicConfig = from _ 
                               in CheckCyclicity(_routingDefinitionToGraph(def))
                               select def;
            return cyclicConfig.Match(
                _ => Right<IConfigError, RoutingDefinition>(def),
                () => Left<IConfigError, RoutingDefinition>(new CyclicConfigurationError())
            );
        }
    }
}