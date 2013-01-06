using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal interface IEvaluatable
    {
        object Evaluate(Frame environment);
    }
}
