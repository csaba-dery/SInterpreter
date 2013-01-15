using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.SpecialForms
{
    internal class Conditional : ISpecialForm
    {
        public object Evaluate(Frame environment, Expression expression)
        {
            List<Expression> operands = expression.GetRest();
            if (operands.Count < 1)
            {
                throw new Exception("Invalid number of cond arguments");
            }
            Frame evalFrame = new Frame(new Dictionary<string, Procedure>(0), environment,environment, null, true, null);
            foreach (Expression expr in operands)
            {
                if (expr.GetFirst().ToString() == "else")
                {
                    return evalFrame.Evaluate(expr.GetRest()[0], true);
                }
                object result = evalFrame.Evaluate(expr.GetFirst());
                bool resultValue = false;
                if (!bool.TryParse(result.ToString(), out resultValue))
                {
                    throw new Exception("Cond clause not a predicate.");
                }
                if (resultValue)
                {
                    object exprResult = string.Empty;

                    for (int i = 0; i < expr.GetRest().Count;i++ )
                    {
                        exprResult = evalFrame.Evaluate(expr.GetRest()[i]);
                    }
                    return exprResult;
                }
            }
            return string.Empty;
        }
    }
}
