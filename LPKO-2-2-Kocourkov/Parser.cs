using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace LPKO_2_2_Kocourkov
{
    public sealed class Parser
    {
        public static Graph ReadInput(TextReader reader)
        {
            var graphInput = reader.ReadLine();
            var splitGraph = graphInput.Split(' ');

            var isNodeCount = int.TryParse(splitGraph[1], out var nodeCount);
            var isEdgeCount = int.TryParse(splitGraph[2].TrimEnd(':'), out var edgeCount);

            if (!isNodeCount || !isEdgeCount)
            {
                throw new ArgumentException($"Invalid arguments on {nameof(graphInput)}: {graphInput}");
            }

            var nodes = new ConcurrentDictionary<int, Node>();
            var edges = new List<Edge>(edgeCount);

            for (int i = 0; i < edgeCount; i++)
            {
                var edgeInputLine = reader.ReadLine();
                var delimiters = new string[] {" -- "};
                var splitEdgeInput = edgeInputLine.Trim().Split(delimiters, StringSplitOptions.None);
                var isNode1 = int.TryParse(splitEdgeInput[0].Trim(), out var node1Number);
                var isNode2 = int.TryParse(splitEdgeInput[1].Trim(), out var node2Number);

                if (!isNode1 || !isNode2 || node1Number > node2Number || node1Number >= nodeCount || node2Number >= nodeCount)
                {
                    throw new ArgumentException($"Invalid arguments on {nameof(edgeInputLine)}: {edgeInputLine}");
                }

                var node1 = nodes.GetOrAdd(node1Number, number => new Node(number));
                var node2 = nodes.GetOrAdd(node2Number, number => new Node(number));

                var edge = new Edge(node1, node2);
                edges.Add(edge);
            }

            return new Graph(nodes.Values.ToList(), edges);
        }
    }
}
