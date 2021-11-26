using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace UnaryMultiplication.Machines
{
    public class TuringMachine
    {
        public HashSet<string> LanguageAlphabet { get; private init; }

        public HashSet<string> TapeAlphabet { get; private init; }

        public Directions Directions { get; private init; }

        public HashSet<string> States { get; private init; }

        public HashSet<string> FinalStates { get; private init; }

        public string StartState { get; private init; }

        public Dictionary<Tuple<string, string>, Tuple<string, string, string>> Transitions { get; private init; }

        public string Blank { get; private init; }

        public TuringMachine(string path)
        {
            dynamic deserializeObject = JsonConvert.DeserializeObject(File.ReadAllText(path)) ??
                                        throw new ArgumentNullException(
                                            $"Can not deserialize json object.\nCheck the file by path:\n {path}");

            LanguageAlphabet = deserializeObject.alphabet.ToObject<HashSet<string>>();
            Blank = deserializeObject.blank.ToObject<string>();
            TapeAlphabet = new HashSet<string>(LanguageAlphabet) { Blank };
            FinalStates = deserializeObject.final_states.ToObject<HashSet<string>>();
            StartState = deserializeObject.start_state.ToObject<string>();
            Directions = new Directions
            {
                Left = deserializeObject.directions.left.ToObject<string>(),
                Right = deserializeObject.directions.right.ToObject<string>()
            };
            States = new HashSet<string>();
            Transitions = new Dictionary<Tuple<string, string>, Tuple<string, string, string>>();
            var transitions = deserializeObject.transitions;
            foreach (var transition in transitions)
            {
                var key = (string)transition.Name;
                var value = (string)transition.Value.Value;
                var stateSymbolFrom = key.Split(',');
                var stateSymbolToDirection = value.Split(',');
                Transitions.Add(new Tuple<string, string>(stateSymbolFrom[0], stateSymbolFrom[1]),
                    new Tuple<string, string, string>(stateSymbolToDirection[0], stateSymbolToDirection[1],
                        stateSymbolToDirection[2]));
                States.Add(stateSymbolFrom[0]);
                States.Add(stateSymbolToDirection[0]);
                TapeAlphabet.Add(stateSymbolFrom[1]);
                TapeAlphabet.Add(stateSymbolToDirection[1]);
            }
        }
    }
}