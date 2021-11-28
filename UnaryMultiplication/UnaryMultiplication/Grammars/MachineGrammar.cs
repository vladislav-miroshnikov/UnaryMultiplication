using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace UnaryMultiplication.Grammars
{
    public abstract class MachineGrammar
    {
        protected const string Eps = "eps";

        protected HashSet<string> Variables { get; init; }

        public HashSet<string> Terminals { get; protected init; }

        public string StartState { get; protected init; }

        public string StartVariable { get; protected init; }

        public string Blank { get; protected init; } = "_";

        public HashSet<Production> GenerativeProductions { get; protected init; }

        public HashSet<Production> InferenceProductions { get; protected init; }

        public HashSet<Production> TerminalProductions { get; protected init; }

        protected MachineGrammar()
        {
        }

        public MachineGrammar(string text)
        {
            dynamic deserializeObject = JsonConvert.DeserializeObject(text) ??
                                        throw new JsonException(
                                            $"Bad grammar json syntax.\nCheck the file by path");

            StartState = deserializeObject.start_state.ToObject<string>();
            Blank = deserializeObject.blank.ToObject<string>();
            StartVariable = deserializeObject.start_variable.ToObject<string>();
            Terminals = deserializeObject.terminals.ToObject<HashSet<string>>();
            GenerativeProductions = new HashSet<Production>();
            Variables = new HashSet<string>();
            InitProductions(GenerativeProductions,
                (HashSet<string>)deserializeObject.generative_productions.ToObject<HashSet<string>>());
            InferenceProductions = new HashSet<Production>();
            InitProductions(InferenceProductions,
                (HashSet<string>)deserializeObject.inference_productions.ToObject<HashSet<string>>());
            TerminalProductions = new HashSet<Production>();
            InitProductions(TerminalProductions,
                (HashSet<string>)deserializeObject.terminal_productions.ToObject<HashSet<string>>());
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
            var size = InferenceProductions.Max(x => x.Head.Count);

            var inference = new List<Tuple<List<string>, Production>>();
            var sentenses = new HashSet<List<string>>();
            var terminalsWithEps = Terminals.Union(new List<string> { Eps }).ToList();

            while (queue.Count > 0)
            {
                var dequeue = queue.Dequeue();
                if (dequeue.All(x => terminalsWithEps.Contains(x)))
                    return (true, inference);
                if (sentenses.Any(x => x.SequenceEqual(dequeue)))
                    continue;
                for (var subTreeSize = 1; subTreeSize < size + 1; subTreeSize++)
                {
                    for (var position = 0; position < dequeue.Count - subTreeSize + 1; position++)
                    {
                        var prefix = dequeue.Take(position);
                        var subStr = dequeue.Take(new Range(position, position + subTreeSize));
                        var suffix = dequeue.Take(new Range(position + subTreeSize, dequeue.Count));
                        var productions = InferenceProductions.Union(TerminalProductions).ToList();
                        foreach (var production in productions)
                        {
                            if (!production.Head.SequenceEqual(subStr)) continue;
                            var list = prefix.Concat(production.Body).Concat(suffix).ToList();
                            queue.Enqueue(list);
                            inference.Add(new Tuple<List<string>, Production>(dequeue, production));
                        }
                    }
                }

                sentenses.Add(dequeue);
            }

            return (false, inference);
        }
    }
}