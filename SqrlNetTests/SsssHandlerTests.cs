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
		private const int Repititions = 100;

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
		[Repeat(Repititions)]
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
		[Repeat(Repititions)]
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

			// This one fails
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
		[Repeat(Repititions)]
		public void Three_Three_Scheme()
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, 3, 3);
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(shares)));
		}

		[Test]
		[Repeat(Repititions)]
		public void Two_Four_Scheme()
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, 2, 4);

			var oneTwo = new Dictionary<int, byte[]>();
			oneTwo[1] = shares[1];
			oneTwo[2] = shares[2];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneTwo)));

			// This one fails
			var oneThree = new Dictionary<int, byte[]>();
			oneThree[1] = shares[1];
			oneThree[3] = shares[3];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneThree)));

			// This one fails
			var oneFour = new Dictionary<int, byte[]>();
			oneFour[1] = shares[1];
			oneFour[4] = shares[4];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneFour)));

			var twoThree = new Dictionary<int, byte[]>();
			twoThree[2] = shares[2];
			twoThree[3] = shares[3];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(twoThree)));

			var twoFour = new Dictionary<int, byte[]>();
			twoFour[2] = shares[2];
			twoFour[4] = shares[4];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(twoFour)));

			var threeFour = new Dictionary<int, byte[]>();
			threeFour[3] = shares[3];
			threeFour[4] = shares[4];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(threeFour)));
		}

		[Test]
		[Repeat(Repititions)]
		public void Four_Four_Scheme()
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, 4, 4);
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(shares)));
		}

		[Test]
		public void Each_Byte_Two_Three_Scheme_With_Shares_One_And_Three()
		{
			for(byte secretByte = 0; secretByte <= 255; secretByte++)
			{
				var secret = new byte[1];
				secret[0] = secretByte;
				var shares = _handler.Split(secret, 2, 3);

				var oneThree = new Dictionary<int, byte[]>();
				oneThree[1] = shares[1];
				oneThree[3] = shares[3];

				var restoredSecret = _handler.Restore(oneThree);

				Console.WriteLine("****************");
				Console.WriteLine("Secret: {0}", secretByte);
				Console.WriteLine("Restored: {0}", restoredSecret[0]);
				Console.WriteLine("Shares: (1, {0}) (2, {1}) (3, {2})", shares[1][0], shares[2][0], shares[3][0]);
				Console.WriteLine("****************");

				Assert.AreEqual(secretByte, restoredSecret[0]);
			}
		}

		[Test]
		public void RestoreByte()
		{
			var shares = new Dictionary<int, byte>();

			shares[1] = 141;
			shares[3] = 167;

			//shares[1] = 130;
			//shares[3] = 134;

			//shares[1] = 140;
			//shares[3] = 164;

			var restored = _handler.ResolveByte(shares);
			Console.WriteLine("Restored: {0}", restored);

			Assert.AreEqual(0, restored);
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

