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
            if (expression.GetRest().Count < 2)
            {
                throw new Exception("Invalid lambda definition");
            }
            Expression paramList = expression.GetRest()[0];
            List<String> parameters = new List<string>(paramList.GetRest().Count + 1);
            parameters.Add(paramList.GetFirst().ToString());
            foreach (Expression param in paramList.GetRest())
            {
                parameters.Add(param.ToString());
            }

            List<Expression> body = new List<Expression>();
            for (int i = 1; i < expression.GetRest().Count; i++)
            {
                body.Add(expression.GetRest()[i]);
            }
            return new Lambda(environment, parameters, body);
        }
    }
}
