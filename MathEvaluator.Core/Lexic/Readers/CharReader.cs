namespace MathEvaluator.Core.Lexic.Readers
{
    public class CharReader : TokenReader
    {
        public CharReader(char c, LexicToken token)
        {
            this.C = c;
            this.Token = token;
        }

        private char C { get; }
        private LexicToken Token { get; }

        public override InputStream TryRead(InputStream stream, out LexicToken token)
        {
            token = null;
            if (stream.Content[0] == C)
            {
                token = this.Token;
                return stream.Move(1);
            }
            return stream;
        }
    }
}
