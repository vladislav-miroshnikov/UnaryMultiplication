using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnaryMultiplication.Grammars;

namespace UnaryMultiplication
{
    public static class Program
    {
        private static string _mode = "";
        private static readonly List<string> Commands = new List<string> {"exit", "check"};

        public static void Main(string[] args)
        {
            if (args.Length != 1) throw new Exception("Wrong arguments count.");
            _mode = args[0];
            if (!_mode.Equals("T0") && !_mode.Equals("T1")) throw new Exception("Set the correct mode.");

            while (true)
            {
                Console.Write(">>> ");
                var input = Console.ReadLine();
                if (input == null) Environment.Exit(0);

                var comArg = input.Trim().Split(' ').ToList().FindAll( word => word != "" );

                switch (comArg.Count)
                {
                    case 1:
                        if (comArg[0].Equals(Commands[0])) Environment.Exit(0);
                        else Console.WriteLine("Unknown command.");
                        
                        break;
                    case 2: 
                        if (comArg[0].Equals(Commands[1]))
                        {
                            switch (_mode)
                            {
                                case "T0":
                                    var directoryPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent
                                        ?.FullName;
                                    var path = Path.Combine(
                                        directoryPath ??
                                        throw new InvalidOperationException("Unable to find grammar file"),
                                        @"Resources\FreeGrammar.json");
                                    var freeGrammar = new FreeGrammar(path);
                                    var (result, inference) = freeGrammar.CheckAccepting(comArg[1]);
                                    if (result)
                                    {
                                        Console.WriteLine($"Word {comArg[1]} accepted!");
                                        GrammarUtil.PrintInference(comArg[1], directoryPath, GrammarType.T0, inference);
                                    }

                                    break;
                                case "T1":
                                    //

                                    break;
                            } 
                        }
                        else Console.WriteLine("Unknown command.");

                        break;
                    default:
                        Console.WriteLine("Unknown command length.");
                        
                        break;
                }
            }
        }
    }
}
