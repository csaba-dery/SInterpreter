using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    class Identity : Procedure
    {
        private object _value;

        internal Identity(Frame defEnv, object value) : base(defEnv, new List<String>(), new List<Expression>(1)) 
        {
            this._value = value;
        }

        public override object Evaluate(Frame frame)
        {
            return _value;
        }

        public override string ToString()
        {
            if (_value is double)
                return "double: " + _value.ToString();
            else if (_value is Int64)
                return "int: " + _value.ToString();
            else
                return _value.ToString();
        }
    }
}
