using System;
using System.Text;
using System.Security.Cryptography;
using SqrlNet.Crypto;
using System.Net;
using System.IO;

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
			var aes = new RijndaelManaged();
			aes.Key = key;
			aes.IV = iv;
			var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

			var ms = new MemoryStream();
			using(var csEncrypt = new CryptoStream(ms, encryptor,CryptoStreamMode.Write))
			{
				using(var bw = new BinaryWriter(csEncrypt))
				{
					bw.Write(data.GetNutStruct().Bytes);
				}
			}

			return ms.ToArray();
		}

		public NutData DycryptNut(byte[] key, byte[] iv, byte[] nut)
		{
			var aes = new RijndaelManaged();
			aes.Key = key;
			aes.IV = iv;
			var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

			var ms = new MemoryStream();
			using(var csDecrypt = new CryptoStream(ms, decryptor,CryptoStreamMode.Write))
			{
				using(var bw = new BinaryWriter(csDecrypt))
				{
					bw.Write(nut);
				}
			}

			var nutStruct = new NutStruct
			{
				Bytes = ms.ToArray()
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

