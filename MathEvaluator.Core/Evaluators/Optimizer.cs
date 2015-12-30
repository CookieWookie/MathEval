using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEvaluator.Core.Syntax;

namespace MathEvaluator.Core.Evaluators
{
    public class Optimizer : Evaluator
    {
        public Optimizer() : base(EvaluationContext.Empty)
        {
        }

        protected override SyntaxToken VisitFunction(FunctionSyntaxToken token)
        {
            if (token.Type == FunctionType.Ln)
            {
                if (!SyntaxToken.Equals(token.Arguments[0], SyntaxToken.E) && token.Arguments[0].TokenType == SyntaxTokenType.Constant)
                {
                    return token;
                }
            }
            else if (token.Type == FunctionType.Log)
            {
                if (!SyntaxToken.Equals(token.Arguments[0], token.Arguments[1]))
                {
                    return token;
                }
            }
            return base.VisitFunction(token);
        }
    }
}
