using System;

namespace SqrlNet
{
	/// <summary>
	/// All the data needed to send a SQRL login request to a server
	/// </summary>
	public class SqrlData
	{
		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>
		/// The full URL to send the request to, including the cryptographic challenge.
		/// </value>
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets the signature.
		/// </summary>
		/// <value>
		/// The signature of the full URL, signed with the private key output of the HMAC function.
		/// </value>
		public byte[] Signature { get; set; }

		/// <summary>
		/// Gets or sets the public key.
		/// </summary>
		/// <value>
		/// The public key that can be used to decrypt the signature.
		/// </value>
		public byte[] PublicKey { get; set; }
	}
}

