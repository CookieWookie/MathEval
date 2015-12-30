namespace MathEvaluator.Core.Syntax
{
    public class VariableSyntaxToken : SyntaxToken
    {
        internal VariableSyntaxToken(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
        public override SyntaxTokenType TokenType => SyntaxTokenType.Variable;

        public override string ToString() => this.Name;
    }
}
