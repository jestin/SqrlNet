using System;
using System.Linq;
using NUnit.Framework;
using SqrlNet.Crypto;
using SqrlNet.Crypto.Sodium;

namespace SqrlNetTests
{
	[TestFixture]
	public class SodiumDiffieHellmanHandlerTests
	{
		private SodiumDiffieHellmanHandler _handler;
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
			var sk1 = new byte[32];
			var sk2 = new byte[32];

			_rng.GetBytes(sk1);
			_rng.GetBytes(sk2);

			var pk1 = _handler.MakePublicKey(sk1);
			var pk2 = _handler.MakePublicKey(sk2);

			var key1 = _handler.CreateKey(pk2, sk1);
			var key2 = _handler.CreateKey(pk1, sk2);

			Console.Error.WriteLine("key1: {0}", Convert.ToBase64String(key1));
			Console.Error.WriteLine("key2: {0}", Convert.ToBase64String(key2));

			Assert.That(key1.SequenceEqual(key2));
		}
	}
}

