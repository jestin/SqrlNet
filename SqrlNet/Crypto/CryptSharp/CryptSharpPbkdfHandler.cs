using System;
using System.Text;
using CryptSharp.Utility;
using System.Security.Cryptography;

namespace SqrlNet.Crypto.CryptSharp
{
	public class CryptSharpPbkdfHandler : IPbkdfHandler
	{
		public CryptSharpPbkdfHandler()
		{
		}

		#region IPbkdfHandler implementation

		public byte[] GeneratePasswordKey(string password, byte[] salt)
		{
			var key = new byte[32];
			SCrypt.ComputeKey(Encoding.UTF8.GetBytes(password),
			                  salt,
			                  32,
			                  1024,
			                  1,
			                  null,
			                  key);

			return key;
		}

		public bool VerifyPassword(string password, byte[] partialHash)
		{
			throw new System.NotImplementedException();
		}

		public byte[] GetPartialHashFromPasswordKey(byte[] passwordKey)
		{
			var sha256 = SHA256Managed.Create();
			var hash = sha256.ComputeHash(passwordKey);
			return new ArraySegment<byte>(hash, 16, 16).Array;
		}

		#endregion
	}
}

