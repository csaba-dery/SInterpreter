using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal enum ExpressionType
    {

    }

    public abstract class Expression
    {
        internal abstract Expression GetFirst();
        internal abstract List<Expression> GetRest();
        internal abstract bool IsLiteral {get;}
    }
}
