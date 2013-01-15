using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    interface IPair
    {
        object GetFirst();
        object GetRest();
    }
}
