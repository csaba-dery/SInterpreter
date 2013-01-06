using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class Constant : IEvaluatable
    {
        private object _value = null;

        internal Constant(object value)
        {
            this._value = value;
        }

        public object Evaluate(Frame environment)
        {
            return this._value;
        }
    }
}
