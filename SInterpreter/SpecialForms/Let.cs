using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.SpecialForms
{
    internal class Let : ISpecialForm
    {
        public object Evaluate(Frame environment, Expression expression)
        {
            if (expression.GetRest().Count < 2)
            {
                throw new Exception("Invalid let definition");
            }
            Expression definitionList = expression.GetRest()[0];
            List<string> paramNames = new List<string>(definitionList.GetRest().Count + 1);
            Dictionary<string, Procedure> bindings = new Dictionary<string, Procedure>(paramNames.Capacity);

            Expression firstDefinition = definitionList.GetFirst();
            AddDefinition(environment, paramNames, bindings, firstDefinition);
            foreach (Expression definition in definitionList.GetRest()) 
            {
                AddDefinition(environment, paramNames, bindings, definition);
            }

            List<Expression> body = new List<Expression>(expression.GetRest().Count - 1);
            for (int i = 1; i < expression.GetRest().Count; i++)
            {
                body.Add(expression.GetRest()[i]);
            }

            Lambda block = new Lambda(environment, paramNames, body);

            Frame child = new Frame(bindings, environment, environment, block,true,null);

            return child.Evaluate(block.Body);
        }


        private void AddDefinition(Frame environment, List<string> paramNames, Dictionary<string, Procedure> bindings, Expression definition)
        {
            if (definition != null)
            {
                string name = definition.GetFirst().ToString();
                paramNames.Add(name);

                if (definition.GetRest().Count == 0)
                {
                    throw new Exception("Let: expression missing for " + name);
                }

                //TODO clean up the constructor and property 
                Frame valueEnv = new Frame(new Dictionary<string, Procedure>(0), environment, null, new Identity(environment, null), false, "let-value");

                object value = valueEnv.Evaluate(definition.GetRest()[0],true);
                if (value is Procedure)
                {
                    bindings.Add(name, (Procedure)value);
                }
                else
                {
                    bindings.Add(name, new Identity(environment, value));
                }
            }
        }
    }
}
