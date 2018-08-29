using Ont.SmartContract.Framework;
using Ont.SmartContract.Framework.Services.Ont;
using System;
using System.Numerics;

namespace AuctionHouse.Contract.Ont.Dungeon
{
	public class AuctionHouse : SmartContract
	{
		public static void Main()
		{
			Storage.Put(Storage.CurrentContext, "Hello", "World");
		}
	}
}
