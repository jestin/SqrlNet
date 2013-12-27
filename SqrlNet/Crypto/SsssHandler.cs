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

			var shares = new Dictionary<int, byte[]>();

			// initialize shares
			for(int i = 0; i < numShares; i++)
			{
				shares[i + 1] = new byte[secret.Length];
			}

			// loop through each byte of the secret
			for(var cur = 0; cur < secret.Length; cur++)
			{
				var coefs = new byte[threshold];

				// generate random coefficients
				Rng.GetBytes(coefs);

				// set the secret to the first coefficient
				coefs[0] = secret[cur];

				foreach(var coef in coefs)
				{
					Console.Error.WriteLine("coef: {0}", coef);
				}

				// grab points
				foreach(var share in shares)
				{
					share.Value[cur] = 0;

					for(int exp = 0; exp < threshold; exp++)
					{
						//share.Value[cur] ^= GFMul(coefs[exp], (byte)Math.Pow(share.Key, exp));
					}
				}
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
 			var length = shares.First().Value.Length;
			var result = new byte[length];

			for(var cur = 0; cur < length; cur++)
			{
				result[cur] = ResolveByte(shares.ToDictionary(x => x.Key, x => x.Value[cur]));
			}

			return result;
		}

		#endregion

		#region Other Public Methods

		/// <summary>
		/// Resolves an individual byte.
		/// </summary>
		/// <returns>
		/// The byte.
		/// </returns>
		/// <param name='shares'>
		/// The shares.
		/// </param>
		public byte ResolveByte(IDictionary<int, byte> shares)
		{
			byte result = 0;

			foreach(var a in shares)
			{
				byte numerator = 1;
				byte denominator = 1;

				foreach(var b in shares)
				{
					if(a.Key == b.Key)
					{
						continue;
					}

					Console.WriteLine("a: {0} {1}", a.Key, a.Value);
					Console.WriteLine("b: {0} {1}", b.Key, b.Value);

					//numerator = GFMul(numerator, (byte)-b.Key);
					//denominator = GFMul(denominator, (byte)((byte)a.Key ^ (byte)b.Key));

					Console.WriteLine("numerator: {0}", numerator);
					Console.WriteLine("denominator: {0}", denominator);
				}

				//result ^= GFDiv(GFMul(a.Value, numerator), denominator);
				Console.WriteLine("result: {0}", result);
			}

			return result;
		}

		#endregion
	}
}