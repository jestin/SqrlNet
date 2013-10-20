using System;

namespace SqrlNet
{
	public class SqrlData
	{
		public string Url { get; set; }
		public byte[] Signature { get; set; }
		public byte[] PublicKey { get; set; }
	}
}

