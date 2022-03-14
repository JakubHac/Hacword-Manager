using Hacword_Manager.Scripts;
using System.Windows;

namespace Hacword_Manager
{
	public partial class NewAccountDialog : Window
	{

		private readonly string _service;
		private readonly ServicePage _servicePage;

		public NewAccountDialog(string service, ServicePage servicePage)
		{
			this.InitializeComponent();
			_service = service;
			_servicePage = servicePage;
			this.DataContext = this;
		}

		private void NewAccountDialog_CreateButtonClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(LoginBox.Text))
			{
				MessageBox.Show("Login nie może być pusty", "Nieprawidłowe dane");
				return;
			}

			if (string.IsNullOrWhiteSpace(PasswordBox.Text))
			{
				MessageBox.Show("Hasło nie może być puste", "Nieprawidłowe dane");
				return;
			}

			if (ServicesManager.AccountExists(_service, LoginBox.Text))
			{
				MessageBox.Show($"Konto {LoginBox.Text} już istnieje w {_service}", "Nieprawidłowe dane");
				return;
			}

			if (ServicesManager.AddNewAccountToService(_service, LoginBox.Text, PasswordBox.Text))
			{
				_servicePage.RefreshAccounts();
				this.Close();
			}
			else
			{
				MessageBox.Show("Wystąpił błąd podczas tworzenia konta", "Nieznany błąd");
			}
		}

		private void NewAccountDialog_CancelButtonClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
