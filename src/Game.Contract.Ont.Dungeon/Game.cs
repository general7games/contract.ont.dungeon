using Ont.SmartContract.Framework;
using System.ComponentModel;
using System.Numerics;
using System;
using ont = Ont.SmartContract.Framework.Services.Ont;
using ontsys = Ont.SmartContract.Framework.Services.System;
using Common.Contract.Ont.Dungeon;

namespace Game.Contract.Ont.Dungeon
{
	public class Game : SmartContract
	{
        struct Point
        {
            string owner;
            int color;
            ulong price;

            public void Capture(string owner)
            {

            }
        }

		public static object Main(string op, params object[] args)
		{
            /*var processed = ProcessOp.ProcessCommon(op, args);
			if (processed)
			{
				return true;
			}*/

            switch (op)
            {
            case "InitAdminAccount":
                InitAdminAccount((byte[]) args[0]);
                return true;
            case "GetAdminAccount":
                GetAdminAccount();
                return true;
            case "Capture":
                //Capture();
                return true;
            }

			return false;
		}

        public static void InitAdminAccount(byte[] address)
        {
            var context = ont.Storage.CurrentContext;
			ont.Storage.Put(context, "admin_account", address);
        }

        public static void GetAdminAccount()
        {
            var context = ont.Storage.CurrentContext;
            var address = ont.Storage.Get(context, "admin_account");
            ont.Runtime.Notify(Errors.SUCCESS, address);
        }

        public static void Capture(byte[] from)
        {
            
        }
	}
}
