using System;

namespace UnaryMultiplication.Machines
{
    public sealed class LinearlyBoundedAutomaton : TuringMachine
    {
        public LinearlyBoundedAutomaton(string path) : base(path)
        {
            if (!this.LanguageAlphabet.Contains(BoundarySymbols.Left.ToString()) ||
                !this.LanguageAlphabet.Contains(BoundarySymbols.Right.ToString()))
                throw new Exception(
                    "No boundary symbol(s) in the alphabet. It must contains '#' as left and '$' as right");
        }
    }
}