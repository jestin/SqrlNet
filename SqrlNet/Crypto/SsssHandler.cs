using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SqrlNet.Crypto
{
	public class SsssHandler : ISsssHandler
	{
		#region Private Variables

		private RNGCryptoServiceProvider _rng;

		#endregion

		#region Public Properties

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
						share.Value[cur] += (byte)(coefs[exp] * Math.Pow(share.Key, exp));
					}
				}
			}

			return shares;
		}

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

		public byte ResolveByte(IDictionary<int, byte> shares)
		{
			byte result = 0;

			foreach(var a in shares)
			{
				int numerator = 1;
				int denominator = 1;

				foreach(var b in shares)
				{
					if(a.Key == b.Key) continue;

					numerator = (numerator * -b.Key);
					denominator *= (a.Key - b.Key);
				}

				result += (byte)(a.Value * (numerator / denominator));
			}

			return result;
		}

		#endregion
	}
}

