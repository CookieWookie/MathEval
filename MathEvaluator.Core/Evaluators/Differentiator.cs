using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEvaluator.Core.Syntax;

namespace MathEvaluator.Core.Evaluators
{
    public class Differentiator : Visitor
    {
        public Differentiator(VariableSyntaxToken variable) : this(variable, new Optimizer())
        {
            this.Variable = variable;
        }
        public Differentiator(VariableSyntaxToken variable, Visitor optimizer)
        {
            this.Variable = variable;
            this.Optimizer = optimizer;
            this.VarDetector = new VariableDetector();
        }

        public VariableSyntaxToken Variable { get; }
        private Visitor Optimizer { get; }
        private VariableDetector VarDetector { get; }

        public override SyntaxToken Visit(SyntaxToken token)
        {
            return this.Optimizer.Visit(base.Visit(token));
        }
        protected override SyntaxToken VisitBinary(BinarySyntaxToken token)
        {
            SyntaxToken leftDiff = this.Visit(token.Left);
            SyntaxToken rightDiff = this.Visit(token.Right);
            switch (token.Type)
            {
                case BinaryOperationType.Add: return leftDiff + rightDiff;
                case BinaryOperationType.Subtract: return leftDiff - rightDiff;
                case BinaryOperationType.Multiply: return leftDiff * token.Right + token.Left * rightDiff;
                case BinaryOperationType.Divide: return (leftDiff * token.Right - token.Left * rightDiff) / SyntaxToken.Pow(token.Right, SyntaxToken.Constant(2));
                // TODO: implement
                case BinaryOperationType.Modulus: return token;
                case BinaryOperationType.Power:
                    if (!this.VarDetector.HasVariable(token.Right))
                    {
                        return token.Right * SyntaxToken.Pow(token.Left, token.Right - SyntaxToken.Constant(1));
                    }
                    return token * (leftDiff * token.Right / token.Left + rightDiff * SyntaxToken.Ln(token.Left));
                default: throw new NotSupportedException();
            }
        }
        protected override SyntaxToken VisitConstant(ConstantSyntaxToken token)
        {
            return SyntaxToken.Constant(0);
        }
        protected override SyntaxToken VisitFunction(FunctionSyntaxToken token)
        {
            SyntaxToken arg = token.Arguments[0];
            SyntaxToken diff = this.Visit(arg);
            SyntaxToken one = SyntaxToken.Constant(1);
            SyntaxToken two = SyntaxToken.Constant(2);
            SyntaxToken result;
            switch (token.Type)
            {
                case FunctionType.Sin:
                    result = SyntaxToken.Cos(arg);
                    break;
                case FunctionType.Cos:
                    result = SyntaxToken.Negate(SyntaxToken.Sin(arg));
                    break;
                case FunctionType.Tan:
                    result = SyntaxToken.Add(one, SyntaxToken.Pow(SyntaxToken.Tan(arg), two));
                    break;
                case FunctionType.Cot:
                    result = SyntaxToken.Negate(SyntaxToken.Add(one, SyntaxToken.Pow(SyntaxToken.Cot(arg), two)));
                    break;
                case FunctionType.Asin:
                    result = SyntaxToken.Divide(one, SyntaxToken.Root(SyntaxToken.Subtract(one, SyntaxToken.Pow(arg, two)), two));
                    break;
                case FunctionType.Acos:
                    result = -(one / SyntaxToken.Root(one - SyntaxToken.Pow(arg, two), two));
                    break;
                case FunctionType.Atan:
                    result = one / (one + SyntaxToken.Pow(arg, two));
                    break;
                case FunctionType.Acot:
                    result = -(one / (one + SyntaxToken.Pow(arg, two)));
                    break;
                case FunctionType.Log:
                    SyntaxToken b = token.Arguments[1];
                    result = one / (arg * SyntaxToken.Ln(b));
                    break;
                case FunctionType.Ln:
                    result = one / arg;
                    break;
                case FunctionType.Abs:
                    result = arg / SyntaxToken.Abs(arg);
                    break;
                case FunctionType.Unsupported:
                default: throw new NotSupportedException();
            }
            return result * diff;
        }
        protected override SyntaxToken VisitNamedConstant(NamedConstantSyntaxToken token)
        {
            if (SyntaxToken.Equals(token, SyntaxToken.E))
            {
                return token;
            }
            return this.VisitConstant(token);
        }
        protected override SyntaxToken VisitUnary(UnarySyntaxToken token)
        {
            switch (token.Type)
            {
                case UnaryOperationType.Factorial: throw new NotImplementedException();
                case UnaryOperationType.Negate: return -this.Visit(token.Value);
                default: throw new NotSupportedException();
            }
        }
        protected override SyntaxToken VisitVariable(VariableSyntaxToken token)
        {
            if (SyntaxToken.Equals(this.Variable, token))
            {
                return SyntaxToken.Constant(1);
            }
            return SyntaxToken.Constant(0);
        }
        
        sealed class VariableDetector : Visitor
        {
            public bool HasVariable(SyntaxToken token)
            {
                return !(this.Visit(token) is NullSyntaxToken);
            }

            protected override SyntaxToken VisitBinary(BinarySyntaxToken token)
            {
                SyntaxToken st = this.Visit(token.Left);
                if (!(st is NullSyntaxToken))
                {
                    return st;
                }
                return this.Visit(token.Right);
            }
            protected override SyntaxToken VisitConstant(ConstantSyntaxToken token)
            {
                return new NullSyntaxToken();
            }
            protected override SyntaxToken VisitFunction(FunctionSyntaxToken token)
            {
                foreach (SyntaxToken arg in token.Arguments)
                {
                    SyntaxToken st = this.Visit(arg);
                    if (!(st is NullSyntaxToken))
                    {
                        return st;
                    }
                }
                return new NullSyntaxToken();
            }
            protected override SyntaxToken VisitNamedConstant(NamedConstantSyntaxToken token)
            {
                return new NullSyntaxToken();
            }
            protected override SyntaxToken VisitUnary(UnarySyntaxToken token)
            {
                return this.Visit(token.Value);
            }
            protected override SyntaxToken VisitVariable(VariableSyntaxToken token)
            {
                return token;
            }

            private class NullSyntaxToken : SyntaxToken
            {
                public override SyntaxTokenType TokenType => (SyntaxTokenType)(-1);

                public override string ToString()
                {
                    return string.Empty;
                }
            }
        }
    }
}
