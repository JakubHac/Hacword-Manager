using EasyEncrypt2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Hacword_Manager.Scripts
{
	[System.Serializable]
	public class SaveData
	{
		private static EasyEncrypt Encrypt = new EasyEncrypt("JakubHac", "HacwordManager");

		private const string FileName = "Services.Hac";
		public static string FolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"HacwordManager");
		public static string FilePath => Path.Combine(FolderPath, FileName);

		public Dictionary<string, ServiceData> Services;

		private string Serialize()
		{
			return JsonConvert.SerializeObject(this);
		}

		[JsonConstructor]
		private SaveData(Dictionary<string, ServiceData> services)
		{
			Services = services;
		}

		private static SaveData Deserialize(string json)
		{
			SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
			return saveData;
		}

		public static Dictionary<string, ServiceData> LoadServicesFromDisk()
		{
			if (Directory.Exists(FolderPath) && File.Exists(FilePath))
			{
				string ecrypted = File.ReadAllText(FilePath);
				string json = Encrypt.Decrypt(ecrypted);
				return Deserialize(json).Services ?? new Dictionary<string, ServiceData>();
			}
			else
			{
				return new Dictionary<string, ServiceData>();
			}
		}

		public static void SaveServicesToDisk(Dictionary<string, ServiceData> services)
		{
			if (!Directory.Exists(FolderPath))
			{
				Directory.CreateDirectory(FolderPath);

				if (!Directory.Exists(FolderPath))
				{
					MessageBox.Show($"Wystąpił błąd podczas tworzenia folderu {FolderPath}", "Błąd zapisu danych");
					return;
				}
			}

			string json = new SaveData(services).Serialize();
			string encrypted= Encrypt.Encrypt(json);
			File.WriteAllText(FilePath, encrypted);
		}
	}
}
