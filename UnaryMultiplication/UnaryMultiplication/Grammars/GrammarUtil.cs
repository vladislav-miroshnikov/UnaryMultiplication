using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnaryMultiplication.Grammars
{
    public static class GrammarUtil
    {
        public static void SerializeGrammarToJson(MachineGrammar grammar, string filePath)
        {
            StreamWriter file = new(filePath);

            file.WriteLine("{");
            WriteJsonArray(file, "terminals", grammar.Terminals);

            file.WriteLine($"\t\"blank\": \"{grammar.Blank}\",");
            file.WriteLine($"\t\"start_variable\": \"{grammar.StartVariable}\",");
            file.WriteLine($"\t\"start_state\": \"{grammar.StartState}\",");

            WriteJsonArray(file, "generated_productions", grammar.GenerativeProductions);
            WriteJsonArray(file, "terminals_productions", grammar.TerminalProductions);
            WriteJsonArray(file, "check_productions", grammar.InferenceProductions, false);
            file.Write("}");
            file.Close();
        }

        private static void WriteJsonArray<T>(TextWriter file, string name, HashSet<T> words, bool needComma = true)
        {
            file.WriteLine($"\t\"{name}\": [");
            foreach (var (word, index) in words.Select((item, index) => (item, index)))
            {
                file.Write($"\t\t\"{word}\"");
                if (index != words.Count - 1)
                    file.WriteLine(",");
                else
                    file.WriteLine();
            }

            file.Write("\t]");
            if (needComma)
                file.WriteLine(",");
        }

        public static void PrintInference(string word, string directoryPath,
            GrammarType grammarType, List<Tuple<List<string>, Production>> inference)
        {
            var path = Path.Combine(directoryPath, @$"Resources\{grammarType}Inference.txt");
            StreamWriter file = new(path);
            
            file.WriteLine($"Word: {word}");
            file.WriteLine($"Inference:");

            foreach (var (tapeWord, production) in inference)
            {
                file.WriteLine("\t" + string.Join(" ", tapeWord));
                var prod = string.Join(" ", production.Head) + " -> " + string.Join(" ", production.Body);
                file.WriteLine($"Applied production: {prod}");
            }

            file.WriteLine($"Result: {string.Join(" ", word)}");
            file.Close();
            
            Console.WriteLine($"You can find file with inference by:\n{path}");
        }
    }
}