using System;
using System.Security.Cryptography;
using System.IO;

namespace SqrlNet.Crypto
{
	public class AesHandler : IAesHandler
	{
		#region IAesHandler implementation

		public byte[] Encrypt(byte[] key, byte[] iv, byte[] message)
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
					bw.Write(message);
				}
			}

			return ms.ToArray();
		}

		public byte[] Decrypt(byte[] key, byte[] iv, byte[] encryptedMessage)
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
					bw.Write(encryptedMessage);
				}
			}

			return ms.ToArray();
		}

		#endregion
	}
}

