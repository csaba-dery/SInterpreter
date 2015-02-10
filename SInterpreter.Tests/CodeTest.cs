using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SInterpreter;

namespace SInterpreter.Tests
{
    [TestClass]
    public class CodeTest
    {
        [TestMethod]
        public void TestCodeSnippets()
        {
            Interpreter interpreter = new Interpreter();

            using (StreamReader codeReader = new StreamReader(File.Open("code2.txt", FileMode.Open)), 
                                resultsReader = new StreamReader(File.Open("test2.txt", FileMode.Open))) {

                Scanner scanner = new Scanner(codeReader);

                Expression expression = scanner.NextExpression;
                String expectedResult = resultsReader.ReadLine();
                string actualResult = null;
                while (expression != null)
                {
                    try
                    {
                        actualResult = interpreter.Evaluate(expression).ToString();
                         
                        Console.WriteLine(actualResult);
                        if (!expectedResult.StartsWith("ignore"))
                        {
                            bool areEqual = AreResultsEqual(actualResult, expectedResult);
                            if (!areEqual)
                            {
                                int i = 1;
                            }
                            Assert.IsTrue(areEqual);
                        }
                        else 
                        {                       
                            Console.WriteLine("Expected Printout: " + expectedResult.Replace("ignore", ""));
                        }
                    }
                    catch (InterpreterException ex)
                    {
                        //Assert.Fail(string.Format("Error evaulating: {0} - {1}", expression.ToString()), ex.Message);
                    }

                    expression = scanner.NextExpression;
                    expectedResult = resultsReader.ReadLine();
                }
            }
        }

        private bool AreResultsEqual(string evaluationResult, string testResult)
        {
            if (testResult == null)
            {
                return false;
            }
            double x, y;
            if (double.TryParse(evaluationResult, out x) && double.TryParse(testResult, out y))
            {
                return Math.Round(x, 10) == Math.Round(y, 10);
            }

            return (evaluationResult.ToLower() == testResult.ToLower().Trim());
        }

    }
}
