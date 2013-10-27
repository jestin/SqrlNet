using System;
using System.Text;
using CryptSharp.Utility;

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

		#endregion
	}
}

