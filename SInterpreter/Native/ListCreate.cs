using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class ListCreate : Procedure
    {
        internal ListCreate(Frame defEnv)
            : base(defEnv, new List<string>(), null)
        {
            Parameters.Add("first");
            Parameters.Add(".");
        }

        public override object Evaluate(Frame environment)
        {
            object first = environment.FindBindingValue(Parameters[0]); //TODO maybe FindBindingValue should throw an exception if a binding is not found.
            RestParameters rest = (RestParameters)environment.FindBindingValue(Parameters[1]);
            return new SList(first,rest.Values.Cast<object>());
        }
    }
}
