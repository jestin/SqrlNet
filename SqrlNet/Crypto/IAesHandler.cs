using System;

namespace SqrlNet.Crypto
{
	public interface IAesHandler
	{
		/// <summary>
		/// Encrypt the message with the specified key and initialization vector.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='iv'>
		/// Initialization vector.
		/// </param>
		/// <param name='message'>
		/// Message.
		/// </param>
		byte[] Encrypt(byte[] key, byte[] iv, byte[] message);

		/// <summary>
		/// Decrypt the encrypted message with the specified key and initialization vector.
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		/// <param name='iv'>
		/// Initialization vector.
		/// </param>
		/// <param name='encryptedMessage'>
		/// Encrypted message.
		/// </param>
		byte[] Decrypt(byte[] key, byte[] iv, byte[] encryptedMessage);
	}
}

