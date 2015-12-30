using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEvaluator.Core.Syntax
{
    public class NamedConstantSyntaxToken : ConstantSyntaxToken
    {
        internal NamedConstantSyntaxToken(string name, double value) : base(value)
        {
            this.Name = name;
        }

        public string Name { get; }
        public override SyntaxTokenType TokenType => SyntaxTokenType.NamedConstant;

        public override string ToString() => this.Name;
    }
}
