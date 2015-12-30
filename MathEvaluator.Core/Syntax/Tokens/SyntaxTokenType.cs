using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEvaluator.Core.Syntax
{
    public enum SyntaxTokenType
    {
        Constant = 1,
        Variable,
        Binary,
        Unary,
        Function,
        NamedConstant
    }
}
