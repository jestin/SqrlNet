using System;
using SqrlNet.Crypto;
using System.Security.Cryptography;
using System.Text;

namespace SqrlNetExample
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var rngCsp = new RNGCryptoServiceProvider();
			ISqrlSigner signer = new SqrlSigner();

			var privateKey = new byte[64];
			rngCsp.GetBytes(privateKey);
			var publicKey = signer.MakePublicKey(privateKey);
			var message = "This is just a test";

			var signedMessage = signer.Sign(privateKey, message);

			var decryptedMessage = signer.Verify(publicKey, signedMessage);

			Console.WriteLine("Private Key: {0}", Convert.ToBase64String(privateKey));
			Console.WriteLine("Public Key: {0}", Convert.ToBase64String(publicKey));
			Console.WriteLine("Message: {0}", message);
			Console.WriteLine("Signed Message: {0}", Convert.ToBase64String(signedMessage));
			Console.WriteLine("Decrypted Message: {0}", Encoding.UTF8.GetString(decryptedMessage));
		}
	}
}
