using System;
using SqrlNet.Crypto;

namespace SqrlNet.Client
{
	public class SqrlClient : ISqrlClient
	{
		#region Dependencies

		private readonly IPbkdfHandler _pbkdfHandler;
		private readonly IHmacGenerator _hmacGenerator;
		private readonly ISqrlSigner _signer;

		#endregion

		#region Constructors

		public SqrlClient(
			IPbkdfHandler pbkdfHandler,
			IHmacGenerator hmacGenerator,
			ISqrlSigner signer)
		{
			_pbkdfHandler = pbkdfHandler;
			_hmacGenerator = hmacGenerator;
			_signer = signer;
		}

		#endregion

		#region ISqrlClient implementation

		public byte[] CalculateMasterKey(byte[] masterIdentityKey, string password)
		{
			if(masterIdentityKey.Length != 64)
			{
				throw new Exception("master identity key must be 256 bits (64 bytes).");
			}

			var passwordKey = _pbkdfHandler.GeneratePasswordKey(password);

			if(passwordKey.Length != 64)
			{
				throw new Exception("password key must be 256 bits (64 bytes).  Check validity of PBKDF.");
			}

			return Xor(masterIdentityKey, passwordKey);
		}

		public byte[] CalculateMasterIdentityKey(byte[] masterKey, string password)
		{
			throw new System.NotImplementedException();
		}

		public byte[] GenerateMasterKey()
		{
			throw new System.NotImplementedException();
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterKey, string url)
		{
			throw new System.NotImplementedException();
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterIdentityKey, string password, string url)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		#region Private Methods

		private byte[] Xor(byte[] a, byte[] b)
		{
			if(a.Length != b.Length)
			{
				throw new Exception("a and b must be of the same length");
			}

			byte[] result = new byte[a.Length];

			for(int i = 0; i < a.Length; i++)
			{
				result[i] = (byte)(a[i] ^ b[i]);
			}

			return result;
		}

		#endregion
	}
}