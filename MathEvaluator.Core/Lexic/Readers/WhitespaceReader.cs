namespace MathEvaluator.Core.Lexic.Readers
{
    public class WhitespaceReader : TokenReader
    {
        public override InputStream TryRead(InputStream stream, out LexicToken token)
        {
            token = null;
            char c = stream.Content[0];
            if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
            {
                token = new WhitespaceLexicToken();
                return stream.Move(1);
            }
            return stream;
        }
    }
}
