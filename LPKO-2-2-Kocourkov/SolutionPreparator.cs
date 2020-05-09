using System.Collections.Generic;
using System.Text;

namespace LPKO_2_2_Kocourkov
{
    public sealed class SolutionPreparator
    {
        public static GlpkProgram PrepareProgram(Graph graph)
        {
            var lines = new List<string>();

            var sets = GetSetLines(graph.NodeCount);
            lines.AddRange(sets);

            var parameters = GetParamLines();
            lines.AddRange(parameters);

            var variables = GetVariableLines();
            lines.AddRange(variables);

            var function = GetFunctionLine();
            lines.Add(function);

            var conditions = GetConditionLines();
            lines.AddRange(conditions);

            var footer = GetFooter();
            lines.AddRange(footer);

            var data = GetDataLines(graph.Edges);
            lines.AddRange(data);

            return new GlpkProgram(lines);
        }

        private static List<string> GetSetLines(int nodeCount)
        {
            return new List<string>
            {
                $"set Nodes := 0..{nodeCount-1};",
                "set Edges, within Nodes cross Nodes;"
            };
        }

        private static List<string> GetParamLines()
        {
            return new List<string> { "param weight{(i,j) in Edges};" };
        }

        private static List<string> GetVariableLines()
        {
            return new List<string> { "var isRemoved{(i,j) in Edges}, binary;" };
        }

        private static string GetFunctionLine()
        {
            return "minimize total: sum{(i,j) in Edges} weight[i,j] * isRemoved[i,j];";
        }

        private static List<string> GetConditionLines()
        {
            return new List<string>
            {
                "s.t. condition_circle4{i in Nodes, j in Nodes, k in Nodes, l in Nodes: not(i == j or j == k or k == l or l == i)}:",
                "  ( if ((i,j) in Edges and (j,k) in Edges and (k,l) in Edges and (l,i) in Edges) then (isRemoved[i,j] + isRemoved[j,k] + isRemoved[k,l] + isRemoved[l,i]) else 1 ) >= 1;",
                "s.t. condition_circle3{i in Nodes, j in Nodes, k in Nodes: not(i == j or j == k or k == i)}:",
                "  ( if ((i,j) in Edges and (j,k) in Edges and (k,i) in Edges) then (isRemoved[i,j] + isRemoved[j,k] + isRemoved[k,i]) else 1 ) >= 1;"
            };
        }

        private static List<string> GetFooter()
        {
            return new List<string>
            {
                "solve;",
                "printf \"#OUTPUT: %d\\n\", sum{(i,j) in Edges} weight[i,j] * isRemoved[i,j];",
                "for {(i,j) in Edges: i != j}",
                "{",
                "  printf (if isRemoved[i,j] = 1 then \"%d --> %d\\n\" else \"\"), i, j;",
                "}",
                "printf \"#OUTPUT END\\n\";",
            };
        }
        private static List<string> GetDataLines(IEnumerable<Edge> edges)
        {
            var data = new List<string>
            {
                "data;"
            };

            var weightBuilder = new StringBuilder();
            weightBuilder.Append("param : Edges := ");
            foreach (var edge in edges)
            {
                weightBuilder.AppendLine($"                {edge.Node1} {edge.Node2}");
            }
            weightBuilder.Remove(weightBuilder.Length - 2, 2); //Remove the trailing "\r\n"
            weightBuilder.Append(";");
            data.Add(weightBuilder.ToString());

            data.Add("end;");
            return data;
        }
    }
}
