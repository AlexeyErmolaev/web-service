using System;
using System.Collections.Generic;

namespace WebService
{
    public static class ComputeService
    {
        public static int Calculate(string expression)
        {
            Stack<int> vals = new Stack<int>();
            Stack<char> operators = new Stack<char>();
            Stack<char> operatorsInBrackets = new Stack<char>();
            Stack<int> valsInBrackets = new Stack<int>();
            char[] input = expression.ToCharArray();
            bool multiplicationInBrackets = false;
            bool multiplication = false;
            bool inBrackets = false;
            int operatorsInBracketsCount = 0;
            int operatorsCount = 0;

            for (int i = 0; i < input.Length; i++)
            {
                var current = input[i];

                switch (current)
                {
                    case '(':
                        inBrackets = true;
                        break;
                    case '+':
                    case '-':
                        if (inBrackets)
                        {
                            operatorsInBracketsCount++;
                            operatorsInBrackets.Push(current);
                        }
                        else
                        {
                            operatorsCount++;
                            operators.Push(current);
                        }
                        break;
                    case '*':
                    case '/':
                        if (inBrackets)
                        {
                            operatorsInBracketsCount++;
                            operatorsInBrackets.Push(current);
                            multiplicationInBrackets = true;
                        }
                        else
                        {
                            operatorsCount++;
                            operators.Push(current);
                            multiplication = true;
                        }
                        break;
                    case ')':
                        for (int r = 0; r < operatorsInBracketsCount; r++)
                        {
                            GetCalc(operatorsInBrackets, valsInBrackets);
                        }
                        multiplicationInBrackets = false;
                        inBrackets = false;
                        operatorsInBracketsCount = 0;
                        int valinbr = valsInBrackets.Pop();
                        vals.Push(valinbr);
                        if (multiplication)
                        {
                            operatorsCount--;
                            GetCalc(operators, vals);
                        }
                        multiplication = false;
                        break;
                    default:
                        if (inBrackets)
                        {
                            valsInBrackets.Push((int)char.GetNumericValue(current));
                            if (multiplicationInBrackets)
                            {
                                GetCalc(operatorsInBrackets, valsInBrackets);
                                operatorsInBracketsCount--;
                                multiplicationInBrackets = false;
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

        private static void GetCalc(Stack<char> operators, Stack<int> vals)
        {
            char op = operators.Pop();
            int lastValue = vals.Pop();
            int penultValue = vals.Pop();
            switch (op)
            {
                case '+':
                    vals.Push(lastValue + penultValue);
                    break;
                case '-':
                    vals.Push(penultValue - lastValue);
                    break;
                case '*':
                    vals.Push(lastValue * penultValue);
                    break;
                case '/':
                    vals.Push(penultValue / lastValue);
                    break;
                default:
                    throw new NotImplementedException("Unexpected operator's value");
            }
        }
    }
}