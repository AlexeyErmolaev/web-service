using System;
using System.Collections.Generic;
using System.Text;
using System.Web;


namespace WebService
{
    public static class ComputeService
    {
        public static double Calculate(string expression)
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
            bool firstValNegative = false;
            bool firstValInBracketsNegative = false;



            for (int i = 0; i < input.Length; i++)
            {
                var current = input[i];

             
                switch (current)
                {
                    case '(':
                        inBrackets = true;
                        break;
                    case '-':
                        if (inBrackets)
                        {
                            if (valsInBrackets.Count == 0)
                            {
                                firstValInBracketsNegative = true;

                            }
                            else
                            {
                                operatorsInBracketsCount++;
                                operatorsInBrackets.Push(current);
                            }
                        }
                        else
                        {
                            if (vals.Count == 0)
                            {
                                firstValNegative = true;

                            }
                            else
                            {
                                operatorsCount++;
                                operators.Push(current);
                            }
                        }
                        
                        
                        break;
                    case '+':
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
                            throw new FormatException($"Unexpected symbol: {current}");
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
                            if (firstValInBracketsNegative)
                            {
                                var val = valsInBrackets.Pop()*-1;
                                valsInBrackets.Push(val);
                                firstValInBracketsNegative = false;
                            }
                            else
                            {
                                if (multiplicationInBrackets)
                                {
                                    GetCalc(operatorsInBrackets, valsInBrackets);
                                    operatorsInBracketsCount--;
                                    multiplicationInBrackets = false;
                                }
                            }
                            }
                            else
                            {
                                vals.Push((int)char.GetNumericValue(current));
                                if (multiplication)
                                {
                                    if (operators.Count == 1 && vals.Count == 1)
                                    {
                                        throw new FormatException("Incorrectly entered expression");
                                    }
                                    else
                                    {
                                        if (firstValNegative)
                                        {
                                            GetCalcs(operators, vals);
                                            firstValNegative = false;
                                        }
                                        else
                                        {
                                            GetCalc(operators, vals);
                                        }
                                        operatorsCount--;
                                        multiplication = false;
                                    }
                                }
                            }
                        
                        break;

                        default:
                            throw new FormatException("Incorrectly entered expression");
                            break;
                }
            }

            for (int l = operatorsCount; l > 0; l--)
            {
                if (operators.Count == 1 && vals.Count == 1)
                {
                    var op = operators.Pop();
                    if(op == '-')
                    {
                        return vals.Pop() * -1;
                    }
                }
                else
                {
                    if (firstValNegative)
                    {
                        GetCalcs(operators, vals);
                        firstValNegative = false;
                    }
                    else
                    {
                        GetCalc(operators, vals);
                    }
                    operatorsCount = 0;
                }
            }
            if (inBrackets)
            {
                throw new FormatException("Missing ')'");
            }
            else
            {
                return vals.Pop();
            }
        }
        private static void GetCalcs(Stack<char> operators, Stack<double> vals)
        {
            char op = operators.Pop();
            double lastValue = vals.Pop();
            double res = vals.Pop()*-1;

            switch (op)
            {
                case '+':
                    vals.Push(lastValue + res);
                    break;
                case '-':
                    vals.Push(res - lastValue);
                    break;
                case '*':
                    vals.Push(lastValue * res);
                    break;
                case '/':
                    vals.Push(res / lastValue);
                    break;
                default:
                    throw new NotImplementedException("Unexpected operator's value");
            }
            lastValue = 0;
            res = 0;
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
