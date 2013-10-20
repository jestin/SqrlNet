using System;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides the cryptographic signature functionality for the SQRL process.
	/// </summary>
	public interface ISqrlSigner
	{
		/// <summary>
		/// Sign the specified message with the private key.
		/// </summary>
		/// <param name='privateKey'>
		/// Private key.
		/// </param>
		/// <param name='message'>
		/// Message.
		/// </param>
		byte[] Sign(byte[] privateKey, byte[] message);

		/// <summary>
		/// Sign the specified message with the private key.
		/// </summary>
		/// <param name='privateKey'>
		/// Private key.
		/// </param>
		/// <param name='message'>
		/// Message.
		/// </param>
		byte[] Sign(byte[] privateKey, string message);

		/// <summary>
		/// Makes the public key from the provider private key.
		/// </summary>
		/// <returns>
		/// The public key.
		/// </returns>
		/// <param name='privateKey'>
		/// Private key.
		/// </param>
		byte[] MakePublicKey(byte[] privateKey);
	}
}