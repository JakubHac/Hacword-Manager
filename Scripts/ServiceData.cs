using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hacword_Manager.Scripts
{
	[Serializable]
	public class ServiceData
	{
		public readonly string Name;
		public readonly Dictionary<string, string> Accounts;

		[JsonConstructor]
		public ServiceData(string name, Dictionary<string, string> accounts)
		{
			Name = name;
			Accounts = accounts;
		}

		public ServiceData(string name)
		{
			Name = name;
			Accounts = new Dictionary<string, string>();
		}
	}
}
