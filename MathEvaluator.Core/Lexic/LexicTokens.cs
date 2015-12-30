using System;

namespace MathEvaluator.Core.Lexic
{
    public abstract class LexicToken
    {
        public abstract override string ToString();
    }

    public abstract class OperatorLexicToken : LexicToken { }

    public class AddLexicToken : OperatorLexicToken
    {
        public override string ToString() => "+";
    }

    public class SubtractLexicToken : OperatorLexicToken
    {
        public override string ToString() => "-";
    }

    public class MultiplyLexicToken : OperatorLexicToken
    {
        public override string ToString() => "*";
    }

    public class DivideLexicToken : OperatorLexicToken
    {
        public override string ToString() => "/";
    }

    public class ModulusLexicToken : OperatorLexicToken
    {
        public override string ToString() => "%";
    }

    public class PowerLexicToken : OperatorLexicToken
    {
        public override string ToString() => "^";
    }

    public class FactorialLexicToken : OperatorLexicToken
    {
        public override string ToString() => "!";
    }

    public class WhitespaceLexicToken : LexicToken
    {
        public override string ToString() => " ";
    }

    public class LeftParenLexicToken : LexicToken
    {
        public override string ToString() => "(";
    }

    public class RightParenLexicToken : LexicToken
    {
        public override string ToString() => ")";
    }

    public class ArgumentSeparatorLexicToken : LexicToken
    {
        public override string ToString() => ";";
    }

    public class NamedLexicToken : LexicToken
    {
        public NamedLexicToken(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException();
            }
            this.Name = name;
        }

        public string Name { get; }

        public override string ToString() => this.Name;
    }

    public class VariableLexicToken : NamedLexicToken
    {
        public VariableLexicToken(string name) : base(name)
        {
        }
    }

    public class FunctionLexicToken : NamedLexicToken
    {
        public FunctionLexicToken(string name) : base(name)
        {
        }
    }

    public class ConstantLexicToken : LexicToken
    {
        public ConstantLexicToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException();
            }
            this.Value = value;
        }

        public string Value { get; }

        public override string ToString() => this.Value;
    }
}
