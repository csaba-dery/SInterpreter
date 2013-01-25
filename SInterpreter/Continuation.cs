using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class Continuation
    {
        internal Continuation(Expression call, Frame environment)
        {
            TailCall = call;
            this.Environment = environment;
        }

        internal Expression TailCall {get;private set;}
        internal Frame Environment { get; private set; }
    }
}
