using System;
using NUnit.Framework;
using Rhino.Mocks;
using SqrlNet.Client;
using SqrlNet.Crypto;

namespace SqrlNetTests
{
	[TestFixture]
	public class SqrlClientTests
	{
		private readonly MockRepository _mocks = new MockRepository();

		private IPbkdfHandler _pbkdfHandler;
		private IHmacGenerator _hmacGenerator;
		private ISqrlSigner _signer;

		private SqrlClient _client;

		[SetUp]
		public void Setup()
		{
			_pbkdfHandler = _mocks.StrictMock<IPbkdfHandler>();
			_hmacGenerator = _mocks.StrictMock<IHmacGenerator>();
			_signer = _mocks.StrictMock<ISqrlSigner>();

			_client = new SqrlClient(_pbkdfHandler, _hmacGenerator, _signer);
		}

		[TearDown]
		public void TearDown()
		{
			_mocks.BackToRecordAll();
		}

		#region CalculateMasterKey Tests

		[Test]
		public void CalculateMasterKey_Succeeds()
		{
			_pbkdfHandler.Expect(x => x.GeneratePasswordKey(Arg<string>.Is.Anything, Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			var result = _client.CalculateMasterKey(new byte[32], "password", new byte[32]);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Length, 32);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "master identity key must be 256 bits (32 bytes).")]
		public void CalculateMasterKey_Bad_MasterIdentityKey_Fails()
		{
			_pbkdfHandler.Expect(x => x.GeneratePasswordKey(Arg<string>.Is.Anything, Arg<byte[]>.Is.Anything)).Return(new byte[64]);
			_mocks.ReplayAll();

			_client.CalculateMasterKey(new byte[31], "password", new byte[32]);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "password key must be 256 bits (32 bytes).  Check validity of PBKDF.")]
		public void CalculateMasterKey_Bad_PBKDF_Output_Fails()
		{
			_pbkdfHandler.Expect(x => x.GeneratePasswordKey(Arg<string>.Is.Anything, Arg<byte[]>.Is.Anything)).Return(new byte[31]);
			_mocks.ReplayAll();

			_client.CalculateMasterKey(new byte[32], "password", new byte[32]);
		}

		#endregion

		#region CalculateMasterIdentityKey Tests

		[Test]
		public void CalculateMasterIdentityKey_Succeeds()
		{
			_pbkdfHandler.Expect(x => x.GeneratePasswordKey(Arg<string>.Is.Anything, Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			var result = _client.CalculateMasterIdentityKey(new byte[32], "password", new byte[32]);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Length, 32);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "master key must be 256 bits (32 bytes).")]
		public void CalculateMasterIdentityKey_Bad_MasterIdentityKey_Fails()
		{
			_pbkdfHandler.Expect(x => x.GeneratePasswordKey(Arg<string>.Is.Anything, Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			_client.CalculateMasterIdentityKey(new byte[31], "password", new byte[32]);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "password key must be 256 bits (32 bytes).  Check validity of PBKDF.")]
		public void CalculateMasterIdentityKey_Bad_PBKDF_Output_Fails()
		{
			_pbkdfHandler.Expect(x => x.GeneratePasswordKey(Arg<string>.Is.Anything, Arg<byte[]>.Is.Anything)).Return(new byte[31]);
			_mocks.ReplayAll();

			_client.CalculateMasterIdentityKey(new byte[32], "password", new byte[32]);
		}

		#endregion

		#region GetSqrlDataForLogin Tests

		[Test]
		public void GetSqrlDataForLogin_Succeeds()
		{
			string url = "sqrl://example.com/auth/asdkjhaiewruhaksdfjiugasdfkjb";
			_hmacGenerator.Expect(x => x.GeneratePrivateKey(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[32]);
			_signer.Expect(x => x.Sign(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[64]);
			_signer.Expect(x => x.MakePublicKey(Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			var result = _client.GetSqrlDataForLogin(new byte[32], url);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Url, url);
		}

		#endregion

		#region Private Method Tests

		#region GetDomainFromUrl Tests

		[Test]
		public void GetDomainFromUrl_SQRL_Succeeds()
		{
			string url = "sqrl://example.com/auth/asdkjhaiewruhaksdfjiugasdfkjb";
			_hmacGenerator.Expect(x => x.GeneratePrivateKey(Arg<byte[]>.Is.Anything, Arg<string>.Matches(domain => domain == "example.com"))).Return(new byte[32]);
			_signer.Expect(x => x.Sign(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[64]);
			_signer.Expect(x => x.MakePublicKey(Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			var result = _client.GetSqrlDataForLogin(new byte[32], url);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Url, url);
		}

		[Test]
		public void GetDomainFromUrl_QRL_Succeeds()
		{
			string url = "qrl://example.com/auth/asdkjhaiewruhaksdfjiugasdfkjb";
			_hmacGenerator.Expect(x => x.GeneratePrivateKey(Arg<byte[]>.Is.Anything, Arg<string>.Matches(domain => domain == "example.com"))).Return(new byte[32]);
			_signer.Expect(x => x.Sign(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[64]);
			_signer.Expect(x => x.MakePublicKey(Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			var result = _client.GetSqrlDataForLogin(new byte[32], url);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Url, url);
		}

		[Test]
		public void GetDomainFromUrl_SQRL_Pipe_Succeeds()
		{
			string url = "sqrl://example.com/auth|/asdkjhaiewruhaksdfjiugasdfkjb";
			_hmacGenerator.Expect(x => x.GeneratePrivateKey(Arg<byte[]>.Is.Anything, Arg<string>.Matches(domain => domain == "example.com/auth"))).Return(new byte[32]);
			_signer.Expect(x => x.Sign(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[64]);
			_signer.Expect(x => x.MakePublicKey(Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			var result = _client.GetSqrlDataForLogin(new byte[32], url);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Url, url);
		}

		[Test]
		public void GetDomainFromUrl_QRL_Pipe_Succeeds()
		{
			string url = "qrl://example.com/auth|/asdkjhaiewruhaksdfjiugasdfkjb";
			_hmacGenerator.Expect(x => x.GeneratePrivateKey(Arg<byte[]>.Is.Anything, Arg<string>.Matches(domain => domain == "example.com/auth"))).Return(new byte[32]);
			_signer.Expect(x => x.Sign(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[64]);
			_signer.Expect(x => x.MakePublicKey(Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			var result = _client.GetSqrlDataForLogin(new byte[32], url);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Url, url);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "SQRL urls must begin with 'sqrl://' or 'qrl://'")]
		public void GetDomainFromUrl_Bad_Scheme_Fails()
		{
			string url = "http://example.com/auth/asdkjhaiewruhaksdfjiugasdfkjb";
			_hmacGenerator.Expect(x => x.GeneratePrivateKey(Arg<byte[]>.Is.Anything, Arg<string>.Matches(domain => domain == "example.com"))).Return(new byte[32]);
			_signer.Expect(x => x.Sign(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[64]);
			_signer.Expect(x => x.MakePublicKey(Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			_client.GetSqrlDataForLogin(new byte[32], url);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "SQRL urls must contain a '/'")]
		public void GetDomainFromUrl_Bad_Url_Fails()
		{
			string url = "sqrl://example.com";
			_hmacGenerator.Expect(x => x.GeneratePrivateKey(Arg<byte[]>.Is.Anything, Arg<string>.Matches(domain => domain == "example.com"))).Return(new byte[32]);
			_signer.Expect(x => x.Sign(Arg<byte[]>.Is.Anything, Arg<string>.Is.Anything)).Return(new byte[64]);
			_signer.Expect(x => x.MakePublicKey(Arg<byte[]>.Is.Anything)).Return(new byte[32]);
			_mocks.ReplayAll();

			_client.GetSqrlDataForLogin(new byte[32], url);
		}

		#endregion

		#region Xor Tests

		[Test]
		public void Xor_Succeeds()
		{
			var passwordKey = new byte[32]
			{
				0xFF, 0xFF, 0xFF, 0xFF,
				0xFF, 0xFF, 0xFF, 0xFF,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
			};

			var masterIdentityKey = new byte[32]
			{
				0xFF, 0xFF, 0xFF, 0xFF,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
			};

			var masterKey = new byte[32]
			{
				0x00, 0x00, 0x00, 0x00,
				0xFF, 0xFF, 0xFF, 0xFF,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
			};

			_pbkdfHandler.Expect(x => x.GeneratePasswordKey(Arg<string>.Is.Anything, Arg<byte[]>.Is.Anything)).Return(passwordKey);
			_mocks.ReplayAll();

			var result = _client.CalculateMasterKey(masterIdentityKey, "password", new byte[32]);

			_mocks.VerifyAll();
			Assert.AreEqual(result.Length, 32);
			Assert.AreEqual(result, masterKey);
		}

		#endregion

		#endregion
	}
}