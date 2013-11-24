using System;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using SqrlNet.Crypto;

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
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "The number of shares cannot be larger or eqaul to 257\nParameter name: numShares")]
		public void Split_Bad_Number_of_Shares()
		{
			_handler.Split(new byte[32], 3, 257);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "The number of shares cannot be larger or eqaul to 257\nParameter name: numShares")]
		public void Split_Bad_Beyond_Prime()
		{
			_handler.Split(new byte[32], 3, 258);
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
			Assert.IsTrue(TestScheme(2, 2));
		}

		[Test]
		[Repeat(20)]
		public void Two_Three_Scheme()
		{
			Assert.IsTrue(TestScheme(2, 3));
		}

		[Test]
		[Repeat(20)]
		public void Two_Four_Scheme()
		{
			Assert.IsTrue(TestScheme(2, 4));
		}

		[Test]
		[Repeat(20)]
		public void Three_Four_Scheme()
		{
			Assert.IsTrue(TestScheme(3, 4));
		}

		[Test]
		[Repeat(20)]
		public void Two_Five_Scheme()
		{
			Assert.IsTrue(TestScheme(2, 5));
		}

		[Test]
		[Repeat(20)]
		public void Three_Five_Scheme()
		{
			Assert.IsTrue(TestScheme(3, 5));
		}

		[Test]
		[Repeat(20)]
		public void Four_Five_Scheme()
		{
			Assert.IsTrue(TestScheme(4, 5));
		}

		[Test]
		[Repeat(20)]
		public void Five_Five_Scheme()
		{
			Assert.IsTrue(TestScheme(5, 5));
		}

		[Test]
		[Repeat(20)]
		public void Two_Six_Scheme()
		{
			Assert.IsTrue(TestScheme(2, 6));
		}

		[Test]
		[Repeat(20)]
		public void Three_Six_Scheme()
		{
			Assert.IsTrue(TestScheme(3, 6));
		}

		[Test]
		[Repeat(20)]
		public void Four_Six_Scheme()
		{
			Assert.IsTrue(TestScheme(4, 6));
		}

		[Test]
		[Repeat(20)]
		public void Five_Six_Scheme()
		{
			Assert.IsTrue(TestScheme(5, 6));
		}

		[Test]
		[Repeat(20)]
		public void Six_Six_Scheme()
		{
			Assert.IsTrue(TestScheme(6, 6));
		}

		[Test]
		[Repeat(20)]
		public void Two_Seven_Scheme()
		{
			Assert.IsTrue(TestScheme(2, 7));
		}

		[Test]
		[Repeat(20)]
		public void Three_Seven_Scheme()
		{
			Assert.IsTrue(TestScheme(3, 7));
		}

		[Test]
		[Repeat(20)]
		public void Four_Seven_Scheme()
		{
			Assert.IsTrue(TestScheme(4, 7));
		}

		[Test]
		[Repeat(20)]
		public void Five_Seven_Scheme()
		{
			Assert.IsTrue(TestScheme(5, 7));
		}

		[Test]
		[Repeat(20)]
		public void Six_Seven_Scheme()
		{
			Assert.IsTrue(TestScheme(6, 7));
		}

		[Test]
		[Repeat(20)]
		public void DragonBalls()
		{
			Assert.IsTrue(TestScheme(7, 7));
		}

		[Test]
		[Repeat(100)]
		public void Random()
		{
			var random = new Random();
			var numShares = random.Next(2, 255);
			var threshold = random.Next(1, numShares);
			Assert.IsTrue(TestScheme(threshold, numShares));
		}

		#endregion

		#region Private Methods

		private bool TestScheme(int threshold, int numShares)
		{
			// generate secret
			var secret = new byte[32];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(secret);

			// split secret
			var shares = _handler.Split(secret, threshold, numShares);

			// reconstruct secret
			var reconstructedSecret = _handler.Restore(threshold, shares);

			return secret.SequenceEqual(reconstructedSecret);
		}

		#endregion
	}
}

