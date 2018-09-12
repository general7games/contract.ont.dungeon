using Ont.SmartContract.Framework;
using ont = Ont.SmartContract.Framework.Services.Ont;
using Common.Contract.Ont.Dungeon;
using System;
using System.Numerics;

namespace Test.Contract.Ont.Dungeon
{
	public class Test : SmartContract
	{
		const string version = "3F7F00E0-2962-4546-8548-A5D7A906EE65";

		struct Value
		{
			public int value;
		}

		public static object Main(string op, params object[] args)
		{
			var processed = ProcessOp.ProcessCommon(op, args);
			if (processed)
			{
				return true;
			}
			if (op == "Cal")
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
				// auth
				Set((string)args[0],	// name
					(int)args[1],		// value
					(string)args[2],	// ontID
					(int)args[3]		// keyNo
					);
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

		public static void Set(string name, int value, string ontID, int keyNo)
		{
			var r = Common.Contract.Ont.Dungeon.Contract.VerifyToken("Set", ontID, keyNo);
			if (r == Errors.SUCCESS)
			{
				var ctx = ont.Storage.CurrentContext;
				var v = new Value() { value = value };
				ont.Storage.Put(ctx, name, Helper.Serialize(v));
			}
			ont.Runtime.Notify(r);
		}

		public static void Get(string name)
		{
			var ctx = ont.Storage.CurrentContext;
			var bytes = ont.Storage.Get(ctx, name);
			var v = (Value)Helper.Deserialize(bytes);
			ont.Runtime.Notify(v.value);
		}
	}
}
