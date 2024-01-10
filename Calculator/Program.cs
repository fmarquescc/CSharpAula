using System;

namespace Calculator {
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                int op = Menu();

                if (op == 0)
                {
                    return;
                }

                double value1 = 0, value2 = 0;
                AskValues(ref value1, ref value2);

                switch (op)
                {
                    case 1:
                        Console.WriteLine($"Resultado: {value1} + {value2} = {value1 + value2}");
                        break;
                    case 2:
                        Console.WriteLine($"Resultado: {value1} - {value2} = {value1 - value2}");
                        break;
                    case 3:
                        Console.WriteLine($"Resultado: {value1} * {value2} = {value1 * value2}");
                        break;
                    case 4:
                        if (value2 == 0)
                        {
                            Console.WriteLine("Não podes dividir por zero!");
                        }
                        else
                        {
                            Console.WriteLine($"Resultado: {value1} / {value2} = {value1 / value2}");
                        }
                        break;
                    case 5:
                        Console.WriteLine($"Resultado: {value1} % {value2} = {value1 % value2}");
                        break;
                    case 6:
                        Console.WriteLine($"Resultado: {value1} ^ {value2} = {Math.Pow(value1, value2)}");
                        break;
                }

                Console.ReadKey();
            }
        }

        static int Menu()
        {
            Console.WriteLine("--- Calculator ---");
            Console.WriteLine("1 - Somar +");
            Console.WriteLine("2 - Subtrair -");
            Console.WriteLine("3 - Multiplicar *");
            Console.WriteLine("4 - Dividir /");
            Console.WriteLine("5 - Resto da divisão %");
            Console.WriteLine("6 - Potência ^");
            Console.WriteLine("0 - Sair");

            do
            {
                Console.Write("> ");
                if (!int.TryParse(Console.ReadLine(), out int op) || op < 0 || op > 6)
                {
                    Console.WriteLine("Erro: Opção inválida!");
                }
                else
                {
                    return op;
                }
            }
            while (true);
        }

        static void AskValues(ref double value1, ref double value2)
        {
            do
            {
                Console.Write("Value 1: ");
            }
            while (!double.TryParse(Console.ReadLine(), out value1));
            do
            {
                Console.Write("Value 2: ");
            }
            while (!double.TryParse(Console.ReadLine(), out value2));
        }
    }
}