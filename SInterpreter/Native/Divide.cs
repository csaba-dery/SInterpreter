using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Divide : MathProcedure
    {
        internal Divide(Frame defEnv) : base(defEnv) {}

        protected override double ProcessValues(List<double> paramValues)
        {
            if (paramValues.Count < 2)
            {
                throw new Exception("Insufficient arguments for division.");
            }
            //TODO does it make sense to have more than 2 arguments?
            double result = paramValues[0];
            for (int i = 1; i < paramValues.Count; i++)
            {
                if (paramValues[i] == 0)
                {
                    throw new Exception("Division by Zero.");
                }
                result /= paramValues[i];
            }
            return result;
        }

        protected override object EvaluateResult(IList<object> paramValues)
        {
            List<double> decimalParamValues = paramValues.Select(x => Convert.ToDouble(x)).ToList();
            double result = ProcessValues(decimalParamValues);
            return result;
        }

        protected override Int64 ProcessValues(List<Int64> paramValues)
        {
            if (paramValues.Count < 2)
            {
                throw new Exception("Insufficient arguments for division.");
            }
            //TODO does it make sense to have more than 2 arguments?
            Int64 result = paramValues[0];
            for (int i = 1; i < paramValues.Count; i++)
            {
                if (paramValues[i] == 0)
                {
                    throw new Exception("Division by Zero.");
                }
                result /= paramValues[i];
            }
            return result;
        }

    }
}
