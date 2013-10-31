using System;
using System.Text;
using SqrlNet.Crypto;

namespace SqrlNet.Server
{
	public class SqrlServer : ISqrlServer
	{
		#region Dependencies

		private readonly ISqrlSigner _sqrlSigner;

		#endregion

		public SqrlServer(ISqrlSigner sqrlSigner)
		{
			_sqrlSigner = sqrlSigner;
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
			var decryptedSignatureData = _sqrlSigner.Verify(data.PublicKey, data.Signature);

			var decryotedUrl = Encoding.UTF8.GetString(decryptedSignatureData);

			var url = Utility.GetUrlWithoutProtocol(data.Url);

			return (decryotedUrl == url);
		}

		#endregion
	}
}

