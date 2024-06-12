// Don't modify, this is automatically generated
using Protobuf;
using Google.Protobuf;

namespace Game.Network
{
	public static class NetworkMessageDefine
	{
		public const int C2SActivityRequest = 1;
		public const int C2SClubRequest = 2;
		public const int C2SFriendRequest = 3;
		public const int C2SLoginRequest = 4;
		public const int C2SMailRequest = 5;
		public const int C2SPurchaseRequest = 6;
		public const int C2SRankingListRequest = 7;
		public const int C2SShopRequest = 8;
		public const int C2STaskRequest = 9;
		public const int C2SWarehouseRequest = 10;
		public const int S2CPlayerResponse = 11;

		public static IMessage Deserialize(RawMessage raw)
		{
			switch (raw.key)
			{
				case C2SActivityRequest:
					return NetworkConvert.Deserialize<C2SActivityRequest>(raw.content);
				case C2SClubRequest:
					return NetworkConvert.Deserialize<C2SClubRequest>(raw.content);
				case C2SFriendRequest:
					return NetworkConvert.Deserialize<C2SFriendRequest>(raw.content);
				case C2SLoginRequest:
					return NetworkConvert.Deserialize<C2SLoginRequest>(raw.content);
				case C2SMailRequest:
					return NetworkConvert.Deserialize<C2SMailRequest>(raw.content);
				case C2SPurchaseRequest:
					return NetworkConvert.Deserialize<C2SPurchaseRequest>(raw.content);
				case C2SRankingListRequest:
					return NetworkConvert.Deserialize<C2SRankingListRequest>(raw.content);
				case C2SShopRequest:
					return NetworkConvert.Deserialize<C2SShopRequest>(raw.content);
				case C2STaskRequest:
					return NetworkConvert.Deserialize<C2STaskRequest>(raw.content);
				case C2SWarehouseRequest:
					return NetworkConvert.Deserialize<C2SWarehouseRequest>(raw.content);
				case S2CPlayerResponse:
					return NetworkConvert.Deserialize<S2CPlayerResponse>(raw.content);
			}
			return default;
		}
	}
}
