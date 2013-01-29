using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Error : Procedure
    {
        internal Error(Frame defEnv) : base(defEnv,new List<String>(), null) 
        {
            Parameters.Add("message");
        }

        public override object Evaluate(Frame environment)
        {
            if (Parameters.Count <= 0) //TODO WTF! How can this be 0??
            {
                throw new Exception("Invalid number of parameters.");
            }
            string message = environment.FindBindingValue(Parameters[0]).ToString();
            throw new RaisedException(message);
        }
    }
}
