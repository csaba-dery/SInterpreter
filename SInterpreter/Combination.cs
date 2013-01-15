using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class Combination : Expression
    {
        private Expression _oprt = null;
        private List<Expression> _operands = new List<Expression>();

        internal Combination(Expression oprt)
        {
            if (oprt == null)
            {
                throw new ArgumentException("Operator cannot be null");
            }
            _oprt = oprt;
        }

        internal override Expression GetFirst()
        {
            return _oprt;
        }

        internal void AddOperand(Expression operand)
        {
            _operands.Add(operand);
        }

        internal override List<Expression> GetRest()
        {
            return _operands;
        }

        internal override bool IsLiteral
        {
            get { return false; }
        }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Combination))
            {
                return false;
            }
            Combination other = (Combination)obj;
            if (!GetFirst().Equals(other.GetFirst()))
            {
                return false;
            }
            if (GetRest().Count != other.GetRest().Count)
            {
                return false;
            }
            List<Expression> operands = GetRest();
            List<Expression> otherOperands = other.GetRest();
            for (int i = 0; i < GetRest().Count; i++)
            {
                if (!operands[i].Equals(otherOperands[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            builder.Append(GetFirst().ToString());
            foreach (Expression expr in GetRest())
            {
                builder.Append(' ');
                builder.Append(expr.ToString());
            }
            builder.Append(')');
            return builder.ToString();
        }
    }
}
