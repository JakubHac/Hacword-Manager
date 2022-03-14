using System.Windows;
using System.Windows.Controls;

namespace Hacword_Manager
{
	public partial class MainWindow : Window
	{
		public static MainWindow Instance;

		public MainWindow()
		{
			Instance = this;
			InitializeComponent();
			Page main = new MainPage();
			this.Content = main;
		}
	}
}
