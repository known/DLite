using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Known.DLite
{
	public interface IProvider : IDisposable
	{
		string Database { get; }
		string ParamPrefix { get; }
		string ConnString { get; set; }

		void Execute(Command command);
		void Execute(List<Command> commands);
		object QueryScalar(Command command);
		IDataReader QueryData(Command command);
	}
}
