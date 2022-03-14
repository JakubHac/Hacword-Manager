using System.Collections.Generic;
using System.Linq;

namespace Hacword_Manager.Scripts
{
	class ServicesManager
	{

		private static readonly Dictionary<string, ServiceData> Services = new Dictionary<string, ServiceData>();

		public static void SetServices(Dictionary<string, ServiceData> services)
		{
			Services.Clear();
			foreach (var serivce in services)
			{
				Services.Add(serivce.Key, serivce.Value);
			}
		}

		public static Dictionary<string, ServiceData> GetServicesCopy()
		{
			Dictionary<string, ServiceData> copy = new Dictionary<string, ServiceData>();
			foreach (var serivce in Services)
			{
				copy.Add(serivce.Key, serivce.Value);
			}
			return copy;
		}

		public static string SelectedService { get; private set; }

		public static string[] AvailableServices => Services.Keys.ToArray();

		public static bool AddNewEmptyService(string serviceName)
		{
			if (string.IsNullOrWhiteSpace(serviceName) || ServiceExists(serviceName)) return false;

			Services.Add(serviceName, new ServiceData(serviceName));
			return true;
		}

		public static bool AddNewAccountToService(string serviceName, string login, string password)
		{
			string[] input = { serviceName, login, password };
			if (input.Any(x => string.IsNullOrWhiteSpace(x)) 
				|| AccountExists(serviceName, login)) return false;

			Services[serviceName].Accounts.Add(login, password);
			return true;
		}

		public static void OpenService(string serviceName)
		{
			if (!ServiceExists(serviceName)) return;
			
			SelectedService = serviceName;
			MainWindow.Instance.Content = new ServicePage();
		}

		public static bool AccountExists(string serviceName, string login)
		{
			return ServiceExists(serviceName) && Services[serviceName].Accounts.ContainsKey(login);
		}

		public static bool ServiceExists(string serviceName) => Services.ContainsKey(serviceName);

		public bool TryGetServiceData(string serviceName, out ServiceData serviceData)
		{
			if (ServiceExists(serviceName))
			{
				serviceData = Services[serviceName];
				return true;
			}
			serviceData = null;
			return false;
		}

		public static AccountData[] GetAccountsInService(string serviceName)
		{
			return ServiceExists(serviceName)
				? Services[serviceName].Accounts.Select(x => new AccountData(x.Key, x.Value)).ToArray()
				: AccountData.EmptyArray;
		}

		internal static void RemoveAccountFromService(string selectedService, string login)
		{
			if (AccountExists(selectedService,login))
			{
				Services[selectedService].Accounts.Remove(login);
			}
		}

		internal static void RemoveService(string selectedService)
		{
			if (Services.ContainsKey(selectedService))
			{
				Services.Remove(selectedService);
			}
		}

		internal static void ChangePassword(string selectedService, string login, string password)
		{
			if (AccountExists(selectedService, login))
			{
				Services[selectedService].Accounts[login] = password;
			}
		}

		internal static void ChangeLogin(string selectedService, string oldLogin, string newLogin)
		{
			if (AccountExists(selectedService, oldLogin) && !AccountExists(selectedService, newLogin))
			{
				string oldPassword = Services[selectedService].Accounts[oldLogin];
				RemoveAccountFromService(selectedService, oldLogin);
				AddNewAccountToService(selectedService, newLogin, oldPassword);
			}
		}
	}
}
