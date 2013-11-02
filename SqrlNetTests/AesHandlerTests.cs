using System;
using System.Text;
using NUnit.Framework;
using System.Security.Cryptography;
using SqrlNet.Crypto;

namespace SqrlNetTests
{
	[TestFixture]
	public class AesHandlerTests
	{
		[Test]
		public void SimpleEncryptionTest()
		{
			var aes = new AesHandler();
			var aesKey = new byte[16];
			var aesIV = new byte[16];

			var message = "This is a test message that has some stuff in it";

			var rng = new RNGCryptoServiceProvider();

			rng.GetBytes(aesKey);
			rng.GetBytes(aesIV);

			var encryptedBytes = aes.Encrypt(aesKey, aesIV, Encoding.UTF8.GetBytes(message));
			var decryptedBytes = aes.Decrypt(aesKey, aesIV, encryptedBytes);

			Assert.AreEqual(message, Encoding.UTF8.GetString(decryptedBytes));
		}
	}
}

