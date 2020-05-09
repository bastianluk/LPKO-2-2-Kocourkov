using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LPKO_2_2_Kocourkov
{
    public sealed class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1 || args[0] == "-h" || args[0] == "--help")
            {
                var builder = new StringBuilder();
                builder.AppendLine("Usage:");
                builder.AppendLine($"{typeof(Program).Namespace}.exe inputFilePath");
                Console.WriteLine(builder.ToString());

                return;
            }

            var reader = new StreamReader(args[0]);
            var graph = Parser.ReadInput(reader);

            reader.Close();
            reader.Dispose();

            var program = SolutionPreparator.PrepareProgram(graph);
            var writer = new StreamWriter($"vygenerovane_lp{DateTime.Now.TimeOfDay.Ticks}.mod");

            GenerateProgramFile(writer, program);
            writer.Flush();
            writer.Dispose();
        }

        private static void GenerateProgramFile(TextWriter writer, GlpkProgram program)
        {
            foreach (var line in program.ProgramLines)
            {
                writer.WriteLine(line);
            }
        }
    }
}
