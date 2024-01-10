using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public List<SearchItem> Bruv { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			Bruv = new List<SearchItem>()
			{
				new ("Ze Manel", 21),
				new ("Trump", 71),
				new ("Hitler", 51)
			};

			myTable.ItemsSource = Bruv;
			Sql
		}
	}

	public class SearchItem
	{
		public string Name { get; set; }
		public int Age { get; set; }

		public SearchItem(string name, int age)
		{
			Name = name;
			Age = age;
		}
	}
}