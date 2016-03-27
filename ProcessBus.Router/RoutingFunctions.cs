using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessBus.Config.Definitions;
using LanguageExt;
using ProcessBus.Config.Errors;
using LanguageExt;
using static LanguageExt.Prelude;
using static LanguageExt.Process;
using static ProcessBus.Prelude;

namespace ProcessBus.Router
{
    public class Node<T>
    {
        public Node(T item)
        {
            Item = item;
            Distance = None;
            Parent = None;
        }
        public T Item { get; }
        public Option<int> Distance { get; set; }
        public Option<Node<T>> Parent { get; set; }
    }

    public static class RouterFunctions
    {
        public static HashSet<IMessageTransport> BfsTransport(RoutingDefinition def, IMessageTransport start)
        {
            var nodes = def.Transports.Select(t => new Node<IMessageTransport>(t));
            var getNode = fun((IMessageTransport transport) => nodes.Find(n => n.Item.Equals(transport)));
            var adjacent = fun((Node<IMessageTransport> node) => def.ForwardMap.Find(node.Item));
            var queue = new Queue<Node<IMessageTransport>>();
            var rootNode = nodes.Find(n => n.Item.Equals(start));
            var set = new HashSet<IMessageTransport>();
            rootNode.IfSome(root =>
            {
                queue.Enqueue(root);
                set.Add(root.Item);
                while (queue.Any())
                {
                    var current = queue.Dequeue();
                    adjacent(current).IfSome(adj =>
                    {
                        foreach (var a in adj)
                        {
                            getNode(a).IfSome(node =>
                            {
                                node.Distance.IfNone(() =>
                                {
                                    node.Distance = current.Distance.Select(d => { return d += 1; } );
                                    node.Parent = current;
                                    queue.Enqueue(node);
                                    set.Add(node.Item);
                                    return 0;
                                });
                            });

                        }
                    });
                }
            });
            return set;
        }

        public static ProcessId SpawnRouter(RoutingDefinition def)
        {
            var router = spawn<ProcessBusMessage>(
                $"router-{ProcessSuffix()}",
                msg =>
                {
                    var transport = def.Transports.Find(t => t.Path == msg.Path);
                    transport.IfSome(t =>
                    {
                        var forwardSet = BfsTransport(def, t);
                        tell($"@{t.Path}", msg);
                        foreach (var forward in forwardSet)
                        {
                            tell($"@{forward.Path}", msg);
                        }
                    });

                },
                ProcessFlags.PersistInbox
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
