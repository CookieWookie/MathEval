using System.Collections.Generic;
using MathEvaluator.Core.Lexic.Readers;

namespace MathEvaluator.Core.Lexic
{
    public interface ILexer
    {
        LexerResult Evaluate(ILexerContext input);
    }

    public interface ILexerContext
    {
        InputStream Stream { get; }
        IReadOnlyList<TokenReader> Readers { get; }
        bool IsFunction(string name);
    }
}