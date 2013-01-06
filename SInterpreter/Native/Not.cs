using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Not : Procedure
    {
        internal Not(Frame defEnv) : base(defEnv, new List<String>(), null) 
        {
            Parameters.Add("x");
        }

        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("Environment can not be null.");
            }
            object predicate = environment.FindBindingValue(Parameters[0]);
            bool result = false;
            if (!bool.TryParse(predicate.ToString(), out result))
            {
                throw new Exception("not: parameter not a predicate.");
            }
            return !result;
        }
    }
}
