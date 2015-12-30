using System;
using System.Collections.Generic;
using System.Linq;

namespace MathEvaluator.Core.Lexic
{
    public class LexerResult
    {
        public LexerResult(IEnumerable<LexicToken> tokens)
        {
            this.InfixNotation = new List<LexicToken>(tokens).AsReadOnly();
            this.LazyRPN = new Lazy<IReadOnlyList<LexicToken>>(this.ToReversePolishNotation);
        }

        public IReadOnlyList<LexicToken> InfixNotation { get; }
        private Lazy<IReadOnlyList<LexicToken>> LazyRPN { get; }
        public IReadOnlyList<LexicToken> PostfixNotation => this.LazyRPN.Value;

        private IReadOnlyList<LexicToken> ToReversePolishNotation()
        {
            List<LexicToken> output = new List<LexicToken>();
            Stack<LexicToken> operators = new Stack<LexicToken>();
            foreach (LexicToken token in this.InfixNotation)
            {
                this.EvaluateToken(token, output, operators);
            }
            output.AddRange(operators);
            return output.AsReadOnly();
        }
        private void EvaluateToken(LexicToken token, List<LexicToken> output, Stack<LexicToken> operators)
        {
            if (token is WhitespaceLexicToken || token is ArgumentSeparatorLexicToken)
            {
                return;
            }
            // if constant, factorial or variable add to output
            if (token is ConstantLexicToken || token is FactorialLexicToken || token is VariableLexicToken)
            {
                output.Add(token);
                return;
            }
            if (token is LeftParenLexicToken)
            {
                operators.Push(token);
                return;
            }
            if (token is RightParenLexicToken)
            {
                LexicToken op;
                while (operators.CanPop())
                {
                    op = operators.Pop();
                    if (op is LeftParenLexicToken)
                    {
                        break;
                    }
                    output.Add(op);
                }
                return;
            }
            if (!operators.Any())
            {
                operators.Push(token);
                return;
            }
            LexicToken prev = operators.Peek();
            if (prev is LeftParenLexicToken || this.HasHigherPrecedence(token, prev))
            {
                operators.Push(token);
            }
            else
            {
                output.Add(operators.Pop());
                operators.Push(token);
            }
        }
        private int GetPrecedence(LexicToken token)
        {
            if (token is AddLexicToken)
            {
                return OperationPriorities.Addition;
            }
            else if (token is SubtractLexicToken)
            {
                return OperationPriorities.Subtraction;
            }
            else if (token is MultiplyLexicToken)
            {
                return OperationPriorities.Multiplication;
            }
            else if (token is DivideLexicToken)
            {
                return OperationPriorities.Division;
            }
            else if (token is ModulusLexicToken)
            {
                return OperationPriorities.Modulus;
            }
            else if (token is PowerLexicToken)
            {
                return OperationPriorities.Power;
            }
            else if (token is FunctionLexicToken)
            {
                return OperationPriorities.Function;
            }
            else if (token is FactorialLexicToken)
            {
                return OperationPriorities.Function;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        private bool HasHigherPrecedence(LexicToken current, LexicToken prev)
        {
            int currentPrecedence = this.GetPrecedence(current);
            int prevPrecedence = this.GetPrecedence(prev);
            return currentPrecedence > prevPrecedence || (current is PowerLexicToken);
        }
    }
}
