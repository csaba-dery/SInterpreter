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
            //TODO think through this, how many frames to go back
            Frame globalFrame = environment;
            while (globalFrame.ParentFrame != null)
            {
                globalFrame = globalFrame.ParentFrame;
            }
            return new Continuation(new Literal(Parameters[0]), globalFrame);
        }
    }
}
