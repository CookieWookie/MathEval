using System;

namespace MathEvaluator.Core.Syntax
{
    static class SyntaxTreeHelpers
    {
        public static SyntaxToken TryCalculate(this SyntaxToken token, Func<double, double> func)
        {
            ConstantSyntaxToken cst = token as ConstantSyntaxToken;
            if (cst != null)
            {
                return SyntaxToken.Constant(func(cst.Value));
            }
            return token;
        }

        public static bool HasValue(this SyntaxToken token, double value)
        {
            return token.TokenType == SyntaxTokenType.Constant && ((ConstantSyntaxToken)token).Value == value;
        }

        public static string GetString(this BinaryOperationType type)
        {
            switch (type)
            {
                case BinaryOperationType.Add: return "+";
                case BinaryOperationType.Subtract: return "-";
                case BinaryOperationType.Multiply: return "*";
                case BinaryOperationType.Divide: return "/";
                case BinaryOperationType.Modulus: return "%";
                case BinaryOperationType.Power: return "^";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
