using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Display : Procedure
    {
        internal Display(Frame defEnv)
            : base(defEnv, new List<string>(), null)
        {
            Parameters.Add("x");    //TODO might be a list?
        }

        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("Environment can not be null.");
            }
            object value = environment.FindBindingValue(Parameters[0]);
            Console.Write(value);
            return string.Empty;
        }
    }
}
