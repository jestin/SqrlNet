using NUnit.Framework;
using SqrlNet.Server;
using System.Net;
using System;
using System.Security.Cryptography;

namespace SqrlNetTests
{
	[TestFixture]
	public class NutDataTests
	{
		[Test]
		public void DataToStruct_IPv4_Address()
		{
			var data = new NutData();
			data.Address = IPAddress.Parse("172.34.56.251");

			var nutStruct = data.GetNutStruct();

			Assert.AreEqual(data.Address.ToString(), new IPAddress(nutStruct.Address).ToString());
		}

		[Test]
		public void DataToStruct_IPv6_Address()
		{
			var data = new NutData();
			data.Address = IPAddress.Parse("2001:db8:85a3:8d3:1319:8a2e:370:7348");

			var sha = SHA256Managed.Create();

			// TODO: I may have to think about how to salt this hash
			var hash = sha.ComputeHash(data.Address.GetAddressBytes());
			var partialHash = BitConverter.ToUInt32(new ArraySegment<byte>(hash, 0, 8).Array, 0);

			var nutStruct = data.GetNutStruct();

			Assert.AreEqual(partialHash, nutStruct.Address);
		}

		[Test]
		public void DataToStruct_Timestamp()
		{
			var data = new NutData();
			data.Address = IPAddress.Parse("172.34.56.251");
			data.Timestamp = new DateTime(1981, 12, 8);

			var nutStruct = data.GetNutStruct();

			Assert.AreEqual(data.Timestamp, new DateTime(1970,1,1,0,0,0,0).AddSeconds(nutStruct.Timestamp).ToLocalTime());
		}

		[Test]
		public void StructToData_IPv4_Address()
		{
			var address = IPAddress.Parse("172.34.56.251");
			var nutStruct = new NutStruct();
			nutStruct.Address = BitConverter.ToUInt32(address.GetAddressBytes(), 0);

			var data = new NutData(nutStruct);

			Assert.AreEqual(new IPAddress(nutStruct.Address).ToString(), data.Address.ToString());
		}

		[Test]
		public void StructToData_Timestamp()
		{
			var date = new DateTime(1981, 12, 8);
			var nutStruct = new NutStruct();
			nutStruct.Timestamp = (UInt32) (date - new DateTime(1970,1,1,0,0,0,0).ToLocalTime()).TotalSeconds;

			var data = new NutData(nutStruct);

			Assert.AreEqual(date, data.Timestamp);
		}
	}
}

