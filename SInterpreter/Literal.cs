using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class Literal : Expression
    {
        private String _value = String.Empty;

        internal Literal(string literalValue)
        {
            _value = literalValue;
        }

        internal override Expression GetOperator()
        {
            return null;
        }

        internal override List<Expression> GetOperands()
        {
            return new List<Expression>();
        }

        internal override bool IsLiteral
        {
            get { return true; }
        }

        public override string ToString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Literal))
            {
                return false;
            }
            Literal other = (Literal)obj;
            return this.ToString() == other.ToString();
        }
    }
}
