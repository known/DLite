using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Known.DLite.Mapping
{
	public class MetaParameter
	{
		public string PropertyName { get; set; }
		public string MappedName { get; set; }
		public object DefaultValue { get; set; }
		public bool IsPrimaryKey { get; set; }
		public bool IsNullable { get; set; }
		public bool IsDbGenerated { get; set; }
		public bool IsChangeable { get; set; }

		internal MetaParameter()
		{
			IsPrimaryKey = false;
			IsNullable = false;
			IsDbGenerated = false;
		}
	}
}
