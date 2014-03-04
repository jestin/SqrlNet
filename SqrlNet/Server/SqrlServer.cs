using System;
using System.Net;
using System.Text;
using SqrlNet.Crypto;

namespace SqrlNet.Server
{
	/// <summary>
	/// A class that contains all the functionality to create the server-side
	/// implentation of the SQRL protocol.
	/// </summary>
	/// <remarks>
	/// This class is to used by a web server and called upon to perform the
	/// server-side tasks of the SQRL protocol.  It should be well-suited for
	/// use with both ASP.NET WebForms and ASP.NET MVC.
	/// </remarks>
	public class SqrlServer : ISqrlServer
	{
		#region Dependencies

		private readonly ISqrlSigner _sqrlSigner;
		private readonly IAesHandler _aesHandler;
		private readonly ISqrlPseudoRandomNumberGenerator _prng;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="SqrlNet.Server.SqrlServer"/> class.
		/// </summary>
		/// <param name='sqrlSigner'>
		/// SQRL signer.
		/// </param>
		/// <param name='aesHandler'>
		/// AES handler.
		/// </param>
		/// <param name='prng'>
		/// The pseudo random number generator.
		/// </param>
		public SqrlServer(
			ISqrlSigner sqrlSigner,
			IAesHandler aesHandler,
			ISqrlPseudoRandomNumberGenerator prng)
		{
			_sqrlSigner = sqrlSigner;
			_aesHandler = aesHandler;
			_prng = prng;
		}

		#region Static Variables

		private static UInt32 counter = 0;

		#endregion

		#region ISqrlServer implementation

		/// <summary>
		///  Generates a nut. 
		/// </summary>
		/// <returns>
		///  The nut. 
		/// </returns>
		/// <param name='key'>
		///  An encryption key that is used on the generated data to return and encrypted nut. 
		/// </param>
		/// <param name='iv'>
		///  The initialization vector for the Rijndael cipher. 
		/// </param>
		public byte[] GenerateNut(byte[] key, byte[] iv)
		{
			var nutData = new NutData
			{
				Address = new IPAddress(0),
				Timestamp = DateTime.UtcNow,
				Counter = counter,
				Entropy = new byte[4]
			};

			_prng.GetBytes(nutData.Entropy);

			counter++;

			return GenerateNut(key, iv, nutData);
		}

		/// <summary>
		///  Generates the nut. 
		/// </summary>
		/// <returns>
		///  The nut. 
		/// </returns>
		/// <param name='key'>
		///  An encryption key that is used on the given data to return and encrypted nut. 
		/// </param>
		/// <param name='iv'>
		///  The initialization vector for the Rijndael cipher. 
		/// </param>
		/// <param name='data'>
		///  Data to encrypt into the nut. 
		/// </param>
		public byte[] GenerateNut(byte[] key, byte[] iv, NutData data)
		{
			var nutStruct = data.GetNutStruct();
			return _aesHandler.Encrypt(key, iv, nutStruct.GetBytes());
		}

		/// <summary>
		///  Dycrypts a nut. 
		/// </summary>
		/// <returns>
		///  The data encrypted into the nut. 
		/// </returns>
		/// <param name='key'>
		///  The encryption key. 
		/// </param>
		/// <param name='iv'>
		///  The initialization vector for the Rijndael cipher. 
		/// </param>
		/// <param name='nut'>
		///  The nut. 
		/// </param>
		public NutData DecryptNut(byte[] key, byte[] iv, byte[] nut)
		{
			var nutStruct = new NutStruct();
			nutStruct.SetBytes(_aesHandler.Decrypt(key, iv, nut));

			return new NutData(nutStruct);
		}

		/// <summary>
		///  Verifies the sqrl request. 
		/// </summary>
		/// <returns>
		///  Whether the public key provided decrypts the signature provided. 
		/// </returns>
		/// <param name='data'>
		///  The data contained in the SQRL request. 
		/// </param>
		/// <param name='expectedUrl'>
		///  The URL that is expected from the request. 
		/// </param>
		public bool VerifySqrlRequest(SqrlLoginData data, string expectedUrl)
		{
			var decryptedSignatureData = _sqrlSigner.Verify(data.PublicKey, data.Signature);

			var decryptedUrl = Encoding.UTF8.GetString(decryptedSignatureData);

			return (decryptedUrl == data.Url) && (decryptedUrl == Utility.GetUrlWithoutProtocol(expectedUrl));
		}

		#endregion
	}
}