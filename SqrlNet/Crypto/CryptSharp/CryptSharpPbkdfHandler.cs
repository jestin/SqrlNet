using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CryptSharp.Utility;

namespace SqrlNet.Crypto.CryptSharp
{
	/// <summary>
	/// Provides all the PBKDF functionality needed by the SQRL process, implemented with
	/// SCRYPT using the CryptSharp library.
	/// </summary>
	public class CryptSharpPbkdfHandler : IPbkdfHandler
	{
		#region IPbkdfHandler implementation

		/// <summary>
		///  Generates the password key. 
		/// </summary>
		/// <returns>
		///  The password key. 
		/// </returns>
		/// <param name='password'>
		///  The password. 
		/// </param>
		/// <param name='salt'>
		///  The salt. 
		/// </param>
		/// <param name='iterations'>
		///  The number of iterations. 
		/// </param>
		/// <param name='onIterationComplete'>
		///  A callback delegate that runs at the completion of each iteration 
		/// </param>
		public byte[] GeneratePasswordKey(string password, byte[] salt, int iterations = 1, IterationDelegate onIterationComplete = null)
		{
			var key = new byte[32];
			byte[] inputKey = String.IsNullOrEmpty(password) ? Utility.GetZeroBytes(32) : Encoding.UTF8.GetBytes(password);
			var runningKey = Utility.GetZeroBytes(32);
			var runningSalt = Utility.GetZeroBytes(32);

			// copy salt
			if(salt != null)
			{
				Buffer.BlockCopy(salt, 0, runningSalt, 0, salt.Length);
			}

			// run SCRYPT in a loop
			for(int i = 0; i < iterations; i++)
			{
				SCrypt.ComputeKey(
					inputKey,
					runningSalt,
					512,
					256,
					1,
					null,
					key);

				runningKey = Utility.Xor(runningKey, key);
				Buffer.BlockCopy(key, 0, runningSalt, 0, 32);

				if(onIterationComplete != null)
				{
					onIterationComplete(i);
				}
			}

			return runningKey;
		}

		/// <summary>
		///  Verifies the password. 
		/// </summary>
		/// <returns>
		///  Returns true if the password given matches the partial hash given 
		/// </returns>
		/// <param name='password'>
		///  The password to be verified. 
		/// </param>
		/// <param name='salt'>
		///  The salt added to the password. 
		/// </param>
		/// <param name='partialHash'>
		///  The lower 128 bits of a hash of the output of the PBKDF (GeneratePasswordKey). 
		/// </param>
		/// <param name='iterations'>
		/// The number of iterations to run through.
		/// </param>
		public bool VerifyPassword(string password, byte[] salt, byte[] partialHash, int iterations = 1)
		{
			var passwordKey = GeneratePasswordKey(password, salt, iterations);
			return partialHash.SequenceEqual(GetPartialHashFromPasswordKey(passwordKey));
		}

		/// <summary>
		///  Gets the partial hash used for password verification from the password key generated from the PBKDF. 
		/// </summary>
		/// <returns>
		///  The partial hash from password key. 
		/// </returns>
		/// <param name='passwordKey'>
		///  Password key. 
		/// </param>
		public byte[] GetPartialHashFromPasswordKey(byte[] passwordKey)
		{
			var sha256 = SHA256Managed.Create();
			var hash = sha256.ComputeHash(passwordKey);
			return new ArraySegment<byte>(hash, 0, 16).Array;
		}

		#endregion
	}
}