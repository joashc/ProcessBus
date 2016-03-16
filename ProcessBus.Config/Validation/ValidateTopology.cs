using ProcessBus.Config.Definitions;
using TopologicalSort;
using LanguageExt;
using ProcessBus.Config.Errors;
using static ProcessBus.Config.Validation.TopologyValidation;

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
        public static Either<ConfigError, RoutingDefinition> CheckDuplicateTransports(RoutingDefinition def)
        {
            return from _ 
                   in CheckDuplicates(def.Transports).ToEither(ConfigError.DuplicateTransports)
                   select def;
        }

        // CheckSelfForwards :: RoutingDefinition -> Either ConfigError RoutingDefinition
        public static Either<ConfigError, RoutingDefinition> CheckSelfForwarding(RoutingDefinition def)
        {
            return from _ 
                   in CheckSelfForwards(_routingDefinitionToGraph(def)).ToEither(ConfigError.SelfForwards)
                   select def;
        }

        // CheckForwardingCyclicity :: RoutingDefinition -> Either ConfigError RoutingDefinition
        public static Either<ConfigError, RoutingDefinition> CheckForwardingCyclicity(RoutingDefinition def)
        {
            return from _ 
                   in CheckCyclicity(_routingDefinitionToGraph(def)).ToEither(ConfigError.CyclicConfiguration)
                   select def;
        }
    }
}