using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class GCD : MathProcedure
    {
        internal GCD(Frame defEnv) : base(defEnv) {}

        protected override long ProcessValues(List<long> paramValues)
        {
            if (paramValues.Count != 2) 
            {
                throw new Exception("gcd: Invalid number of parameters. 2 integers expected.");
            }
            return Calculate(paramValues[0],paramValues[1]);
        }

        private long Calculate(long n, long d)
        {
            if (d == 0)
            {
                return Math.Abs(n);
            }
            return Calculate(d, (n % d));
        }

        protected override double ProcessValues(List<double> paramValues)
        {
            if (paramValues.Count != 2)
            {
                throw new Exception("gcd: Invalid number of parameters. 2 integers expected.");
            }

            long value1, value2;
            if (long.TryParse(paramValues[0].ToString(), out value1) && 
                long.TryParse(paramValues[1].ToString(), out value2))
            {
                return Calculate(value1, value2);
            }

            throw new Exception("gcd: Only integers allowed,");
        }
    }
}
