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

		#endregion
	}
}