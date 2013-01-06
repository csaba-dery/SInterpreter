using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class RestParameters
    {
        private IList _paramList = new ArrayList();

        internal void Add(object parameter)
        {
            _paramList.Add(parameter);
        }

        public override string ToString()
        {
            string result = string.Empty;
            foreach (object parameter in _paramList)
            {
                if (parameter is double)
                {
                    result += ((double)parameter).ToString("") + " ";
                }
                else
                {
                    result += parameter.ToString() + " ";
                }
            }
            return result;
        }


        internal IList Values { get { return _paramList; } }
    }
}
