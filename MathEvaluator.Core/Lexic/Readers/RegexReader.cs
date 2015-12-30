using System.Text.RegularExpressions;

namespace MathEvaluator.Core.Lexic.Readers
{
    public abstract class RegexReader : TokenReader
    {
        public RegexReader(string pattern)
        {
            this.Evaluator = new Regex(pattern);
        }

        private Regex Evaluator { get; }

        public override InputStream TryRead(InputStream stream, out LexicToken token)
        {
            token = null;
            Match match = this.Evaluator.Match(stream.Content);
            if (match.Success)
            {
                token = GetToken(match.Value);
                return stream.Move(match.Value.Length);
            }
            return stream;
        }

        protected abstract LexicToken GetToken(string value);
    }
}
