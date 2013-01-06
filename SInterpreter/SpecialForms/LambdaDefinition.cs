using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.SpecialForms
{
    internal class LambdaDefinition : ISpecialForm
    {
        public object Evaluate(Frame environment, Expression expression)
        {
            if (expression.GetOperands().Count < 2)
            {
                throw new Exception("Invalid lambda definition");
            }
            Expression paramList = expression.GetOperands()[0];
            List<String> parameters = new List<string>(paramList.GetOperands().Count + 1);
            parameters.Add(paramList.GetOperator().ToString());
            foreach (Expression param in paramList.GetOperands())
            {
                parameters.Add(param.ToString());
            }

            List<Expression> body = new List<Expression>();
            for (int i = 1; i < expression.GetOperands().Count; i++)
            {
                body.Add(expression.GetOperands()[i]);
            }
            return new Lambda(environment, parameters, body);
        }
    }
}
