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
    }
}
