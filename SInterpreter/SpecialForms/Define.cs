using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.SpecialForms
{
    internal class Define : ISpecialForm
    {
        public object Evaluate(Frame environment, Expression expression)
        {
            List<Expression> operands = expression.GetRest();
            if (operands.Count < 2)
            {
                throw new Exception("Invalid define arguments");
            }
            bool isFunctionDeclaration = false;
            if (operands[0] is Combination) //then it is not (define x (lambda ...
            {
                isFunctionDeclaration = true;
            }
            String name = operands[0].GetFirst().ToString();
            List<string> parameters = new List<string>();
            foreach (Expression expr in operands[0].GetRest())
            {
                parameters.Add(expr.ToString());
            }
            List<Expression> body = new List<Expression>(operands.Count - 1);
            if (!isFunctionDeclaration && operands[1].GetFirst() != null && operands[1].GetFirst().ToString() == "lambda")
            {
                LambdaDefinition factory = new LambdaDefinition();
                environment.AddBinding(name, (Lambda)factory.Evaluate(environment, operands[1]));
                return name;
            }
            for (int i = 1; i < operands.Count; i++)
            {
                body.Add(operands[i]);
            }
            environment.AddBinding(name, new Lambda(environment,parameters, body));
            return name;
        }
    }
}
