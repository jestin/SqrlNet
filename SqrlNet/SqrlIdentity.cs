using System;

namespace SqrlNet
{
	/// <summary>
	/// All the data needed to store in a SQRL client application in order to send SQRL login requests
	/// </summary>
	public class SqrlIdentity
	{
		/// <summary>
		/// Gets or sets the friendly name of this identity.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the master identity key.
		/// </summary>
		/// <value>
		/// The master identity key.
		/// </value>
		public byte[] MasterIdentityKey { get; set; }

		/// <summary>
		/// Gets or sets the salt.
		/// </summary>
		/// <value>
		/// The salt.
		/// </value>
		public byte[] Salt { get; set; }

		/// <summary>
		/// Gets or sets the partial password hash.
		/// </summary>
		/// <value>
		/// The partial password hash.
		/// </value>
		public byte[] PartialPasswordHash { get; set; }
	}
}

