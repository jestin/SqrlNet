using System;
using SqrlNet.Crypto;
using System.Security.Cryptography;
using System.Text;
using SqrlNet.Client;

namespace SqrlNetExample
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var rngCsp = new RNGCryptoServiceProvider();
			ISqrlSigner signer = new SqrlSigner();
			IPbkdfHandler pbkdfHandler = new PbkdfHandler();
			IHmacGenerator hmac = new HmacGenerator();
			ISqrlClient client = new SqrlClient(pbkdfHandler, hmac, signer);

			var masterIdentityKey = new byte[32];
			rngCsp.GetBytes(masterIdentityKey);

			var salt = new byte[32];
			rngCsp.GetBytes(salt);

			Console.Write("Password:  ");
			var password = Console.ReadLine();

			var url = "sqrl://www.example.com/sqrl?KJAnLFDQWWmvt10yVjNDoQ81uTvNorPrr53PPRJesz";

			var sqrlData = client.GetSqrlDataForLogin(masterIdentityKey, password, salt, url);

			var decryptedSignature = signer.Verify(sqrlData.PublicKey, sqrlData.Signature);

			Console.WriteLine("Url: {0}", sqrlData.Url);
			Console.WriteLine("Public Key: {0}", Convert.ToBase64String(sqrlData.PublicKey));
			Console.WriteLine("Signature: {0}", Convert.ToBase64String(sqrlData.Signature));
			Console.WriteLine("Decrypted Signature: {0}", Encoding.UTF8.GetString(decryptedSignature));
		}
	}
}
