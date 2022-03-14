namespace Hacword_Manager.Scripts
{
	public class AccountData
	{
		public readonly string Login;
		public readonly string Password;
		public AccountData(string login, string password)
		{
			Login = login;
			Password = password;
		}

		public static readonly AccountData[] EmptyArray = new AccountData[0];
	}
}
