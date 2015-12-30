using System;

namespace MathEvaluator.Core.Lexic.Readers
{
    public class StringReader : TokenReader
    {
        public StringReader(string s, LexicToken token)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException();
            }
            this.S = s;
            this.Token = token;
        }

        private string S { get; }
        private LexicToken Token { get; }

        public override InputStream TryRead(InputStream stream, out LexicToken token)
        {
            token = null;
            if (stream.Content.StartsWith(this.S))
            {
                token = this.Token;
                return stream.Move(this.S.Length);
            }
            return stream;
        }
    }
}
