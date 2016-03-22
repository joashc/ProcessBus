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
        public static Either<A, Lst<A>> CheckDuplicates<A>(Lst<A> list) where A : IEquatable<A>
        {
            var set = new HashSet<A>();
            foreach (var a in list)
            {
                if (!set.Add(a)) return Left<A, Lst<A>>(a);
            }
            return Right<A, Lst<A>>(list);
        }

        // CheckSelfForwards :: (Eq a, Graph g) => g -> Either a g
        public static Either<A, Graph<A>> CheckSelfForwards<A>(Graph<A> graph) where A : IEquatable<A>
        {
            return Optional(graph.Edges.FirstOrDefault(e => e.From.Equals(e.To)))
                .Match(
                    selfForward => Left<A, Graph<A>>(selfForward.From),
                    () => Right<A, Graph<A>>(graph)
                );
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
