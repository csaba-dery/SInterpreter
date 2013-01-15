using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.SpecialForms
{
    internal class Or : ISpecialForm
    {
        public object Evaluate(Frame environment, Expression expression)
        {
            List<Expression> operands = expression.GetRest();
            if (operands.Count == 0)
            {
                throw new Exception("or: invalid number of arguments.");
            }
            Frame evalFrame = new Frame(new Dictionary<string, Procedure>(), environment,environment, null, true, null);
            for (int i = 0; i < operands.Count; i++)
            {
                object result = null;
                if (i == operands.Count - 1)
                {
                    result = evalFrame.Evaluate(operands[i], true);
                }
                else
                {
                    result = evalFrame.Evaluate(operands[i]);
                }
                bool resultValue = false;
                if (!bool.TryParse(result.ToString(), out resultValue))
                {
                    throw new Exception("or: clause not a predicate.");
                }
                if (resultValue)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
