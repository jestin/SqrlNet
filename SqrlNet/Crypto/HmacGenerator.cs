using System.Security.Cryptography;
using System.Text;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides the HMAC functionality for the SQRL process
	/// </summary>
	public class HmacGenerator : IHmacGenerator
	{
		#region IHmacGenerator implementation

		/// <summary>
		///  Generates the private key for a specific domain, based on the master key. 
		/// </summary>
		/// <returns>
		///  The private key. 
		/// </returns>
		/// <param name='masterKey'>
		///  The master key. 
		/// </param>
		/// <param name='domain'>
		///  The domain. 
		/// </param>
		public byte[] GeneratePrivateKey(byte[] masterKey, string domain)
		{
			using(HMACSHA256 hmac = new HMACSHA256(masterKey))
			{
				return hmac.ComputeHash(Encoding.UTF8.GetBytes(domain));
			}
		}

		#endregion
	}
}