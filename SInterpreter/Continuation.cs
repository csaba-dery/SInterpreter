using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class Continuation
    {
        internal Continuation(Expression call,Procedure proc, Frame environment)
        {
            TailCall = call;
            IsTailCall = true;
            this.Environment = environment;
        }

        internal bool IsTailCall { get; private set; }
        internal Expression TailCall {get;private set;}
        internal Frame Environment { get; private set; }
    }
}
