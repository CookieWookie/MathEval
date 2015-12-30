using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEvaluator.Core.Syntax;

namespace MathEvaluator.Core.Evaluators
{
    public abstract class Visitor 
    {
        public virtual SyntaxToken Visit(SyntaxToken token)
        {
            switch (token.TokenType)
            {
                case SyntaxTokenType.Constant: return this.VisitConstant((ConstantSyntaxToken)token);
                case SyntaxTokenType.Variable: return this.VisitVariable((VariableSyntaxToken)token);
                case SyntaxTokenType.Binary: return this.VisitBinary((BinarySyntaxToken)token);
                case SyntaxTokenType.Unary: return this.VisitUnary((UnarySyntaxToken)token);
                case SyntaxTokenType.Function: return this.VisitFunction((FunctionSyntaxToken)token);
                case SyntaxTokenType.NamedConstant: return this.VisitNamedConstant((NamedConstantSyntaxToken)token);
                default: return token;
            }
        }

        protected abstract SyntaxToken VisitFunction(FunctionSyntaxToken token);
        protected abstract SyntaxToken VisitUnary(UnarySyntaxToken token);
        protected abstract SyntaxToken VisitBinary(BinarySyntaxToken token);
        protected abstract SyntaxToken VisitVariable(VariableSyntaxToken token);
        protected abstract SyntaxToken VisitConstant(ConstantSyntaxToken token);
        protected abstract SyntaxToken VisitNamedConstant(NamedConstantSyntaxToken token);
    }
}
