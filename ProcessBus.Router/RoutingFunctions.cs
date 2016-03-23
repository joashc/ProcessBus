using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessBus.Config.Definitions;
using LanguageExt;
using ProcessBus.Config.Errors;
using static LanguageExt.Prelude;
using static LanguageExt.Process;
using static ProcessBus.Prelude;

namespace ProcessBus.Router
{
    public static class RouterFunctions
    {
        public static Map<string, List<string>> CreateRouteMap(RoutingDefinition def)
        {
            return Map(def.Transports.
                Map(t => 
                Tuple
                (
                    t.Path, 
                    def.Forwarding.Where(f => f.Bus.Path == t.Path)
                                  .Select(f => f.ForwardTo.Path)
                                  .ToList()
                )).ToArray());
        }

        public static ProcessId SpawnRouter(RoutingDefinition def)
        {
            var routeMap = CreateRouteMap(def);
            var router = spawn<ProcessBusMessage>(
                $"router-{ProcessSuffix()}",
                msg =>
                {
                    var path = msg.Path;
                    if (!routeMap.ContainsKey(path)) return;

                    tell($"@{path}", msg);
                    routeMap.Find(path)
                        .IfSome(forwards =>
                        {
                            foreach (var forward in forwards)
                            {
                                tell($"@{forward}", msg);
                            }
                        });

                }
            );

            register("router", router);
            return router;
        }


        public static ProcessId SpawnRouterFromConfig(Either<IConfigError, RoutingDefinition> def)
        {
            if (def.IsLeft) throw new ConfigurationException(def.LeftToArray()[0]);
            return SpawnRouter(def.RightToArray()[0]);
        }

    }
}
