using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal abstract class Comparison : Procedure
    {
        internal Comparison(Frame defEnv) : base(defEnv, new List<string>(), null) 
        {
            Parameters.Add("x");
            Parameters.Add("y");
        }


        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("Environment can not be null.");
            }
            List<double> paramValues = new List<double>();
            foreach (String param in Parameters)
            {
                String binding = environment.FindBindingValue(param).ToString();
                if (binding == null)
                {
                    throw new Exception("can't find binding");
                }
                double number;
                if (double.TryParse(binding, out number))
                {
                    paramValues.Add(number);
                }
            }
            if (paramValues.Count < 2)
            {
                throw new Exception("Illegal number of arguments, need at least 2.");
            }

            return CompareValues(paramValues[0], paramValues[1]);
        }

        protected abstract bool CompareValues(double x, double y);
    }


    internal class GreaterThan : Comparison
    {
        internal GreaterThan(Frame defEnv) : base(defEnv) { }

        protected override bool CompareValues(double x, double y)
        {
            return x > y;
        }
    }


    internal class LessThan : Comparison
    {
        internal LessThan(Frame defEnv) : base(defEnv) { }

        protected override bool CompareValues(double x, double y)
        {
            return x < y;
        }
    }


    internal class Equals : Comparison
    {
        internal Equals(Frame defEnv) : base(defEnv) { }

        protected override bool CompareValues(double x, double y)
        {
            return x == y;
        }
    }
}
