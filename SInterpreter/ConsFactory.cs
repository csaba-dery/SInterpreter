using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    static class ConsFactory
    {
        internal static object CreatePair(object first, object rest)
        {
            if (rest is IPair)
            {
                var pair = (IPair)rest;
                return pair.Cons(first);
            }
            else
            {
                return new Pair(first, rest);
            }
        }
    }
}
