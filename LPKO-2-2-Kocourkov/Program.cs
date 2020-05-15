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
            Console.WriteLine("Generator - starting.");
            if (args.Length != 1 || args[0] == "-h" || args[0] == "--help")
            {
                var builder = new StringBuilder();
                builder.AppendLine("Usage:");
                builder.AppendLine($"{typeof(Program).Namespace}.exe inputFilePath");
                Console.WriteLine(builder.ToString());

                Console.WriteLine("Generator - finished.");
                return;
            }

            // Reading the input:
            var inputFilePath = args[0];
            Console.WriteLine($"Reading inputfile: {inputFilePath}.");
            var reader = new StreamReader(inputFilePath);
            var graph = Parser.ReadInput(reader);

            reader.Close();
            reader.Dispose();
            Console.WriteLine("File read.");

            // Generating linear program:
            Console.WriteLine("Generating the linear program.");
            var program = GlpkProgramGenerator.PrepareProgram(graph);

            // Write output.
            var outputFileName = "vygenerovane_lp.mod";
            var writer = new StreamWriter($"{outputFileName}");

            Console.WriteLine($"Writing the linear program to file: {outputFileName}.");
            WriteGeneratedProgramFile(writer, program);
            writer.Flush();
            writer.Dispose();
            Console.WriteLine("Generator - finished.");
        }

        private static void WriteGeneratedProgramFile(TextWriter writer, GlpkProgram program)
        {
            foreach (var line in program.ProgramLines)
            {
                writer.WriteLine(line);
            }
        }
    }
}
