using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class SList : IPair
    {
        private List<object> _list = new List<object>();

        internal SList(object first)
        {
            _list.Add(first);
        }

        internal SList(IEnumerable<object> collection)
        {
            _list.AddRange(collection);
        }

        internal SList(object first, IEnumerable<object> collection)
        {
            _list.Add(first);
            _list.AddRange(collection);
        }

        public object GetFirst()
        {
            return _list.FirstOrDefault();
        }

        public object GetRest()
        {
            return new SList(_list.GetRange(1,_list.Count-1));
        }

        public object Cons(object item)
        {
            List<object> list = new List<object>(_list);
            return new SList(item,list);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            foreach (object o in _list)
            {
                builder.Append(o.ToString());
                builder.Append(" ");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(')');
            return builder.ToString();
        }
    }
}
