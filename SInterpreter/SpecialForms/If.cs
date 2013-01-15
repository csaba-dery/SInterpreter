using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.SpecialForms
{
    internal class If : ISpecialForm
    {
        public object Evaluate(Frame environment, Expression expression)
        {
            List<Expression> operands = expression.GetRest();
            if (operands.Count < 2)
            {
                throw new Exception("Invalid number of If arguments");
            }

            Frame evalFrame = new Frame(new Dictionary<string, Procedure>(), environment, environment, null, true, null);
            object result = evalFrame.Evaluate(operands[0]);
            bool resultValue = false;
            if (!bool.TryParse(result.ToString(), out resultValue))
            {
                 throw new Exception("If clause not a predicate.");
            }
            if (resultValue)
            {
                return evalFrame.Evaluate(operands[1],true);
            }
            else
            {
                if (operands.Count > 2)
                {
                    return evalFrame.Evaluate(operands[2], true);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
