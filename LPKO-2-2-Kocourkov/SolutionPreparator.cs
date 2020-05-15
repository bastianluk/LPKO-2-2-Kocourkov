using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPKO_2_2_Kocourkov
{
    public sealed class SolutionPreparator
    {
        public static GlpkProgram PrepareProgram(Graph graph)
        {
            var lines = new List<string>();

            var sets = GetSetLines(graph.NodeCount, graph.Edges);
            lines.AddRange(sets);

            var variables = GetVariableLines(graph.NodeCount);
            lines.AddRange(variables);

            var function = GetFunctionLines();
            lines.AddRange(function);

            var conditions = GetConditionLines();
            lines.AddRange(conditions);

            var footer = GetFooter();
            lines.AddRange(footer);

            return new GlpkProgram(lines);
        }

        private static IEnumerable<string> GetSetLines(int nodeCount, IEnumerable<Edge> edges)
        {
            var edgeBuidler = new StringBuilder();
            edgeBuidler.Append("set Edges := {");

            var complementaryEdges = GetComplementaryEdges(nodeCount, edges);

            foreach (var edge in complementaryEdges)
            {
                edgeBuidler.Append($"({edge.Node1},{edge.Node2}),");
            }
            edgeBuidler.Remove(edgeBuidler.Length - 1, 1);
            edgeBuidler.Append("};");

            var edgeSet = edgeBuidler.ToString();
            
            return new List<string>
            {
                $"set Nodes := 0..{nodeCount-1};",
                $"set PossibleParties := 1..{nodeCount};",
                edgeSet
            };
        }

        private static IEnumerable<Edge> GetComplementaryEdges(int count, IEnumerable<Edge> edges)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    var edge = new Edge(new Node(i), new Node(j));
                    if (!edges.Contains(edge))
                    {
                        yield return edge;
                    }
                }
            }
        }

        private static IEnumerable<string> GetVariableLines(int count)
        {
            return new List<string>
            {
                $"var Parties, >= 1, <= {count}, integer;",
                "var nodeInParty{i in Nodes, p in PossibleParties}, binary;"
            };
        }

        private static IEnumerable<string> GetFunctionLines()
        {
            return new List<string>
            {
                "minimize obj: Parties;"
            };
        }

        private static IEnumerable<string> GetConditionLines()
        {
            return new List<string>
            {
                "s.t. edgeCon{(i, j) in Edges, p in PossibleParties}:",
                "  nodeInParty[i, p] + nodeInParty[j , p] <= 1;",
                "s.t. oneParty{i in Nodes}:",
                "  sum{p in PossibleParties} nodeInParty[i, p] = 1;",
                "s.t. smallestSet{i in Nodes, p in PossibleParties}:",
                "  nodeInParty[i, p] * p <= Parties;",
                "solve;"
            };
        }

        private static IEnumerable<string> GetFooter()
        {
            return new List<string>
            {
                "printf \"#OUTPUT: %d\\n\", Parties;",
                "for {i in Nodes}",
                "{",
                "  for {p in PossibleParties}",
                "  {",
                "    printf (if nodeInParty[i,p] = 1 then \"v_%d: %d\\n\" else \"\"), i, (p-1);",
                "  }",
                "}",
                "printf \"#OUTPUT END\\n\";",
            };
        }
    }
}
