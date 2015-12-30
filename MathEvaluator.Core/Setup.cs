using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEvaluator.Core.Evaluators;
using MathEvaluator.Core.Lexic;
using MathEvaluator.Core.Lexic.Readers;
using MathEvaluator.Core.Syntax;

namespace MathEvaluator.Core
{
    public sealed class Setup
    {
        private Setup()
        {
            this.Functions = new Dictionary<string, FunctionType>(StringComparer.OrdinalIgnoreCase);
            this.Constants = new Dictionary<string, double>();
            this.LexerTokenReaders = new List<TokenReader>();

            this.RegisterLexer(() => new Lexer());
            this.RegisterParser(() => new Parser());

            this.RegisterFunction(FunctionType.Abs, "abs");
            this.RegisterFunction(FunctionType.Acos, "acos", "arccos");
            this.RegisterFunction(FunctionType.Acot, "acot", "arccot");
            this.RegisterFunction(FunctionType.Asin, "asin", "arcsin");
            this.RegisterFunction(FunctionType.Atan, "atan", "arctan");
            this.RegisterFunction(FunctionType.Cos, "cos", "cosine");
            this.RegisterFunction(FunctionType.Cot, "cot", "cotan", "cotg");
            this.RegisterFunction(FunctionType.Ln, "ln");
            this.RegisterFunction(FunctionType.Log, "log");
            this.RegisterFunction(FunctionType.Sin, "sin", "sine");
            this.RegisterFunction(FunctionType.Tan, "tan", "tg");

            //this.RegisterConstant("PI", Math.PI);
            //this.RegisterConstant("Pi", Math.PI);
            //this.RegisterConstant("pi", Math.PI);
            //this.RegisterConstant("π", Math.PI);
            //this.RegisterConstant("e", Math.E);

            this.RegisterLexerTokenReader(new StringReader("**", new PowerLexicToken()));
            //this.RegisterLexerTokenReader(new StringReader("PI", NamedConstantLexicToken.PI));
            //this.RegisterLexerTokenReader(new StringReader("Pi", NamedConstantLexicToken.PI));
            //this.RegisterLexerTokenReader(new StringReader("pi", NamedConstantLexicToken.PI));
            //this.RegisterLexerTokenReader(new CharReader('π', NamedConstantLexicToken.PI));
            //this.RegisterLexerTokenReader(new CharReader('e', NamedConstantLexicToken.E));
            this.RegisterLexerTokenReader(new CharReader('+', new AddLexicToken()));
            this.RegisterLexerTokenReader(new CharReader('-', new SubtractLexicToken()));
            this.RegisterLexerTokenReader(new CharReader('*', new MultiplyLexicToken()));
            this.RegisterLexerTokenReader(new CharReader('/', new DivideLexicToken()));
            this.RegisterLexerTokenReader(new CharReader('%', new ModulusLexicToken()));
            this.RegisterLexerTokenReader(new CharReader('^', new PowerLexicToken()));
            this.RegisterLexerTokenReader(new CharReader('!', new FactorialLexicToken()));
            this.RegisterLexerTokenReader(new CharReader('(', new LeftParenLexicToken()));
            this.RegisterLexerTokenReader(new CharReader(')', new RightParenLexicToken()));
            this.RegisterLexerTokenReader(new CharReader(';', new ArgumentSeparatorLexicToken()));
            this.RegisterLexerTokenReader(new CharReader(',', new ArgumentSeparatorLexicToken()));
            this.RegisterLexerTokenReader(new ConstantReader(@"^[0-9]+(?:\.[0-9]+)?(?:[eE]-?[0-9]+)?"));
            this.RegisterLexerTokenReader(new WhitespaceReader());
            this.RegisterLexerTokenReader(new NamedReader());
        }

        private static Lazy<Setup> LazyInstance { get; } = new Lazy<Setup>(() => new Setup());
        public static Setup Instance => Setup.LazyInstance.Value;

        private Dictionary<string, FunctionType> Functions { get; }
        private Dictionary<string, double> Constants { get; }
        private List<TokenReader> LexerTokenReaders { get; }
        private Func<ILexer> Lexer { get; set; }
        private Func<IParser> Parser { get; set; }

        public void RegisterLexer(Func<ILexer> lexer)
        {
            this.Lexer = lexer;
        }
        public void RegisterParser(Func<IParser> parser)
        {
            this.Parser = parser;
        }
        public void RegisterFunction(FunctionType function, params string[] aliases)
        {
            foreach (string alias in aliases)
            {
                if (!string.IsNullOrEmpty(alias))
                {
                    this.Functions[alias] = function;
                }
            }
        }
        public void RegisterConstant(string name, double value)
        {
            this.Constants[name] = value;
        }
        public void RegisterLexerTokenReader(TokenReader reader)
        {
            this.LexerTokenReaders.Add(reader);
        }

        public ILexer GetLexer()
        {
            return this.Lexer();
        }
        public IParser GetParser()
        {
            return this.Parser();
        }

        public ILexerContext CreateLexerContext(string input)
        {
            return new LexerContext(this, input);
        }
        public IParserContext CreateParserContext(IEnumerable<LexicToken> rpn)
        {
            return new ParserContext(this, rpn.ToList());
        }

        public EvaluationContext CreateEvaluationContext()
        {
            EvaluationContext context = new EvaluationContext();
            foreach (var item in this.Constants)
            {
                context.SetVariable(item.Key, SyntaxToken.Constant(item.Value));
            }
            return context;
        }

        private class LexerContext : ILexerContext
        {
            public LexerContext(Setup setup, string input)
            {
                this.Stream = new InputStream(input);
                this.Readers = setup.LexerTokenReaders.AsReadOnly();
                this.Functions = setup.Functions.AsReadOnly();
            }

            private IReadOnlyDictionary<string, FunctionType> Functions { get; }
            public IReadOnlyList<TokenReader> Readers { get; }
            public InputStream Stream { get; }

            public bool IsFunction(string name)
            {
                FunctionType result;
                if (this.Functions.TryGetValue(name, out result))
                {
                    return result != FunctionType.Unsupported;
                }
                return false;
            }
        }
        private class ParserContext : IParserContext
        {
            public ParserContext(Setup setup, IReadOnlyList<LexicToken> input)
            {
                this.Stream = input;
                this.Functions = setup.Functions.AsReadOnly();
            }

            private IReadOnlyDictionary<string, FunctionType> Functions { get; }
            public IReadOnlyList<LexicToken> Stream { get; }

            public FunctionType GetFunction(string name)
            {
                FunctionType result;
                if (this.Functions.TryGetValue(name, out result))
                {
                    return result;
                }
                return FunctionType.Unsupported;
            }
        }
    }
}
