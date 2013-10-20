using System;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides the HMAC functionality for the SQRL process
	/// </summary>
	public interface IHmacGenerator
	{
		/// <summary>
		/// Generates the private key for a specific domain, based on the master key.
		/// </summary>
		/// <returns>
		/// The private key.
		/// </returns>
		/// <param name='masterKey'>
		/// Master key.
		/// </param>
		/// <param name='domain'>
		/// Domain.
		/// </param>
		byte[] GeneratePrivateKey(byte[] masterKey, string domain);
	}
}

