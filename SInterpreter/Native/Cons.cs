using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Cons : Procedure
    {
        internal Cons(Frame defineEnv)  
            : base(defineEnv, new List<string>(), null)
        {
            Parameters.Add("x");
            Parameters.Add("y");
        }

        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("cons: Environment can not be null.");
            }

            object firstBinding = environment.FindBindingValue(Parameters[0]);
            if (firstBinding == null)
            {
                throw new Exception("cons: Can't find binding for first argument.");
            }
            object restBinding = environment.FindBindingValue(Parameters[1]);
            if (restBinding == null)
            {
                throw new Exception("cons: Can't find binding for second argument.");
            }
            return new Pair(firstBinding, restBinding);
        }
    }
}
