// Don't modify, this is automatically generated
using Protobuf;
using Google.Protobuf;

namespace Game.Network
{
	public static class NetworkMessageDefine
	{
		public const int C2SLoginRequest = 1;
		public const int C2SPurchaseRequest = 2;
		public const int S2CPlayerResponse = 3;

		public static IMessage Deserialize(RawMessage raw)
		{
			switch (raw.key)
			{
				case C2SLoginRequest:
					return NetworkConvert.Deserialize<C2SLoginRequest>(raw.content);
				case C2SPurchaseRequest:
					return NetworkConvert.Deserialize<C2SPurchaseRequest>(raw.content);
				case S2CPlayerResponse:
					return NetworkConvert.Deserialize<S2CPlayerResponse>(raw.content);
			}
			return default;
		}
	}
}
