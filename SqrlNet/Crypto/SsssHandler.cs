using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Numerics;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// This is an implementation of Shamir's Secret Sharing Scheme, which can break up a secret
	/// into several parts, and can restore the secret from a specified number of he parts.
	/// WARNING:  This implementation is not yet functional!  DO NOT USE!
	/// </summary>
	public class SsssHandler : ISsssHandler
	{
		#region Private Variables

		/// <summary>
		/// The pseudo random number generator.  Since this is Microsoft's implementation, I may want to
		/// consider swapping it for a more trustworty prng.  In light of the Snowden leaks, closed source
		/// implementations of cryptography from large companies cannot be trusted.
		/// </summary>
		private RNGCryptoServiceProvider _rng;

		/// <summary>
		/// A large print to use with SSSS.
		/// </summary>
		private BigInteger _prime = BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457584007913129640233");

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the pseudo random number generator.
		/// </summary>
		/// <value>
		/// The pseudo random number generator.
		/// </value>
		public RNGCryptoServiceProvider Rng
		{
			private get
			{
				if(_rng == null)
				{
					_rng = new RNGCryptoServiceProvider();
				}

				return _rng;
			}

			set
			{
				_rng = value;
			}
		}

		#endregion

		#region ISsssHandler implementation

		/// <summary>
		///  Split the secret into a number of shares, such that it can be reconstructed by a subset of the total shares. 
		/// </summary>
		/// <param name='secret'>
		///  The secret to be split into parts. 
		/// </param>
		/// <param name='threshold'>
		///  The minimum number of shares required to reconstruct the secret. 
		/// </param>
		/// <param name='numShares'>
		///  The number of shares to divide the secret into. 
		/// </param>
		/// <returns>
		///  A dictionary of the shares where the key is the x-coordinate and the value is the y-coordinate 
		/// </returns>
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

			if(threshold > numShares)
			{
				throw new ArgumentException("The threshold cannot be larger than the number of shares", "threshold");
			}

			var secretInt = PositiveBigIntegerFromBytes(secret);

			var shares = new Dictionary<int, byte[]>();

			var coefs = new BigInteger[threshold];

			for(var i = 0; i < coefs.Length; i++)
			{
				coefs[i] = RandomBigIntegerBelow(_prime);
			}

			// grab points
			for(int xCoord = 1; xCoord <= numShares; xCoord++)
			{
				// start with the constant term
				var intValue = secretInt;

				// add in all the other terms in the polynomial
				for(int exp = 1; exp < threshold; exp++)
				{
					intValue += BigInteger.Pow(new BigInteger(xCoord), exp);
				}

				var shareValue = intValue.ToByteArray();

				// ensures that all shares are equal to the size of the original secret
				Array.Resize(ref shareValue, secret.Length);
				shares[xCoord] = shareValue;
			}

			return shares;
		}

		/// <summary>
		///  Restore the secret given the threshhold number, and that number of shares. 
		/// </summary>
		/// <param name='shares'>
		///  The shares being used to reconstruct the secret. 
		/// </param>
		/// <returns>
		///  The reconstructed secret. 
		/// </returns>
		public byte[] Restore(IDictionary<int, byte[]> shares)
		{
			// get the size of the shares for padding later
			int shareLength = shares.First().Value.Count();

			// Calculate P_k(x) = \sum_{i=0}^{i<k} y_i * l_i(x)
			// We're only interested in P_k(0).
			var intercept = new BigInteger(0);

			// Need to compute the lagrange bases.
			foreach(var firstShare in shares)
			{
				// The x value from first coordinate.
				var firstX = new BigInteger(firstShare.Key);

				// To keep track of the numerator and denominator of the Lagrange basis.
				var numerator = new BigInteger(1);
				var denominator = new BigInteger(1);

				// Loop through all of the other coordinates to make the Lagrange basis.
				foreach(var secondShare in shares)
				{
					// The x value of the second coordinate.
					var secondX = new BigInteger(secondShare.Key);

					// We don't want to look at pairs that are the same.
					if(firstShare.Key != secondShare.Key)
					{
						// Using BigNumbers here helps with precision.
						// We only care about x = 0, so we do 0-secondX rather than x-secondX.
						numerator = numerator * -secondX;
						denominator = denominator * (firstX - secondX);
					}
				}

				// We multiply this way to minimize the amount of error we get from
				// dividing a smaller number by the denominator.
				intercept += (PositiveBigIntegerFromBytes(firstShare.Value) * numerator) / denominator;
				//intercept += (PositiveBigIntegerFromBytes(firstShare.Value) * numerator) * ModInverse(denominator);
			}

			var result = intercept % _prime;

			Console.Error.WriteLine("result {0}", result);

			var original = result.ToByteArray();

			// ensure that the array gets padded with zeros if it's too small
			Array.Resize(ref original, shareLength);

			// ensure that negative numbers that got padded use 0xFF instead
			if(result.Sign < 0 && original[shareLength - 1] == 0x00)
			{
				original[shareLength - 1] = 0xFF;
			}

			return original;
		}

		#endregion

		#region Other Public Methods

		public BigInteger RandomBigIntegerBelow(BigInteger n)
		{
			byte[] bytes = n.ToByteArray();
			BigInteger r;

			do
			{
				Rng.GetBytes(bytes);
				bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
				r = new BigInteger(bytes);
			} while(r >= n);

			return r;
		}

		public BigInteger PositiveBigIntegerFromBytes(byte[] bytes)
		{
			// create the BigInteger by appending 0x00 so that it becomes a positive integer
			var modifiedBytes = new byte[bytes.Length + 1];
			Buffer.BlockCopy(bytes, 0, modifiedBytes, 0, bytes.Length);
			modifiedBytes[bytes.Length] = 0x00;

			try
			{
				return new BigInteger(modifiedBytes);
			}
			catch(IndexOutOfRangeException)
			{
				// account for a bug in the BigInteger constructor
				return new BigInteger(0);
			}
		}

		public BigInteger[] GreatestCommonDivisorDecomposition(BigInteger a,BigInteger b)
		{ 
			if (b == 0)
			{
				return new BigInteger[] { a, new BigInteger(1), new BigInteger(0) };
			}
			else
			{ 
				var n = a / b;
				var c = a % b;
				var r = GreatestCommonDivisorDecomposition(b, c); 
				return new BigInteger[] { r[0], r[2], r[1] - r[2] * n };
			}
		}

		public BigInteger ModInverse(BigInteger k)
		{ 
			k = k % _prime;
			var r = (k < 0) ? -GreatestCommonDivisorDecomposition(_prime, -k)[2]
							: GreatestCommonDivisorDecomposition(_prime, k)[2];
			return (_prime + r) % _prime;
		}

		#endregion
	}
}