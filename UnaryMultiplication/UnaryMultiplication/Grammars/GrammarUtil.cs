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
    }
}