using System;
using SqrlNet.Crypto;
using System.Security.Cryptography;
using System.Text;
using SqrlNet.Client;
using SqrlNet.Server;
using System.Net;
using System.Web;
using System.Collections.Generic;

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
			ISsssHandler ssss = new SsssHandler();
			var rng = new RNGCryptoServiceProvider();

			var secret = new byte[32];
			rng.GetBytes(secret);
			Console.WriteLine("{0}", Convert.ToBase64String(secret));

			var shares = ssss.Split(secret, 2, 2);
			//var oneThree = new Dictionary<int, byte[]>();
			//oneThree[1] = shares[1];
			//oneThree[3] = shares[3];
			var reconstructedSecret = ssss.Restore(2, shares);
			Console.WriteLine("{0}", Convert.ToBase64String(reconstructedSecret));

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

			var nut = HttpServerUtility.UrlTokenEncode(server.GenerateNut(aesKey, aesIV, nutData));

			var protocol = "sqrl://";
			var urlBase = "www.example.com/sqrl/";
			var url = string.Format("{0}{1}{2}", protocol, urlBase, nut);

			var sqrlData = client.GetSqrlDataForLogin(identity.MasterIdentityKey, password, identity.Salt, url);

			var decryptedSignature = signer.Verify(sqrlData.PublicKey, sqrlData.Signature);

			// This is the data that the client passes to the server
			Console.WriteLine("Url: {0}", sqrlData.Url);
			Console.WriteLine("Public Key: {0}", Convert.ToBase64String(sqrlData.PublicKey));
			Console.WriteLine("Signature: {0}", Convert.ToBase64String(sqrlData.Signature));
			Console.WriteLine("Decrypted Signature: {0}", Encoding.UTF8.GetString(decryptedSignature));

			Console.WriteLine();
			Console.WriteLine("=========== Server ===========");

			// The server will verify that the data sent from the client matches what is expected
			Console.WriteLine("Verified by server:  {0}", server.VerifySqrlRequest(sqrlData, string.Format("{0}{1}", urlBase, nut)));
		}
	}
}
