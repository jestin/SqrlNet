using System;
using System.Linq;
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

			byte theByte = 0;

			while(true)
			{
				var secret = new byte[1];
				//rng.GetBytes(secret);
				secret[0] = theByte;

				var shares = ssss.Split(secret, 3, 4);
				var subset = new Dictionary<int, byte[]>();
				subset[1] = shares[1];
				subset[2] = shares[2];
				//subset[3] = shares[3];
				subset[4] = shares[4];
				//subset[5] = shares[5];
				//subset[6] = shares[6];
				//subset[7] = shares[7];
				//subset[8] = shares[8];
				//subset[9] = shares[9];
				//subset[10] = shares[10];
				//subset[11] = shares[11];
				//subset[12] = shares[12];
				//subset[13] = shares[13];
				//subset[14] = shares[14];
				//subset[15] = shares[15];
				//subset[16] = shares[16];
				//subset[17] = shares[17];
				//subset[18] = shares[18];
				//subset[19] = shares[19];
				//subset[20] = shares[20];
				//subset[21] = shares[21];
				//subset[22] = shares[22];
				//subset[23] = shares[23];
				//subset[24] = shares[24];
				//subset[25] = shares[25];
				var reconstructedSecret = ssss.Restore(subset);

				if(!secret.SequenceEqual(reconstructedSecret))
				{
					Console.WriteLine("the byte: {0}", theByte);
					foreach(var share in shares)
					{
						Console.WriteLine("{0}-{1}", share.Key, BitConverter.ToString(share.Value).Replace("-", ""));
					}
					Console.WriteLine("{0}", Convert.ToBase64String(secret));
					Console.WriteLine("{0}", Convert.ToBase64String(reconstructedSecret));
					break;
				}
				theByte++;

				if(theByte == 0)
				{
					Console.WriteLine("Success!");
					break;
				}
			}

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
