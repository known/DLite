using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Known.DLite.Mapping
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableAttribute : Attribute
	{
		public string Name { get; set; }
		//public string History { get; set; }

		public TableAttribute() { }

		public TableAttribute(string name)
		{
			Name = name;
		}

		//public TableAttribute(string name, string history)
		//{
		//    Name = name;
		//    History = history;
		//}
	}
}
