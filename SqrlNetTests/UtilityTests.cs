using NUnit.Framework;
using SqrlNet;
using System.Security.Cryptography;
using System.Linq;
using System;

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

		[Test]
		public void SqrlBase64Encoding_16_Bits()
		{
			var rng = new RNGCryptoServiceProvider();
			var bytes = new byte[2];

			rng.GetBytes(bytes);

			var encoded = Utility.ConvertToSqrlBase64String(bytes);
			Console.Error.WriteLine(encoded);
			var decoded = Utility.ConvertFromSqrlBase64String(encoded);

			Assert.That(bytes.SequenceEqual(decoded));
		}

		[Test]
		public void SqrlBase64Encoding_32_Bits()
		{
			var rng = new RNGCryptoServiceProvider();
			var bytes = new byte[4];

			rng.GetBytes(bytes);

			var encoded = Utility.ConvertToSqrlBase64String(bytes);
			Console.Error.WriteLine(encoded);
			var decoded = Utility.ConvertFromSqrlBase64String(encoded);

			Assert.That(bytes.SequenceEqual(decoded));
		}

		[Test]
		public void SqrlBase64Encoding_64_Bits()
		{
			var rng = new RNGCryptoServiceProvider();
			var bytes = new byte[8];

			rng.GetBytes(bytes);

			var encoded = Utility.ConvertToSqrlBase64String(bytes);
			Console.Error.WriteLine(encoded);
			var decoded = Utility.ConvertFromSqrlBase64String(encoded);

			Assert.That(bytes.SequenceEqual(decoded));
		}

		[Test]
		public void SqrlBase64Encoding_128_Bits()
		{
			var rng = new RNGCryptoServiceProvider();
			var bytes = new byte[16];

			rng.GetBytes(bytes);

			var encoded = Utility.ConvertToSqrlBase64String(bytes);
			Console.Error.WriteLine(encoded);
			var decoded = Utility.ConvertFromSqrlBase64String(encoded);

			Assert.That(bytes.SequenceEqual(decoded));
		}

		[Test]
		public void SqrlBase64Encoding_256_Bits()
		{
			var rng = new RNGCryptoServiceProvider();
			var bytes = new byte[32];

			rng.GetBytes(bytes);

			var encoded = Utility.ConvertToSqrlBase64String(bytes);
			var decoded = Utility.ConvertFromSqrlBase64String(encoded);

			Assert.That(bytes.SequenceEqual(decoded));
		}

		[Test]
		public void SqrlBase64Encoding_512_Bits()
		{
			var rng = new RNGCryptoServiceProvider();
			var bytes = new byte[64];

			rng.GetBytes(bytes);

			var encoded = Utility.ConvertToSqrlBase64String(bytes);
			var decoded = Utility.ConvertFromSqrlBase64String(encoded);

			Assert.That(bytes.SequenceEqual(decoded));
		}

		[Test]
		public void SqrlBase64Encoding_1024_Bits()
		{
			var rng = new RNGCryptoServiceProvider();
			var bytes = new byte[128];

			rng.GetBytes(bytes);

			var encoded = Utility.ConvertToSqrlBase64String(bytes);
			var decoded = Utility.ConvertFromSqrlBase64String(encoded);

			Assert.That(bytes.SequenceEqual(decoded));
		}
	}
}

