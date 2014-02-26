namespace SqrlNet
{
	/// <summary>
	/// All the data needed to associate a SQRL identity with a domain.
	/// </summary>
	public class SqrlAssociationData
	{
		/// <summary>
		/// Gets or sets the identity.
		/// </summary>
		/// <value>
		/// The domain-specific public key used to identify a SQRL user.
		/// </value>
		public byte[] Identity { get; set; }

		/// <summary>
		/// Gets or sets the server unlock key.
		/// </summary>
		/// <value>
		/// The server unlock key.
		/// </value>
		public byte[] ServerUnlockKey { get; set; }

		/// <summary>
		/// Gets or sets the verify unlock key.
		/// </summary>
		/// <value>
		/// The verify unlock key.
		/// </value>
		public byte[] VerifyUnlockKey { get; set; }
	}
}