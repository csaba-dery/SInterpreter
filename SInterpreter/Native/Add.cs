using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Add : MathProcedure
    {
        internal Add(Frame defEnv) : base(defEnv) {         }

        protected override Int64 ProcessValues(List<Int64> paramValues)
        {
            Int64 sum = 0L;
            foreach (Int64 number in paramValues)
            {
                sum += number;
            }
            return sum;
        }

        protected override double ProcessValues(List<double> paramValues)
        {
            double sum = 0;
            foreach (double number in paramValues)
            {
                sum += number;
            }
            return sum;
        }
    }
}
