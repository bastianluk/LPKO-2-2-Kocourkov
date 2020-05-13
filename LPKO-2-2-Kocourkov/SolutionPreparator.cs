using System.Collections.Generic;
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
            foreach (var edge in edges)
            {
                edgeBuidler.Append($"({edge.Node1},{edge.Node2}),");
                edgeBuidler.Append($"({edge.Node2},{edge.Node1}),");
            }
            edgeBuidler.Remove(edgeBuidler.Length - 1, 1);
            edgeBuidler.Append("};");

            var edgeSet = edgeBuidler.ToString();
            
            return new List<string>
            {
                $"set NodeIndexes := 0..{nodeCount-1};",
                $"set Nodes := (0..{nodeCount * nodeCount - 1});",
                edgeSet,
                "set Complement := Nodes cross Nodes diff Edges;"
            };
        }

        private static IEnumerable<string> GetVariableLines(int count)
        {
            return new List<string>
            {
                $"var Parties, >= 0, <= {count}, integer;",
                "var nodeColor{i in Nodes}, binary;"
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
                "s.t. edgeCon{(i, j) in Complement, k in NodeIndexes}:",
                "  nodeColor[i*N + k] + nodeColor[j*N + k] <= 1;",
                "s.t. exactlyOne{i in Nodes}:",
                "  sum{p in Parties} isNodeInParty[i,p] = 1;",
                "s.t. smallest{i in NodeIndexes, j in NodeIndexes}:",
                "  nodeColor[i*N + j] * j <= Parties + 1;"
            };
        }

        private static IEnumerable<string> GetFooter()
        {
            return new List<string>
            {
                "solve;",
                "printf \"#OUTPUT: %d\\n\", Parties;",
                "for {i in Nodes}",
                "{",
                "  for {p in Parties}",
                "  {",
                "    printf (if isNodeInParty[i,p] = 1 then \"v_%d: %d\\n\" else \"\"), i, p;",
                "  }",
                "}",
                "printf \"#OUTPUT END\\n\";",
            };
        }
    }
}
