using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Known.DLite.Mapping
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnAttribute : Attribute
	{
		private bool isChangeable;

		public string Name { get; set; }
		public object DefaultValue { get; set; }
		public bool IsPrimaryKey { get; set; }
		public bool IsNullable { get; set; }
		public bool IsDbGenerated { get; set; }

		public bool IsChangeable
		{
			get
			{
				if (IsPrimaryKey || IsDbGenerated)
				{
					return false;
				}
				return isChangeable;
			}
			set { isChangeable = value; }
		}

		public ColumnAttribute()
		{
			IsPrimaryKey = false;
			IsNullable = false;
			IsDbGenerated = false;
			isChangeable = true;
		}
	}
}
