using System;
using Sodium;

namespace SqrlNet.Crypto.Sodium
{
	/// <summary>
	/// Handler for Diffie-Hellman Key Agreement implemented with libsodium
	/// </summary>
	public class SodiumDiffieHellmanHandler : IDiffieHellmanHandler
	{
		#region IDiffieHellmanHandler implementation

		/// <summary>
		///  Creates a key that that can be used with a symetric cipher. 
		/// </summary>
		/// <returns>
		///  The key. 
		/// </returns>
		/// <param name='publicKey'>
		///  The public key to use in the key calculation, which should come from the opposite source as the private key. 
		/// </param>
		/// <param name='privateKey'>
		///  The private key to use in the key calculation, which should come from the opposite source as the public key. 
		/// </param>
		public byte[] CreateKey(byte[] publicKey, byte[] privateKey)
		{
			var key = new byte[32];
			ScalarMult.Mult(key, privateKey, publicKey);

			return key;
		}

		/// <summary>
		/// Makes the public key.
		/// </summary>
		/// <returns>A Curve25519 public key that can be used for a Diffie-Hellman exchange</returns>
		/// <param name="privateKey">Private key.</param>
		public byte[] MakePublicKey(byte[] privateKey)
		{
			var publicKey = new byte[32];
			ScalarMult.Base(publicKey, privateKey);

			return publicKey;
		}

		#endregion
	}
}