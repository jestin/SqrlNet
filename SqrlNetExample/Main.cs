using System;
using SqrlNet.Crypto;
using System.Security.Cryptography;
using System.Text;
using SqrlNet.Client;
using SqrlNet.Server;
using System.Net;

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
			IAesHandler aesHandler = new AesHandler();
			ISqrlServer server = new SqrlServer(signer, aesHandler);
			var rng = new RNGCryptoServiceProvider();

			Console.Write("Password:  ");
			var password = Console.ReadLine();

			// very insecure way of gathering entropy, but good enough for testing temporary identities
			var identity = client.CreateIdentity(password, Encoding.UTF8.GetBytes(DateTime.Now.ToLongDateString()));

			if(client.VerifyPassword(password, identity))
			{
				Console.WriteLine("Password verified");
			}

			var nutData = new NutData
			{
				Address = IPAddress.Parse("172.8.92.254"),
				Timestamp = DateTime.UtcNow,
				Counter = 4,
				Entropy = new byte[4]
			};

			var aesKey = new byte[16];
			var aesIV = new byte[16];

			rng.GetBytes(aesKey);
			rng.GetBytes(aesIV);

			var url = string.Format("sqrl://www.example.com/sqrl?{0}", Convert.ToBase64String(server.GenerateNut(aesKey, aesIV, nutData)).TrimEnd('='));

			var sqrlData = client.GetSqrlDataForLogin(identity.MasterIdentityKey, password, identity.Salt, url);

			var decryptedSignature = signer.Verify(sqrlData.PublicKey, sqrlData.Signature);

			Console.WriteLine("Url: {0}", sqrlData.Url);
			Console.WriteLine("Public Key: {0}", Convert.ToBase64String(sqrlData.PublicKey));
			Console.WriteLine("Signature: {0}", Convert.ToBase64String(sqrlData.Signature));
			Console.WriteLine("Decrypted Signature: {0}", Encoding.UTF8.GetString(decryptedSignature));
		}
	}
}
