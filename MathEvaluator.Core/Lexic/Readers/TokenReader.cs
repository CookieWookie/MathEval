namespace MathEvaluator.Core.Lexic.Readers
{
    public abstract class TokenReader
    {
        public abstract InputStream TryRead(InputStream stream, out LexicToken token);
    }
}
