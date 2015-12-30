using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEvaluator.Core.Syntax;

namespace MathEvaluator.Core.Evaluators
{
    public class Evaluator : Visitor
    {
        public Evaluator(EvaluationContext context)
        {
            this.Context = context;
        }

        public EvaluationContext Context { get; }

        protected override SyntaxToken VisitBinary(BinarySyntaxToken token)
        {
            SyntaxToken left = this.Visit(token.Left);
            SyntaxToken right = this.Visit(token.Right);
            BinaryOperationType type = token.Type;
            if (left.TokenType == SyntaxTokenType.Constant && right.TokenType == SyntaxTokenType.Constant)
            {
                return this.EvalBinary((ConstantSyntaxToken)left, (ConstantSyntaxToken)right, type);
            }
            // swap constant to the right
            if ((type == BinaryOperationType.Add || type == BinaryOperationType.Multiply) && (left.TokenType == SyntaxTokenType.Constant))
            {
                SyntaxToken temp = right;
                right = left;
                left = temp;
            }
            // add
            if (type == BinaryOperationType.Add)
            {
                if (right.HasValue(0))
                {
                    return left;
                }
            }
            // subtract
            else if (type == BinaryOperationType.Subtract)
            {
                if (right.HasValue(0))
                {
                    return left;
                }
                if (SyntaxToken.Equals(left, right))
                {
                    return SyntaxToken.Constant(0);
                }
            }
            // multiply
            else if (type == BinaryOperationType.Multiply)
            {
                if (right.HasValue(0))
                {
                    return SyntaxToken.Constant(0);
                }
                if (right.HasValue(1))
                {
                    return left;
                }
                if (left.TokenType == SyntaxTokenType.Binary && right.TokenType == SyntaxTokenType.Binary)
                {
                    BinarySyntaxToken bl = (BinarySyntaxToken)left;
                    BinarySyntaxToken br = (BinarySyntaxToken)right;
                    if (bl.Type == BinaryOperationType.Power && br.Type == BinaryOperationType.Power && SyntaxToken.Equals(bl.Left, br.Left))
                    {
                        return this.Visit(SyntaxToken.Pow(bl.Left, SyntaxToken.Add(bl.Right, br.Right)));
                    }
                }
            }
            // divide
            else if (type == BinaryOperationType.Divide)
            {
                if (left.HasValue(0))
                {
                    return SyntaxToken.Constant(0);
                }
                if (SyntaxToken.Equals(left, right))
                {
                    return SyntaxToken.Constant(1);
                }
            }
            // modulus
            else if (type == BinaryOperationType.Modulus)
            {
                if (left.HasValue(0))
                {
                    return SyntaxToken.Constant(0);
                }
                if (SyntaxToken.Equals(left, right))
                {
                    return SyntaxToken.Constant(0);
                }
            }
            // pow
            else if (type == BinaryOperationType.Power)
            {
                if (right.HasValue(0))
                {
                    return SyntaxToken.Constant(1);
                }
                if (right.HasValue(1))
                {
                    return left;
                }
                if (left.HasValue(0))
                {
                    return SyntaxToken.Constant(1);
                }
                if (left.TokenType == SyntaxTokenType.Binary)
                {
                    BinarySyntaxToken bl = (BinarySyntaxToken)left;
                    if (bl.Type == BinaryOperationType.Power)
                    {
                        return this.Visit(SyntaxToken.Pow(bl.Left, SyntaxToken.Multiply(bl.Right, right)));
                    }
                }
            }

            return SyntaxToken.Binary(left, right, type);
        }
        private SyntaxToken EvalBinary(ConstantSyntaxToken left, ConstantSyntaxToken right, BinaryOperationType type)
        {
            double lval = left.Value;
            double rval = right.Value;
            switch (type)
            {
                case BinaryOperationType.Add: return SyntaxToken.Constant(lval + rval);
                case BinaryOperationType.Subtract: return SyntaxToken.Constant(lval - rval);
                case BinaryOperationType.Multiply: return SyntaxToken.Constant(lval * rval);
                case BinaryOperationType.Divide: return SyntaxToken.Constant(lval / rval);
                case BinaryOperationType.Modulus: return SyntaxToken.Constant(lval % rval);
                case BinaryOperationType.Power: return SyntaxToken.Constant(Math.Pow(lval, rval));
                default: throw new NotSupportedException();
            }
        }

        protected override SyntaxToken VisitUnary(UnarySyntaxToken token)
        {
            SyntaxToken value = this.Visit(token.Value);
            if (value.TokenType == SyntaxTokenType.Constant)
            {
                return this.EvalUnary((ConstantSyntaxToken)value, token.Type);
            }
            return SyntaxToken.Unary(value, token.Type);
        }
        private SyntaxToken EvalUnary(ConstantSyntaxToken value, UnaryOperationType type)
        {
            switch (type)
            {
                case UnaryOperationType.Factorial: return SyntaxToken.Constant(MathEx.Factorial(value.Value));
                default: throw new NotSupportedException();
            }
        }

        protected override SyntaxToken VisitVariable(VariableSyntaxToken token)
        {
            SyntaxToken result;
            if (this.Context.Variables.TryGetValue(token.Name, out result) && result != null)
            {
                return this.Visit(result);
            }
            return token;
        }

        protected override SyntaxToken VisitFunction(FunctionSyntaxToken token)
        {
            SyntaxToken[] arguments = token.Arguments.Select(this.Visit).ToArray();
            if (arguments.All(t => t.TokenType == SyntaxTokenType.Constant))
            {
                return this.EvalFunction(arguments.Cast<ConstantSyntaxToken>().ToArray(), token.Type);
            }
            return SyntaxToken.Function(token.Type, arguments);
        }
        private SyntaxToken EvalFunction(ConstantSyntaxToken[] values, FunctionType type)
        {
            switch (type)
            {
                case FunctionType.Sin: return SyntaxToken.Constant(Math.Sin(values[0].Value));
                case FunctionType.Cos: return SyntaxToken.Constant(Math.Cos(values[0].Value));
                case FunctionType.Tan: return SyntaxToken.Constant(Math.Tan(values[0].Value));
                case FunctionType.Cot: return SyntaxToken.Constant(MathEx.Cot(values[0].Value));
                case FunctionType.Asin: return SyntaxToken.Constant(Math.Asin(values[0].Value));
                case FunctionType.Acos: return SyntaxToken.Constant(Math.Acos(values[0].Value));
                case FunctionType.Atan: return SyntaxToken.Constant(Math.Atan(values[0].Value));
                case FunctionType.Acot: return SyntaxToken.Constant(MathEx.Acot(values[0].Value));
                case FunctionType.Log: return SyntaxToken.Constant(Math.Log(values[0].Value, values[1].Value));
                case FunctionType.Ln: return SyntaxToken.Constant(Math.Log(values[0].Value));
                case FunctionType.Abs: return SyntaxToken.Constant(Math.Abs(values[0].Value));
                default: throw new NotSupportedException();
            }
        }

        protected override SyntaxToken VisitConstant(ConstantSyntaxToken token)
        {
            return token;
        }
        protected override SyntaxToken VisitNamedConstant(NamedConstantSyntaxToken token)
        {
            return SyntaxToken.Constant(token.Value);
        }
    }
}
