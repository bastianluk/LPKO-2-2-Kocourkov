using System.Collections.Generic;

namespace LPKO_2_2_Kocourkov
{
    public sealed class GlpkProgram
    {
        public GlpkProgram(IEnumerable<string> programLines)
        {
            ProgramLines = programLines;
        }

        public IEnumerable<string> ProgramLines { get; }
    }
}
