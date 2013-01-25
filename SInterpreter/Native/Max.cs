using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Max : MathProcedure
    {
        internal Max(Frame defEnv) : base(defEnv) {}

        protected override long ProcessValues(List<long> paramValues)
        {
            return paramValues.Max();
        }

        protected override double ProcessValues(List<double> paramValues)
        {
            return paramValues.Max();
        }
    }
}
