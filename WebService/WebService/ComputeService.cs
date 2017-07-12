using System;
using System.Collections.Generic;

namespace WebService
{
    public static class ComputeService
    {
        public static string Calculate(string expression)
        {
            Stack<double> vals = new Stack<double>();
            Stack<char> operators = new Stack<char>();
            Stack<char> operatorsInBrackets = new Stack<char>();
            Stack<double> valsInBrackets = new Stack<double>();
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
                        if(!inBrackets)
                        {
                            return ("Expected '(' ");
                        }
                        for (int r = 0; r < operatorsInBracketsCount; r++)
                        {
                            GetCalc(operatorsInBrackets, valsInBrackets);
                        }
                        multiplicationInBrackets = false;
                        inBrackets = false;
                        operatorsInBracketsCount = 0;
                        double valInbr = valsInBrackets.Pop();
                        vals.Push(valInbr);
                        if (multiplication)
                        {
                            operatorsCount--;
                            GetCalc(operators, vals);
                        }
                        multiplication = false;
                        break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
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
                                if (operators.Count == 1 && vals.Count == 1)
                                {
                                    return ("Incorrectly entered expression");
                                }
                                else
                                {
                                    GetCalc(operators, vals);
                                    operatorsCount--;
                                    multiplication = false;
                                }
                            }
                        }
                        break;

                    default:
                        return ("Incorrectly entered expression");
                        break;
                }
            }

            for (int l = operatorsCount; l > 0; l--)
            {
                if (operators.Count == 1 && vals.Count == 1)
                {
                    return (Convert.ToString(operators.Pop()) + Convert.ToString(vals.Pop()));
                }
                else
                {
                    GetCalc(operators, vals);
                    operatorsCount = 0;
                }
            }
            if (inBrackets)
            {
                return ("Expected')'");
            }
            else
            {
                return Convert.ToString(vals.Pop());
            }
        }

        private static void GetCalc(Stack<char> operators, Stack<double> vals)
        {
            char op = operators.Pop();
            double lastValue = vals.Pop();
            double penultValue = vals.Pop();
            
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
            lastValue = 0;
            penultValue = 0;
        }
    }
}
