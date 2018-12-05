using Ont.SmartContract.Framework;
using ont = Ont.SmartContract.Framework.Services.Ont;
using Common.Contract.Ont.Dungeon;

namespace Game.Contract.Ont.Dungeon
{
    public class Game : SmartContract
	{
        struct Point
        {
            public byte[] owner;
            public uint color;
            public ulong price;
        }

        struct Transfer
        {
            public byte[] from;
            public byte[] to;
            public ulong amount;
        }

        const int MAX_LINE = 10;
        const ulong INITIAL_PRICE = 100;
        const ulong PRICE_MULTIPLIER = 13;
        const ulong PRICE_DIVISOR = 10;
        const ulong PRICE_SPREAD_TO_OLD_OWNER_MULTIPLIER = 7;
        const ulong PRICE_SPREAD_TO_OLD_OWNER_DIVISOR = 10;

        static readonly byte[] ONT_ADDRESS = "AFmseVrdL9f9oyCzZefL9tG6UbvhUMqNMV".ToScriptHash();

        public static object Main(string op, params object[] args)
		{
            /*var processed = ProcessOp.ProcessCommon(op, args);
			if (processed)
			{
				return true;
			}*/

            switch (op)
            {
            case "InitContract":
                InitContract((byte[]) args[0]);
                break;
            case "GetContractInfos":
                GetContractInfos();
                break;
            case "Capture":
                Capture((byte[]) args[0], (int[]) args[1], (int[]) args[2], (uint[]) args[3], (ulong[]) args[4]);
                break;
            case "GetPoints":
                GetPoints((int[]) args[0], (int[]) args[1]);
                break;
            case "GetAllPoints":
                GetAllPoints();
                break;
            }
            return true;
		}

        public static void InitContract(byte[] address)
        {
            var context = ont.Storage.CurrentContext;
            if (_GetAdminAccount() != null)
            {
                ont.Runtime.Notify(Errors.HAS_INITIALIZED);
                return;
            }
			ont.Storage.Put(context, "admin_account", address);
            GetContractInfos();
        }

        public static void GetContractInfos()
        {
            var context = ont.Storage.CurrentContext;
            var address = _GetAdminAccount();
            if (address != null)
            {
                ont.Runtime.Notify(Errors.SUCCESS, BuildInfo.BUILD_DATE, BuildInfo.GIT_COMMIT, address, MAX_LINE, INITIAL_PRICE, 
                    PRICE_MULTIPLIER, PRICE_DIVISOR, PRICE_SPREAD_TO_OLD_OWNER_MULTIPLIER, PRICE_SPREAD_TO_OLD_OWNER_DIVISOR);
            }
            else
            {
                ont.Runtime.Notify(Errors.NOT_INITIALIZED);
            }
        }

        private static byte[] _GetAdminAccount()
        {
            var context = ont.Storage.CurrentContext;
            return ont.Storage.Get(context, "admin_account");
        }

        public static void Capture(byte[] from, int[] xPoints, int[] yPoints, uint[] colors, ulong[] prices)
        {
            if (xPoints.Length != yPoints.Length || yPoints.Length != colors.Length || colors.Length != prices.Length)
            {
                ont.Runtime.Notify(Errors.FAILED, "Input length of each args not equal.");
                return;
            }

            byte[] adminAccount = _GetAdminAccount();
            for (int i = 0; i < xPoints.Length; ++i)
            {
                int x = xPoints[i];
                int y = yPoints[i];
                if (x < 1 || x > MAX_LINE || y < 1 || y > MAX_LINE)
                {
                    ont.Runtime.Notify(Errors.FAILED, "Point is out of range.");
                    return;
                }
                uint color = colors[i];
                ulong price = prices[i];

                var key = _GeneratePointKey(x, y);
                Point point;
                var bytes = ont.Storage.Get(ont.Storage.CurrentContext, key);
                if (bytes == null)
                {
                    point = new Point()
                    {
                        owner = from,
                        color = color,
                        price = INITIAL_PRICE
                    };
                }
                else
                {
                    point = (Point) Helper.Deserialize(bytes);
                }

                if (point.price * PRICE_MULTIPLIER / PRICE_DIVISOR > price)
                {
                    ont.Runtime.Notify(Errors.NOT_ENOUGH_PRICE, i);
                    return;
                }

                if (bytes == null) // No owner
                {
                    if (!TransferONT(from, adminAccount, price))
                    {
                        ont.Runtime.Notify(Errors.FAILED, "Transfer error.", from, adminAccount, price);
                        return;
                    }
                }
                else
                {
                    ulong spreadToOldOwner = (price - point.price) * PRICE_SPREAD_TO_OLD_OWNER_MULTIPLIER / PRICE_SPREAD_TO_OLD_OWNER_DIVISOR;
                    ulong toOldOwner = point.price + spreadToOldOwner;
                    ulong toAdmin = price - point.price - spreadToOldOwner;
                    if (!TransferONT(from, point.owner, toOldOwner))
                    {
                        ont.Runtime.Notify(Errors.FAILED, "Transfer error.", from, point.owner, toOldOwner);
                        return;
                    }
                    if (!TransferONT(from, adminAccount, toAdmin))
                    {
                        ont.Runtime.Notify(Errors.FAILED, "Transfer error.", from, adminAccount, toAdmin);
                        return;
                    }
                }

                
                point.owner = from;
                point.color = color;
                point.price = price;
                ont.Storage.Put(ont.Storage.CurrentContext, key, Helper.Serialize(point));
            }

            ont.Runtime.Notify(Errors.SUCCESS, "OK");
        }

        public static void GetPoints(int[] xPoints, int[] yPoints)
        {
            Point[] points = new Point[xPoints.Length];
            for (int i = 0; i < xPoints.Length; ++i)
            {
                int x = xPoints[i];
                int y = yPoints[i];
                points[i] = _GetPoint(x, y);
            }
            ont.Runtime.Notify(Errors.SUCCESS, points);
        }

        public static void GetAllPoints()
        {
            Point[] points = new Point[MAX_LINE * MAX_LINE];
            for (int y = 1; y <= MAX_LINE; ++y)
            {
                for (int x = 1; x <= MAX_LINE; ++x)
                {
                    Point point = _GetPoint(x, y);
                    points[(y - 1) * MAX_LINE + x - 1] = point;
                }
            }
            ont.Runtime.Notify(Errors.SUCCESS, MAX_LINE, points);
        }

        private static Point _GetPoint(int x, int y)
        {
            var key = _GeneratePointKey(x, y);
            var bytes = ont.Storage.Get(ont.Storage.CurrentContext, key);
            if (bytes != null)
            {
                return (Point) Helper.Deserialize(bytes);
            }
            return new Point()
            {
                owner = {},
                color = 0xFFFFFF,
                price = INITIAL_PRICE
            };
        }

        private static string _GeneratePointKey(int x, int y)
        {
            return "point_" + ((y - 1) * MAX_LINE + x);
        }

        private static bool TransferONT(byte[] from, byte[] to, ulong ontAmount)
        {
            Transfer param = new Transfer { from = from, to = to, amount = ontAmount };
            object[] p = new object[1] { param };
            byte[] ret = ont.Native.Invoke(0, ONT_ADDRESS, "transfer", p);
            return ret[0] == 1;
        }
	}
}
