using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Runtime : Procedure
    {
        internal Runtime(Frame defineEnv) : base(defineEnv) { }

        public override object Evaluate(Frame environment)
        {
            return System.Environment.TickCount;
        }
    }
}
