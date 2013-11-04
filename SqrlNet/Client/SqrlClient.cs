using System;
using System.Security.Cryptography;
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

			var masterKey =  Xor(masterIdentityKey, passwordKey);

			Array.Clear(passwordKey, 0, passwordKey.Length);

			return masterKey;
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

			var masterIdentityKey = Xor(masterKey, passwordKey);

			Array.Clear(passwordKey, 0, passwordKey.Length);

			return masterIdentityKey;
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterKey, string url)
		{
			var domain = Utility.GetDomainFromUrl(url);
			var privateKey = _hmacGenerator.GeneratePrivateKey(masterKey, domain);

			var sqrlData = new SqrlData
			{
				Domain = domain,
				Url = Utility.GetUrlWithoutProtocol(url),
				Signature = _signer.Sign(privateKey, Utility.GetUrlWithoutProtocol(url)),
				PublicKey = _signer.MakePublicKey(privateKey)
			};

			Array.Clear(privateKey, 0, privateKey.Length);

			return sqrlData;
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterIdentityKey, string password, byte[] salt, string url)
		{
			var masterKey = CalculateMasterKey(masterIdentityKey, password, salt);
			var sqrlData = GetSqrlDataForLogin(masterKey, url);

			Array.Clear(masterKey, 0, masterKey.Length);

			return sqrlData;
		}

		public SqrlData GetSqrlDataForLogin(SqrlIdentity identity, string password, string url)
		{
			return GetSqrlDataForLogin(identity.MasterIdentityKey, password, identity.Salt, url);
		}

		public SqrlIdentity CreateIdentity(string password, byte[] entropy)
		{
			var identity = new SqrlIdentity();

			identity.Salt = new byte[8];
			var masterKey = new byte[32];

			var rngCsp = new RNGCryptoServiceProvider();
			var sha256 = SHA256Managed.Create();

			rngCsp.GetBytes(identity.Salt);
			rngCsp.GetBytes(masterKey);

			// XOR the generated master key with the entropy (making any potential backdoors in the implementation of RNGCryptoServiceProvider irrelevent)
			masterKey = Xor(masterKey, sha256.ComputeHash(entropy));

			// call the SCrypt PBKDF to create the password key
			var passwordKey = _pbkdfHandler.GeneratePasswordKey(password, identity.Salt);

			// get the partial hash for password verification
			identity.PartialPasswordHash = _pbkdfHandler.GetPartialHashFromPasswordKey(passwordKey);

			// XOR the master key and the password key to get the master identity key
			identity.MasterIdentityKey = Xor(passwordKey, masterKey);

			Array.Clear(masterKey, 0, masterKey.Length);
			Array.Clear(passwordKey, 0, passwordKey.Length);

			return identity;
		}

		public SqrlIdentity ChangePassword(string oldPassword, byte[] oldSalt, string newPassword, byte[] masterIdentityKey)
		{
			var identity = new SqrlIdentity();
			var rngCsp = new RNGCryptoServiceProvider();

			// calculate the master key
			var oldPasswordKey = _pbkdfHandler.GeneratePasswordKey(oldPassword, oldSalt);
			var masterKey = Xor(oldPasswordKey, masterIdentityKey);

			// generate new salt
			identity.Salt = new byte[8];
			rngCsp.GetBytes(identity.Salt);

			// generate the new password key
			var newPasswordKey = _pbkdfHandler.GeneratePasswordKey(newPassword, identity.Salt);

			// get the partial hash for password verification
			identity.PartialPasswordHash = _pbkdfHandler.GetPartialHashFromPasswordKey(newPasswordKey);

			// XOR the master key and the new password key to get the master identity key
			identity.MasterIdentityKey = Xor(newPasswordKey, masterKey);

			Array.Clear(masterKey, 0, masterKey.Length);
			Array.Clear(oldPasswordKey, 0, oldPasswordKey.Length);
			Array.Clear(newPasswordKey, 0, newPasswordKey.Length);

			return identity;
		}

		public bool VerifyPassword(string password, SqrlIdentity identity)
		{
			return _pbkdfHandler.VerifyPassword(password, identity.Salt, identity.PartialPasswordHash);
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