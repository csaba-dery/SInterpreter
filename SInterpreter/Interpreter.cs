using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SInterpreter.Native;

namespace SInterpreter
{
    public class Interpreter
    {
        private Scanner _scanner;
        private Frame _global;
        private Frame _running;

        public Interpreter()
        {
            _global = CreateGlobalFrame();
            _running = new Frame(null, _global, null, null, null);
        }

        public object Evaluate(Expression expression)
        {
            return _running.Evaluate(expression);
        }

        public void run()
        {
            String line = Console.ReadLine();
            while (line != null)
            {
                line = Console.ReadLine();
            }
        }

        private Frame CreateGlobalFrame()
        {
            Dictionary<String, Procedure> globalBindings = new Dictionary<string, Procedure>();
            Frame global = new Frame(globalBindings, null, null, new Identity(null, "0"), null); //TODO replace this with an eval proc    

            global.AddBinding("+", new Add(global));
            global.AddBinding("*", new Multiply(global));
            global.AddBinding("-", new Subtract(global));
            global.AddBinding("/", new Divide(global));
            global.AddBinding(">", new GreaterThan(global));
            global.AddBinding("<", new LessThan(global));
            global.AddBinding(">=", new GreaterOrEqualThan(global));
            global.AddBinding("<=", new LessOrEqualThan(global));
            global.AddBinding("=>", new GreaterOrEqualThan(global));
            global.AddBinding("=<", new LessOrEqualThan(global));
            global.AddBinding("=", new Equals(global));
            global.AddBinding("not", new Not(global));
            global.AddBinding("remainder", new Remainder(global));
            global.AddBinding("random", new SInterpreter.Native.Random(global));
            global.AddBinding("true", new SInterpreter.Native.Boolean(global, true));
            global.AddBinding("false", new SInterpreter.Native.Boolean(global, false));
            global.AddBinding("display", new Display(global));
            global.AddBinding("runtime", new Runtime(global));
            global.AddBinding("newline", new Newline(global));
            global.AddBinding("#t", new SInterpreter.Native.Boolean(global, true));
            global.AddBinding("#f", new SInterpreter.Native.Boolean(global, false));
            global.AddBinding("sin", new Sine(global));
            global.AddBinding("cos", new Cosine(global));
            global.AddBinding("log", new Logarithm(global));
            global.AddBinding("expt", new Power(global));
            global.AddBinding("floor", new Floor(global));
            global.AddBinding("load", new Load(global));
            global.AddBinding("cons", new Cons(global));
            global.AddBinding("car", new Car(global));
            global.AddBinding("cdr", new Cdr(global));
            global.AddBinding("gcd", new GCD(global));
            global.AddBinding("min", new Min(global));
            global.AddBinding("max", new Max(global));
            global.AddBinding("error", new Error(global));
            global.AddBinding("list", new ListCreate(global));
            global.AddBinding("null?", new NullCheck(global));

            return global;
        }
    }
}
