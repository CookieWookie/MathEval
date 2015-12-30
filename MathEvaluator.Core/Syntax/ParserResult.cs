namespace MathEvaluator.Core.Syntax
{
    public class ParserResult
    {
        public ParserResult(SyntaxToken tree)
        {
            this.Tree = tree;
        }

        public SyntaxToken Tree { get; }
    }
}
