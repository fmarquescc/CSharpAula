namespace Ex4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Write("Insira um número par (ou ímpar para sair): ");
                if (!double.TryParse(Console.ReadLine(), out var value))
                {
                    Console.WriteLine("Erro: escreva um número válido");
                    continue;
                }

                if (value % 2 != 0)
                {
                    Console.WriteLine("Número ímpar inserido. A sair do programa.");
                    break;
                }
                
                Console.WriteLine("Número par inserido. Continue a inserir números pares.");
            } while (true);
        }
    }
}