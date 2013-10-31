using System;

namespace SqrlNet.Server
{
	public class SqrlServer : ISqrlServer
	{
		public SqrlServer()
		{
		}

		#region ISqrlServer implementation

		public byte[] GenerateNut(byte[] key)
		{
			throw new System.NotImplementedException();
		}

		public byte[] GenerateNut(byte[] key, NutData data)
		{
			throw new System.NotImplementedException();
		}

		public NutData DycryptNut(byte[] key, byte[] nut)
		{
			throw new System.NotImplementedException();
		}

		public bool VerifySqrlRequest(SqrlData data)
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}

