namespace MicroML_AST_WebApp.AST
{
    public class Parser
    {
        private List<string> _tokens;
        private int _pos;

        public AstNode Parse(string code)
        {
            var tokenizer = new Tokenizer();
            _tokens = tokenizer.Tokenize(code);
            _pos = 0;
            return ParseExpression();
        }

        private AstNode ParseExpression()
        {
            if (Match("fun"))
            {
                var param = Consume(); // e.g. x
                Expect("->");
                var body = ParseExpression();
                var funcNode = new AstNode("Function");
                funcNode.Children.Add(new AstNode(param));
                funcNode.Children.Add(body);
                return funcNode;
            }

            return ParseApplication();
        }

        private AstNode ParseApplication()
        {
            var expr = ParseBinaryExpression();

            while (Peek() != null && Peek() != ")" && Peek() != "->")
            {
                var next = ParseBinaryExpression();
                var appNode = new AstNode("Apply");
                appNode.Children.Add(expr);
                appNode.Children.Add(next);
                expr = appNode;
            }

            return expr;
        }

        private AstNode ParseBinaryExpression()
        {
            var left = ParseTerm();

            while (Peek() == "+" || Peek() == "-" || Peek() == "*" || Peek() == "/")
            {
                var op = Consume();
                var right = ParseTerm();
                var opNode = new AstNode(op);
                opNode.Children.Add(left);
                opNode.Children.Add(right);
                left = opNode;
            }

            return left;
        }

        private AstNode ParseTerm()
        {
            if (Match("("))
            {
                var expr = ParseExpression();
                Expect(")");
                return expr;
            }

            var token = Consume();
            return new AstNode(token);
        }

        private bool Match(string expected)
        {
            if (Peek() == expected)
            {
                _pos++;
                return true;
            }
            return false;
        }

        private string Consume()
        {
            if (_pos >= _tokens.Count)
                throw new Exception("Unexpected end of input.");
            return _tokens[_pos++];
        }

        private void Expect(string expected)
        {
            if (!Match(expected))
                throw new Exception($"Expected '{expected}'");
        }

        private string Peek()
        {
            return _pos < _tokens.Count ? _tokens[_pos] : null;
        }
    }
}
