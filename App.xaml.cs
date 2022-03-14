using Hacword_Manager.Scripts;
using System.Windows;

namespace Hacword_Manager
{
	public partial class App : Application
	{
		private void Start(object sender, StartupEventArgs s)
		{
			var LoadedServices = SaveData.LoadServicesFromDisk();
			ServicesManager.SetServices(LoadedServices);
		}

		private void Shutdown(object sender, ExitEventArgs e)
		{
			SaveData.SaveServicesToDisk(ServicesManager.GetServicesCopy());
		}


		public static void ReturnToMainPage()
		{
			Hacword_Manager.MainWindow.Instance.Content = new MainPage();
		}
	}

	
}
