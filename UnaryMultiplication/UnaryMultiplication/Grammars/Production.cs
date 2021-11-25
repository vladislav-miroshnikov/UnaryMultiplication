using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnaryMultiplication.Grammars
{
    public class Production
    {
        public List<string> Head { get; private init; }
        public List<string> Body { get; private init; }

        public Production(List<string> head, List<string> body)
        {
            Head = head;
            Body = body;
        }

        public override int GetHashCode()
        {
            return Head.GetHashCode() * 31 + Body.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            const string delimiter = ";";

            Head.ForEach(symbol => stringBuilder.Append(symbol + delimiter));
            stringBuilder.Append("->");
            Body.ForEach(symbol => stringBuilder.Append(symbol + delimiter));

            return stringBuilder.ToString();
        }
    }
}