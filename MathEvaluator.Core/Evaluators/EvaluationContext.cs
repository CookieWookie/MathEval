using System.Collections.Generic;
using MathEvaluator.Core.Syntax;

namespace MathEvaluator.Core.Evaluators
{
    public sealed class EvaluationContext
    {
        internal EvaluationContext()
        {
            this.VariablesInternal = new Dictionary<string, SyntaxToken>();
            this.Variables = this.VariablesInternal.AsReadOnly();
        }

        public static EvaluationContext Empty => new EvaluationContext();
        private Dictionary<string, SyntaxToken> VariablesInternal { get; }
        public IReadOnlyDictionary<string, SyntaxToken> Variables { get; }

        public void SetVariable(string name, SyntaxToken value)
        {
            this.VariablesInternal[name] = value;
        }
    }
}
