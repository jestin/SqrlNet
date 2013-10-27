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
		/// Verifies the message was signed with the private key that matches this public key.
		/// </summary>
		/// <param name='publicKey'>
		/// Public key.
		/// </param>
		/// <param name='signedMessage'>
		/// Signed message.
		/// </param>
		byte[] Verify(byte[] publicKey, byte[] signedMessage);

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