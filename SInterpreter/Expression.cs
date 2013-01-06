using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal enum ExpressionType
    {

    }

    internal abstract class Expression
    {
        internal abstract Expression GetOperator();
        internal abstract List<Expression> GetOperands();
        internal abstract bool IsLiteral {get;}
    }
}
