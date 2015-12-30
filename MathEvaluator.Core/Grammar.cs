using System;
using System.Collections.Generic;
using System.Linq;
using MathEvaluator.Core.Lexic;
using MathEvaluator.Core.Lexic.Readers;
using MathEvaluator.Core.Syntax;

namespace MathEvaluator.Core
{
    public abstract class Grammar
    {
        internal protected Grammar()
        {
        }
        static Grammar()
        {
            Grammar.FunctionAliases = new Dictionary<string, SupportedFunctions>(StringComparer.OrdinalIgnoreCase);
            Grammar.RegisterDefaultLexicReader(new ConstantReader(@"^[0-9]+(?:\.[0-9]+)?(?:[eE]-?[0-9]+)?"));

            Grammar.RegisterCalculator<double>(new DoubleCalculator());

            Grammar.AddFunctionAlias(SupportedFunctions.Abs, "abs", "absolute");
            Grammar.AddFunctionAlias(SupportedFunctions.Acos, "acos", "acosine", "arccos", "arccosine");
            Grammar.AddFunctionAlias(SupportedFunctions.Acot, "acot", "arccot", "arccotg");
            Grammar.AddFunctionAlias(SupportedFunctions.Asin, "asin", "asine", "arcsin", "arcsine");
            Grammar.AddFunctionAlias(SupportedFunctions.Atan, "atan", "atg", "arctan", "arctg");
            Grammar.AddFunctionAlias(SupportedFunctions.Cos, "cos", "cosine");
            Grammar.AddFunctionAlias(SupportedFunctions.Cot, "cot", "cotg");
            Grammar.AddFunctionAlias(SupportedFunctions.Ln, "ln");
            Grammar.AddFunctionAlias(SupportedFunctions.Log, "log");
            Grammar.AddFunctionAlias(SupportedFunctions.Sin, "sin", "sine");
            Grammar.AddFunctionAlias(SupportedFunctions.Tan, "tan", "tg");

            Grammar.RegisterLexer(() => new Lexer());
            Grammar.RegisterParser(() => new Parser());
        }

        protected internal static Func<ILexer> LazyLexer { get; private set; }
        protected internal static Func<IParser> LazyParser { get; private set; }

        private static IDictionary<string, SupportedFunctions> FunctionAliases { get; }
        public static IEnumerable<string> RegisteredFunctions => Grammar.FunctionAliases.Keys;

        public static void RegisterLexer(Func<ILexer> lexer)
        {
            Grammar.LazyLexer = lexer;
        }
        public static void RegisterParser(Func<IParser> parser)
        {
            Grammar.LazyParser = parser;
        }
        public static void RegisterCalculator(ICalculator calculator)
        {
            Defaults.Calculator = calculator;
        }
        public static void RegisterDefaultLexicReader(TokenReader reader)
        {
            Defaults.LexicReaders.Add(reader);
        }

        public static void AddFunctionAlias(SupportedFunctions function, params string[] aliases)
        {
            foreach (string alias in aliases)
            {
                Grammar.FunctionAliases[alias] = function;
            }
        }
        internal protected static SupportedFunctions GetFunction(string name)
        {
            SupportedFunctions result;
            return Grammar.FunctionAliases.TryGetValue(name, out result) ? result : SupportedFunctions.Unsupported;
        }
        public static bool IsFunction(string name)
        {
            return Grammar.GetFunction(name) != SupportedFunctions.Unsupported;
        }

        internal protected static class Defaults
        {
            static Defaults()
            {
                Defaults.LexicReaders = new List<TokenReader>();

                Grammar.RegisterDefaultLexicReader(new NamedReader());
                Grammar.RegisterDefaultLexicReader(new WhitespaceReader());
                Grammar.RegisterDefaultLexicReader(new StringReader("**", new PowerLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('+', new AddLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('-', new SubtractLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('*', new MultiplyLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('/', new DivideLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('%', new ModulusLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('^', new PowerLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('!', new FactorialLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader('(', new LeftParenLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader(')', new RightParenLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader(';', new ArgumentSeparatorLexicToken()));
                Grammar.RegisterDefaultLexicReader(new CharReader(',', new ArgumentSeparatorLexicToken()));
            }

            public static ICalculator Calculator { get; set; }
            public static List<TokenReader> LexicReaders { get; }
        }
    }

    public class Grammar : Grammar
    {
        public Grammar()
        {
            this.Lexer = Grammar.LazyLexer();
            this.Parser = Grammar.LazyParser();
            this.Calculator = Defaults.Calculator;
            this.LexerReaders = new List<TokenReader>(Defaults.LexicReaders).AsReadOnly();
            this.LexerFilters = new List<ILexicTokenLexerFilter>
            {
                new WhitespaceTokenLexerFilter()
            };
        }

        private ILexer Lexer { get; }
        private IParser Parser { get; }
        public ICalculator Calculator { get; }
        public IReadOnlyList<TokenReader> LexerReaders { get; }
        private IEnumerable<ILexicTokenLexerFilter> LexerFilters { get; }

        public LexerResult Lex(string input)
        {
            return this.Lex(new InputStream(input));
        }
        public LexerResult Lex(InputStream input)
        {
            return this.Lexer.Evaluate(new LexerInput(this.LexerFilters, this.LexerReaders, input));
        }

        public ParserResult Parse(LexerResult lexerResult)
        {
            ParserInput input = new ParserInput(lexerResult, this.Calculator, Grammar.GetFunction);
            return this.Parser.Evaluate(input);
        }

        private struct LexerInput : ILexerInput
        {
            public LexerInput(IEnumerable<ILexicTokenLexerFilter> filters, IEnumerable<TokenReader> readers, InputStream input)
            {
                this.Filters = filters.ToList().AsReadOnly();
                this.Readers = readers.ToList().AsReadOnly();
                this.Stream = input;
            }

            public IReadOnlyList<ILexicTokenLexerFilter> Filters { get; }
            public IReadOnlyList<TokenReader> Readers { get; }
            public InputStream Stream { get; }

            public bool IsFunction(string name)
            {
                return Grammar.IsFunction(name);
            }
        }

        private struct ParserInput : IParserInput
        {
            public ParserInput(LexerResult lexerResult, ICalculator calculator, Func<string, SupportedFunctions> getFunc)
            {
                this.Stream = lexerResult.ReversePolishNotation;
                this.Calculator = calculator;
                this.GetFunc = getFunc;
            }

            public IReadOnlyList<LexicToken> Stream { get; }
            private ICalculator Calculator { get; }
            private Func<string, SupportedFunctions> GetFunc { get; }

            public SupportedFunctions GetFunction(string name)
            {
                return this.GetFunc(name);
            }
            public T ParseConstant(string value)
            {
                return this.Calculator.Parse(value);
            }
        }
    }
}
