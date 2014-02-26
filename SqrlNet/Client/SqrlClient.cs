using System;
using System.Security.Cryptography;
using SqrlNet.Crypto;

namespace SqrlNet.Client
{
	/// <summary>
	/// This class provides all the SQRL functionality needed to implement a SQRL client.
	/// </summary>
	public class SqrlClient : ISqrlClient
	{
		#region Dependencies

		private readonly IPbkdfHandler _pbkdfHandler;
		private readonly IHmacGenerator _hmacGenerator;
		private readonly ISqrlSigner _signer;
		private readonly ISqrlPseudoRandomNumberGenerator _prng;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SqrlNet.Client.SqrlClient"/> class.
		/// </summary>
		/// <param name='pbkdfHandler'>
		/// Pbkdf handler.
		/// </param>
		/// <param name='hmacGenerator'>
		/// Hmac generator.
		/// </param>
		/// <param name='signer'>
		/// The signer.
		/// </param>
		/// <param name='prng'>
		/// The pseudo random number generator.
		/// </param>
		public SqrlClient(
			IPbkdfHandler pbkdfHandler,
			IHmacGenerator hmacGenerator,
			ISqrlSigner signer,
			ISqrlPseudoRandomNumberGenerator prng)
		{
			_pbkdfHandler = pbkdfHandler;
			_hmacGenerator = hmacGenerator;
			_signer = signer;
			_prng = prng;
		}

		#endregion

		#region ISqrlClient implementation

		/// <summary>
		///  Calculates the master key that is used with the HMAC function to generate the private key for a domain. 
		/// </summary>
		/// <returns>
		///  The master key. 
		/// </returns>
		/// <param name='masterIdentityKey'>
		///  The master identity key that is stored on the client. 
		/// </param>
		/// <param name='password'>
		///  The password that converts the master identity key into the master key 
		/// </param>
		/// <param name='salt'>
		///  A salt for adding entropy to the password hash 
		/// </param>
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

			var masterKey = Utility.Xor(masterIdentityKey, passwordKey);

			Array.Clear(passwordKey, 0, passwordKey.Length);

			return masterKey;
		}

		/// <summary>
		///  Calculates the master identity key that is stored on the client. This is needed when changing passwords. 
		/// </summary>
		/// <returns>
		///  The master identity key to be stored on the client. 
		/// </returns>
		/// <param name='masterKey'>
		///  The master key that is normally used as input to the HMAC function. 
		/// </param>
		/// <param name='password'>
		///  The password to be used to calculate the master identity key. 
		/// </param>
		/// <param name='salt'>
		///  A salt for adding entropy to the password hash 
		/// </param>
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

			var masterIdentityKey = Utility.Xor(masterKey, passwordKey);

			Array.Clear(passwordKey, 0, passwordKey.Length);

			return masterIdentityKey;
		}

		/// <summary>
		///  Creates the remote unlock keys. 
		/// </summary>
		/// <param name='identityLockKey'>
		///  Identity lock key. 
		/// </param>
		/// <param name='verifyUnlockKey'>
		///  Verify unlock key. 
		/// </param>
		/// <param name='serverUnlockKey'>
		///  Server unlock key. 
		/// </param>
		public void CreateRemoteUnlockKeys(byte[] identityLockKey, out byte[] verifyUnlockKey, out byte[] serverUnlockKey)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///  Gets the sqrl data for login. 
		/// </summary>
		/// <returns>
		///  The sqrl data for login. 
		/// </returns>
		/// <param name='masterKey'>
		///  Master key. 
		/// </param>
		/// <param name='url'>
		///  The URL. 
		/// </param>
		public SqrlLoginData GetSqrlDataForLogin(byte[] masterKey, string url)
		{
			var domain = Utility.GetDomainFromUrl(url);
			var privateKey = _hmacGenerator.GeneratePrivateKey(masterKey, domain);

			var sqrlData = new SqrlLoginData
			{
				Url = Utility.GetUrlWithoutProtocol(url),
				Signature = _signer.Sign(privateKey, Utility.GetUrlWithoutProtocol(url)),
				PublicKey = _signer.MakePublicKey(privateKey)
			};

			Array.Clear(privateKey, 0, privateKey.Length);

			return sqrlData;
		}

		/// <summary>
		///  Gets the sqrl data for login. 
		/// </summary>
		/// <returns>
		///  The sqrl data for login. 
		/// </returns>
		/// <param name='masterIdentityKey'>
		///  Master identity key. 
		/// </param>
		/// <param name='password'>
		///  The password. 
		/// </param>
		/// <param name='salt'>
		///  The salt. 
		/// </param>
		/// <param name='url'>
		///  The URL. 
		/// </param>
		public SqrlLoginData GetSqrlDataForLogin(byte[] masterIdentityKey, string password, byte[] salt, string url)
		{
			var masterKey = CalculateMasterKey(masterIdentityKey, password, salt);
			var sqrlData = GetSqrlDataForLogin(masterKey, url);

			Array.Clear(masterKey, 0, masterKey.Length);

			return sqrlData;
		}

		/// <summary>
		///  Gets the sqrl data for login. 
		/// </summary>
		/// <returns>
		///  The sqrl data for login. 
		/// </returns>
		/// <param name='identity'>
		///  The identity. 
		/// </param>
		/// <param name='password'>
		///  The password. 
		/// </param>
		/// <param name='url'>
		///  The URL. 
		/// </param>
		public SqrlLoginData GetSqrlDataForLogin(SqrlIdentity identity, string password, string url)
		{
			return GetSqrlDataForLogin(identity.MasterIdentityKey, password, identity.Salt, url);
		}

		/// <summary>
		///  Creates an identity for use with SQRL. 
		/// </summary>
		/// <returns>
		///  All the data needed to define an identity. 
		/// </returns>
		/// <param name='password'>
		///  The password. 
		/// </param>
		/// <param name='entropy'>
		///  Random data from some non-deterministic source that allows for more secure master key generation. 
		/// </param>
		/// <param name='identityUnlockKey'>
		///  This is the super-secret unlock key that is never to be stored in the client, or on any other computer. It is
		/// intended to be turned into a QR code and hidden in a safe. 
		/// </param>
		public SqrlIdentity CreateIdentity(string password, byte[] entropy, out byte[] identityUnlockKey)
		{
			var identity = new SqrlIdentity();

			identity.Salt = new byte[8];
			var masterKey = new byte[32];

			var sha256 = SHA256Managed.Create();

			_prng.GetBytes(identity.Salt);
			_prng.GetBytes(masterKey);

			// XOR the generated master key with the entropy (making any potential backdoors in the implementation of the pseudo random number generator irrelevent)
			masterKey = Xor(masterKey, sha256.ComputeHash(entropy));

			// call the SCrypt PBKDF to create the password key
			var passwordKey = _pbkdfHandler.GeneratePasswordKey(password, identity.Salt);

			// get the partial hash for password verification
			identity.PartialPasswordHash = _pbkdfHandler.GetPartialHashFromPasswordKey(passwordKey);

			// XOR the master key and the password key to get the master identity key
			identity.MasterIdentityKey = Utility.Xor(passwordKey, masterKey);

			Array.Clear(masterKey, 0, masterKey.Length);
			Array.Clear(passwordKey, 0, passwordKey.Length);

			// generate identity unlock key
			identityUnlockKey = new byte[32];
			_prng.GetBytes(identityUnlockKey);

			// create a public key from this secret identity unlock key
			identity.IdentityLockKey = _signer.MakePublicKey(identityUnlockKey);

			return identity;
		}

		/// <summary>
		///  Changes the password. 
		/// </summary>
		/// <returns>
		///  The new identity to be stored 
		/// </returns>
		/// <param name='oldPassword'>
		///  Old password. 
		/// </param>
		/// <param name='oldSalt'>
		///  Old salt. 
		/// </param>
		/// <param name='newPassword'>
		///  New password. 
		/// </param>
		/// <param name='masterIdentityKey'>
		///  Master identity key. 
		/// </param>
		public SqrlIdentity ChangePassword(string oldPassword, byte[] oldSalt, string newPassword, byte[] masterIdentityKey)
		{
			var identity = new SqrlIdentity();

			// calculate the master key
			var oldPasswordKey = _pbkdfHandler.GeneratePasswordKey(oldPassword, oldSalt);
			var masterKey = Utility.Xor(oldPasswordKey, masterIdentityKey);

			// generate new salt
			identity.Salt = new byte[8];
			_prng.GetBytes(identity.Salt);

			// generate the new password key
			var newPasswordKey = _pbkdfHandler.GeneratePasswordKey(newPassword, identity.Salt);

			// get the partial hash for password verification
			identity.PartialPasswordHash = _pbkdfHandler.GetPartialHashFromPasswordKey(newPasswordKey);

			// XOR the master key and the new password key to get the master identity key
			identity.MasterIdentityKey = Utility.Xor(newPasswordKey, masterKey);

			Array.Clear(masterKey, 0, masterKey.Length);
			Array.Clear(oldPasswordKey, 0, oldPasswordKey.Length);
			Array.Clear(newPasswordKey, 0, newPasswordKey.Length);

			return identity;
		}

		/// <summary>
		///  Verifies the password. 
		/// </summary>
		/// <returns>
		///  True if the password is correct. 
		/// </returns>
		/// <param name='password'>
		///  The password. 
		/// </param>
		/// <param name='identity'>
		///  The identity to verify against. 
		/// </param>
		public bool VerifyPassword(string password, SqrlIdentity identity)
		{
			return _pbkdfHandler.VerifyPassword(password, identity.Salt, identity.PartialPasswordHash);
		}

		/// <summary>
		///  Gets the domain from URL using the rules that SQRL uses for defining a domain (such as vertical bars). 
		/// </summary>
		/// <returns>
		///  The domain from URL. 
		/// </returns>
		/// <param name='url'>
		///  The URL. 
		/// </param>
		public string GetDomainFromUrl(string url)
		{
			return Utility.GetDomainFromUrl(url);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// XOR two byte arrays of equal length.
		/// </summary>
		/// <param name='a'>
		/// A byte array.
		/// </param>
		/// <param name='b'>
		/// Another byte array.
		/// </param>
		/// <returns>
		/// The result of the XOR operation of the two parameters
		/// </returns>
		private byte[] Xor(byte[] a, byte[] b)
		{
			if(a.Length != b.Length)
			{
				throw new Exception("a and b must be of the same length");
			}

			var result = new byte[a.Length];

			for(int i = 0; i < a.Length; i++)
			{
				result[i] = (byte)(a[i] ^ b[i]);
			}

			return result;
		}

		#endregion
	}
}