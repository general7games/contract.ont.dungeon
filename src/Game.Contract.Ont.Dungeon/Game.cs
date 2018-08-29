using Ont.SmartContract.Framework;
using Ont.SmartContract.Framework.Services.Ont;
using System;
using System.Numerics;

namespace Game.Contract.Ont.Dungeon
{
	public class Game : SmartContract
	{
		public static void Main()
		{
			Storage.Put(Storage.CurrentContext, "Hello", "World");
		}
	}
}
