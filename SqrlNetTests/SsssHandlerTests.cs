using System;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using SqrlNet.Crypto;
using System.Collections.Generic;

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
		[Repeat(20)]
		public void Two_Two_Scheme()
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, 2, 2);
			Console.Error.WriteLine(Convert.ToBase64String(secret));

			var restoredSecret = _handler.Restore(2, shares);
			Console.Error.WriteLine(Convert.ToBase64String(restoredSecret));
			Assert.IsTrue(secret.SequenceEqual(restoredSecret));
		}

		[Test]
		[Repeat(20)]
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
			Assert.IsTrue(secret.SequenceEqual(_handler.Restore(2, oneTwo)));

			var oneThree = new Dictionary<int, byte[]>();
			oneThree[1] = shares[1];
			oneThree[3] = shares[3];
			//Assert.IsTrue(secret.SequenceEqual(_handler.Restore(2, oneThree)));

			var twoThree = new Dictionary<int, byte[]>();
			twoThree[2] = shares[2];
			twoThree[3] = shares[3];
			//Assert.IsTrue(secret.SequenceEqual(_handler.Restore(2, twoThree)));
		}

		#endregion
	}
}

