using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    public class InterpreterException : Exception
    {
        internal InterpreterException() : base() { }

        internal InterpreterException(string message) : base(message) { }

        internal InterpreterException(string message, Exception innerException) : base(message,innerException) { }
    }
}
