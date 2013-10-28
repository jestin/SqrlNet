using System;
using System.Linq;
using System.Text;
using CryptSharp.Utility;
using System.Security.Cryptography;

namespace SqrlNet.Crypto.CryptSharp
{
	public class CryptSharpPbkdfHandler : IPbkdfHandler
	{
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

		public bool VerifyPassword(string password, byte[] salt, byte[] partialHash)
		{
			var passwordKey = GeneratePasswordKey(password, salt);
			return partialHash.SequenceEqual(GetPartialHashFromPasswordKey(passwordKey));
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

