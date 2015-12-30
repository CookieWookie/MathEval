using System;
using System.Collections.Generic;

namespace MathEvaluator.Core.Syntax
{
    public class FunctionSyntaxToken : SyntaxToken
    {
        internal FunctionSyntaxToken(FunctionType type, params SyntaxToken[] arguments)
        {
            this.Arguments = arguments.AsReadOnly();
            this.Type = type;
        }

        public IReadOnlyList<SyntaxToken> Arguments { get; }
        public sealed override SyntaxTokenType TokenType => SyntaxTokenType.Function;
        public FunctionType Type { get; }

        public override string ToString() => $"{this.Type}({string.Join(", ", this.Arguments)})";
    }
}
