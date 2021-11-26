using System;

namespace UnaryMultiplication.Machines
{
    public sealed class LinearlyBoundedAutomaton : TuringMachine
    {
        public LinearlyBoundedAutomaton(string path) : base(path)
        {
            if (!this.LanguageAlphabet.Contains(BoundarySymbol.Left.ToString()) ||
                !this.LanguageAlphabet.Contains(BoundarySymbol.Right.ToString()))
                throw new Exception(
                    "No boundary symbol(s) in the alphabet. It must contains '#' as left and '$' as right");
        }
    }
}