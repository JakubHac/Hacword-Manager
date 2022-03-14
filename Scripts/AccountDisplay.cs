using System.ComponentModel;

namespace Hacword_Manager.Scripts
{
	public class AccountDisplay: INotifyPropertyChanged
	{
		public bool PasswordVisible { get; private set; } = false;
		public string Login => Data.Login;
		public string Password => PasswordVisible ? Data.Password : "********";

		public readonly AccountData Data;

		public event PropertyChangedEventHandler PropertyChanged;

		public void TogglePasswordVisibility()
		{
			PasswordVisible = !PasswordVisible;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
		}

		public AccountDisplay(AccountData data)
		{
			Data = data;
		}
	}
}
