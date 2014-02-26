namespace SqrlNet.Crypto
{
	/// <summary>
	/// Interface for classes that implement a Diffie-Hellman key agreement function.
	/// </summary>
	public interface IDiffieHellmanHandler
	{
		/// <summary>
		/// Creates a key that that can be used with a symetric cipher.
		/// </summary>
		/// <returns>
		/// The key.
		/// </returns>
		/// <param name='publicKey'>
		/// The public key to use in the key calculation, which should come from the
		/// opposite source as the private key.
		/// </param>
		/// <param name='privateKey'>
		/// The private key to use in the key calculation, which should come from the
		/// opposite source as the public key.
		/// </param>
		byte[] CreateKey(byte[] publicKey, byte[] privateKey);
	}
}