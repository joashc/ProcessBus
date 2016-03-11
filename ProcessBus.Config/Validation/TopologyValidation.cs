using System;
using System.Collections.Generic;
using System.Linq;
using TopologicalSort;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ProcessBus.Config.Validation
{
    public static class TopologyValidation
    {
        // CheckDuplicates :: Eq a => [a] -> Maybe [a]
        public static Option<Lst<A>> CheckDuplicates<A>(Lst<A> list) where A : IEquatable<A>
        {
            var set = new HashSet<A>(list);
            var hasDuplicates = set.Count < list.Count;
            return hasDuplicates ? None : Some(list);
        }

        // CheckSelfForwards :: (Eq a, Graph g) => g -> Maybe g
        public static Option<Graph<A>> CheckSelfForwards<A>(Graph<A> graph) where A : IEquatable<A>
        {
            var selfForward = graph.Edges.Any(e => e.From.Equals(e.To));
            return selfForward ? None : Some(graph);
        }

        // CheckCyclicity :: (Eq a, Graph g) => g -> Maybe g
        public static Option<Graph<A>> CheckCyclicity<A>(Graph<A> graph) where A : IEquatable<A>
        {
            try
            {
                TopologicalSorter.TopologicalSort(graph.Verticies, graph.Edges);
                return Some(graph);
            }
            // Topsort will throw ArgumentException if graph is cyclic
            catch (ArgumentException)
            {
                return None;
            }
        }
    }
}
