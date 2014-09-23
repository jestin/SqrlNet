using NUnit.Framework;
using SqrlNet;
using SqrlNet.Crypto;
using System.Text;

namespace SqrlNetTests
{
	[TestFixture]
	public class IdLockHandlerTests
	{
		private IdLockHandler _idLockHandler;

		private ISqrlPseudoRandomNumberGenerator _prng;
		private ISqrlSigner _signer;
		private IDiffieHellmanHandler _diffieHellmanHandler;

		[SetUp]
		public void Setup()
		{
			_prng = new SqrlPseudoRandomNumberGenerator();
			_diffieHellmanHandler = new DiffieHellmanHandler();
			_signer = new SqrlSigner();

			_idLockHandler = new IdLockHandler(_prng, _diffieHellmanHandler, _signer);
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void IdLock_Succeeds()
		{
			byte[] serverUnlockKey;
			byte[] verifyUnlockKey;

			var idUnlockKey = new byte[32];

			_prng.GetBytes(idUnlockKey);

			var idLockKey = _signer.MakePublicKey(idUnlockKey);

			_idLockHandler.CreateUnlockKeys(out serverUnlockKey, out verifyUnlockKey, idLockKey);

			var unlockSigningKey = _diffieHellmanHandler.CreateKey(serverUnlockKey, idUnlockKey);

			var testRequest = "this is a request";

			var signedRequest = _signer.Sign(unlockSigningKey, testRequest);

			var verifiedRequest = _signer.Verify(verifyUnlockKey, signedRequest);
			var verifiedRequestString = Encoding.UTF8.GetString(verifiedRequest);

			Assert.AreEqual(testRequest, verifiedRequestString);
		}
	}
}