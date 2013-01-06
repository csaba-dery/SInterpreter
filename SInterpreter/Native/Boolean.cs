using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    //TODO, not sure if necessary
    internal class Boolean : Procedure
    {
        private bool _value;

        internal Boolean(Frame definitionEnv, bool value)
            : base(definitionEnv, new List<string>(), null)
        {
            _value = value;
        }

        public override object Evaluate(Frame environment)
        {
            return _value;
        }
    }
}
