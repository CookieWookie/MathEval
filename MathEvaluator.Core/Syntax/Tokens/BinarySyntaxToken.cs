using System;

namespace MathEvaluator.Core.Syntax
{
    public class BinarySyntaxToken : SyntaxToken
    {
        internal BinarySyntaxToken(SyntaxToken left, SyntaxToken right, BinaryOperationType type)
        {
            this.Left = left;
            this.Right = right;
            this.Type = type;
        }

        public SyntaxToken Left { get; }
        public SyntaxToken Right { get; }
        public override SyntaxTokenType TokenType => SyntaxTokenType.Binary;
        public BinaryOperationType Type { get; }

        public override string ToString()
        {
            return $"({this.Left} {this.Type.GetString()} {this.Right})";
        }
    }
}
