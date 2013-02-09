using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SInterpreter
{
    internal class Scanner
    {
        private int[] _expressionSeparators = { ')', '(' };
        private int[] _whiteSpaceSeparators = { ' ', '\t', '\r','\n' };
        private int[] _separators = { ' ', '\t', ')', '(' ,'\r','\n'};
        private int _currentChar = ' ';

        private StreamReader _streamReader;


        internal Scanner(Stream stream)
        {
            this._streamReader = new StreamReader(stream);
        }


        internal Expression NextExpression
        {
            get
            {
                return ReadExpression();
            }
        }


        private Expression ReadExpression()
        {
            while (_whiteSpaceSeparators.Contains(_currentChar = _streamReader.Read()) && _currentChar != -1) { }

            if (_streamReader.EndOfStream) 
            {
                return null;
            }

            return GetExpression();
        }


        private Expression GetExpression()
        {
            int nextChar = _streamReader.Peek();
            if (_currentChar == '(')
            {
                return ReadCombination();
            }
            else if (Char.IsDigit((char)_currentChar) ||
                _currentChar == '\'' ||
                _currentChar == '\"' ||
                (_currentChar == '-' && Char.IsDigit((char)nextChar)))
            {
                return ReadLiteral();
            }
            else
            {
                return ReadVariable();
            }
        }


        private Expression ReadCombination()
        {
            StringBuilder oprtBuilder = new StringBuilder();
            while (!_separators.Contains(_currentChar=_streamReader.Read()) && _currentChar != -1)
            {
                oprtBuilder.Append((char)_currentChar);
            }
            if (_streamReader.EndOfStream)
            {
                throw new ApplicationException("Invalid combination");
            }
            String oprt = oprtBuilder.ToString();
            Combination combination;
            bool isLiteralOperator = false;
            if (!String.IsNullOrEmpty(oprt))
            {
                combination = new Combination(new Literal(oprt));
                isLiteralOperator = true;
            }
            else
            {
                combination = new Combination(ReadCombination());
            }
            if (isLiteralOperator && _currentChar == ')')
            {
                return combination;
            }

            while ((_currentChar = _streamReader.Read()) != ')')
            {
                if (_whiteSpaceSeparators.Contains(_currentChar))
                {
                    continue;
                }
                if (_currentChar == -1)    //EOF
                {
                    throw new Exception("Premature end of expression.");
                }

                combination.AddOperand(GetExpression());
            }
            return combination;

        }


        private Expression ReadLiteral()
        {
            StringBuilder literalBuilder = new StringBuilder();
            bool isDoubleQuoted = false;
            if (_currentChar == '\"')
            {
                isDoubleQuoted = true;
            }
            else
            {
                literalBuilder.Append((char)_currentChar);
            }
            int nextChar = _streamReader.Peek();
            while (((!isDoubleQuoted && !_separators.Contains(nextChar)) || (isDoubleQuoted && nextChar != '\"')) && nextChar != -1)
            {
                _currentChar = _streamReader.Read();
                literalBuilder.Append((char)_currentChar);
                nextChar = _streamReader.Peek();
            }
            if (isDoubleQuoted && nextChar == '\"')
            {
                _currentChar = _streamReader.Read();
            }
            //TODO add type detection
            return new Literal(literalBuilder.ToString());
        }


        private Expression ReadVariable()
        {
            StringBuilder variableBuilder = new StringBuilder();
            variableBuilder.Append((char)_currentChar);
            int nextChar;
            while (!_separators.Contains(nextChar = _streamReader.Peek()) && nextChar != -1)
            {
                _currentChar = _streamReader.Read();
                variableBuilder.Append((char)_currentChar);
            }
            return new Variable(variableBuilder.ToString());
        }
    }
}
