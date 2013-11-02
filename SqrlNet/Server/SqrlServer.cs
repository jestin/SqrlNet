using System;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using SqrlNet.Crypto;

namespace SqrlNet.Server
{
	public class SqrlServer : ISqrlServer
	{
		#region Dependencies

		private readonly ISqrlSigner _sqrlSigner;
		private readonly IAesHandler _aesHandler;

		#endregion

		public SqrlServer(
			ISqrlSigner sqrlSigner,
			IAesHandler aesHandler)
		{
			_sqrlSigner = sqrlSigner;
			_aesHandler = aesHandler;
		}

		#region Static Variables

		private static Int32 _counter = 0;

		#endregion

		#region ISqrlServer implementation

		public byte[] GenerateNut(byte[] key, byte[] iv)
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

			return GenerateNut(key, iv, nutData);
		}

		public byte[] GenerateNut(byte[] key, byte[] iv, NutData data)
		{
			return _aesHandler.Encrypt(key, iv, data.GetNutStruct().Bytes);
		}

		public NutData DecryptNut(byte[] key, byte[] iv, byte[] nut)
		{
			var nutStruct = new NutStruct
			{
				Bytes = _aesHandler.Decrypt(key, iv, nut)
			};

			return new NutData(nutStruct);
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

