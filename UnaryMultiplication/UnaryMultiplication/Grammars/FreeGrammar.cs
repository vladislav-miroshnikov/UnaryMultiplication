using System;
using System.Collections.Generic;
using System.Linq;
using UnaryMultiplication.Machines;

namespace UnaryMultiplication.Grammars
{
    public sealed class FreeGrammar : MachineGrammar
    {
        public FreeGrammar(string path) : base(path)
        {
        }

        public FreeGrammar(TuringMachine turingMachine)
        {
            GenerativeProductions = new HashSet<Production>();
            InitGenProductions(turingMachine);

            InferenceProductions = new HashSet<Production>();

            var alphabetWithEps = turingMachine.LanguageAlphabet.Union(new List<string> { Eps }).ToList();
            InitCheckProductions(turingMachine, alphabetWithEps);

            TerminalProductions = new HashSet<Production>();
            InitTerminalProductions(turingMachine, alphabetWithEps);

            var variables = new HashSet<string>();
            foreach (var cartesianPair in alphabetWithEps.SelectMany(_ => turingMachine.TapeAlphabet,
                (x, y) => new { x, y }))
            {
                variables.Add($"[{cartesianPair.x}, {cartesianPair.y}]");
            }

            Variables = variables.Union(turingMachine.States).Union(new HashSet<string> { "S1", "S2", "S3" })
                .ToHashSet();
            Terminals = turingMachine.LanguageAlphabet;
            StartVariable = "S1";
            Blank = turingMachine.Blank;
            StartState = turingMachine.StartState;
        }


        private void InitGenProductions(TuringMachine turingMachine)
        {
            GenerativeProductions.Add(new Production(new List<string> { "S1" },
                new List<string> { turingMachine.StartState, "S2" }));
            foreach (var symbol in turingMachine.LanguageAlphabet)
            {
                GenerativeProductions.Add(new Production(new List<string> { "S2" },
                    new List<string> { $"[{symbol}, {symbol}]", "S2" }));
            }

            GenerativeProductions.Add(new Production(new List<string> { "S2" }, new List<string> { "S3" }));

            GenerativeProductions.Add(new Production(new List<string> { "S3" },
                new List<string> { $"[{Eps}, {Blank}]", "S3" }));
            GenerativeProductions.Add(new Production(new List<string> { "S3" }, new List<string> { Eps }));
        }

        private void InitCheckProductions(TuringMachine turingMachine, List<string> alphabetWithEps)
        {
            foreach (var symbol in alphabetWithEps)
            {
                foreach (var ((stateFrom, symbolFrom), (stateTo, symbolTo, direction)) in turingMachine.Transitions)
                {
                    if (direction == Directions.Right)
                        InferenceProductions.Add(new Production(new List<string> { stateFrom, $"[{symbol}, {symbolFrom}]" },
                            new List<string> { $"[{symbol}, {symbolTo}]", stateTo }));
                }
            }

            var cartesianProduct = alphabetWithEps.SelectMany(_ => alphabetWithEps, (x, y) => new { x, y });
            foreach (var cartesianPair in cartesianProduct)
            {
                foreach (var tapeSymbol in turingMachine.TapeAlphabet)
                {
                    foreach (var ((stateFrom, symbolFrom), (stateTo, symbolTo, direction)) in turingMachine.Transitions)
                    {
                        if (direction == Directions.Left)
                            InferenceProductions.Add(new Production(
                                new List<string>
                                {
                                    $"[{cartesianPair.y}, {tapeSymbol}]", stateFrom,
                                    $"[{cartesianPair.x}, {symbolFrom}]"
                                },
                                new List<string>
                                {
                                    stateTo, $"[{cartesianPair.y}, {tapeSymbol}]", $"[{cartesianPair.x}, {symbolTo}]"
                                }));
                    }
                }
            }
        }

        private void InitTerminalProductions(TuringMachine turingMachine, List<string> alphabetWithEps)
        {
            foreach (var symbol in alphabetWithEps)
            {
                foreach (var tapeSymbol in turingMachine.TapeAlphabet)
                {
                    foreach (var finalState in turingMachine.FinalStates)
                    {
                        TerminalProductions.Add(new Production(
                            new List<string> { $"[{symbol}, {tapeSymbol}]", finalState },
                            new List<string> { finalState, symbol, finalState }));
                        TerminalProductions.Add(new Production(
                            new List<string> { finalState, $"[{symbol}, {tapeSymbol}]" },
                            new List<string> { finalState, symbol, finalState }));
                    }
                }
            }

            foreach (var finalState in turingMachine.FinalStates)
            {
                TerminalProductions.Add(new Production(new List<string> { finalState }, new List<string> { Eps }));
            }
        }

        public (bool result, List<Tuple<List<string>, Production>>) CheckAccepting(string word)
        {
            var symbs = word.Select(s => $"[{s},{s}]").ToList();
            var tapeWord = new List<string> { $"[{Eps},{Blank}]", StartState };
            tapeWord.AddRange(symbs);
            tapeWord.Add($"[{Eps},{Blank}]");
            var inference = new List<Tuple<List<string>, Production>>
            {
                new Tuple<List<string>, Production>(new List<string> { StartVariable },
                    new Production(new List<string> { "S1" }, new List<string> { "[eps,_]", "q0", "S2" }))
            };
            var current = new List<string> { "[eps,_]", "q0", "S2" };
            foreach (var s in word)
            {
                inference.Add(new Tuple<List<string>, Production>(current,
                    new Production(new List<string> { "S2" }, new List<string> { $"[{s},{s}]", "S2" })));
                current = current.Take(current.Count - 1).ToList();
                current.AddRange(new List<string> { $"[{s},{s}]", "S2" });
            }

            inference.Add(new Tuple<List<string>, Production>(current,
                new Production(new List<string> { "S2" }, new List<string> { "[eps,_]" })));
            var (result, tuples) = CheckTreeInference(tapeWord);
            return (result, inference.Concat(tuples).ToList());
        }
    }
}