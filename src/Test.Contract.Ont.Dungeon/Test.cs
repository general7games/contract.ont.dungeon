using Ont.SmartContract.Framework;
using ont = Ont.SmartContract.Framework.Services.Ont;
using System;
using System.Numerics;

namespace Test.Contract.Ont.Dungeon
{
	public class Test : SmartContract
	{
		const string version = "A408B71B-1B76-48CB-AC27-A0F20A313E8C";

		struct Value
		{
			public int value;
		}

		public static object Main(string op, params object[] args)
		{
			if (op == "Migrate")
			{
				Migrate(
					(byte[])args[0], 
					(bool)args[1],		// needStorage
					(string)args[2],	// name
					(string)args[3],	// version
					(string)args[4],	// author
					(string)args[5],	// email
					(string)args[6]		// description
					);
				return true;
			}
			else if (op == "Cal")
			{
				Cal((int)args[0], (int)args[1]);
				return true;
			}
			else if (op == "Get")
			{
				Get((string)args[0]);
				return true;
			}
			else if (op == "Set")
			{
				Set((string)args[0], (int)args[1]);
				return true;
			}
			else if (op == "Destroy")
			{
				Destroy();
				return true;
			}
			else if (op == "Version")
			{
				ont.Runtime.Notify(version);
				return true;
			}
			return false;
		}

		public static void Cal(int a, int b)
		{
			var c = a + b;
			ont.Runtime.Notify(c);
		}

		public static void Set(string name, int value)
		{
			var ctx = ont.Storage.CurrentContext;
			var v = new Value() { value = value };
			ont.Storage.Put(ctx, name, Helper.Serialize(v));
		}

		public static void Get(string name)
		{
			var ctx = ont.Storage.CurrentContext;
			var bytes = ont.Storage.Get(ctx, name);
			var v = (Value)Helper.Deserialize(bytes);
			ont.Runtime.Notify(v.value);
		}

		public static void Migrate(
			byte[] script,
			bool needStorage,
			string name,
			string version,
			string author,
			string email,
			string description)
		{
			ont.Contract.Migrate(script, needStorage, name, version, author, email, description);
		}

		public static void Destroy()
		{
			ont.Contract.Destroy();
		}
	}
}
