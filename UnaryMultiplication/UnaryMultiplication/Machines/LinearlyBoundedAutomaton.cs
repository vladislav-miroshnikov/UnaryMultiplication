using System;
using System.Text.Json;

namespace UnaryMultiplication.Machines
{
    public sealed class LinearlyBoundedAutomaton : TuringMachine
    {
        public LinearlyBoundedAutomaton(string path) : base(path)
        {
            if (!LanguageAlphabet.Contains(BoundarySymbols.Left) ||
                !LanguageAlphabet.Contains(BoundarySymbols.Right))
                throw new ArgumentException(
                    "No boundary symbol(s) in the alphabet.\nIt must contains '#' as left and '$' as right.");
        }
    }
}