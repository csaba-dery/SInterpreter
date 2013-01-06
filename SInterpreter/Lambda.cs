using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal class Lambda : Procedure
    {
        internal Lambda(Frame defEnv, List<string> parameters, List<Expression> body)
            : base(defEnv, parameters, body) 
        {
        }

        public override object Evaluate(Frame environment)
        {
            return environment.Evaluate(Body);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Lambda))
            {
                return false;
            }
            Lambda other = (Lambda)obj;
            if (Parameters.Count != other.Parameters.Count)
            {
                return false;
            }
            List<Expression> body = Body;
            List<Expression> otherBody = other.Body;
            for (int i = 0; i < Body.Count; i++)
            {
                if (!body[i].Equals(otherBody[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
