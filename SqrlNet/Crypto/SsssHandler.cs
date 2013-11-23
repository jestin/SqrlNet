using System;
using System.Collections.Generic;

namespace SqrlNet.Crypto
{
	public class SsssHandler : ISsssHandler
	{
		#region ISsssHandler implementation

		public IDictionary<int, byte[]> Split(byte[] secret, int threshhold, int numShares)
		{
			throw new NotImplementedException();
		}

		public byte[] Restore(int threshold, IDictionary<int, byte[]> shares)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

