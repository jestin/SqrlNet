using System;
using System.Text;
using Sodium;

namespace SqrlNet.Crypto.Sodium
{
	public class SodiumSigner : ISqrlSigner
	{
		#region Private Properties

		private KeyPair Keys { get; set; }

		#endregion

		#region ISqrlSigner implementation

		public byte[] Sign(byte[] privateKey, byte[] message)
		{
			if(Keys == null)
			{
				Keys = PublicKeyAuth.GenerateKeyPair(privateKey);
			}

			return PublicKeyAuth.Sign(message, Keys.PrivateKey);
		}

		public byte[] Sign(byte[] privateKey, string message)
		{
			return Sign(privateKey, Encoding.UTF8.GetBytes(message));
		}

		public byte[] Verify(byte[] publicKey, byte[] signedMessage)
		{
			return PublicKeyAuth.Verify(signedMessage, publicKey);
		}

		public byte[] MakePublicKey(byte[] privateKey)
		{
			Keys = PublicKeyAuth.GenerateKeyPair(privateKey);
			return Keys.PublicKey;
		}

		#endregion
	}
}

