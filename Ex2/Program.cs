namespace Ex2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var frutas = new List<Fruta>()
            {
                new Fruta { Nome = "Banana",  Acida = false, Cor = "amarelo" },
                new Fruta { Nome = "Laranja",  Acida = true, Cor = "laranja" },
                new Fruta { Nome = "Morango",  Acida = true, Cor = "vermelho" },
                new Fruta { Nome = "Roma",  Acida = true, Cor = "vermelho" },
                new Fruta { Nome = "Tamara",  Acida = false, Cor = "castanho" },
            };

            Console.WriteLine($"Escolha uma fruta ({string.Join(", ", frutas.Select(f => f.Nome))}):");
            var input = Console.ReadLine();

            var fruta = frutas.Find(fruta => fruta.Nome.ToLower() == input?.ToLower());
        
            if (fruta == null)
            {
                Console.WriteLine("Fruta não reconhecida!");
            }
            else
            {
                Console.WriteLine($"Fruta {fruta.Cor} é {(fruta.Acida ? "ácida" : "não ácida")}");
            }
        }

        class Fruta
        {
            public string Nome { get; init; } = string.Empty;
            public string Cor { get; init; } = string.Empty;
            public bool Acida { get; init; }
        }
    }
}