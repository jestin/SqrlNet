using System;
using System.Collections.Generic;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// This is an implementation of Shamir's Secret Sharing Scheme, which can break up a secret
	/// into several parts, and can restore the secret from a specified number of he parts.
	/// </summary>
	public interface ISsssHandler
	{
		/// <summary>
		/// Split the secret into a number of shares, such that it can be reconstructed by a
		/// subset of the total shares.
		/// </summary>
		/// <param name='secret'>
		/// The secret to be split into parts.
		/// </param>
		/// <param name='threshold'>
		/// The minimum number of shares required to reconstruct the secret.
		/// </param>
		/// <param name='numShares'>
		/// The number of shares to divide the secret into.
		/// </param>
		/// <returns>
		/// A dictionary of the shares where the key is the x-coordinate and the value is the y-coordinate
		/// </returns>
		IDictionary<int, byte[]> Split(byte[] secret, int threshold, int numShares);

		/// <summary>
		/// Restore the secret given the threshhold number, and that number of shares.
		/// </summary>
		/// <param name='threshold'>
		/// The threshhold that was used when splitting the secret into shares.
		/// </param>
		/// <param name='shares'>
		/// The shares being used to reconstruct the secret.
		/// </param>
		/// <returns>
		/// The reconstructed secret.
		/// </returns>
		byte[] Restore(int threshold, IDictionary<int, byte[]> shares);
	}
}

