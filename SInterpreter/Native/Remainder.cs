using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    class Remainder : MathProcedure
    {
        internal Remainder(Frame defEnv) : base(defEnv) {}

        protected override double ProcessValues(List<double> paramValues)
        {
            if (paramValues.Count < 2)
            {
                throw new Exception("Insufficient arguments for division.");
            }
            if (paramValues[1] == 0)
            {
                throw new Exception("Division by Zero.");
            }
            return paramValues[0] % paramValues[1];
        }

        protected override Int64 ProcessValues(List<Int64> paramValues)
        {
            if (paramValues.Count < 2)
            {
                throw new Exception("Insufficient arguments for division.");
            }
            if (paramValues[1] == 0)
            {
                throw new Exception("Division by Zero.");
            }
            return paramValues[0] % paramValues[1];
        }

    }
}
