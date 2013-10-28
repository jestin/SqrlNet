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
			ISqrlSigner signer = new SqrlSigner();
			IPbkdfHandler pbkdfHandler = new PbkdfHandler();
			IHmacGenerator hmac = new HmacGenerator();
			ISqrlClient client = new SqrlClient(pbkdfHandler, hmac, signer);

			Console.Write("Password:  ");
			var password = Console.ReadLine();

			// very insecure way of gathering entropy, but good enough for testing temporary identities
			var identity = client.CreateIdentity(password, Encoding.UTF8.GetBytes(DateTime.Now.ToLongDateString()));

			if(client.VerifyPassword(password, identity))
			{
				Console.WriteLine("Password verified");
			}

			var url = "sqrl://www.example.com/sqrl?KJAnLFDQWWmvt10yVjNDoQ81uTvNorPrr53PPRJesz";

			var sqrlData = client.GetSqrlDataForLogin(identity.MasterIdentityKey, password, identity.Salt, url);

			var decryptedSignature = signer.Verify(sqrlData.PublicKey, sqrlData.Signature);

			Console.WriteLine("Url: {0}", sqrlData.Url);
			Console.WriteLine("Public Key: {0}", Convert.ToBase64String(sqrlData.PublicKey));
			Console.WriteLine("Signature: {0}", Convert.ToBase64String(sqrlData.Signature));
			Console.WriteLine("Decrypted Signature: {0}", Encoding.UTF8.GetString(decryptedSignature));
		}
	}
}
