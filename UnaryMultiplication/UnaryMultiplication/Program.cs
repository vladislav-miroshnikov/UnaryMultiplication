using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnaryMultiplication.Grammars;

namespace UnaryMultiplication
{
    public static class Program
    {
        private static GrammarType _mode;
        private static readonly List<string> Commands = new() {"exit", "check"};

        public static void Main(string[] args)
        {
            if (args.Length != 1) throw new ArgumentException("Wrong arguments count.");

            if (args[0].Equals(GrammarType.T0.ToString())) _mode = GrammarType.T0;
            else if (args[0].Equals(GrammarType.T1.ToString())) _mode = GrammarType.T1;
            else throw new ArgumentException("Set the correct grammar type.");

            while (true)
            {
                Console.Write(">>> ");
                var input = Console.ReadLine();
                if (input == null) Environment.Exit(0);

                var comArg = input.Trim().Split(' ').ToList().FindAll(word => word != "");

                
                switch (comArg.Count)
                {
                    case 1:
                        if (comArg[0].Equals(Commands[0])) Environment.Exit(0);
                        else Console.WriteLine("Unknown command.");

                        break;
                    case 2:
                        try
                        {
                            if (comArg[0].Equals(Commands[1]))
                            {
                                string directoryPath =
                                    Path.GetFullPath(Directory.GetCurrentDirectory());
                                
                                bool result;
                                List<Tuple<List<string>, Production>> inference;

                                switch (_mode)
                                {
                                    case GrammarType.T0:
                                        var freeGrammar = new FreeGrammar( System.Text.Encoding.Default.GetString(Resources.FreeGrammar));
                                        (result, inference) = freeGrammar.CheckAccepting(comArg[1]);

                                        if (result)
                                        {
                                            Console.WriteLine($"Word {comArg[1]} accepted.");
                                            GrammarUtil.PrintInference(comArg[1], directoryPath, GrammarType.T0,
                                                inference);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Word {comArg[1]} not accepted.");
                                        }

                                        break;
                                    case GrammarType.T1:
                                        var contextSensitiveGrammar = new ContextSensitiveGrammar(System.Text.Encoding.Default.GetString(Resources.ContextSensitiveGrammar));
                                        (result, inference) = contextSensitiveGrammar.CheckAccepting(comArg[1]);

                                        if (result)
                                        {
                                            Console.WriteLine($"Word {comArg[1]} accepted.");
                                            GrammarUtil.PrintInference(comArg[1], directoryPath, GrammarType.T1,
                                                inference);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Word {comArg[1]} not accepted.");
                                        }

                                        break;
                                }
                            }
                            else Console.WriteLine("Unknown command.");
                        }
                        catch(Exception exception)
                        {
                            Console.WriteLine(exception.Message);
                        }

                        break;
                    default:
                        Console.WriteLine("Unknown command length.");

                        break;
                }
            }
        }
    }
}