using System;

namespace UnaryMultiplication.Machines
{
    public class LinearlyBoundedAutomaton : TuringMachine
    {
        public LinearlyBoundedAutomaton(string path) : base(path)
        {
            if (!this.LanguageAlphabet.Contains(BoundarySymbol.Left.ToString()) ||
                !this.LanguageAlphabet.Contains(BoundarySymbol.Right.ToString()))
                throw new Exception(
                    "No boundary symbol(s) in the alphabet. It must to contains '#' as left and '$' as right");
        }
    }
}