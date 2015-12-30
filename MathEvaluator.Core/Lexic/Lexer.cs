using System.Collections.Generic;
using System.Linq;
using MathEvaluator.Core.Lexic.Readers;

namespace MathEvaluator.Core.Lexic
{
    sealed class Lexer : ILexer
    {
        public LexerResult Evaluate(ILexerContext input)
        {
            InputStream stream = input.Stream;
            List<LexicToken> tokens = new List<LexicToken>();
            while (!stream.IsEmpty)
            {
                InputStream copy = stream;
                foreach (TokenReader reader in input.Readers)
                {
                    LexicToken token = null;
                    stream = reader.TryRead(stream, out token);
                    if (token != null && !(token is WhitespaceLexicToken))
                    {
                        if (token is NamedLexicToken)
                        {
                            NamedLexicToken nlt = (NamedLexicToken)token;
                            if (input.IsFunction(nlt.Name))
                            {
                                token = new FunctionLexicToken(nlt.Name);
                            }
                            else
                            {
                                token = new VariableLexicToken(nlt.Name);
                            }
                        }
                        tokens.Add(token);
                        break;
                    }
                }
                if (stream == copy)
                {
                    throw new ParserException($"Unexpected character '{stream.Content[0]}' at position {stream.Position}");
                }
            }
            this.ValidateLexingOutput(tokens);
            return new LexerResult(tokens);
        }

        private void ValidateLexingOutput(IList<LexicToken> tokens)
        {
            if (tokens.Count < 2)
            {
                return;
            }
            LexicToken t1 = null;
            LexicToken t2 = tokens[0];
            for (int i = 1; i < tokens.Count; i++)
            {
                t1 = t2;
                t2 = tokens[i];
                if ((t1 is ConstantLexicToken || t1 is VariableLexicToken) && (t2 is VariableLexicToken || t2 is FunctionLexicToken || t2 is LeftParenLexicToken))
                {
                    tokens.Insert(i++, new MultiplyLexicToken());
                }
            }
        }
    }
}
