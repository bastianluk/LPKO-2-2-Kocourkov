using System.Collections.Generic;
using System.Linq;

namespace LPKO_2_2_Kocourkov
{
    public sealed class Graph
    {
        public Graph(IEnumerable<Node> nodes, IEnumerable<Edge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public int NodeCount
        {
            get { return Nodes.Count(); }
        }

        public int EdgeCount
        {
            get { return Edges.Count(); }
        }
        
        public IEnumerable<Node> Nodes { get; }

        public IEnumerable<Edge> Edges { get; }
    }
}
