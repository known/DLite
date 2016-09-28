using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Known.DLite.Mapping;

namespace Known.DLite
{
	public class Transaction : IDisposable
	{
		public IProvider Provider { get; set; }
		public List<Command> Commands { get; private set; }

		public int CommandCount
		{
			get
			{
				if (Commands == null)
				{
					return 0;
				}
				return Commands.Count;
			}
		}

		public bool HasCommand
		{
			get { return CommandCount > 0; }
		}

		public Transaction()
			: this(ProviderFactory.GetProvider())
		{
		}

		public Transaction(IProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			Provider = provider;
		}

		public void Insert<T>(T entity)
		{
			AddCommand<T>(TextType.Insert, entity);
		}

		//public void Remove<T>(T entity)
		//{
		//    AddCommand<T>(TextType.Remove, entity);
		//    Delete<T>(entity);
		//}

		public void Update<T>(T entity)
		{
			AddCommand<T>(TextType.Update, entity);
		}

		public void Delete<T>(T entity)
		{
			AddCommand<T>(TextType.Delete, entity);
		}

		public void InsertAll<T>(List<T> entities)
		{
			foreach (T entity in entities)
			{
				Insert<T>(entity);
			}
		}

		public void UpdateAll<T>(List<T> entities)
		{
			foreach (T entity in entities)
			{
				Update<T>(entity);
			}
		}

		public void DeleteAll<T>(List<T> entities)
		{
			foreach (T entity in entities)
			{
				Delete<T>(entity);
			}
		}

		public void SubmitChanges()
		{
			if (CommandCount > 1)
			{
				Provider.Execute(Commands);
			}
			else if (CommandCount > 0)
			{
				Provider.Execute(Commands[0]);
			}
		}

		public void Dispose()
		{
			if (Commands != null)
			{
				Commands.Clear();
			}
			Provider.Dispose();
		}

		public override string ToString()
		{
			if (HasCommand)
			{
				StringBuilder sb = new StringBuilder();
				foreach (Command command in Commands)
				{
					sb.AppendLine(command.ToString());
				}
				return sb.ToString();
			}
			return String.Empty;
		}

		private void AddCommand<T>(TextType textType, T entity)
		{
			Command command = null;
			MetaTable table = MetaManager.GetMetaTable(typeof(T));
			switch (textType)
			{
				case TextType.Insert:
					command = table.GetInsertCommand(Provider.ParamPrefix, entity);
					break;
				//case TextType.Remove:
				//    command = table.GetRemoveCommand(Provider.ParamPrefix, entity);
				//    break;
				case TextType.Update:
					command = table.GetUpdateCommand(Provider.ParamPrefix, entity);
					break;
				case TextType.Delete:
					command = table.GetDeleteCommand(Provider.ParamPrefix, entity);
					break;
			}
			if (command != null)
			{
				if (Commands == null)
				{
					Commands = new List<Command>();
				}
				Commands.Add(command);
			}
		}

		private enum TextType
		{
			Insert,
			//Remove,
			Update,
			Delete
		}
	}
}
