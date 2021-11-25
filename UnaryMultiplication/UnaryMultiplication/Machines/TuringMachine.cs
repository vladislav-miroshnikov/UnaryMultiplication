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

        public Dictionary<Tuple<string, string>, Tuple<string, string, Directions>> Transitions { get; private init; }

        public string Blank { get; private init; }

        public TuringMachine(string path)
        {
            dynamic deserializeObject = JsonConvert.DeserializeObject(File.ReadAllText(path))!;
            LanguageAlphabet = deserializeObject.language_alphabet.ToObject<HashSet<string>>();
            Blank = deserializeObject.blank.ToObject<string>();
            TapeAlphabet = new HashSet<string>(LanguageAlphabet) { Blank };
            FinalStates = deserializeObject.final_states.ToObject<HashSet<string>>();
        }
    }
}