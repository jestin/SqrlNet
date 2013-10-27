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

		public byte[] CalculateMasterKey(byte[] masterIdentityKey, string password, byte[] salt)
		{
			if(masterIdentityKey.Length != 32)
			{
				throw new Exception("master identity key must be 256 bits (32 bytes).");
			}

			var passwordKey = _pbkdfHandler.GeneratePasswordKey(password, salt);

			if(passwordKey.Length != 32)
			{
				throw new Exception("password key must be 256 bits (32 bytes).  Check validity of PBKDF.");
			}

			return Xor(masterIdentityKey, passwordKey);
		}

		public byte[] CalculateMasterIdentityKey(byte[] masterKey, string password, byte[] salt)
		{
			if(masterKey.Length != 32)
			{
				throw new Exception("master key must be 256 bits (32 bytes).");
			}

			var passwordKey = _pbkdfHandler.GeneratePasswordKey(password, salt);

			if(passwordKey.Length != 32)
			{
				throw new Exception("password key must be 256 bits (32 bytes).  Check validity of PBKDF.");
			}

			return Xor(masterKey, passwordKey);
		}

		// TODO:  Determine if this method is even a good idea, since we want as much entropy as possible when generating the master key
		// Gibson suggests using data from accelerometers and cameras in order to generate this value
		public byte[] GenerateMasterKey()
		{
			throw new System.NotImplementedException();
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterKey, string url)
		{
			var domain = GetDomainFromUrl(url);
			var privateKey = _hmacGenerator.GeneratePrivateKey(masterKey, domain);

			return new SqrlData
				{
					Url = url,
					Signature = _signer.Sign(privateKey, GetUrlWithoutProtocol(url)),
					PublicKey = _signer.MakePublicKey(privateKey)
				};
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterIdentityKey, string password, byte[] salt, string url)
		{
			var masterKey = CalculateMasterKey(masterIdentityKey, password, salt);
			return GetSqrlDataForLogin(masterKey, url);
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

		private string GetUrlWithoutProtocol(string url)
		{
			// only use this variable for validity checking, never for any cryptographic features because ToLower() will modify nonces
			var lowerUrl = url.ToLower();

			if(lowerUrl.StartsWith("sqrl://"))
			{
				return url.Substring(7);
			}

			if(lowerUrl.StartsWith("qrl://"))
			{
				return url.Substring(6);
			}

			throw new Exception("SQRL urls must begin with 'sqrl://' or 'qrl://'");
		}

		private string GetDomainFromUrl(string url)
		{
			// only use this variable for validity checking, never for any cryptographic features because ToLower() will modify nonces
			var lowerUrl = url.ToLower();

			if(!lowerUrl.StartsWith("sqrl://") && !lowerUrl.StartsWith("qrl://"))
			{
				throw new Exception("SQRL urls must begin with 'sqrl://' or 'qrl://'");
			}

			// strip off scheme
			var domain = url.Substring(url.IndexOf("://") + 3);

			var pipeIndex = domain.IndexOf('|');

			if(pipeIndex >= 0)
			{
				return domain.Substring(0, pipeIndex);
			}

			var slashIndex = domain.IndexOf('/');

			if(slashIndex < 0)
			{
				throw new Exception("SQRL urls must contain a '/'");
			}

			return domain.Substring(0, slashIndex);
		}

		#endregion
	}
}