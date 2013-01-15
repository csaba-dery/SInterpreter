using System;
using System.Collections.Generic;
using SInterpreter.SpecialForms;

namespace SInterpreter
{
    class Frame
    {
        private IDictionary<String, Procedure> _bindings = null;
        private static IDictionary<String, ISpecialForm> _specialFormBindings = new Dictionary<string,ISpecialForm>();
        private static int frameCount = 0; //debugging variable
        private static int callCount = 0;
        private static int identityCount = 0;
        private bool _isLastCall = false;
        private bool _evalOperands = false;

        static Frame()
        {
            _specialFormBindings.Add("define", new Define());
            _specialFormBindings.Add("cond", new Conditional());
            _specialFormBindings.Add("if", new If());
            _specialFormBindings.Add("and", new And());
            _specialFormBindings.Add("or", new Or());
            _specialFormBindings.Add("lambda", new LambdaDefinition());
            _specialFormBindings.Add("let", new Let());
        }


        internal Frame(IDictionary<String, Procedure> bindings, Frame parent, Frame callingFrame, Procedure proc,string name) :
            this(bindings,parent,callingFrame,proc,false,name)
        {
        }


        internal Frame(IDictionary<String, Procedure> bindings, Frame parent, Frame callingFrame, Procedure proc,bool isSpecialForm,string name): 
            this(bindings,parent,callingFrame,proc,isSpecialForm,name,false)
        {
        }


        internal Frame(IDictionary<String, Procedure> bindings, Frame parent, Frame callingFrame, Procedure proc, bool isSpecialForm, string name,bool evalOperands)
        {
            _bindings = bindings;
            ParentFrame = parent;
            CallingFrame = callingFrame;
            EvaluatedProcedure = proc;
            EvaluatedProcedureName = name;
            IsSpecialForm = isSpecialForm;
            frameCount++;
            //_evalOperands = evalOperands;
            if (CallingFrame != null && CallingFrame._evalOperands)
            {
                //OperatorFrame = CallingFrame.OperatorFrame;
                //_evalOperands = true;
            }
        }



        private bool IsSpecialForm { get; set; }


        internal Frame ParentFrame
        {
            get;
            private set;
        }


        internal Frame CallingFrame
        {
            get;
            private set;
        }

        private Frame OperatorFrame
        {
            get;
            set;
        }


        internal Procedure FindProcedure(string name) 
        {
            if (!_bindings.Keys.Contains(name))
            {
                if (ParentFrame == null) {
                    return null;
                }
                return ParentFrame.FindProcedure(name);
            }
            return _bindings[name];
        }


        private ISpecialForm FindSpecialForm(string name)
        {
            if (!_specialFormBindings.Keys.Contains(name))
            {
                return null;
            }
            return _specialFormBindings[name];
        }


        protected Procedure EvaluatedProcedure
        {
            get;
            private set;
        }


        protected string EvaluatedProcedureName
        {
            get;
            private set;
        }


        protected Expression EvaluatedExpression
        {
            get;
            private set;
        }

        /*internal object Evaluate(Expression expression)
        {
            return Evaluate(expression, false);
        }

        internal object Evaluate(Expression expression, bool checkTailCall)
        {
            List<Expression> expressions = new List<Expression>(1);
            expressions.Add(expression);
            return Evaluate(expressions,checkTailCall);
        }

        internal object Evaluate(List<Expression> expressions)
        {
            return Evaluate(expressions, true);
        }*/


        internal object Evaluate(List<Expression> expressions)
        {
            object result = null;
            for (int i = 0; i < expressions.Count; i++ )
            {
                if (i == expressions.Count - 1)
                {
                    _isLastCall = true;
                    result = Evaluate(expressions[i], true);
                    Continuation evResult;
                    while (result is Continuation)
                    {
                        evResult = (Continuation)result;
                        if (evResult.Environment != this)
                        {
                            return evResult;
                        }
                        else
                        {
                            result = Evaluate(evResult.TailCall, false);
                        }
                    }
                }
                else
                {
                    result = Evaluate(expressions[i]);
                }
            }
            return result;
        }


        internal object Evaluate(Expression expression)
        {
            return Evaluate(expression,false);
        }


        internal object Evaluate(Expression expression,bool checkTailCall)
        {
            EvaluatedExpression = expression;

            callCount++;

            if (expression == null)
            {
                throw new Exception("Invalid expression.");
            }

            if (expression.IsLiteral)
            {
                callCount--;
                return expression;
            }

            object oprt = Evaluate(expression.GetFirst());
            String procName = oprt.ToString();
            Procedure proc = null;
            if (oprt != null && oprt is Procedure)
            {
                proc = (Procedure)oprt;
            }
            else
            {
                proc = FindProcedure(procName);
            }

            if (proc == null)
            {
                ISpecialForm specialForm = FindSpecialForm(procName);
                if (specialForm == null)
                {
                    throw new Exception("No such binding found.");
                }
                else
                {
                    object o = specialForm.Evaluate(this, expression);
                    callCount--;
                    return o;
                }
            }


            if (proc.Parameters.Count > 0 && !(expression is Combination))
            {
                callCount--;
                return proc;
            }

            if (checkTailCall)
            {
                Frame tailCallFrame = FindCallingTailCallFrame(proc, procName);
                //Frame tailCallCallingFrame = FindCallingTailCallFrame(proc, procName);
                if (tailCallFrame != null)
                {
                    /*if (tailCallCallingFrame != null && tailCallFrame != tailCallCallingFrame)
                    {
                        int i=1;
                    }*/
                    List<IDictionary<string, Procedure>> bindings = new List<IDictionary<string, Procedure>>();                    
                    Frame current = this;
                    while (current != null && current.ParentFrame != null)
                    {
                        if (current != tailCallFrame)
                        {
                            if (current._bindings.Count > 0)
                            {
                                bindings.Add(current._bindings);
                            }
                        }
                        else
                        {
                            break;
                        }
                        current = current.ParentFrame;
                    }
                    bindings.Reverse();
                    foreach (IDictionary<string, Procedure> binding in bindings)
                    {
                        tailCallFrame.ReplaceBindings(binding);
                    }
                    /*Frame current = this;
                    while (current != tailCallFrame)
                    {
                        tailCallFrame.ReplaceBindings(current._bindings);
                        current = current.ParentFrame;
                    }*/
                    /*tailCallFrame.ReplaceBindings(this._bindings);
                    if (this.ParentFrame != tailCallFrame)
                    {
                        tailCallFrame.ParentFrame = this.ParentFrame;
                    }*/
                    return new Continuation(expression, proc, tailCallFrame);
                }
            }

            if (proc is Identity)
            {
                callCount--;
                return proc.Evaluate(null);
            }
            else
            {
                object o = ApplyProc(expression, procName, proc, checkTailCall);
                callCount--;
                return o;
            }
        }

        private object OperandEvaluate(Expression expr)
        {
            _evalOperands = true;
            object o = Evaluate(expr, false);
            _evalOperands = false;
            return o;
        }

        private object ApplyProc(Expression expression,String name, Procedure proc, bool checkTailCall)
        {
            //Console.WriteLine(expression);
            //Console.WriteLine(name);
            //this.EvaluatedProcedure = proc;

            List<Expression> operands = expression.GetRest();
            if (proc is Identity && operands.Count > 0)
            {
                throw new Exception(String.Format("The object {0} is not applicable.",proc.Evaluate(this)));
            }
            if (operands.Count < proc.MinParameterCount)
            {
                throw new Exception("Insufficient arguments for procedure.");
            }
            if (!proc.HasVariableParameter && operands.Count > proc.Parameters.Count)
            {
                throw new Exception(String.Format("Procedure expects {0} arguments but {1} were supplied.",proc.Parameters.Count,operands.Count));
            }
            IDictionary<String, Procedure> bindings = new Dictionary<string, Procedure>();
            for (int i = 0; i < proc.Parameters.Count; i++)
            {
                Frame operandEnv = new Frame(new Dictionary<string, Procedure>(0), this, null, new Identity(this, null),false, "operand",true);
                operandEnv.OperatorFrame = this;
                if (proc.Parameters[i] == ".")
                {
                    RestParameters restParameters = new RestParameters();
                    int j = i;
                    while (operands.Count - j >= proc.Parameters.Count - i)
                    {
                        restParameters.Add(operandEnv.Evaluate(operands[j]));
                        j++;
                    }
                    if (restParameters.Values.Count > 0)
                    {
                        bindings.Add(".", new Identity(this, restParameters));
                    }
                    continue;
                }
                object result = operandEnv.Evaluate(operands[i]);
                if (result is Procedure)
                {
                    bindings.Add(proc.Parameters[i], (Procedure)result);
                }
                else if (result is Continuation)
                {
                    int x = 0;
                }
                else
                {
                    //Console.WriteLine(proc.Parameters[i] + " - " + result);
                    bindings.Add(proc.Parameters[i], new Identity(this, result));
                }
            }

            /*if (name == "expmod")
            {
                Console.WriteLine(expression.ToString());
                foreach (String key in bindings.Keys)
                {
                    Console.WriteLine(key + " - " + bindings[key].Evaluate(null).ToString());
                }
                Console.WriteLine();
            }*/

            Frame env = new Frame(bindings, proc.DefinitionEnvironment, this,proc,name);
            object o;
            if (proc.Body != null && proc.Body.Count > 0)
            {
                o = env.Evaluate(proc.Body);
            }
            else
            {
                o = proc.Evaluate(env);
            }
            /*if (true || name == "random" || name == "expmod" || name == "try-it" || name=="*")
            {
                foreach (String key in bindings.Keys)
                {
                    //Console.WriteLine(key + " - " + bindings[key].Evaluate(null).ToString());
                }
                Console.WriteLine(name + ": " + o.ToString());
            }*/
            return o;
        }


        private Frame FindCallingTailCallFrame(Procedure proc,string name)
        {
            Frame current = this.CallingFrame;
            while (current != null && current.IsSpecialForm)
            {
                current = current.CallingFrame;
            }
            if (current != null &&
                current._isLastCall &&
                current.EvaluatedProcedure.Equals(proc) &&
                current.EvaluatedProcedureName == name)
            {
                Frame tailCallFrame = current.CallingFrame;
                while (tailCallFrame != null && tailCallFrame.IsSpecialForm)
                {
                    tailCallFrame = tailCallFrame.CallingFrame;
                }
                if (tailCallFrame != null && tailCallFrame.CallingFrame != null)
                {
                    return tailCallFrame;
                }
                else
                {
                    return current;
                }
            }
            /*while (current != null)
            {
                if (current._isLastCall && 
                    !current.IsSpecialForm && 
                    current.EvaluatedProcedure.Equals(proc) && 
                    current.EvaluatedProcedureName == name) 
                {
                    break;
                }
                if (!current.IsSpecialForm)
                {
                    i++;
                }
                current = current.CallingFrame;
            }
            if (current != null && i > 1)
            {
                Console.WriteLine();
            }*/
            return null;
        }

        private Frame FindTailCallFrame(Procedure proc, string name)
        {
            Frame current = this.ParentFrame;
            int i = 0;
            while (current != null && current.IsSpecialForm)
            {
                current = current.ParentFrame;
            }
            if (current != null &&
                current._isLastCall &&
                current.EvaluatedProcedure.Equals(proc) &&
                current.EvaluatedProcedureName == name)
            {
                Frame tailCallFrame = current.ParentFrame;
                /*while (tailCallFrame != null && tailCallFrame.IsSpecialForm)
                {
                    tailCallFrame = tailCallFrame.ParentFrame;
                }*/
                if (tailCallFrame.ParentFrame != null)
                {
                    return tailCallFrame;
                }
                else
                {
                    return current;
                }
            }
            /*while (current != null)
            {
                if (current._isLastCall &&
                    !current.IsSpecialForm &&
                    current.EvaluatedProcedure.Equals(proc) &&
                    current.EvaluatedProcedureName == name)
                {
                    break;
                }
                if (!current.IsSpecialForm)
                {
                    i++;
                }
                current = current.ParentFrame;
            }*/
            if (current != null && i > 1)
            {
                Console.WriteLine();
            }
            return null;
        }


        private void ReplaceBindings(IDictionary<String, Procedure> newBindings)
        {
            foreach (String name in newBindings.Keys)
            {
                if (_bindings.Keys.Contains(name))
                {
                    _bindings[name] = newBindings[name];
                }
                else
                {
                    _bindings.Add(name, newBindings[name]);
                }
            }
        }


        internal object FindBindingValue(String name)
        {
            if (!_bindings.Keys.Contains(name))
            {
                if (ParentFrame == null)
                {
                    return null;
                }
                return ParentFrame.FindBindingValue(name);
            }
            return _bindings[name].Evaluate(this);
        }


        internal void AddBinding(String name, Procedure proc)
        {
            _bindings[name] = proc;
        }
    }
}
