using Ont.SmartContract.Framework;
using Ont.SmartContract.Framework.Services.Ont;
using System;
using System.Numerics;

namespace Gacha
{
	public class Gacha : SmartContract
	{
		public static void Main()
		{
			Storage.Put(Storage.CurrentContext, "Hello", "World");
		}
	}
}
