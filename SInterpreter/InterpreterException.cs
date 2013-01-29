using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class RaisedException : Exception
    {
        internal RaisedException() : base() { }

        internal RaisedException(string message) : base(message) { }

        internal RaisedException(string message, Exception innerException) : base(message,innerException) { }
    }
}
