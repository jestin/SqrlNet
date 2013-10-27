using System;
using NUnit.Framework;
using System.Security.Cryptography;
using Sodium;

namespace libsodiumnettests
{
	[TestFixture]
	public class PublicKeyAuthTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void Key_Generation_Is_Deterministic()
		{
			var rng = new RNGCryptoServiceProvider();
			var seed = new byte[32];

			rng.GetBytes(seed);

			var kp1 = PublicKeyAuth.GenerateKeyPair(seed);
			var kp2 = PublicKeyAuth.GenerateKeyPair(seed);

			Assert.AreEqual(kp1.PrivateKey, kp2.PrivateKey);
			Assert.AreEqual(kp1.PublicKey, kp2.PublicKey);
		}
	}
}

