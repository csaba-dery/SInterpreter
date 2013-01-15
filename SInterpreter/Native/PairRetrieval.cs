using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal abstract class PairRetrieval : Procedure
    {
        internal PairRetrieval(Frame defineEnv)  
            : base(defineEnv, new List<string>(), null)
        {
            Parameters.Add("pair");
        }

        protected abstract object GetPairPart(IPair pair);

        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("Environment can not be null.");
            }

            object pairObject = environment.FindBindingValue(Parameters[0]);
            if (pairObject == null || !(pairObject is IPair))
            {
                throw new Exception("Invalid argument: not a pair");
            }
            IPair pair = (IPair)pairObject;
            return GetPairPart(pair);
        }
    }
    

    internal class Car : PairRetrieval
    {
        internal Car(Frame defineEnv)  
            : base(defineEnv)
        {
        }

        protected override object GetPairPart(IPair pair)
        {
            return pair.GetFirst();
        }
    }


    internal class Cdr : PairRetrieval
    {
        internal Cdr(Frame defineEnv)  
            : base(defineEnv)
        {
        }

        protected override object GetPairPart(IPair pair)
        {
            return pair.GetRest();
        }
    }
}
