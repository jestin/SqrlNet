using System;
using System.Text;
using System.Security.Cryptography;
using SqrlNet.Crypto;
using System.Net;

namespace SqrlNet.Server
{
	public class SqrlServer : ISqrlServer
	{
		#region Dependeusing System.Security.Cryptography;ncies

		private readonly ISqrlSigner _sqrlSigner;

		#endregion

		public SqrlServer(ISqrlSigner sqrlSigner)
		{
			_sqrlSigner = sqrlSigner;
		}

		#region Static Variables

		private static Int32 _counter = 0;

		#endregion

		#region ISqrlServer implementation

		public byte[] GenerateNut(byte[] key)
		{
			var rng = new RNGCryptoServiceProvider();

			var nutData = new NutData
			{
				Address = new IPAddress(0),
				Timestamp = DateTime.UtcNow,
				Counter = _counter,
				Entropy = new byte[4]
			};

			rng.GetBytes(nutData.Entropy);

			_counter++;

			return GenerateNut(key, nutData);
		}

		public byte[] GenerateNut(byte[] key, NutData data)
		{
			throw new System.NotImplementedException();
		}

		public NutData DycryptNut(byte[] key, byte[] nut)
		{
			throw new System.NotImplementedException();
		}

		public bool VerifySqrlRequest(SqrlData data)
		{
			var decryptedSignatureData = _sqrlSigner.Verify(data.PublicKey, data.Signature);

			var decryptedUrl = Encoding.UTF8.GetString(decryptedSignatureData);

			var url = Utility.GetUrlWithoutProtocol(data.Url);

			return (decryptedUrl == url);
		}

		#endregion
	}
}

