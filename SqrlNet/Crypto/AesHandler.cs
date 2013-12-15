using System.IO;
using System.Security.Cryptography;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// A class for handling AES (Rijndael) encryption and decryption.
	/// </summary>
	public class AesHandler : IAesHandler
	{
		#region IAesHandler implementation

		/// <summary>
		///  Encrypt the message with the specified key and initialization vector. 
		/// </summary>
		/// <param name='key'>
		///  The key. 
		/// </param>
		/// <param name='iv'>
		///  The initialization vector. 
		/// </param>
		/// <param name='message'>
		///  The message. 
		/// </param>
		/// <returns>
		///  Returns the encrypted message. 
		/// </returns>
		public byte[] Encrypt(byte[] key, byte[] iv, byte[] message)
		{
			var aes = new RijndaelManaged();
			aes.Key = key;
			aes.IV = iv;
			var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

			var ms = new MemoryStream();
			using(var stream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
			{
				using(var bw = new BinaryWriter(stream))
				{
					bw.Write(message);
				}
			}

			return ms.ToArray();
		}

		/// <summary>
		///  Decrypt the encrypted message with the specified key and initialization vector. 
		/// </summary>
		/// <param name='key'>
		///  The key. 
		/// </param>
		/// <param name='iv'>
		///  The initialization vector. 
		/// </param>
		/// <param name='encryptedMessage'>
		///  The encrypted message. 
		/// </param>
		/// <returns>
		///  The decrypted message. 
		/// </returns>
		public byte[] Decrypt(byte[] key, byte[] iv, byte[] encryptedMessage)
		{
			var aes = new RijndaelManaged();
			aes.Key = key;
			aes.IV = iv;
			var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

			var ms = new MemoryStream();
			using(var stream = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
			{
				using(var bw = new BinaryWriter(stream))
				{
					bw.Write(encryptedMessage);
				}
			}

			return ms.ToArray();
		}

		#endregion
	}
}