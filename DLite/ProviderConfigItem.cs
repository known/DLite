using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Known.DLite
{
	public class ProviderConfigItem
	{
		public string Name { get; set; }
		public string Assembly { get; set; }
		public string Provider { get; set; }
		public string ConnString { get; set; }

		public ProviderConfigItem() { }

		public ProviderConfigItem(string name, string assembly, string provider, string connString)
		{
			Name = name;
			Assembly = assembly;
			Provider = provider;
			ConnString = connString;
		}
	}
}
