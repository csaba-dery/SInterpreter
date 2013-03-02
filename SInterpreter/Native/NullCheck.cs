using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class NullCheck : Procedure
    {
        internal NullCheck(Frame defEnv) : base(defEnv,new List<String>(), null) 
        {
            Parameters.Add("list");
        }

        public override object Evaluate(Frame environment)
        {
            if (Parameters.Count <= 0)
            {
                throw new Exception("Invalid number of parameters.");
            }
            SList list = environment.FindBindingValue(Parameters[0]) as SList; 
            return list.IsEmpty;    //TODO what if it's not a list, but a binding that can't be found.
        }

    }
}
