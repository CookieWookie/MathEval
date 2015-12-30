using System;

namespace MathEvaluator.Core.Syntax
{
    public class ConstantSyntaxToken : SyntaxToken
    {
        internal ConstantSyntaxToken(double value)
        {
            this.Value = value;
        }

        public double Value { get; }
        public override SyntaxTokenType TokenType => SyntaxTokenType.Constant;
        
        public override string ToString() => this.Value.ToString();
    }
}
