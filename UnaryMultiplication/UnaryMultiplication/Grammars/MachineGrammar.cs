using System.Collections.Generic;

namespace UnaryMultiplication.Grammars
{
    public abstract class MachineGrammar
    {
        public HashSet<string> Variables { get; protected init; }

        public HashSet<string> Terminals { get; protected init; }

        public string StartState { get; protected init; }

        public string StartVariable { get; protected init; }

        public string Blank { get; protected init; } = "_";

        public HashSet<Production> GeneratedProductions { get; protected init; }

        public HashSet<Production> CheckProductions { get; protected init; }

        public HashSet<Production> TerminalProductions { get; protected init; }
    }
}