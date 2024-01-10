namespace Ex3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int value, limite;

            do
            {
                Console.Write("Insira o número para a tabuada: ");
            } while (!int.TryParse(Console.ReadLine(), out value));

            do
            {
                Console.Write("Insira o limite para a tabuada: ");
            } while (!int.TryParse(Console.ReadLine(), out limite));

            Console.WriteLine($"Tabuada do {value} até {limite}");

            if (limite > 0)
            {
                for (int i = 0; i <= limite; i++)
                {
                    Console.WriteLine($"{value} x {i} = {value * i}");
                }
            }
            else
            {
                for (int i = 0; i >= limite; i--)
                {
                    Console.WriteLine($"{value} x {i} = {value * i}");
                }
            }
        }
    }
}