using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    class Random : Procedure
    {
        internal Random(Frame defEnv)
            : base(defEnv, new List<string>(1), null)
        {
            Parameters.Add("n");
        }

        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("Environment can not be null.");
            }
            int limit;
            object bindingValue = environment.FindBindingValue(Parameters[0]);
            if (bindingValue is int) 
            {
                limit = (int)bindingValue;
            } 
            else if (!int.TryParse(bindingValue.ToString(), out limit))
            {
                throw new Exception("random: Argument must be an integer");
            }
            System.Random rand = new System.Random();
            int result = rand.Next(limit);
            return result;
        }
    }
}
