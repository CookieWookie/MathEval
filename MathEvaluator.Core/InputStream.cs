using System;

namespace MathEvaluator.Core
{
    public sealed class InputStream
    {
        public InputStream(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException();
            }
            this.InitialContext = input;
            this.Content = input;
        }

        private string InitialContext { get; }
        public string Content { get; private set; }
        public int Position { get; private set; }
        public bool IsEmpty => this.Content.Length == 0;

        public InputStream Move(int shift)
        {
            InputStream result = new InputStream(this.InitialContext)
            {
                Position = this.Position + shift,
                Content = this.Content.Substring(shift)
            };
            return result;
        }
    }
}
