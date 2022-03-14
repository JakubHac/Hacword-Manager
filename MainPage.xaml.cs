using Hacword_Manager.Scripts;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Hacword_Manager
{
	public partial class MainPage : Page, INotifyPropertyChanged
	{
		public string[] AvailableServices
		{
			get
			{
				return ServicesManager.AvailableServices;
			}
			set
			{

			}
		}
		

		public MainPage()
		{
			InitializeComponent();
			this.DataContext = this;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailableServices)));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void CreateServiceButton_Click(object sender, RoutedEventArgs e)
		{
			string serviceName = CreateServiceText.Text;
			if (string.IsNullOrWhiteSpace(serviceName))
			{
				MessageBox.Show("Nazwa Serwisu Nie Może Być Pusta", "Nieprawidłowe dane");
			}

			if (ServicesManager.ServiceExists(serviceName) || ServicesManager.AddNewEmptyService(serviceName))
			{
				ServicesManager.OpenService(serviceName);
			}
		}

		private void OpenServiceButton_Click(object sender, RoutedEventArgs e)
		{
			Button serviceButton = sender as Button;

			TextBlock serviceText = serviceButton.Content as TextBlock;

			ServicesManager.OpenService(serviceText.Text);
		}
	}
}
