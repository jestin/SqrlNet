using NUnit.Framework;
using SqrlNet;

namespace SqrlNetTests
{
	[TestFixture]
	public class UtilityTests
	{
		[Test]
		public void GetDomainFromUrl_Sqrl()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://www.jestinstoffel.com/sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com", domain);
		}

		[Test]
		public void GetDomainFromUrl_Qrl()
		{
			var domain = Utility.GetDomainFromUrl("qrl://www.jestinstoffel.com/sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com", domain);
		}

		[Test]
		public void GetDomainFromUrl_Vertical_Bar()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://www.jestinstoffel.com/test|sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com/test", domain);
		}

		[Test]
		public void GetDomainFromUrl_Mixed_Case()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://www.JestinStoffel.COM/sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com", domain);
		}

		[Test]
		public void GetDomainFromUrl_Username_Password()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://username:password@www.jestinstoffel.com/sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com", domain);
		}

		[Test]
		public void GetDomainFromUrl_Username_Password_Vertical_Bar()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://username:password@www.jestinstoffel.com/test|sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com/test", domain);
		}

		[Test]
		public void GetDomainFromUrl_Port()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://www.jestinstoffel.com:8080/sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com", domain);
		}

		[Test]
		public void GetDomainFromUrl_Port_Vertical_Bar()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://www.jestinstoffel.com:8080/test|sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com/test", domain);
		}

		[Test]
		public void GetDomainFromUrl_Username_Password_And_Port()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://username:password@www.jestinstoffel.com:8080/sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com", domain);
		}

		[Test]
		public void GetDomainFromUrl_Username_Password_And_Port_Vertical_Bar()
		{
			var domain = Utility.GetDomainFromUrl("sqrl://username:password@www.jestinstoffel.com:8080/test|sqrllogin/1234qwerasdfzcxv");
			Assert.AreEqual("www.jestinstoffel.com/test", domain);
		}
	}
}

