namespace SqrlNet.Crypto
{
	/// <summary>
	/// Handler for Diffie-Hellman Key Agreement.
	/// </summary>
	public class DiffieHellmanHandler : IDiffieHellmanHandler
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
			throw new System.NotImplementedException();
		}

		#endregion
	}
}