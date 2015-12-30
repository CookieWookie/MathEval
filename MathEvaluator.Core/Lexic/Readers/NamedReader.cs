namespace MathEvaluator.Core.Lexic.Readers
{
    public class NamedReader : RegexReader
    {
        public NamedReader() : base(@"^[a-zA-Z]+")
        {
        }

        protected override LexicToken GetToken(string value)
        {
            return new NamedLexicToken(value);
        }
    }
}
