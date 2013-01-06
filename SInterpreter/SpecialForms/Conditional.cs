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
            List<Expression> operands = expression.GetOperands();
            if (operands.Count < 1)
            {
                throw new Exception("Invalid number of cond arguments");
            }
            Frame evalFrame = new Frame(new Dictionary<string, Procedure>(0), environment,environment, null, true, null);
            foreach (Expression expr in operands)
            {
                if (expr.GetOperator().ToString() == "else")
                {
                    return evalFrame.Evaluate(expr.GetOperands()[0], true);
                }
                object result = evalFrame.Evaluate(expr.GetOperator());
                bool resultValue = false;
                if (!bool.TryParse(result.ToString(), out resultValue))
                {
                    throw new Exception("Cond clause not a predicate.");
                }
                if (resultValue)
                {
                    object exprResult = string.Empty;

                    for (int i = 0; i < expr.GetOperands().Count;i++ )
                    {
                        exprResult = evalFrame.Evaluate(expr.GetOperands()[i]);
                    }
                    return exprResult;
                }
            }
            return string.Empty;
        }
    }
}
