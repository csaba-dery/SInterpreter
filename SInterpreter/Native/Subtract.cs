using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Subtract : MathProcedure
    {
        internal Subtract(Frame defEnv) : base(defEnv) {}

        protected override double ProcessValues(List<double> paramValues)
        {
            if (paramValues.Count == 1)
            {
                return -paramValues[0];
            }
            double result = paramValues[0];
            for (int i = 1; i < paramValues.Count; i++)
            {
                result -= paramValues[i];
            }
            return result;
        }

        protected override Int64 ProcessValues(List<Int64> paramValues)
        {
            if (paramValues.Count == 1)
            {
                return -paramValues[0];
            }
            Int64 result = paramValues[0];
            for (int i = 1; i < paramValues.Count; i++)
            {
                result -= paramValues[i];
            }
            return result;
        }
    }
}
