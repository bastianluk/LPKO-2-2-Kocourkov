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

            var variables = GetVariableLines();
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
            }
            edgeBuidler.Remove(edgeBuidler.Length - 1, 1);
            edgeBuidler.Append("};");

            var edgeSet = edgeBuidler.ToString();
            
            return new List<string>
            {
                $"set Nodes := 0..{nodeCount-1};",
                $"set Parties := 0..{nodeCount-1};",
                edgeSet
            };
        }

        private static IEnumerable<string> GetParams(int nodeCount)
        {
            return new List<string>
            {
                $"param PartiesLimit := {nodeCount};"
            };
        }

        private static IEnumerable<string> GetVariableLines()
        {
            return new List<string>
            {
                "var isNodeInParty{i in Nodes, j in Parties}, binary;",
                "var isPartyAssigned{p in Parties}, binary;"
            };
        }

        private static IEnumerable<string> GetFunctionLines()
        {
            return new List<string>
            {
                "minimize total: sum{p in Parties} isPartyAssigned[p];"
            };
        }

        private static IEnumerable<string> GetConditionLines()
        {
            return new List<string>
            {
                "s.t. partyMember{i in Nodes, j in Nodes, p in Parties: i != j}:",
                "  ( if (not( (i,j) in Edges ) or not ( (j,i) in Edges )) then (isNodeInParty[i,p] + isNodeInParty[j,p]) else 0 ) <= 1;",
                "s.t. partyUsed{i in Nodes, p in Parties}:",
                "  isNodeInParty[i,p] <= isPartyAssigned[p];",
                "s.t. exactlyOne{i in Nodes}:",
                "  sum{p in Parties} isNodeInParty[i,p] = 1;"
            };
        }

        private static IEnumerable<string> GetFooter()
        {
            return new List<string>
            {
                "solve;",
                "printf \"#OUTPUT: %d\\n\", sum{p in Parties} isPartyAssigned[p];",
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
