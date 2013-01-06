using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter
{
    internal abstract class Procedure
    {
        internal Procedure(Frame environment)
        {
            Parameters = new List<string>();
        }

        internal Procedure(Frame definitionEnv, List<String> parameters, List<Expression> body)
        {
            Parameters = parameters;
            Body = body;
            DefinitionEnvironment = definitionEnv;
        }

        internal Frame DefinitionEnvironment
        {
            get;
            private set;
        }

        internal protected List<Expression> Body
        {
            get;
            private set;
        }

        public abstract object Evaluate(Frame environment);

        internal List<string> Parameters
        {
            get;
            private set;
        }

        internal bool HasVariableParameter
        {
            get
            {
                return Parameters != null && Parameters.Contains(".");
            }
        }

        internal int MinParameterCount
        {
            get
            {
                if (Parameters == null)
                {
                    return 0;
                }
                else if (HasVariableParameter)
                {
                    return Parameters.Count - 1;
                }
                else
                {
                    return Parameters.Count;
                }
            }
        }
    }
}
