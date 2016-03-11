using System.Collections.Generic;
using TopologicalSort;

namespace ProcessBus.Config.Validation
{
    /// <summary>
    /// Represents a directed graph
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public class Graph<A>
    {
        public Graph(IEnumerable<A> verticies, IEnumerable<Edge<A>> edges)
        {
            Verticies = verticies;
            Edges = edges;
        }

        public IEnumerable<A> Verticies { get; }
        public IEnumerable<Edge<A>> Edges { get; }
    }
}
