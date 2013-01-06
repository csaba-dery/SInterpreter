using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal class Newline : Procedure
    {
        internal Newline(Frame defineEnv) : base(defineEnv) { }

        public override object Evaluate(Frame environment)
        {
            Console.WriteLine();
            return string.Empty;
        }
    }
}
