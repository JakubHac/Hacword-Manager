using Hacword_Manager.Scripts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hacword_Manager
{
	public partial class ServicePage : Page, INotifyPropertyChanged
	{
		public string SelectedService
		{
			get
			{
				return ServicesManager.SelectedService;
			}
			set
			{

			}
		}

		private ObservableCollection<AccountDisplay> _accounts = new ObservableCollection<AccountDisplay>();

		public ObservableCollection<AccountDisplay> Accounts
		{
			get
			{
				return _accounts;
			}
			set
			{

			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ServicePage()
		{
			InitializeComponent();
			this.DataContext = this;
			RefreshAccounts();
		}

		private void ExitServiceDetailsButton_Click(object sender, RoutedEventArgs e) => App.ReturnToMainPage();

		private void NewAccountButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new NewAccountDialog(SelectedService, this) { Owner = Application.Current.MainWindow};
			dialog.ShowDialog();
		}

		private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
		{
			ServicesManager.RemoveService(SelectedService);
			App.ReturnToMainPage();
		}

		private void CopyLoginButton_Click(object sender, RoutedEventArgs e)
		{
			AccountDisplay account = GetAccountFromButton(sender);
			Clipboard.SetText(account.Login);
		}

		private void CopyPasswordButton_Click(object sender, RoutedEventArgs e)
		{
			AccountDisplay account = GetAccountFromButton(sender);
			Clipboard.SetText(account.Data.Password);
		}

		private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
		{
			AccountDisplay account = GetAccountFromButton(sender);
			account.TogglePasswordVisibility();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_accounts)));
		}

		private void EditAccountButton_Click(object sender, RoutedEventArgs e)
		{
			Grid loginGrid = GetLoginGridFromButton(sender);
			TextBlock readonlyLogin = loginGrid.Children[0] as TextBlock;
			TextBox writeEnabledLogin = loginGrid.Children[1] as TextBox;
			Grid passwordGrid = GetPasswordGridFromButton(sender);
			TextBlock readonlyPassword = passwordGrid.Children[0] as TextBlock;
			TextBox writeEnabledPassword = passwordGrid.Children[1] as TextBox;
			AccountData oldData = GetAccountFromButton(sender).Data;

			if (double.IsPositiveInfinity(readonlyLogin.MaxWidth))
			{
				writeEnabledLogin.Text = oldData.Login;
				writeEnabledPassword.Text = oldData.Password;
				readonlyLogin.MaxWidth = 0;
				writeEnabledLogin.MaxWidth = double.PositiveInfinity;
				readonlyPassword.MaxWidth = 0;
				writeEnabledPassword.MaxWidth = double.PositiveInfinity;
			}
			else
			{
				AccountData newData = new AccountData(writeEnabledLogin.Text, writeEnabledPassword.Text);
				if (string.IsNullOrWhiteSpace(newData.Login))
				{
					MessageBox.Show("Nowy Login Jest Pusty", "Nieprawidłowe dane");
					return;
				}

				if (string.IsNullOrWhiteSpace(newData.Password))
				{
					MessageBox.Show("Nowe Hasło Jest Puste", "Nieprawidłowe dane");
					return;
				}

				if (oldData.Login != newData.Login && ServicesManager.AccountExists(SelectedService, newData.Login))
				{
					MessageBox.Show($"Konto {newData.Login} już istnieje w {SelectedService}", "Nieprawidłowe dane");
					return;
				}

				bool changedAnything = false;

				if (oldData.Password != newData.Password)
				{
					ServicesManager.ChangePassword(SelectedService, oldData.Login, newData.Password);
					changedAnything = true;
				}

				if (oldData.Login != newData.Login)
				{
					ServicesManager.ChangeLogin(SelectedService, oldData.Login, newData.Login);
					changedAnything = true;
				}

				readonlyLogin.MaxWidth = double.PositiveInfinity;
				writeEnabledLogin.MaxWidth = 0;
				readonlyPassword.MaxWidth = double.PositiveInfinity;
				writeEnabledPassword.MaxWidth = 0;

				if (changedAnything)
				{
					RefreshAccounts();
				}
			}
		}

		private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
		{
			AccountDisplay account = GetAccountFromButton(sender);
			_accounts.Remove(account);
			ServicesManager.RemoveAccountFromService(SelectedService, account.Login);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_accounts)));
		}

		public void RefreshAccounts()
		{
			_accounts.Clear();

			var accounts = ServicesManager.GetAccountsInService(SelectedService).Select(x => new AccountDisplay(x)).ToArray();

			foreach (var account in accounts)
			{
				_accounts.Add(account);
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_accounts)));
		}

		private AccountDisplay GetAccountFromButton(object sender)
		{
			Grid loginGrid = GetLoginGridFromButton(sender);
			TextBlock loginText = loginGrid.Children[0] as TextBlock;
			string login = loginText.Text;
			AccountDisplay account = _accounts.First(x => x.Login == login);
			return account;
		}

		private static Grid GetLoginGridFromButton(object sender)
		{
			Grid accountGrid = GetAccountGridFromButton(sender);
			Button loginButton = accountGrid.Children[0] as Button;
			Grid loginGrid = loginButton.Content as Grid;
			return loginGrid;
		}

		private static Grid GetPasswordGridFromButton(object sender)
		{
			Grid accountGrid = GetAccountGridFromButton(sender);
			Button passwordButton = accountGrid.Children[1] as Button;
			Grid passwordGrid = passwordButton.Content as Grid;
			return passwordGrid;
		}

		private static Grid GetAccountGridFromButton(object sender)
		{
			Button button = sender as Button;
			Grid accountGrid = button.Parent as Grid;
			return accountGrid;
		}
	}
}
