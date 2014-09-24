using System;
using NUnit.Framework;
using SqrlNet.Crypto;
using SqrlNet.Crypto.Sodium;

namespace SqrlNetTests
{
	[TestFixture]
	public class SodiumDiffieHellmanHandlerTests
	{
		private SodiumDiffieHellmanHandler _handler;
		private readonly ISqrlSigner _signer = new SqrlSigner();
		private readonly ISqrlPseudoRandomNumberGenerator _rng = new SqrlPseudoRandomNumberGenerator();

		[SetUp]
		public void Setup()
		{
			_handler = new SodiumDiffieHellmanHandler();
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void Basic_Key_Agreement_Succeeds()
		{
			var alicePrivateKey = new byte[32];
			var bobPrivateKey = new byte[32];

			_rng.GetBytes(alicePrivateKey);
			_rng.GetBytes(bobPrivateKey);

			var alicePublicKey = _signer.MakePublicKey(alicePrivateKey);
			var bobPublicKey = _signer.MakePublicKey(bobPrivateKey);

			var key1 = _handler.CreateKey(bobPublicKey, alicePrivateKey);
			var key2 = _handler.CreateKey(alicePublicKey, bobPrivateKey);

			Console.Error.WriteLine("key1: {0}", Convert.ToBase64String(key1));
			Console.Error.WriteLine("key2: {0}", Convert.ToBase64String(key2));

			CollectionAssert.AreEqual(key1, key2);
		}

		[Test]
		public void Basic_Key_Agreement_Fails()
		{
			var alicePrivateKey = new byte[32];
			var bobPrivateKey = new byte[32];
			var charliePrivateKey = new byte[32];

			_rng.GetBytes(alicePrivateKey);
			_rng.GetBytes(bobPrivateKey);
			_rng.GetBytes(charliePrivateKey);

			var alicePublicKey = _signer.MakePublicKey(alicePrivateKey);
			var bobPublicKey = _signer.MakePublicKey(bobPrivateKey);
			var charliePublicKey = _signer.MakePublicKey(charliePrivateKey);

			// key for alice and bob
			var key1 = _handler.CreateKey(bobPublicKey, alicePrivateKey);

			// key for bob and charlie
			var key2 = _handler.CreateKey(charliePublicKey, bobPrivateKey);

			// the keys should not be the same
			CollectionAssert.AreNotEqual(key1, key2);
		}
	}
}

