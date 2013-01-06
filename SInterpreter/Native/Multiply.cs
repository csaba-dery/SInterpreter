using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Multiply : MathProcedure 
    {
        internal Multiply(Frame defEnv) : base(defEnv) {}

        protected override double ProcessValues(List<double> paramValues)
        {
            double product = 1;
            foreach (double number in paramValues)
            {
                product *= number;
            }
            return product;
        }


        protected override Int64 ProcessValues(List<Int64> paramValues)
        {
            Int64 product = 1;
            foreach (Int64 number in paramValues)
            {
                checked
                {
                    product *= number;
                }
            }
            return product;
        }

    }
}
