using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class Pair : IPair
    {
        private object _first = null;
        private object _rest = null;

        internal Pair(object first) : this(first, null) { }

        internal Pair(object first, object rest)
        {
            if (first == null)
            {
                throw new ArgumentNullException("First pair element cannot be null.");
            }
            _first = first;
            _rest = rest;
        }

        public object GetFirst()
        {
            return _first;
        }

        public object GetRest()
        {
            return _rest;
        }

        public object Cons(object item)
        {
            return new Pair(item,this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            builder.Append(GetFirst());
            builder.Append(" . ");
            builder.Append(GetRest());
            builder.Append(')');
            return builder.ToString();
        }
    }
}
