using MySql.Data;
using MySql.Data.MySqlClient;
using MARC4J.Net;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Collections;

namespace ConsoleApp1
{
	internal class Program
	{
		static bool logged;

		static void MainA(string[] args)
		{
			using (var fs = new FileStream(@"C:\Users\fmarques\Desktop\fm\csharp\CSharpAula\ConsoleApp1\testeExportacao.iso", FileMode.Open))
			{
				using (IMarcReader reader = new MarcStreamReader(fs))
				{
					foreach (var record in reader)
					{
						Console.WriteLine(record.ToString());
						break;
					}
				}
			}
		}

		static MySqlConnection connection;

		static void Main(string[] args)
		{
			connection = new MySqlConnection("Server=localhost; database=bd_teste_bb; UID=root; password=");

			Console.WriteLine("Connecting to database...");

			try
			{
				connection.Open();
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to connect to database!" );
				return;
			}

			Console.WriteLine("\nBiblioteca\n");

			Menu();

			connection.Close();
		}

		static string ReadInput()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("> ");
			Console.ForegroundColor = ConsoleColor.White;
			var cmd = Console.ReadLine()!;
			Console.ForegroundColor = ConsoleColor.Gray;
			return cmd;
		}

		static void Menu()
		{
			do
			{
				if (!logged)
				{
					Console.WriteLine("Username: ");
					var user = ReadInput();
					Console.WriteLine("Password: ");
					var pass = ReadInput();

					bool firstTime = !File.Exists("biblio.dat");

					if (user == "admin" && pass == (firstTime ? "admin" : File.ReadAllText("biblio.dat")))
					{
						Console.WriteLine("Logged successfully");
						if (firstTime)
						{
							Console.WriteLine("Please change the password!");
						}
						logged = true;
					}
					else
					{
						Console.WriteLine("Username or password is wrong!");
					}

					continue;
				}

				var cmd = ReadInput();

				if (cmd == "exit")
				{
					return;
				}
				else if (cmd == "help")
				{
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.WriteLine(" - help");
					Console.WriteLine(" - search");
					Console.WriteLine(" - show users");
					Console.WriteLine(" - show user");
					Console.WriteLine(" - register user");
					Console.WriteLine(" - changepass");
					Console.WriteLine(" - exit");
					Console.ForegroundColor = ConsoleColor.Gray;
				}
				else if (cmd == "changepass")
				{
					Console.WriteLine("New Password:");
					var newPass = ReadInput();
					Console.WriteLine("Confirm New Passowrd:");
					var confNewPass = ReadInput();

					if (newPass == confNewPass)
					{
						File.WriteAllText("biblio.dat", newPass);
						Console.WriteLine("Password changed successfully!");
					}
					else
					{
						Console.WriteLine("Passwords are not equal! Cancelling password change!");
					}
				}
				else if (cmd == "show users")
				{
					Console.WriteLine("Limit:");
					var limit = Convert.ToInt32(ReadInput());

					var query = "SELECT id_user, nome, nif FROM tb_users";

					Console.WriteLine($"--");

					var sqlcmd = new MySqlCommand(query, connection);
					var sqlreader = sqlcmd.ExecuteReader();
					int idx = 0;
					while (sqlreader.Read())
					{
						if (idx >= limit)
						{
							break;
						}
						for (int i = 0; i < sqlreader.FieldCount; i++)
						{
							Console.Write(sqlreader.GetValue(i) + " ");
						}
						Console.WriteLine();
						idx++;
					}

					sqlreader.Close();
				
					Console.WriteLine($"--");
				}
				else if (cmd == "show user")
				{
					Console.WriteLine("User Id:");
					var userId = Convert.ToInt32(ReadInput());

					Console.WriteLine($"--");

					Console.WriteLine($"Name: {Convert.ToString(SqlExecuteScalar($"SELECT nome FROM tb_users WHERE id_user = '{userId}'"))}");
					Console.WriteLine($"CC: {Convert.ToString(SqlExecuteScalar($"SELECT cc FROM tb_users WHERE id_user = '{userId}'"))}");
					Console.WriteLine($"NIF: {Convert.ToString(SqlExecuteScalar($"SELECT nif FROM tb_users WHERE id_user = '{userId}'"))}");

					{
						var telefones = new List<string>();
						var query = $"SELECT telefone FROM tb_telefones WHERE id_user = '{userId}'";
						var sqlcmd = new MySqlCommand(query, connection);
						var sqlreader = sqlcmd.ExecuteReader();
						while (sqlreader.Read())
						{
							telefones.Add(sqlreader.GetString(0));
						}

						if (telefones.Count > 0)
						{
							Console.WriteLine($"Phone Numbers ({telefones.Count}):");
							foreach (var telefone in telefones)
							{
								Console.WriteLine("	" + telefone);
							}
						}

						sqlreader.Close();
					}

					Console.WriteLine($"--");
				}

				else if (cmd == "edit user")
				{
					Console.WriteLine("User Id:");
					var userId = Convert.ToInt32(ReadInput());

					Console.WriteLine("Name (Leave blank to skip):");
					var nome = ReadInput();

					Console.WriteLine("CC (Leave blank to skip to next property):");
					var cc = ReadInput();

					Console.WriteLine("NIF (Leave blank to skip to next property):");
					var nif = ReadInput();

					Console.WriteLine("Phone Numbers (Leave blank to skip to next property/Prepend with r# to remove):");
					var telefoneAdd = new List<string>();
					var telefoneRem = new List<string>();
					do
					{
						var telefone = ReadInput();

						if (telefone.Length == 0)
						{
							break;
						}
						else if (telefone.StartsWith("r#"))
						{
							telefoneRem.Add(telefone.Substring(2));
						}
						else
						{
							telefoneAdd.Add(telefone);
						}
					} while (true);

					Console.WriteLine("Emails (Leave blank to skip to next property/Prepend with r# to remove):");
					var emailAdd = new List<string>();
					var emailRem = new List<string>();
					do
					{
						var email = ReadInput();

						if (email.Length == 0)
						{
							break;
						}
						else if (email.StartsWith("r#"))
						{
							emailRem.Add(email.Substring(2));
						}
						else
						{
							emailAdd.Add(email);
						}
					} while (true);

					if (nome.Length > 0)
					{
						SqlExecuteNonQuery($"UPDATE tb_users SET nome = '{nome}' WHERE id_user = '{userId}'");
					}

					if (cc.Length > 0)
					{
						SqlExecuteNonQuery($"UPDATE tb_users SET cc = '{cc}' WHERE id_user = '{userId}'");
					}

					if (nif.Length > 0)
					{
						SqlExecuteNonQuery($"UPDATE tb_users SET nif = '{nif}' WHERE id_user = '{userId}'");
					}

					foreach (var telefone in telefoneAdd)
					{
						SqlExecuteNonQuery($"INSERT INTO tb_telefones (id_user, telefone) VALUES ('{userId}', '{telefone}')");
					}

					foreach (var telefone in telefoneRem)
					{
						SqlExecuteNonQuery($"DELETE FROM tb_telefones WHERE id_user = '{userId}' AND telefone = '{telefone}'");
					}

					foreach (var email in emailAdd)
					{
						SqlExecuteNonQuery($"INSERT INTO tb_emails (id_user, email) VALUES ('{userId}', '{email}')");
					}

					foreach (var email in emailRem)
					{
						SqlExecuteNonQuery($"DELETE FROM tb_emails WHERE id_user = '{userId}' AND email = '{email}'");
					}

					Console.WriteLine("Edit finished successfully!");
				}
				else if (cmd == "register user")
				{
					Console.WriteLine("Name:");
					var nome = ReadInput();
					Console.WriteLine("CC:");
					var cc = ReadInput();
					Console.WriteLine("NIF:");
					var nif = ReadInput();
					Console.WriteLine("Birth Date:");
					var birthDate = DateTime.Parse(ReadInput());
					Console.WriteLine("Type of Reader:");
					var typeOfReader = ReadInput();

					var query = $"INSERT INTO tb_users (nome, cc, nif, data_nasc, tipo_leitor) VALUES ('{nome}', '{cc}', '{nif}', '{birthDate.Year}-{birthDate.Month}-{birthDate.Day}', '{typeOfReader}')";
					var sqlcmd = new MySqlCommand(query, connection);
					sqlcmd.ExecuteNonQuery();

					query = $"SELECT id_user FROM tb_users WHERE nome LIKE {nome}";
					sqlcmd = new MySqlCommand(query, connection);
					var userId = Convert.ToInt32(sqlcmd.ExecuteScalar());

					Console.WriteLine("User registered successfully!");
				}
				else if (cmd == "search mode=tag")
				{
					Console.WriteLine("Tag:");
					var tag = ReadInput();
					Console.WriteLine("Limit:");
					var limit = int.Parse(ReadInput());

					if (tag.Length == 0)
					{
						var query = "SELECT, descricao FROM tb_cod_cabecalho";

						var sqlcmd = new MySqlCommand(query, connection);
						var sqlreader = sqlcmd.ExecuteReader();
						int idx = 0;
						while (sqlreader.Read())
						{
							if (idx >= limit)
							{
								break;
							}
							for (int i = 0; i < sqlreader.FieldCount; i++)
							{
								Console.Write(sqlreader.GetValue(i) + " ");
							}
							Console.WriteLine();
							idx++;
						}

						sqlreader.Close();
					}
					else
					{
						var query = $"SELECT sec_cod, descricao FROM tb_sec_cod WHERE cod LIKE '{tag}'";
						var sqlcmd = new MySqlCommand(query, connection);
						var sqlreader = sqlcmd.ExecuteReader();
						int idx = 0;
						while (sqlreader.Read())
						{
							if (idx >= limit)
							{
								break;
							}
							for (int i = 0; i < sqlreader.FieldCount; i++)
							{
								Console.Write(sqlreader.GetValue(i) + " ");
							}
							Console.WriteLine();
							idx++;
						}

						sqlreader.Close();
					}
				}
				else if (cmd == "search")
				{
					Console.WriteLine("Table:");
					var table = ReadInput();
					Console.WriteLine("Pesquisa:");
					var term = ReadInput();
					Console.WriteLine("Column:");
					var column = ReadInput();
					Console.WriteLine("Limit:");
					var limit = Convert.ToInt32(ReadInput());

					var query = $"SELECT {(column.Length == 0 ? '*' : column)} FROM {table}{(term.Length != 0 && column.Length != 0 ? " WHERE " + column + " LIKE '%" + term + "%'" : "")}";
					var sqlcmd = new MySqlCommand(query, connection);
					var sqlreader = sqlcmd.ExecuteReader();
					int idx = 0;
					while (sqlreader.Read())
					{
						if (idx >= limit)
						{
							break;
						}
						for (int i = 0; i < sqlreader.FieldCount; i++)
						{
							Console.Write(sqlreader.GetValue(i) + " ");
						}
						Console.WriteLine();
						idx++;
					}

					sqlreader.Close();
				}
			}
			while (true);
		}

		static void SqlExecuteNonQuery(string query)
		{
			var sqlcmd = new MySqlCommand(query, connection);
			sqlcmd.ExecuteNonQuery();
		}

		static object SqlExecuteScalar(string query)
		{
			var sqlcmd = new MySqlCommand(query, connection);
			return sqlcmd.ExecuteScalar();
		}
	}
}
