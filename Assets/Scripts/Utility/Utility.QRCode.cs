using QRCoder;
using System.Drawing;

namespace Game
{
	public static partial class Utility
	{
		/// <summary>
		/// 二维码
		/// </summary>
		public static class QRCode
		{
			public static Bitmap Create(string msg, int pixel, int version)
			{
				QRCodeGenerator generator = new QRCodeGenerator();

				QRCodeData data = generator.CreateQrCode(msg, QRCodeGenerator.ECCLevel.M, true, true, QRCodeGenerator.EciMode.Utf8, version);

				QRCoder.QRCode code = new QRCoder.QRCode(data);

				return code.GetGraphic(pixel);
			}
		}
	}
}