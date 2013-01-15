using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    class Variable : Expression
    {
        private string _name;

        internal Variable(String name)
        {
            _name = name;
        }

        internal override Expression GetFirst()
        {
            return new Literal(_name);
        }

        internal override List<Expression> GetRest()
        {
            return new List<Expression>();
        }

        internal override bool IsLiteral
        {
            get
            {
                return false;
            }
        }

        public override string ToString()
        {
            return _name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Variable))
            {
                return false;
            }
            Variable other = (Variable)obj;
            return this.ToString() == other.ToString();
        }
    }
}
