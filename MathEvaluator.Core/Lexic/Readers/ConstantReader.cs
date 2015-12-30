namespace MathEvaluator.Core.Lexic.Readers
{
    public class ConstantReader : RegexReader
    {
        public ConstantReader(string pattern) : base(pattern)
        {
        }

        protected override LexicToken GetToken(string value)
        {
            return new ConstantLexicToken(value);
        }
    }
}
