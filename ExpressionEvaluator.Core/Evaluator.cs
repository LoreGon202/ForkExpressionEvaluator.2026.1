namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        Console.WriteLine("Postfix: " + postfix); // Agregamos esta línea para mostrar la expresión en postfix
        return EvaluatePostfix(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var postFix = string.Empty;
        var stack = new Stack<char>();

        for (int i = 0; i < infix.Length; i++)
        {
            char item = infix[i];

            // Manejo de números (incluye decimales)
            if (char.IsDigit(item) || item == '.')
            {
                while (i < infix.Length && (char.IsDigit(infix[i]) || infix[i] == '.'))
                {
                    postFix += infix[i];
                    i++;
                }

                postFix += " ";
                i--;
            }
            else if (IsOperator(item))
            {
                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ')')
                    {
                        while (stack.Peek() != '(')
                        {
                            postFix += stack.Pop() + " ";
                        }
                        stack.Pop();
                    }
                    else
                    {
                        if (item == '(')
                        {
                            stack.Push(item);
                        }
                        else if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            postFix += stack.Pop() + " ";
                            stack.Push(item);
                        }
                    }
                }
            }
        }

        while (stack.Count > 0)
        {
            postFix += stack.Pop() + " ";
        }

        return postFix;
    }

    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static double EvaluatePostfix(string postfix)
    {
        var stack = new Stack<double>();
        var tokens = postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var token in tokens)
        {
            if (token.Length == 1 && IsOperator(token[0]))
            {
                var b = stack.Pop();
                var a = stack.Pop();

                stack.Push(token[0] switch
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    '^' => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                stack.Push(double.Parse(token, System.Globalization.CultureInfo.InvariantCulture)); //agregamos InvariantCulture para asegurar que el punto decimal se interprete correctamente
            }
        }

        return stack.Pop();
    }
    private static bool IsOperator(char item) => "+-*/^()".Contains(item);
}