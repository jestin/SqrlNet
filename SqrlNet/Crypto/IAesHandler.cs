namespace SqrlNet.Crypto
{
	/// <summary>
	/// An interface for handling AES (Rijndael) encryption and decryption.
	/// </summary>
	public interface IAesHandler
	{
		/// <summary>
		/// Encrypt the message with the specified key and initialization vector.
		/// </summary>
		/// <param name='key'>
		/// The key.
		/// </param>
		/// <param name='iv'>
		/// The initialization vector.
		/// </param>
		/// <param name='message'>
		/// The message.
		/// </param>
		/// <returns>
		/// Returns the encrypted message.
		/// </returns>
		byte[] Encrypt(byte[] key, byte[] iv, byte[] message);

		/// <summary>
		/// Decrypt the encrypted message with the specified key and initialization vector.
		/// </summary>
		/// <param name='key'>
		/// The key.
		/// </param>
		/// <param name='iv'>
		/// The initialization vector.
		/// </param>
		/// <param name='encryptedMessage'>
		/// The encrypted message.
		/// </param>
		/// <returns>
		/// The decrypted message.
		/// </returns>
		byte[] Decrypt(byte[] key, byte[] iv, byte[] encryptedMessage);
	}
}