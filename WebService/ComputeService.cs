using System;
using System.Collections.Generic;

namespace WebService
{
    public class ComputeService
    {
        public int Calculate(string expression)
        {
            Stack<int> vals = new Stack<int>();
            Stack<char> operators = new Stack<char>();
            Stack<char> operatorsinbrackets = new Stack<char>();
            Stack<int> valsinbrackets = new Stack<int>();
            char[] input = expression.ToCharArray();
            bool multiplicationinbrackets = false;
            bool multiplication = false;
            bool inbrackets = false;
            int operatorinbrackets = 0;
            int operatorsCount = 0;

            for (int i = 0; i < input.Length; i++)
            {
                var current = input[i];

                switch (current)
                {
                    case '(':
                        inbrackets = true;
                        break;
                    case '+':
                    case '-':
                        if (inbrackets)
                        {
                            operatorinbrackets++;
                            operatorsinbrackets.Push(current);
                        }
                        else
                        {
                            operatorsCount++;
                            operators.Push(current);
                        }
                        break;
                    case '*':
                    case '/':
                        if (inbrackets)
                        {
                            operatorinbrackets++;
                            operatorsinbrackets.Push(current);
                            multiplicationinbrackets = true;
                        }
                        else
                        {
                            operatorsCount++;
                            operators.Push(current);
                            multiplication = true;
                        }
                        break;
                    case ')':
                        for (int r = 0; r < operatorinbrackets; r++)
                        {
                            GetCalcinbr(operatorsinbrackets, valsinbrackets);
                        }
                        multiplicationinbrackets = false;
                        inbrackets = false;
                        operatorinbrackets = 0;
                        int valinbr = valsinbrackets.Pop();
                        vals.Push(valinbr);
                        if (multiplication)
                        {
                            operatorsCount--;
                            GetCalc(operators, vals);
                        }
                        multiplication = false;
                        break;
                    default:
                        if (inbrackets)
                        {
                            valsinbrackets.Push((int)char.GetNumericValue(current));
                            if (multiplicationinbrackets)
                            {
                                GetCalc(operatorsinbrackets, valsinbrackets);
                                operatorinbrackets--;
                                multiplicationinbrackets = false;
                            }
                        }
                        else
                        {
                            vals.Push((int)char.GetNumericValue(current));
                            if (multiplication)
                            {
                                GetCalc(operators, vals);
                                operatorsCount--;
                                multiplication = false;
                            }
                        }
                        break;

                }
            }
            for (int l = operatorsCount; l > 0; l--)
            {
                GetCalc(operators, vals);
                operatorsCount = 0;
            }
            return vals.Pop();
        }
        private static void GetCalcinbr(Stack<char> operatorsinbrackets, Stack<int> valsinbrackets)
        {
            char opinbr = operatorsinbrackets.Pop();
            if (opinbr.Equals('*'))
            {
                valsinbrackets.Push(valsinbrackets.Pop() * valsinbrackets.Pop());
            }
            if (opinbr.Equals('/'))
            {
                int vlast = valsinbrackets.Pop();
                int penult = valsinbrackets.Pop();
                valsinbrackets.Push(penult / vlast);
            }
            if (opinbr.Equals('+'))
            {
                valsinbrackets.Push(valsinbrackets.Pop() + valsinbrackets.Pop());
            }
            if (opinbr.Equals('-'))
            {
                int vlast = valsinbrackets.Pop();
                int penult = valsinbrackets.Pop();
                valsinbrackets.Push(penult - vlast);
            }
        }
        private static void GetCalc(Stack<char> operators, Stack<int> vals)
        {
            char op = operators.Pop();
            if (op.Equals('*'))
            {
                vals.Push(vals.Pop() * vals.Pop());
            }
            if (op.Equals('/'))
            {
                int vlast = vals.Pop();
                int penult = vals.Pop();
                vals.Push(penult / vlast);
            }
            if (op.Equals('+'))
            {
                vals.Push(vals.Pop() + vals.Pop());
            }
            if (op.Equals('-'))
            {
                int vlast = vals.Pop();
                int penult = vals.Pop();
                vals.Push(penult - vlast);
            }
        }
    }
}