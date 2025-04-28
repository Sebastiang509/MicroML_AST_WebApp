namespace MicroML_AST_WebApp.AST
{
    public class AstNode
    {
        public string Value { get; set; }
        public List<AstNode> Children { get; set; } = new List<AstNode>();

        public AstNode(string value)
        {
            Value = value;
        }
    }
}
