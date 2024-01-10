namespace Ex1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double num;
            
            do
            {
                Console.Write("Escreva um número: ");

                if (double.TryParse(Console.ReadLine(), out num))
                {
                    break;
                }
                Console.WriteLine("Erro: escreva um número válido!");
            }  while (true);
            
            
            Console.WriteLine(num < 0
                    ? "O número é negativo"
                    : num == 0 ? "O número é nulo" : "O número é positivo");
        }
    }
}