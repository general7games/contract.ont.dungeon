using Ont.SmartContract.Framework;
using ont = Ont.SmartContract.Framework.Services.Ont;
using ontsys = Ont.SmartContract.Framework.Services.System;


namespace Common.Contract.Ont.Dungeon
{

	public static class Contract
	{
		struct VerifyTokenParam
		{
			public byte[] contractAddr;
			public byte[] calllerOntID;
			public string funcName;
			public int keyNo;
		}

		internal static int InitAdmin(string adminOntID)
		{
			byte[] kAUTH_CONTRACT = new byte[]
			{
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x06
			};
			var ret = ont.Native.Invoke(0, kAUTH_CONTRACT, "initContractAdmin", adminOntID.AsByteArray());
			if (ret[0] == 1)
			{
				return Errors.SUCCESS;
			}
			return Errors.FAILED;
		}

		public static int VerifyToken(string funcName, string callerOntID, int callerPubKeyNo)
		{
			byte[] kAUTH_CONTRACT = new byte[]
			{
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x06
			};
			VerifyTokenParam param = new VerifyTokenParam
			{
				contractAddr = ontsys.ExecutionEngine.ExecutingScriptHash,
				funcName = funcName,
				calllerOntID = callerOntID.AsByteArray(),
				keyNo = callerPubKeyNo
			};
			byte[] ret = ont.Native.Invoke(0, kAUTH_CONTRACT, "verifyToken", param);
			if (ret[0] == 1)
			{
				return Errors.SUCCESS;
			}
			return Errors.UNAUTHORIZED;
		}
		public static int Migrate(
			byte[] script,
			bool needStorage,
			string name,
			string version,
			string author,
			string email,
			string description,
			string ontAdminID,
			int keyNo
			)
		{
			var r = VerifyToken("Migrate", ontAdminID, keyNo);
			if (Errors.SUCCESS != r)
			{
				return r;
			}
			ont.Contract.Migrate(script, needStorage, name, version, author, email, description);
			return Errors.SUCCESS;
		}

		public static int Destroy(string ontAdminID, int keyNo)
		{
			var r = VerifyToken("Destroy", ontAdminID, keyNo);
			if (Errors.SUCCESS != r)
			{
				return r;
			}
			ont.Contract.Destroy();
			return Errors.SUCCESS;
		}
	}
}
