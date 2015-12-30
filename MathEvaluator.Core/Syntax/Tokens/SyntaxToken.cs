using System;

namespace MathEvaluator.Core.Syntax
{
    public abstract class SyntaxToken
    {
        public abstract SyntaxTokenType TokenType { get; }

        public abstract override string ToString();

        public static NamedConstantSyntaxToken E { get; } = new NamedConstantSyntaxToken("e", Math.E);
        public static NamedConstantSyntaxToken PI { get; } = new NamedConstantSyntaxToken("pi", Math.PI);

        public static bool Equals(SyntaxToken left, SyntaxToken right)
        {
            if (!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) && left.TokenType == right.TokenType)
            {
                if (left.TokenType == SyntaxTokenType.Variable && ((VariableSyntaxToken)left).Name == ((VariableSyntaxToken)right).Name)
                {
                    return true;
                }
                if (left.TokenType == SyntaxTokenType.NamedConstant && ((NamedConstantSyntaxToken)left).Name == ((NamedConstantSyntaxToken)right).Name)
                {
                    return true;
                }
            }
            return object.Equals(left, right);
        }
        public static ConstantSyntaxToken Constant(double value)
        {
            return new ConstantSyntaxToken(value);
        }
        public static VariableSyntaxToken Variable(string name)
        {
            return new VariableSyntaxToken(name);
        }
        public static BinarySyntaxToken Add(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Binary(left, right, BinaryOperationType.Add);
        }
        public static BinarySyntaxToken Subtract(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Binary(left, right, BinaryOperationType.Subtract);
        }
        public static BinarySyntaxToken Multiply(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Binary(left, right, BinaryOperationType.Multiply);
        }
        public static BinarySyntaxToken Divide(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Binary(left, right, BinaryOperationType.Divide);
        }
        public static BinarySyntaxToken Mod(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Binary(left, right, BinaryOperationType.Modulus);
        }
        public static BinarySyntaxToken Pow(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Binary(left, right, BinaryOperationType.Power);
        }
        public static BinarySyntaxToken Root(SyntaxToken value, SyntaxToken lvl)
        {
            return SyntaxToken.Pow(value, SyntaxToken.Divide(SyntaxToken.Constant(1), lvl));
        }
        public static UnarySyntaxToken Factorial(SyntaxToken value)
        {
            return SyntaxToken.Unary(value, UnaryOperationType.Factorial);
        }
        public static FunctionSyntaxToken Ln(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Ln, value);
        }
        public static FunctionSyntaxToken Log(SyntaxToken value, SyntaxToken logBase)
        {
            return SyntaxToken.Function(FunctionType.Log, value, logBase);
        }
        public static FunctionSyntaxToken Abs(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Abs, value);
        }
        public static FunctionSyntaxToken Sin(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Sin, value);
        }
        public static FunctionSyntaxToken Asin(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Asin, value);
        }
        public static FunctionSyntaxToken Cos(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Cos, value);
        }
        public static FunctionSyntaxToken Acos(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Acos, value);
        }
        public static FunctionSyntaxToken Tan(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Tan, value);
        }
        public static FunctionSyntaxToken Atan(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Atan, value);
        }
        public static FunctionSyntaxToken Cot(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Cot, value);
        }
        public static FunctionSyntaxToken Acot(SyntaxToken value)
        {
            return SyntaxToken.Function(FunctionType.Acot, value);
        }
        public static UnarySyntaxToken Negate(SyntaxToken value)
        {
            return SyntaxToken.Unary(value, UnaryOperationType.Negate);
        }

        public static BinarySyntaxToken Binary(SyntaxToken left, SyntaxToken right, BinaryOperationType type)
        {
            return new BinarySyntaxToken(left, right, type);
        }
        public static UnarySyntaxToken Unary(SyntaxToken value, UnaryOperationType type)
        {
            return new UnarySyntaxToken(value, type);
        }
        public static FunctionSyntaxToken Function(FunctionType type, params SyntaxToken[] arguments)
        {
            return new FunctionSyntaxToken(type, arguments);
        }

        public static SyntaxToken operator +(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Add(left, right);
        }
        public static SyntaxToken operator -(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Subtract(left, right);
        }
        public static SyntaxToken operator *(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Multiply(left, right);
        }
        public static SyntaxToken operator /(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Divide(left, right);
        }
        public static SyntaxToken operator %(SyntaxToken left, SyntaxToken right)
        {
            return SyntaxToken.Mod(left, right);
        }
        public static SyntaxToken operator -(SyntaxToken value)
        {
            return SyntaxToken.Negate(value);
        }
        public static SyntaxToken operator +(SyntaxToken value)
        {
            return value;
        }
    }
}
