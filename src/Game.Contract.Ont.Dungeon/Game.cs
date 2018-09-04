using Ont.SmartContract.Framework;
using System.ComponentModel;
using System.Numerics;
using System;
using ont = Ont.SmartContract.Framework.Services.Ont;
using ontsys = Ont.SmartContract.Framework.Services.System;

namespace Game.Contract.Ont.Dungeon
{
	public class Game : SmartContract
	{
		struct Transfer
		{
			public byte[] From;
			public byte[] To;
			public int Value;
		}

		public delegate void TransferDelegate(byte[] from, byte[] to, BigInteger value);
		public delegate void MigrateDelegate(bool suc);

		[DisplayName("transfered")]
		public static event TransferDelegate transfered;
		[DisplayName("migrated")]
		public static event MigrateDelegate migrated;

		const string kOPAdminMigrateContract = "AdminMigrateContract";
		const string kOPAdminCollectONT = "AdminCollectONT";

		const int kErr_Success = 0;
		const int kErr_MigrateFailed = 1001;


		static readonly byte[] kONTContractAddr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };

		static byte[] contractAccount
		{
			get
			{
				return ontsys.ExecutionEngine.ExecutingScriptHash;
			}
		}



		public static object Main(string op, params object[] args)
		{
			if (op == kOPAdminMigrateContract)
				return AdminMigrateContract(
					(byte[])args[0],
					(string)args[1],(string)args[2],(string)args[3],(string)args[4],(string)args[5]);
			if (op == kOPAdminCollectONT)
				return AdminCollectONT();
			return false;
		}

		static bool AdminMigrateContract(byte[] script, string name, string version ,string author, string email, string description)
		{
			var contract = ont.Contract.Migrate(script, true, name, version, author, email, description);
			if (contract != null)
			{
				migrated(true);
				ont.Runtime.Notify(kErr_Success);
				return true;
			}
			migrated(false);
			ont.Runtime.Notify(kErr_MigrateFailed);
			return false;
		}

		static bool AdminCollectONT()
		{
			int value = 1;
			byte[] adminAccount = { };

			var transfer = new Transfer()
			{
				From = contractAccount,
				To = adminAccount,
				Value = value,
			};
			byte[] ret = ont.Native.Invoke(0, kONTContractAddr, "transfer", transfer);
			if (ret[0] != 0)
			{
				return false;
			}
			transfered(contractAccount, adminAccount, value);
			return true;
		}



	}
}
