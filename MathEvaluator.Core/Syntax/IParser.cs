using System.Collections.Generic;
using MathEvaluator.Core.Lexic;

namespace MathEvaluator.Core.Syntax
{
    public interface IParser
    {
        ParserResult Evaluate(IParserContext input);
    }

    public interface IParserContext
    {
        IReadOnlyList<LexicToken> Stream { get; }
        FunctionType GetFunction(string name);
    }
}
