using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SInterpreter.Native
{
    internal abstract class MathProcedure : Procedure
    {
        protected bool isDecimal = false;

        internal MathProcedure(Frame defEnv) : base(defEnv,new List<String>(), null) 
        {
            Parameters.Add("x");
            Parameters.Add(".");
        }

        public override object Evaluate(Frame environment)
        {
            isDecimal = false;
            if (environment == null)
            {
                throw new ArgumentNullException("Environment can not be null.");
            }
            List<object> paramValues = new List<object>();
            foreach (String param in Parameters)
            {
                if (param == ".")
                {
                    continue;
                }
                object binding = environment.FindBindingValue(param);
                if (binding == null)
                {
                    throw new Exception("can't find binding");
                }
                paramValues.Add(GetNumber(binding));
            }
            object restText = environment.FindBindingValue(".");
            if (restText != null)
            {
                RestParameters rest = (RestParameters)restText;
                foreach (object number in rest.Values)
                {
                    paramValues.Add(GetNumber(number));
                }
            }
            return EvaluateResult(paramValues);
        }

        protected virtual object EvaluateResult(IList<object> paramValues)
        {
            if (isDecimal)
            {
                List<double> decimalParamValues = paramValues.Select(x => Convert.ToDouble(x)).ToList();
                double result = ProcessValues(decimalParamValues);
                return result;
            }
            else
            {
                List<Int64> intParamValues = paramValues.Select(x => (Int64)x).ToList();
                try
                {
                    Int64 result = ProcessValues(intParamValues);
                    return result;
                }
                catch (OverflowException)
                {
                    isDecimal = true;
                    return EvaluateResult(paramValues);
                }
            }
       }

        protected abstract Int64 ProcessValues(List<Int64> paramValues);

        protected abstract double ProcessValues(List<double> paramValues);

        protected object GetNumber(object numberText)
        {
            Int64 intValue = 0;
            double decimalVal = 0;
            
            //TODO what if numberText is a double, but have no decimals
            if (numberText is Int64)
            {
                return (Int64)numberText;
            }
            else if (numberText is double)
            {
                isDecimal = true;
                return (double)numberText;
            }

            if (numberText != null && Int64.TryParse(numberText.ToString(), out intValue))
            {
                return intValue;
            }
            else if (numberText != null && double.TryParse(numberText.ToString(), out decimalVal))
            {
                isDecimal = true;
                return decimalVal;
            }
            else
            {
                throw new Exception("Invalid argument for this operation;");
            }
        }
    }
}
