using System;
using CryptSharp.Utility;

namespace SqrlNet.Crypto.CryptSharp
{
	public class CryptSharpPbkdfHandler : IPbkdfHandler
	{
		public CryptSharpPbkdfHandler()
		{
		}

		#region IPbkdfHandler implementation

		public byte[] GeneratePasswordKey(string password)
		{
			throw new System.NotImplementedException();
		}

		public bool VerifyPassword(string password, byte[] partialHash)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}

