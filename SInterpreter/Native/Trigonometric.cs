using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Trigonometric : Procedure
    {
        private Func<double, double> _function = null;
        

        internal Trigonometric(Frame defEnv,Func<double,double> function)
            : base(defEnv)
        {
            Parameters.Add("x");
            _function = function;
        }

        
        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("sin: Environment can not be null.");
            }
            double angle = 0;
            if (!double.TryParse(environment.FindBindingValue(Parameters[0]).ToString(), out angle))
            {
                throw new Exception("sin: Argument must be an number.");
            }
            return _function(angle);
        }
    }

    internal class Sine : Trigonometric
    {
        internal Sine(Frame defEnv)
            : base(defEnv, Math.Sin)
        {
        }
    }

    internal class Cosine : Trigonometric
    {
        internal Cosine(Frame defEnv)
            : base(defEnv, Math.Cos)
        {
        }
    }

    internal class Logarithm : Trigonometric
    {
        internal Logarithm(Frame defEnv)
            : base(defEnv, Math.Log)
        {
        }
    }

    internal class Floor : Trigonometric
    {
        internal Floor(Frame defEnv)
            : base(defEnv, Math.Floor)
        {
        }
    }


}
