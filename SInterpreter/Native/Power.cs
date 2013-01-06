using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    class Power : MathProcedure
    {
        internal Power(Frame defEnv) : base(defEnv) {         }

        protected override double ProcessValues(List<double> paramValues)
        {
            if (paramValues.Count < 2)
            {
                throw new Exception("Insufficient arguments for power.");
            }
            return Math.Pow(paramValues[0], paramValues[1]);
        }

        protected override Int64 ProcessValues(List<Int64> paramValues)
        {
            if (paramValues.Count < 2)
            {
                throw new Exception("Insufficient arguments for power.");
            }
            return (Int64)Math.Pow(paramValues[0], paramValues[1]);
        }

    }
}
