using System.Text.RegularExpressions;

namespace MicroML_AST_WebApp.AST
{
    public class Tokenizer
    {
        private static readonly Regex TokenRegex = new Regex(
            @"\s*(->|fun|\(|\)|[a-zA-Z_][a-zA-Z0-9_]*|\+|\-|\*|\/|\d+)",
            RegexOptions.Compiled
        );

        public List<string> Tokenize(string input)
        {
            var matches = TokenRegex.Matches(input);
            return matches.Select(m => m.Groups[1].Value).ToList();
        }
    }
}
