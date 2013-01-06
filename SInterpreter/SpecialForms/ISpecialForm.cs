using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.SpecialForms
{
    internal interface ISpecialForm
    {
        object Evaluate(Frame environment,Expression expression);
    }
}
