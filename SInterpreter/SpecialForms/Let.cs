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
            if (expression.GetOperands().Count < 2)
            {
                throw new Exception("Invalid let definition");
            }
            Expression definitionList = expression.GetOperands()[0];
            List<string> paramNames = new List<string>(definitionList.GetOperands().Count + 1);
            Dictionary<string, Procedure> bindings = new Dictionary<string, Procedure>(paramNames.Capacity);

            Expression firstDefinition = definitionList.GetOperator();
            AddDefinition(environment, paramNames, bindings, firstDefinition);
            foreach (Expression definition in definitionList.GetOperands()) 
            {
                AddDefinition(environment, paramNames, bindings, definition);
            }

            List<Expression> body = new List<Expression>(expression.GetOperands().Count - 1);
            for (int i = 1; i < expression.GetOperands().Count; i++)
            {
                body.Add(expression.GetOperands()[i]);
            }

            Lambda block = new Lambda(environment, paramNames, body);

            Frame child = new Frame(bindings, environment, environment, block,true,null);

            return child.Evaluate(block.Body);
        }


        private void AddDefinition(Frame environment, List<string> paramNames, Dictionary<string, Procedure> bindings, Expression definition)
        {
            if (definition != null)
            {
                string name = definition.GetOperator().ToString();
                paramNames.Add(name);

                if (definition.GetOperands().Count == 0)
                {
                    throw new Exception("Let: expression missing for " + name);
                }
                object value = environment.Evaluate(definition.GetOperands()[0]);
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
