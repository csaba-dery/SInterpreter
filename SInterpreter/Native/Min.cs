using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Min : MathProcedure
    {
        internal Min(Frame defEnv) : base(defEnv) {}

        protected override long ProcessValues(List<long> paramValues)
        {
            return paramValues.Min();
        }

        protected override double ProcessValues(List<double> paramValues)
        {
            return paramValues.Min();
        }
    }
}
