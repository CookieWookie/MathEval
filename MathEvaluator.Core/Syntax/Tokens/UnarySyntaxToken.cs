using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEvaluator.Core.Syntax
{
    public class UnarySyntaxToken : SyntaxToken
    {
        internal UnarySyntaxToken(SyntaxToken value, UnaryOperationType type)
        {
            this.Value = value;
            this.Type = type;
        }

        public override SyntaxTokenType TokenType => SyntaxTokenType.Unary;
        public SyntaxToken Value { get; }
        public UnaryOperationType Type { get; }

        public override string ToString()
        {
            if (this.Type == UnaryOperationType.Factorial)
            {
                return $"{this.Value}!";
            }
            if (this.Type == UnaryOperationType.Negate)
            {
                return $"-{this.Value}";
            }
            throw new InvalidOperationException();
        }
    }
}
