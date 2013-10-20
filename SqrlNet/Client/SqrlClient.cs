using System;

namespace SqrlNet.Client
{
	public class SqrlClient : ISqrlClient
	{
		public SqrlClient()
		{
		}

		#region ISqrlClient implementation

		public byte[] CalculateMasterKey(byte[] masterIdentityKey, string password)
		{
			throw new System.NotImplementedException();
		}

		public byte[] CalculateMasterIdentityKey(byte[] masterKey, string password)
		{
			throw new System.NotImplementedException();
		}

		public byte[] GenerateMasterKey()
		{
			throw new System.NotImplementedException();
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterKey, string url)
		{
			throw new System.NotImplementedException();
		}

		public SqrlData GetSqrlDataForLogin(byte[] masterIdentityKey, string password, string url)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}