using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEvaluator.Core;
using MathEvaluator.Core.Syntax;
using MathEvaluator.Core.Lexic;
using System.Text.RegularExpressions;
using MathEvaluator.Core.Evaluators;

namespace MathEvaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            do
            {
                try
                {
                    Console.Clear();
                    Console.Write("Zadajte vzorec:");
                    input = Console.ReadLine();
                    ILexer lexer = Setup.Instance.GetLexer();
                    ILexerContext lexerContext = Setup.Instance.CreateLexerContext(input);
                    LexerResult lexerResult = lexer.Evaluate(lexerContext);

                    IParser parser = Setup.Instance.GetParser();
                    IParserContext parserContext = Setup.Instance.CreateParserContext(lexerResult.PostfixNotation);
                    ParserResult parserResult = parser.Evaluate(parserContext);

                    Visitor evaluator = new Evaluator(Setup.Instance.CreateEvaluationContext());
                    Visitor differentiator = new Differentiator(SyntaxToken.Variable("x"));

                    Console.WriteLine($"Vstup: {input}");
                    Console.WriteLine($"Výsledok: {evaluator.Visit(parserResult.Tree)}");
                    Console.WriteLine($"Derivácia: {differentiator.Visit(parserResult.Tree)}");
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("Vyskytla sa chyba!");
                    Console.WriteLine("Ohláste to prosím Maťovi.");
                    Console.WriteLine();
                    Console.WriteLine("Za pochopenie ďakujeme!");
                    Console.WriteLine("Váš kalkulátor");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Chybová správa");
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
            while (true);

        }
    }
}
