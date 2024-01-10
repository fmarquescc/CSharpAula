namespace GestaoAlunos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new ConsoleGui().Run();
        }
    }

    class ConsoleGui
    {
        private readonly Escola escola;

        public ConsoleGui()
        {
            escola = new Escola { Nome = "Raul Proença" };
        }

        public void AddDefaultAlunos()
        {
            escola.AdicionarAluno(new Aluno("João", DateTime.Parse("2015/01/01"), "5A06", "A", 5, 6));
            escola.AdicionarAluno(new Aluno("Rodrigo", DateTime.Parse("2014/08/12"), "6B12", "B", 6, 12));
            escola.AdicionarAluno(new Aluno("Beatriz", DateTime.Parse("2013/03/07"), "5A03", "A", 5, 3));
            escola.AdicionarAluno(new Aluno("António", DateTime.Parse("2014/02/14"), "6B02", "B", 6, 2));
        }

        public void Run()
        {
            AddDefaultAlunos();

            while (true)
            {
                int op;
                if ((op = Menu()) == 0)
                {
                    break;
                }

                Console.Clear();

                switch (op)
                {
                    case 1:
                        MenuAdicionarAluno();
                        break;
                    case 2:
                        MenuObterAlunoMatricula();
                        break;
                    case 3:
                        MenuListarAlunos();
                        break;
                    case 4:
                        MenuListarAlunosTurma();
                        break;
                }

                Console.Write("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            };
        }

        private void MenuAdicionarAluno()
        {
            Console.WriteLine("--- Menu: Adicionar Aluno ---");
            Console.Write("Nome: ");
            string nome = Console.ReadLine()!;
            DateTime datanasc;
            do
            {
                Console.Write("Data de Nascimento: ");
            }
            while (!DateTime.TryParse(Console.ReadLine(), out datanasc));

            Console.Write("Matrícula: ");
            var matricula = Console.ReadLine()!;

            Console.Write("Turma: ");
            string turma = Console.ReadLine()!;

            int ano;
            do
            {
                Console.Write("Ano: ");
            }
            while (!int.TryParse(Console.ReadLine(), out ano));

            int numero;
            do
            {
                Console.Write("Número: ");
            }
            while (!int.TryParse(Console.ReadLine(), out numero));

            try
            {
                escola.AdicionarAluno(new Aluno(nome, datanasc, matricula, turma, ano, numero));
                Console.Write("\nAluno adicionado com sucesso!");
            }
            catch (Exception e)
            {
                Console.Write("Erro: " + e.Message);
            }
        }

        private void MenuObterAlunoMatricula()
        {
            Console.WriteLine("--- Menu: Obter Aluno ---");
            Console.Write("Matrícula do Aluno: ");

            var aluno = escola[Console.ReadLine()];

            if (aluno == null)
            {
                Console.WriteLine("Nenhum aluno com a dada matrícula!");
            }
            else
            {
				Console.WriteLine("Escolha:");
				Console.WriteLine("1 - Apresentar");
				Console.WriteLine("2 - Mostrar Idade");
				Console.WriteLine("3 - Ação Desenvolvida");
				Console.WriteLine("4 - Remover Aluno");
				Console.WriteLine("0 - Sair do Menu");

				do
                {
                    int op;
                    do
                    {
                        Console.Write("> ");
                        if (!int.TryParse(Console.ReadLine(), out op) || op < 0 || op > 4)
                        {
                            Console.WriteLine("Erro: Digite um Número Válido!");
                        }
                        else
                        {
                            break;
                        }

                    } while (true);

                    if (op == 0)
                    {
                        return;
                    }

                    switch (op)
                    {
                        case 1:
                            aluno.Apresentar();
                            break;
                        case 2:
                            Console.WriteLine($"Idade: {aluno.CalcularIdade()}");
                            break;
                        case 3:
                            aluno.RealizarAcao();
                            break;
                        case 4:
                            escola.Alunos.Remove(aluno);
                            Console.WriteLine("Aluno removido!");
                            return;
                    }
                } while (true);
            }
        }

        private void MenuListarAlunos()
        {
            Console.WriteLine("--- Menu: Lista de Alunos ---");
            Console.WriteLine($"Alunos: {escola.Alunos.Count}");
            foreach (var aluno in escola.Alunos)
            {
                aluno.Apresentar();
            }
        }

        private void MenuListarAlunosTurma()
        {
            Console.WriteLine("--- Menu: Lista de Alunos De Determinada Turma ---");
            Console.Write("Turma: ");
            string turma = Console.ReadLine()!;

            escola.ListarAlunosTurma(turma);
		}

        private int Menu()
        {
            Console.Clear();
            Console.WriteLine($"--- Menu: {escola.Nome} ---");
            Console.WriteLine("1 - Adicionar Aluno");
            Console.WriteLine("2 - Obter Aluno por Matrícula");
            Console.WriteLine("3 - Listar Alunos");
            Console.WriteLine("4 - Listar Alunos de Determinada Turma");
            Console.WriteLine("0 - Sair");

            int op;
            do
            {
                Console.Write("> ");
                if (!int.TryParse(Console.ReadLine(), out op) || op < 0 || op > 4)
                {
                    Console.WriteLine("Erro: Digite um Número Válido!");
                }
                else
                {
                    break;
                }

            } while (true);

            return op;
        }
    }

    abstract class Pessoa
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }

        public Pessoa(string nome, DateTime dataNascimento)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
        }

        public virtual void Apresentar()
        {
            Console.WriteLine($"Olá, o meu nome é {this.Nome}!");
        }

        public abstract void RealizarAcao();
    }

    class Aluno : Pessoa
    {
        public string Matricula { get; set; } = string.Empty;
        public int Ano { get; set; }
        public int Numero { get; set; }
        public string Turma { get; set; } = string.Empty;

        public Aluno(string nome, DateTime dataNascimento, string matricula, string turma, int ano, int numero) : base(nome, dataNascimento)
        {
            Matricula = matricula;
            Ano = ano;
            Numero = numero;
            Turma = turma;
        }

        public int CalcularIdade()
        {
            var now = DateTime.Now;
			var age = now.Year - DataNascimento.Year;
            if (now.Month < DataNascimento.Month || (now.Month == DataNascimento.Month && now.Day < DataNascimento.Day))
            {
                age--;
            }
            return age;
        }

        public override void RealizarAcao()
        {
            Console.WriteLine("Estou na escola a estudar!");
        }

        public override void Apresentar()
        {
            Console.WriteLine($"Nome: {this.Nome} Matrícula: {this.Matricula} Turma: {this.Turma} Ano: {this.Ano} Número: {this.Numero}");
        }
    }

    class Escola
    {
        public string Nome { get; init; } = string.Empty;
        public List<Aluno> Alunos = new();

        public Aluno? this[string? matricula] {
            get
            {
                return Alunos.Find(aluno => aluno.Matricula == matricula);
            }
        }

        public List<Aluno> GetAlunosPorTurma(string turma)
        {
            return this.Alunos.Where(aluno => aluno.Turma == turma).ToList();
        }

		public void ListarAlunosTurma(string turma)
		{
            var alunos = GetAlunosPorTurma(turma);
			foreach (var aluno in alunos)
			{
				aluno.Apresentar();
			}
		}

		public void AdicionarAluno(Aluno aluno)
        {
            if (this[aluno.Matricula] == null)
            {
                this.Alunos.Add(aluno);
            }
            else
            {
                throw new InvalidOperationException("Já existe um aluno com esta matrícula na escola.");
            }
        }
    }
}