using System;
using System.Collections.Generic;
using System.Linq;
using UnaryMultiplication.Machines;

namespace UnaryMultiplication.Grammars
{
    public sealed class ContextSensitiveGrammar : MachineGrammar
    {
        public ContextSensitiveGrammar(TuringMachine turingMachine)
        {
            var terminals = turingMachine.LanguageAlphabet
                .Where(symbol =>
                    symbol != BoundarySymbols.Left.ToString() && symbol != BoundarySymbols.Right.ToString())
                .ToHashSet();
            var tapeSymbols = turingMachine.TapeAlphabet
                .Where(symbol =>
                    symbol != BoundarySymbols.Left.ToString() && symbol != BoundarySymbols.Right.ToString())
                .ToHashSet();
            HashSet<Production> productions = new HashSet<Production>();

            foreach (var terminal in terminals)
            {
                productions.Add(new Production(new List<string> {"A1"},
                    new List<string>
                    {
                        $"[{turingMachine.StartState},{BoundarySymbols.Left},{terminal},{terminal},{BoundarySymbols.Right}]"
                    }));
                productions.Add(new Production(new List<string> {"A1"},
                    new List<string>
                    {
                        $"[{turingMachine.StartState},{BoundarySymbols.Left},{terminal},{terminal}]", "A2"
                    }));
                productions.Add(new Production(new List<string> {"A2"},
                    new List<string>
                    {
                        $"[{terminal},{terminal}]", "A2"
                    }));
                productions.Add(new Production(new List<string> {"A2"},
                    new List<string>
                    {
                        $"[{terminal},{terminal},{BoundarySymbols.Right}]"
                    }));
            }

            foreach (var ((q, x), (p, y, direction)) in turingMachine.Transitions)
            {
                foreach (var terminal in terminals)
                {
                    if (turingMachine.FinalStates.Contains(p))
                    {
                        foreach (var tapeSymbol in tapeSymbols)
                        {
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{p},{BoundarySymbols.Left},{tapeSymbol},{terminal},{BoundarySymbols.Right}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{BoundarySymbols.Left},{p},{tapeSymbol},{terminal},{BoundarySymbols.Right}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{BoundarySymbols.Left},{tapeSymbol},{terminal},{p},{BoundarySymbols.Right}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{p},{BoundarySymbols.Left},{tapeSymbol},{terminal}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{BoundarySymbols.Left},{p},{tapeSymbol},{terminal}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{p},{tapeSymbol},{terminal}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{p},{tapeSymbol},{terminal},{BoundarySymbols.Right}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));
                            productions.Add(new Production(
                                new List<string>
                                {
                                    $"[{tapeSymbol},{terminal},{p},{BoundarySymbols.Right}]"
                                },
                                new List<string>
                                {
                                    $"{terminal}"
                                }));

                            foreach (var terminal2 in terminals)
                            {
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"{terminal}", $"[{tapeSymbol},{terminal2}]"
                                    },
                                    new List<string>
                                    {
                                        $"{terminal}", $"{terminal2}"
                                    }));
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"{terminal}", $"[{tapeSymbol},{terminal2},{BoundarySymbols.Right}]"
                                    },
                                    new List<string>
                                    {
                                        $"{terminal}", $"{terminal2}"
                                    }));
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"[{tapeSymbol},{terminal}]", $"{terminal2}"
                                    },
                                    new List<string>
                                    {
                                        $"{terminal}", $"{terminal2}"
                                    }));
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"[{BoundarySymbols.Left},{tapeSymbol},{terminal}]", $"{terminal2}"
                                    },
                                    new List<string>
                                    {
                                        $"{terminal}", $"{terminal2}"
                                    }));
                            }
                        }

                        if (x == BoundarySymbols.Left.ToString())
                        {
                            if (direction == Directions.Right)
                            {
                                foreach (var tapeSymbol in tapeSymbols)
                                {
                                    productions.Add(new Production(
                                        new List<string>
                                        {
                                            $"[{q},{BoundarySymbols.Left},{tapeSymbol},{terminal},{BoundarySymbols.Right}]"
                                        },
                                        new List<string>
                                        {
                                            $"[{BoundarySymbols.Left},{p},{tapeSymbol},{terminal},{BoundarySymbols.Right}]"
                                        }));
                                    productions.Add(new Production(
                                        new List<string>
                                        {
                                            $"[{q},{BoundarySymbols.Left},{tapeSymbol},{terminal}]"
                                        },
                                        new List<string>
                                        {
                                            $"[{BoundarySymbols.Left},{p},{tapeSymbol},{terminal}]"
                                        }));
                                }
                            }
                            else if (x == BoundarySymbols.Right.ToString())
                            {
                                if (direction == Directions.Left)
                                {
                                    foreach (var tapeSymbol in tapeSymbols)
                                    {
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{BoundarySymbols.Left},{tapeSymbol},{terminal},{q},{BoundarySymbols.Right}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{BoundarySymbols.Left},{p},{tapeSymbol},{terminal},{BoundarySymbols.Right}]"
                                            }));
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{tapeSymbol},{terminal},{q},{BoundarySymbols.Right}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{p},{tapeSymbol},{terminal},{BoundarySymbols.Right}]"
                                            }));
                                    }
                                }
                            }

                            if (direction == Directions.Right)
                            {
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"[{BoundarySymbols.Left},{q},{x},{terminal},{BoundarySymbols.Right}]"
                                    },
                                    new List<string>
                                    {
                                        $"[{BoundarySymbols.Left},{y},{terminal},{p},{BoundarySymbols.Right}]"
                                    }));
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"[{q},{x},{terminal},{BoundarySymbols.Right}]"
                                    },
                                    new List<string>
                                    {
                                        $"[{y},{terminal},{p},{BoundarySymbols.Right}]"
                                    }));
                            }
                            else
                            {
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"[{BoundarySymbols.Left},{q},{x},{terminal},{BoundarySymbols.Right}]"
                                    },
                                    new List<string>
                                    {
                                        $"[{p},{BoundarySymbols.Left},{y},{terminal},{BoundarySymbols.Right}]"
                                    }));
                                productions.Add(new Production(
                                    new List<string>
                                    {
                                        $"[{BoundarySymbols.Left},{q},{x},{terminal}]"
                                    },
                                    new List<string>
                                    {
                                        $"[{p},{BoundarySymbols.Left},{y},{terminal}]"
                                    }));
                            }

                            foreach (var tapeSymbol in tapeSymbols)
                            {
                                foreach (var terminal2 in terminals)
                                {
                                    if (direction == Directions.Right)
                                    {
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{BoundarySymbols.Left},{q},{x},{terminal}]",
                                                $"[{tapeSymbol},{terminal2}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{BoundarySymbols.Left},{y},{terminal}]",
                                                $"[{p},{tapeSymbol},{terminal2}]"
                                            }));
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{q},{x},{terminal}]",
                                                $"[{tapeSymbol},{terminal2}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{y},{terminal}]",
                                                $"[{p},{tapeSymbol},{terminal2}]"
                                            }));
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{q},{x},{terminal}]",
                                                $"[{tapeSymbol},{terminal2},{BoundarySymbols.Right}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{y},{terminal}]",
                                                $"[{p},{tapeSymbol},{terminal2},{BoundarySymbols.Right}]"
                                            }));
                                    }
                                    else
                                    {
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{tapeSymbol},{terminal2}]",
                                                $"[{q},{x},{terminal}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{p},{tapeSymbol},{terminal2}]",
                                                $"[{y},{terminal}]"
                                            }));
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{BoundarySymbols.Left},{tapeSymbol},{terminal2}]",
                                                $"[{q},{x},{terminal}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{BoundarySymbols.Left},{p},{tapeSymbol},{terminal2}]",
                                                $"[{y},{terminal}]"
                                            }));
                                        productions.Add(new Production(
                                            new List<string>
                                            {
                                                $"[{tapeSymbol},{terminal2}]",
                                                $"[{q},{x},{terminal},{BoundarySymbols.Right}]"
                                            },
                                            new List<string>
                                            {
                                                $"[{p},{tapeSymbol},{terminal2}]",
                                                $"[{y},{terminal},{BoundarySymbols.Right}]"
                                            }));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // var new_variables = new HashSet<string>();
            // ...
        }
    }
}