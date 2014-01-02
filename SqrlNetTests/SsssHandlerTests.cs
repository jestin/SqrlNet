using System;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using SqrlNet.Crypto;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace SqrlNetTests
{
	[TestFixture]
	public class SsssHandlerTests
	{
		private SsssHandler _handler;
		private const int Repititions = 1000;

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
			//Console.Error.WriteLine("original {0} : {1}", new BigInteger(secret), Convert.ToBase64String(secret));
			Console.Error.WriteLine("original {0} : {1}", new BigInteger(secret), BitConverter.ToString(secret));

			var restoredSecret = _handler.Restore(shares);
			//Console.Error.WriteLine("restored {0} : {1}", new BigInteger(secret), Convert.ToBase64String(restoredSecret));
			Console.Error.WriteLine("restored {0} : {1}", new BigInteger(restoredSecret), BitConverter.ToString(restoredSecret));
			Assert.IsTrue(secret.SequenceEqual(restoredSecret));
		}

		[Test]
		[Repeat(Repititions)]
		public void Two_Two_Scheme_Single_Byte()
		{
			// generate secret
			var secret = new byte[1];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, 2, 2);
			Console.Error.WriteLine(Convert.ToInt32(secret[0]));

			var restoredSecret = _handler.Restore(shares);
			Console.Error.WriteLine(Convert.ToInt32(restoredSecret[0]));
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

			var oneThree = new Dictionary<int, byte[]>();
			oneThree[1] = shares[1];
			oneThree[3] = shares[3];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneThree)));

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
		public void Three_Four_Scheme()
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			Console.Error.WriteLine("original {0} : {1}", new BigInteger(secret), BitConverter.ToString(secret));

			// split secret
			var shares = _handler.Split(secret, 3, 4);

			var oneTwoThree = new Dictionary<int, byte[]>();
			oneTwoThree[1] = shares[1];
			oneTwoThree[2] = shares[2];
			oneTwoThree[3] = shares[3];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneTwoThree)));

			var oneThreeFour = new Dictionary<int, byte[]>();
			oneThreeFour[1] = shares[1];
			oneThreeFour[3] = shares[3];
			oneThreeFour[4] = shares[4];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(oneThreeFour)));

			var oneTwoFour = new Dictionary<int, byte[]>();
			oneTwoFour[1] = shares[1];
			oneTwoFour[2] = shares[2];
			oneTwoFour[4] = shares[4];
			var oneTwoFourResult = _handler.Restore(oneTwoFour);
			Console.Error.WriteLine("restored {0} : {1}", new BigInteger(oneTwoFourResult), BitConverter.ToString(oneTwoFourResult));
			Assert.IsTrue(secret.SequenceEqual(oneTwoFourResult));

			var twoThreeFour = new Dictionary<int, byte[]>();
			twoThreeFour[2] = shares[2];
			twoThreeFour[3] = shares[3];
			twoThreeFour[4] = shares[4];
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(twoThreeFour)));
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

		#endregion
	}
}

