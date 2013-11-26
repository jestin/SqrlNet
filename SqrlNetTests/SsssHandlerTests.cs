using System;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using SqrlNet.Crypto;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SqrlNetTests
{
	[TestFixture]
	public class SsssHandlerTests
	{
		private SsssHandler _handler;

		[SetUp]
		public void Setup()
		{
			_handler = new SsssHandler();
		}

		[TearDown]
		public void TearDown()
		{
		}

		#region Tests

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "The threshold cannot be larger than the number of shares\nParameter name: threshold")]
		public void Split_Bad_Threshold()
		{
			_handler.Split(new byte[32], 5, 4);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "The secret cannot be null\nParameter name: secret")]
		public void Split_Null_Secret()
		{
			_handler.Split(null, 3, 4);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "The secret cannot be empty\nParameter name: secret")]
		public void Split_Empty_Secret()
		{
			_handler.Split(new byte[0], 3, 4);
		}

		[Test]
		[Repeat(1000)]
		public void Two_Two_Scheme()
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, 2, 2);
			Console.Error.WriteLine(Convert.ToBase64String(secret));

			var restoredSecret = _handler.Restore(shares);
			Console.Error.WriteLine(Convert.ToBase64String(restoredSecret));
			Assert.IsTrue(secret.SequenceEqual(restoredSecret));
		}

		[Test]
		[Repeat(1000)]
		public void Two_Three_Scheme()
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, 2, 3);

			var oneTwo = new Dictionary<int, byte[]>();
			oneTwo[1] = shares[1];
			oneTwo[2] = shares[2];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneTwo)));

			var oneThree = new Dictionary<int, byte[]>();
			oneThree[1] = shares[1];
			oneThree[3] = shares[3];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneThree)));

			var twoThree = new Dictionary<int, byte[]>();
			twoThree[2] = shares[2];
			twoThree[3] = shares[3];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(twoThree)));
		}

		[Test]
		[Ignore]
		public void Each_Byte_Each_Scheme()
		{
			for(var numShares = 2; numShares < 256; numShares++)
			{
				for(var threshold = 2; threshold <= numShares; threshold++)
				{
					for(var theByte = 0; theByte < 256; theByte++)
					{
						var secret = new Byte[1] { (byte)theByte };
						var shares = _handler.Split(secret, threshold, numShares);

						foreach(var permutation in GetAllPermutations(shares, threshold))
						{
							var restoredSecret = _handler.Restore(permutation);
							Assert.IsTrue(secret.SequenceEqual(restoredSecret));
						}
					}
				}
			}
		}

		#endregion

		#region Private Methods

		private IEnumerable<IDictionary<int, byte[]>> GetAllPermutations(IDictionary<int, byte[]> allShares, int threshold)
		{
			return new Collection<IDictionary<int, byte[]>>();
		}

		#endregion
	}
}

