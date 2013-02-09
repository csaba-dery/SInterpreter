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
        private bool _isLastCall = false;


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


        internal Frame(IDictionary<String, Procedure> bindings, Frame parent, Frame callingFrame, Procedure proc, bool isSpecialForm, string name)
        {
            _bindings = bindings;
            ParentFrame = parent;
            CallingFrame = callingFrame;
            EvaluatedProcedure = proc;
            EvaluatedProcedureName = name;
            IsSpecialForm = isSpecialForm;
            frameCount++;
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
                if (tailCallFrame != null)
                {
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

                    return new Continuation(expression, tailCallFrame);
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
                Frame operandEnv = new Frame(new Dictionary<string, Procedure>(0), this, null, new Identity(this, null),false, "operand");
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
                    throw new Exception("");
                }
                else
                {
                    //Console.WriteLine(proc.Parameters[i] + " - " + result);
                    bindings.Add(proc.Parameters[i], new Identity(this, result));
                }
            }

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
