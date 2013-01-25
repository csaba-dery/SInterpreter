using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SInterpreter.Native;

namespace SInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Frame globalFrame = CreateGlobalFrame();
            BufferedStream codeStream = new BufferedStream(new FileStream("code2.txt",FileMode.Open));
            Scanner scanner = new Scanner(codeStream);
            StreamReader testReader = new StreamReader(File.Open("test2.txt", FileMode.Open));

            Expression expr = scanner.NextExpression;
            String result = testReader.ReadLine();
            string evaluation = null;
            while (expr != null)
            {
                evaluation = globalFrame.Evaluate(expr).ToString();
                Console.WriteLine(evaluation);
                if (!result.StartsWith("ignore") && !EqualEvals(evaluation,result)) 
                {
                    Console.WriteLine(evaluation + " did not match result: " + result);
                    //return;
                }
                if (result.StartsWith("ignore"))
                {
                    Console.WriteLine("Expected Printout: " + result.Replace("ignore", ""));
                }
                expr = scanner.NextExpression;
                result = testReader.ReadLine();
            }

            //bool b = fermat_test(1000000007);

            codeStream.Close();
            testReader.Close();
        }

        private static bool EqualEvals(string evaluation, string testResult)
        {
            if (testResult == null)
            {
                return false;
            }
            double x, y;
            if (double.TryParse(evaluation, out x) && double.TryParse(testResult, out y))
            {
                return Math.Round(x, 10) == Math.Round(y, 10);
            }

            return (evaluation.ToLower() == testResult.ToLower().Trim());
        }


        private static Frame CreateGlobalFrame()
        {
            Dictionary<String, Procedure> globalBindings = new Dictionary<string, Procedure>();
            Frame global = new Frame(globalBindings, null, null,new Identity(null,"0"),null); //TODO replace this with an eval proc    

            global.AddBinding("+", new Add(global));
            global.AddBinding("*", new Multiply(global));
            global.AddBinding("-", new Subtract(global));
            global.AddBinding("/", new Divide(global));
            global.AddBinding(">", new GreaterThan(global));
            global.AddBinding("<", new LessThan(global));
            global.AddBinding("=", new Equals(global));
            global.AddBinding("not", new Not(global));
            global.AddBinding("remainder", new Remainder(global));
            global.AddBinding("random", new SInterpreter.Native.Random(global));
            global.AddBinding("true", new SInterpreter.Native.Boolean(global,true));
            global.AddBinding("false", new SInterpreter.Native.Boolean(global,false));
            global.AddBinding("display", new Display(global));
            global.AddBinding("runtime", new Runtime(global));
            global.AddBinding("newline", new Newline(global));
            global.AddBinding("#t", new SInterpreter.Native.Boolean(global,true));
            global.AddBinding("#f", new SInterpreter.Native.Boolean(global,false));
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
            return global;
        }


        private static double square(double x) {
            double result = x * x;
            return result;
        }

        private static bool even(double x)
        {
            bool result = (x % 2) == 0;
            return result;
        }

        private static double expmod(double b, double exp, double m)
        {
            if (exp == 0)
                return 1;
            else if (even(exp)) {
                double expm = expmod(b, exp / 2, m);
                double sq = square(expm);
                double result = sq % m;
                return result;
            } else {
                double expm = expmod(b,exp-1,m);
                double pr = b * expm;
                double result = pr % m;
                return result;
            }
        }

        private static bool fermat_test(Int32 n)
        {
            if (n == Int32.MaxValue)
            {
                int i = 0;
            }
            return try_it(n,new System.Random().Next(n - 1)+1);
        }

        private static bool try_it(double n, double a)
        {
            return expmod(a,n,n) == a;
        }
    }
}
