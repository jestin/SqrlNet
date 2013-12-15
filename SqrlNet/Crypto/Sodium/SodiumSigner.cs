using System.Text;
using Sodium;

namespace SqrlNet.Crypto.Sodium
{
	/// <summary>
	/// Provides the cryptographic signature functionality for the SQRL process,
	/// implemented with the libsodium-net library.
	/// </summary>
	public class SodiumSigner : ISqrlSigner
	{
		#region Private Properties

		/// <summary>
		/// Gets or sets the keys.  Since the primary key passed into this class through
		/// the methods is really just a seed to generate the keypair, this property is
		/// used as a local cache.
		/// </summary>
		/// <value>
		/// The keys.
		/// </value>
		private KeyPair Keys { get; set; }

		#endregion

		#region ISqrlSigner implementation

		/// <summary>
		///  Sign the specified message with the private key. 
		/// </summary>
		/// <param name='privateKey'>
		///  The private key. 
		/// </param>
		/// <param name='message'>
		///  The message. 
		/// </param>
		/// <returns>
		///  The message that has been signed (encrypted) by the private key. 
		/// </returns>
		public byte[] Sign(byte[] privateKey, byte[] message)
		{
			if(Keys == null)
			{
				Keys = PublicKeyAuth.GenerateKeyPair(privateKey);
			}

			return PublicKeyAuth.Sign(message, Keys.PrivateKey);
		}

		/// <summary>
		///  Sign the specified message with the private key. 
		/// </summary>
		/// <param name='privateKey'>
		///  The private key. 
		/// </param>
		/// <param name='message'>
		///  The message. 
		/// </param>
		/// <returns>
		///  The message that has been signed (encrypted) by the private key. 
		/// </returns>
		public byte[] Sign(byte[] privateKey, string message)
		{
			return Sign(privateKey, Encoding.UTF8.GetBytes(message));
		}

		/// <summary>
		///  Verifies the message was signed with the private key that matches this public key. 
		/// </summary>
		/// <param name='publicKey'>
		///  Public key. 
		/// </param>
		/// <param name='signedMessage'>
		///  Signed message. 
		/// </param>
		/// <returns>
		///  The decrypted (verified) message. 
		/// </returns>
		public byte[] Verify(byte[] publicKey, byte[] signedMessage)
		{
			return PublicKeyAuth.Verify(signedMessage, publicKey);
		}

		/// <summary>
		///  Makes the public key from the provider private key. 
		/// </summary>
		/// <returns>
		///  The public key. 
		/// </returns>
		/// <param name='privateKey'>
		///  Private key. 
		/// </param>
		public byte[] MakePublicKey(byte[] privateKey)
		{
			Keys = PublicKeyAuth.GenerateKeyPair(privateKey);
			return Keys.PublicKey;
		}

		#endregion
	}
}