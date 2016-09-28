using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Known.DLite.Mapping
{
	public abstract class MetaType
	{
		public Type EntityType { get; private set; }

		public virtual bool IsDerived
		{
			get
			{
				if (EntityType.BaseType != null)
				{
					if (EntityType.BaseType == typeof(Object))
					{
						return true;
					}
				}
				return false;
			}
		}

		public virtual List<MetaParameter> MetaParameters
		{
			get
			{
				List<MetaParameter> parameters = GetMetaParameters();
				if (IsDerived)
				{
					parameters.AddRange(BaseMetaType.MetaParameters);
				}
				return parameters;
			}
		}

		public List<MetaParameter> PrimaryMetaParameters
		{
			get { return MetaParameters.Where(p => p.IsPrimaryKey).ToList(); }
		}

		public abstract string TableName { get; }
		//public abstract string HistoryName { get; }
		public abstract MetaType BaseMetaType { get; }

		protected MetaType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			EntityType = type;
		}

		protected abstract List<MetaParameter> GetMetaParameters();
	}
}
