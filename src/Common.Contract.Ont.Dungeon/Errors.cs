using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract.Ont.Dungeon
{
	public static class Errors
	{
		public const int SUCCESS = 0;
		public const int FAILED = 20001;

		public const int UNAUTHORIZED = 20401;
        public const int NOT_ENOUGH_PRICE = 20402;
        public const int NOT_INITIALIZED = 20403;
        public const int HAS_INITIALIZED = 20404;
	}
}
