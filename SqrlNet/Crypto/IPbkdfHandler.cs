using System;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides all the PBKDF functionality needed by the SQRL process.
	/// </summary>
	public interface IPbkdfHandler
	{
		/// <summary>
		/// Generates the password key.
		/// </summary>
		/// <returns>
		/// The password key.
		/// </returns>
		/// <param name='password'>
		/// Password.
		/// </param>
		/// <param name='salt'>
		/// Salt.
		/// </param>
		byte[] GeneratePasswordKey(string password, byte[] salt);

		/// <summary>
		/// Verifies the password.
		/// </summary>
		/// <returns>
		/// Returns true if the password given matches the partial hash given
		/// </returns>
		/// <param name='password'>
		/// The password to be verified.
		/// </param>
		/// <param name='salt'>
		/// The salt added to the password.
		/// </param>
		/// <param name='partialHash'>
		/// The lower 128 bits of a hash of the output of the PBKDF (GeneratePasswordKey).
		/// </param>
		bool VerifyPassword(string password, byte[] salt, byte[] partialHash);

		/// <summary>
		/// Gets the partial hash used for password verification from the password key generated from the PBKDF.
		/// </summary>
		/// <returns>
		/// The partial hash from password key.
		/// </returns>
		/// <param name='passwordKey'>
		/// Password key.
		/// </param>
		byte[] GetPartialHashFromPasswordKey(byte[] passwordKey);
	}
}

