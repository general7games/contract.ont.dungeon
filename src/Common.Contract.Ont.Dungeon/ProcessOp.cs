using ont = Ont.SmartContract.Framework.Services.Ont;

namespace Common.Contract.Ont.Dungeon
{
	public static class ProcessOp
	{
		public static bool ProcessCommon(string op, object[] args)
		{
			if (op == "InitAdmin")
			{
				var r = Contract.InitAdmin((string)args[0]);
				ont.Runtime.Notify(r);
				return true;
			}
			else if (op == "Migrate")
			{
				// auth
				var r = Contract.Migrate(
					(byte[])args[0], 
					(bool)args[1],		// needStorage
					(string)args[2],	// name
					(string)args[3],	// version
					(string)args[4],	// author
					(string)args[5],	// email
					(string)args[6],	// description
					(string)args[7],	// adminOntID
					(int)args[8]		// keyNo
					);
				ont.Runtime.Notify(r);
				return true;
			}
			else if (op == "Destroy")
			{
				// auth
				var r = Contract.Destroy(
					(string)args[0],	// adminOntID
					(int)args[1]		// keyNo
					);
				ont.Runtime.Notify(r);
				return true;
			}
			return false;
		}
	}
}
