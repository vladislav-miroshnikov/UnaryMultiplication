using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace UnaryMultiplication.Grammars
{
    public abstract class MachineGrammar
    {
        public const string Eps = "eps";

        public HashSet<string> Variables { get; protected init; }

        public HashSet<string> Terminals { get; protected init; }

        public string StartState { get; protected init; }

        public string StartVariable { get; protected init; }

        public string Blank { get; protected init; } = "_";

        public HashSet<Production> GeneratedProductions { get; protected init; }

        public HashSet<Production> CheckProductions { get; protected init; }

        public HashSet<Production> TerminalProductions { get; protected init; }

        protected MachineGrammar()
        {
        }

        public MachineGrammar(string path)
        {
            dynamic deserializeObject = JsonConvert.DeserializeObject(File.ReadAllText(path)) ??
                                        throw new ArgumentNullException(
                                            $"Can not deserialize json object.\nCheck the file by path:\n {path}");

            StartState = deserializeObject.start_state.ToObject<string>();
            Blank = deserializeObject.blank.ToObject<string>();
            StartVariable = deserializeObject.start_variable.ToObject<string>();
            Terminals = deserializeObject.terminals.ToObject<HashSet<string>>();
            GeneratedProductions = new HashSet<Production>();
            Variables = new HashSet<string>();
            InitProductions(GeneratedProductions,
                (HashSet<string>)deserializeObject.generated_productions.ToObject<HashSet<string>>());
            CheckProductions = new HashSet<Production>();
            InitProductions(CheckProductions,
                (HashSet<string>)deserializeObject.check_productions.ToObject<HashSet<string>>());
            TerminalProductions = new HashSet<Production>();
            InitProductions(TerminalProductions,
                (HashSet<string>)deserializeObject.terminals_productions.ToObject<HashSet<string>>());
            Variables = Variables.Except(Terminals).ToHashSet();
        }

        private void InitProductions(HashSet<Production> grammarProductions, HashSet<string> jsonProductions)
        {
            foreach (var production in jsonProductions)
            {
                var head = production.Split("->")[0].Split(";").ToList();
                var body = production.Split("->")[1].Split(";").ToList();
                head.Union(body).ToList().ForEach(x => Variables.Add(x));
                grammarProductions.Add(new Production(head, body));
            }
        }

        protected (bool, List<Tuple<List<string>, Production>> inference) CheckTreeInference(List<string> tapeWord)
        {
            var queue = new Queue<List<string>>();
            queue.Enqueue(tapeWord);
            var size = CheckProductions.Max(x => x.Head.Count);

            var inference = new List<Tuple<List<string>, Production>>();
            var sentenses = new HashSet<Tuple<List<string>>>();
            while (queue.Count > 0)
            {
                var dequeue = queue.Dequeue();
                if (Terminals.Union(new List<string> { Eps }).All(x => dequeue.Contains(x)))
                    return (true, inference);
                if (sentenses.Contains(new Tuple<List<string>>(dequeue)))
                    continue;
                for (var subTreeSize = 1; subTreeSize < size + 1; subTreeSize++)
                {
                    for (var position = 0; position < dequeue.Count - subTreeSize + 1; position++)
                    {
                        var prefix = dequeue.Take(position);
                        var subStr = dequeue.Take(new Range(position, position + subTreeSize));
                        var suffix = dequeue.Take(new Range(position + subTreeSize, dequeue.Count));
                        var productions = CheckProductions.Union(TerminalProductions).ToList();
                        foreach (var production in productions)
                        {
                            if (!production.Head.SequenceEqual(subStr)) continue;
                            queue.Enqueue(prefix.Concat(production.Body).Concat(suffix).ToList());
                            inference.Add(new Tuple<List<string>, Production>(dequeue, production));
                        }
                    }
                }

                sentenses.Add(new Tuple<List<string>>(dequeue));
            }

            return (false, inference);
        }
    }
}