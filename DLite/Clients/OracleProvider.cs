using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace Known.DLite.Clients
{
	public class OracleProvider : IProvider
	{
		private string connString;
		private OracleConnection conn;

		public OracleProvider() { }

		public OracleProvider(string connString)
		{
			this.connString = connString;
			conn = new OracleConnection(connString);
		}

		public OracleConnection Connection
		{
			get
			{
				if (conn == null)
				{
					conn = new OracleConnection(connString);
				}
				return conn;
			}
		}

		public string Database
		{
			get { return "Oracle"; }
		}

		public string ParamPrefix
		{
			get { return ":"; }
		}

		public string ConnString
		{
			get { return connString; }
			set { connString = value; }
		}

		public void Execute(Command command)
		{
			using (OracleCommand cmd = Connection.CreateCommand())
			{
				PrepareCommand(cmd, null, command);
				cmd.ExecuteNonQuery();
			}
		}

		public void Execute(List<Command> commands)
		{
			OpenConnection();
			using (OracleTransaction trans = Connection.BeginTransaction())
			{
				OracleCommand cmd = Connection.CreateCommand();
				try
				{
					foreach (Command command in commands)
					{
						PrepareCommand(cmd, trans, command);
						cmd.ExecuteNonQuery();
					}
					trans.Commit();
				}
				catch (OracleException e)
				{
					trans.Rollback();
					cmd.Dispose();
					throw new Exception(e.Message);
				}
			}
		}

		public object QueryScalar(Command command)
		{
			using (OracleCommand cmd = Connection.CreateCommand())
			{
				PrepareCommand(cmd, null, command);
				return cmd.ExecuteScalar();
			}
		}

		public IDataReader QueryData(Command command)
		{
			using (OracleCommand cmd = Connection.CreateCommand())
			{
				PrepareCommand(cmd, null, command);
				return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		public void Dispose()
		{
			if (Connection != null)
			{
				if (Connection.State != ConnectionState.Closed)
				{
					Connection.Close();
				}
				Connection.Dispose();
			}
		}

		private void OpenConnection()
		{
			if (Connection.State != ConnectionState.Open)
			{
				Connection.Open();
			}
		}

		private void PrepareCommand(OracleCommand cmd, OracleTransaction trans, Command command)
		{
			if (trans != null)
			{
				cmd.Transaction = trans;
			}
			cmd.CommandText = command.Text;
			if (command.HasParameter)
			{
				cmd.Parameters.Clear();
				foreach (Parameter param in command.Parameters)
				{
					cmd.Parameters.Add(new OracleParameter(param.Name, param.Value));
				}
			}
			OpenConnection();
		}
	}
}
