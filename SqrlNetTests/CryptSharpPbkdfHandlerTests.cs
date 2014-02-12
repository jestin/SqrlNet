using NUnit.Framework;
using SqrlNet.Crypto.CryptSharp;
using System;
using SqrlNet;

namespace SqrlNetTests
{
	[TestFixture]
	public class CryptSharpPbkdfHandlerTests
	{
		private CryptSharpPbkdfHandler _handler;

		[SetUp]
		public void Setup()
		{
			_handler = new CryptSharpPbkdfHandler();
		}

		[TearDown]
		public void TearDown()
		{
		}

		#region Tests from GRC
		// As of this comment, I am apparently not handling null values the same way as
		// EnScrypt, so none of the tests using nulls are valid.

		// enscrypt 1i
  		// Password: <null>
		// Salt: <null>
		// Iterations: 1

		// Output Key: a8ea62a6e1bfd20e4275011595307aa302645c1801600ef5cd79bf9d884d911c
		[Test]
		[Ignore]
		public void GeneratePasswordKey_Null_Null_1()
		{
			var hash = _handler.GeneratePasswordKey(null, null, 1);

			var hex = BitConverter.ToString(hash).Replace("-","").ToLower();

			Console.Error.WriteLine(hex);

			Assert.AreEqual("a8ea62a6e1bfd20e4275011595307aa302645c1801600ef5cd79bf9d884d911c", hex);
		}

		// enscrypt 100i
  		// Password: <null>
		// Salt: <null>
		// Iterations: 100

		// Output Key: 45a42a01709a0012a37b7b6874cf16623543409d19e7740ed96741d2e99aab67
		[Test]
		[Ignore]
		public void GeneratePasswordKey_Null_Null_100()
		{
			var hash = _handler.GeneratePasswordKey(null, null, 100);

			var hex = BitConverter.ToString(hash).Replace("-","").ToLower();

			Console.Error.WriteLine(hex);

			Assert.AreEqual("45a42a01709a0012a37b7b6874cf16623543409d19e7740ed96741d2e99aab67", hex);
		}

		// enscrypt 1000i
  		// Password: <null>
		// Salt: <null>
		// Iterations: 1000

		// Output Key: 3f671adf47d2b1744b1bf9b50248cc71f2a58e8d2b43c76edb1d2a2c200907f5
		[Test]
		[Ignore]
		public void GeneratePasswordKey_Null_Null_1000()
		{
			var hash = _handler.GeneratePasswordKey(null, null, 1000);

			var hex = BitConverter.ToString(hash).Replace("-","").ToLower();

			Console.Error.WriteLine(hex);

			Assert.AreEqual("3f671adf47d2b1744b1bf9b50248cc71f2a58e8d2b43c76edb1d2a2c200907f5", hex);
		}

		// enscrypt password 123i
  		// Password: <null>
		// Salt: <null>
		// Iterations: 123

		// Output Key: 129d96d1e735618517259416a605be7094c2856a53c14ef7d4e4ba8e4ea36aeb
		[Test]
		[Ignore]
		public void GeneratePasswordKey_password_Null_123()
		{
			var hash = _handler.GeneratePasswordKey("password", null, 123);

			var hex = BitConverter.ToString(hash).Replace("-","").ToLower();

			Console.Error.WriteLine(hex);

			Assert.AreEqual("129d96d1e735618517259416a605be7094c2856a53c14ef7d4e4ba8e4ea36aeb", hex);
		}

		// enscrypt password 0000000000000000000000000000000000000000000000000000000000000000 123i
  		// Password: <null>
		// Salt: 0000000000000000000000000000000000000000000000000000000000000000
		// Iterations: 123

		// Output Key: 2f30b9d4e5c48056177ff90a6cc9da04b648a7e8451dfa60da56c148187f6a7d
		[Test]
		public void GeneratePasswordKey_password_zeros_123()
		{
			var hash = _handler.GeneratePasswordKey("password", Utility.GetZeroBytes(32), 123);

			var hex = BitConverter.ToString(hash).Replace("-","").ToLower();

			Console.Error.WriteLine(hex);

			Assert.AreEqual("2f30b9d4e5c48056177ff90a6cc9da04b648a7e8451dfa60da56c148187f6a7d", hex);
		}

		#endregion

		#region My Own Tests Using GRC's EnScrypt

		// enscrypt password 0000000000000000000000000000000000000000000000000000000000000000 1i
  		// Password: <null>
		// Salt: 0000000000000000000000000000000000000000000000000000000000000000
		// Iterations: 123

		// Output Key: 532bcc911c16df81996258158de460b2e59d9a86531d59661da5fbeb69f7cd54
		[Test]
		public void GeneratePasswordKey_password_zeros_1()
		{
			var hash = _handler.GeneratePasswordKey("password", Utility.GetZeroBytes(32), 1);

			var hex = BitConverter.ToString(hash).Replace("-","").ToLower();

			Console.Error.WriteLine(hex);

			Assert.AreEqual("532bcc911c16df81996258158de460b2e59d9a86531d59661da5fbeb69f7cd54", hex);
		}

		// enscrypt password 0000000000000000000000000000000000000000000000000000000000000000 1i
  		// Password: <null>
		// Salt: 0000000000000000000000000000000000000000000000000000000000000000
		// Iterations: 123

		// Output Key: 2d516e99bceb1f49e4dc02217ffc6bac28ea1a9b2d67c1dabd85185163ffe2de
		[Test]
		public void GeneratePasswordKey_password_zeros_2()
		{
			var hash = _handler.GeneratePasswordKey("password", Utility.GetZeroBytes(32), 2);

			var hex = BitConverter.ToString(hash).Replace("-","").ToLower();

			Console.Error.WriteLine(hex);

			Assert.AreEqual("2d516e99bceb1f49e4dc02217ffc6bac28ea1a9b2d67c1dabd85185163ffe2de", hex);
		}

		#endregion
	}
}