using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SInterpreter.Native
{
    internal class Load : Procedure
    {
        internal Load(Frame defineEnv)  
            : base(defineEnv, new List<string>(), null)
        {
            Parameters.Add("file");
        }

        public override object Evaluate(Frame environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("Environment can not be null.");
            }
            string path = environment.FindBindingValue(Parameters[0]).ToString();
            if (!File.Exists(path))
            {
                throw new Exception("Could not find file: " + path);
            }

            using (StreamReader codeReader = new StreamReader(File.Open(path, FileMode.Open)))
            {
                Scanner scanner = new Scanner(codeReader);

                Expression expr = scanner.NextExpression;
                string evaluation = string.Empty;
                while (expr != null)
                {
                    evaluation = environment.Evaluate(expr).ToString();
                    expr = scanner.NextExpression;
                }

                return evaluation;
            }
        }
    }
}
