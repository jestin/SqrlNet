using System;
using System.Security.Cryptography;
using System.Text;

namespace SqrlNet.Crypto
{
	public class HmacGenerator : IHmacGenerator
	{
		#region IHmacGenerator implementation

		public byte[] GeneratePrivateKey(byte[] masterKey, string domain)
		{
			using(HMACSHA256 hmac = new HMACSHA256(masterKey))
			{
				return hmac.ComputeHash(Encoding.UTF8.GetBytes(domain));
			}
		}

		#endregion
	}
}

