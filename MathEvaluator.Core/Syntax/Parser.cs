using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MathEvaluator.Core.Lexic;

namespace MathEvaluator.Core.Syntax
{
    sealed class Parser : IParser
    {
        private static bool IsPI(string t)
        {
            return t == "PI" || t == "Pi" || t == "pi" || t == "π";
        }
        private static bool IsE(string t)
        {
            return t == "e";
        }

        public ParserResult Evaluate(IParserContext input)
        {
            Stack<SyntaxToken> stack = new Stack<SyntaxToken>();
            foreach (LexicToken lt in input.Stream)
            {
                if (lt is ConstantLexicToken)
                {
                    ConstantLexicToken clt = (ConstantLexicToken)lt;
                    SyntaxToken st = SyntaxToken.Constant(double.Parse(clt.Value, CultureInfo.InvariantCulture));
                    stack.Push(st);
                }
                else if (lt is VariableLexicToken)
                {
                    VariableLexicToken vlt = (VariableLexicToken)lt;
                    SyntaxToken st = SyntaxToken.Variable(vlt.Name);
                    if (Parser.IsPI(vlt.Name))
                    {
                        st = SyntaxToken.PI;
                    }
                    else if (Parser.IsE(vlt.Name))
                    {
                        st = SyntaxToken.E;
                    }
                    stack.Push(st);
                }
                else if (lt is OperatorLexicToken)
                {
                    if (lt is FactorialLexicToken)
                    {
                        stack.Push(SyntaxToken.Factorial(stack.Pop()));
                        continue;
                    }
                    SyntaxToken right = stack.Pop();
                    SyntaxToken left = stack.Pop();
                    stack.Push(this.GetOperatorFunction((OperatorLexicToken)lt)(left, right));
                }
                else if (lt is FunctionLexicToken)
                {
                    FunctionType function = input.GetFunction(((FunctionLexicToken)lt).Name);
                    switch (function)
                    {
                        case FunctionType.Sin:
                            stack.Push(SyntaxToken.Sin(stack.Pop()));
                            break;
                        case FunctionType.Cos:
                            stack.Push(SyntaxToken.Cos(stack.Pop()));
                            break;
                        case FunctionType.Tan:
                            stack.Push(SyntaxToken.Tan(stack.Pop()));
                            break;
                        case FunctionType.Cot:
                            stack.Push(SyntaxToken.Cot(stack.Pop()));
                            break;
                        case FunctionType.Asin:
                            stack.Push(SyntaxToken.Asin(stack.Pop()));
                            break;
                        case FunctionType.Acos:
                            stack.Push(SyntaxToken.Acos(stack.Pop()));
                            break;
                        case FunctionType.Atan:
                            stack.Push(SyntaxToken.Atan(stack.Pop()));
                            break;
                        case FunctionType.Acot:
                            stack.Push(SyntaxToken.Acot(stack.Pop()));
                            break;
                        case FunctionType.Log:
                            SyntaxToken right = stack.Pop();
                            SyntaxToken left = stack.Pop();
                            stack.Push(SyntaxToken.Log(left, right));
                            break;
                        case FunctionType.Ln:
                            stack.Push(SyntaxToken.Ln(stack.Pop()));
                            break;
                        case FunctionType.Abs:
                            stack.Push(SyntaxToken.Abs(stack.Pop()));
                            break;
                        case FunctionType.Unsupported:
                        default: throw new NotSupportedException();
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            ParserResult result = new ParserResult(stack.Pop());
            if (stack.Any())
            {
                throw new InvalidOperationException("There are still values in the queue.");
            }
            return result;
        }
        private Func<SyntaxToken, SyntaxToken, SyntaxToken> GetOperatorFunction(OperatorLexicToken token)
        {
            if (token is AddLexicToken)
            {
                return SyntaxToken.Add;
            }
            if (token is SubtractLexicToken)
            {
                return SyntaxToken.Subtract;
            }
            if (token is MultiplyLexicToken)
            {
                return SyntaxToken.Multiply;
            }
            if (token is DivideLexicToken)
            {
                return SyntaxToken.Divide;
            }
            if (token is ModulusLexicToken)
            {
                return SyntaxToken.Mod;
            }
            if (token is PowerLexicToken)
            {
                return SyntaxToken.Pow;
            }
            throw new ArgumentException();
        }
    }
}
