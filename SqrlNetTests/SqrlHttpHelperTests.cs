using NUnit.Framework;
using SqrlNet.Client;

namespace SqrlNetTests
{
	[TestFixture]
	public class SqrlHttpHelperTests
	{
		private SqrlHttpHelper _httpHelper;

		[SetUp]
		public void Setup()
		{
			_httpHelper = new SqrlHttpHelper();
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void GetCommandParameter_Create_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.Create | SqrlNet.SqrlCommand.SetKey | SqrlNet.SqrlCommand.SetLock);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("setkey~setlock~create", result);
		}

		[Test]
		public void GetCommandParameter_SetKey_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.SetKey);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("setkey", result);
		}

		[Test]
		public void GetCommandParameter_SetLock_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.SetLock);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("setlock", result);
		}

		[Test]
		public void GetCommandParameter_Disable_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.Disable);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("disable", result);
		}

		[Test]
		public void GetCommandParameter_Enable_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.Enable);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("enable", result);
		}

		[Test]
		public void GetCommandParameter_Login_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.Login);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("login", result);
		}

		[Test]
		public void GetCommandParameter_Login_SetKey_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.Login | SqrlNet.SqrlCommand.SetKey);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("setkey~login", result);
		}

		[Test]
		public void GetCommandParameter_Login_SetLock_Succeeds()
		{
			var result = _httpHelper.GetCommandParameter(SqrlNet.SqrlCommand.Login | SqrlNet.SqrlCommand.SetLock);

			Assert.IsNotNullOrEmpty(result);
			Assert.AreEqual("setlock~login", result);
		}
	}
}