using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Numerics;

namespace SqrlNet.Crypto
{
	public class SsssHandler : ISsssHandler
	{
		private int _prime = 257;

		#region ISsssHandler implementation

		public IDictionary<int, byte[]> Split(byte[] secret, int threshold, int numShares)
		{
			if(secret == null)
			{
				throw new ArgumentException("The secret cannot be null", "secret");
			}

			if(secret.Length == 0)
			{
				throw new ArgumentException("The secret cannot be empty", "secret");
			}

			if(numShares >= _prime)
			{
				throw new ArgumentException("The number of shares cannot be larger or eqaul to 257", "numShares");
			}

			if(threshold > numShares)
			{
				throw new ArgumentException("The threshold cannot be larger than the number of shares", "threshold");
			}

			var coefs = new BigInteger[threshold];
			var rng = new RNGCryptoServiceProvider();

			// set the secret to the first coefficient
			coefs[0] = new BigInteger(secret);

			// generate random numbers for the rest of the coefficients
			for(int i = 1; i < threshold - 1; i++)
			{
				var bytes = new byte[32];
				rng.GetBytes(bytes);
				coefs[i] = new BigInteger(bytes);
			}

			// grab points
			var shares = new Dictionary<int, byte[]>();

			for(int shareIndex = 0; shareIndex < numShares; shareIndex++)
			{
				// start the accumulator as the first coefficient (the secret)
				var accum = coefs[0];

				for(int exp = 1; exp < threshold; exp++)
				{
					accum = (accum + (coefs[exp] * (BigInteger.Pow(new BigInteger(shareIndex + 1), exp) % _prime) % _prime)) % _prime;
				}

				shares[shareIndex] = accum.ToByteArray();
			}

			return shares;
		}

		public byte[] Restore(int threshold, IDictionary<int, byte[]> shares)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

