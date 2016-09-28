using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Known.DLite.Mapping
{
	public class MetaTable
	{
		public MetaType RowType { get; set; }

		internal MetaTable(MetaType rowType)
		{
			RowType = rowType;
		}

		public Command GetInsertCommand(string paramPrefix, object entity)
		{
			string text = String.Format(
				"INSERT INTO {0}({1}) VALUES({2})",
				RowType.TableName,
				GetAllFieldString(", "),
				paramPrefix + GetAllFieldString(", " + paramPrefix)
			);
			List<Parameter> parameters = GetAllParameters(paramPrefix, entity);
			return new Command(text, parameters);
		}

		//public Command GetRemoveCommand(string paramPrefix, object entity)
		//{
		//    if (String.IsNullOrEmpty(RowType.HistoryName))
		//    {
		//        throw new Exception("Not history table.");
		//    }
		//    string text = String.Format(
		//        "INSERT INTO {0}({1}) SELECT {1} FROM {2} WHERE {3}",
		//        RowType.HistoryName,
		//        GetAllFieldString(", "),
		//        RowType.TableName,
		//        GetKeyFieldString(paramPrefix)
		//    );
		//    List<Parameter> parameters = GetKeyParameters(paramPrefix, entity);
		//    return new Command(text, parameters);
		//}

		public Command GetUpdateCommand(string paramPrefix, object entity)
		{
			string text = String.Format(
				"UPDATE {0} SET {1} WHERE {2}",
				RowType.TableName,
				GetUpdateFieldString(paramPrefix),
				GetKeyFieldString(paramPrefix)
			);
			List<Parameter> parameters = GetUpdateParameters(paramPrefix, entity);
			if (parameters != null)
			{
				parameters.AddRange(GetKeyParameters(paramPrefix, entity));
			}
			return new Command(text, parameters);
		}

		public Command GetDeleteCommand(string paramPrefix, object entity)
		{
			string text = String.Format(
				"DELETE FROM {0} WHERE {1}",
				RowType.TableName,
				GetKeyFieldString(paramPrefix)
			);
			List<Parameter> parameters = GetKeyParameters(paramPrefix, entity);
			return new Command(text, parameters);
		}

		public string GetKeyFieldString(string paramPrefix)
		{
			return String.Join(
				" AND ",
				RowType.PrimaryMetaParameters
				.Select(p => p.MappedName + "=" + paramPrefix + p.MappedName)
				.ToArray()
			);
		}

		public List<Parameter> GetKeyParameters(string parmPrefix, params object[] keys)
		{
			List<Parameter> parameters = new List<Parameter>();
			int index = 0;
			foreach (var key in RowType.PrimaryMetaParameters)
			{
				parameters.Add(new Parameter
				{
					Name = parmPrefix + key.MappedName,
					Value = FormatValue(keys[index])
				});
				index++;
			}
			return parameters;
		}

		private string GetAllFieldString(string separator)
		{
			return String.Join(
				separator,
				RowType.MetaParameters
				.Where(p => !p.IsDbGenerated)
				.Select(p => p.MappedName)
				.ToArray()
			);
		}

		private string GetUpdateFieldString(string paramPrefix)
		{
			return String.Join(
				", ",
				RowType.MetaParameters
				.Where(p => p.IsChangeable)
				.Select(p => p.MappedName + "=" + paramPrefix + p.MappedName)
				.ToArray()
			);
		}

		private List<Parameter> GetAllParameters(string paramPrefix, object entity)
		{
			return RowType.MetaParameters.Where(p => !p.IsDbGenerated)
				.Select(p => new Parameter
				{
					Name = paramPrefix + p.MappedName,
					Value = FormatValue(RowType.EntityType.GetProperty(p.PropertyName).GetValue(entity, null))
				}).ToList();
		}

		private List<Parameter> GetUpdateParameters(string paramPrefix, object entity)
		{
			return RowType.MetaParameters.Where(p => p.IsChangeable)
				.Select(p => new Parameter
				{
					Name = paramPrefix + p.MappedName,
					Value = FormatValue(RowType.EntityType.GetProperty(p.PropertyName).GetValue(entity, null))
				}).ToList();
		}

		private List<Parameter> GetKeyParameters(string paramPrefix, object entity)
		{
			return RowType.MetaParameters.Where(p => p.IsPrimaryKey)
				.Select(p => new Parameter
				{
					Name = paramPrefix + p.MappedName,
					Value = FormatValue(RowType.EntityType.GetProperty(p.PropertyName).GetValue(entity, null))
				}).ToList();
		}

		private object FormatValue(object value)
		{
			if (value == null)
			{
				return DBNull.Value;
			}
			if (String.IsNullOrEmpty(value.ToString()))
			{
				return DBNull.Value;
			}
			return value.ToString().Trim();
		}
	}
}
