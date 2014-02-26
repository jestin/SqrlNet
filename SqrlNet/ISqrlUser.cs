namespace SqrlNet
{
	/// <summary>
	/// Interface that defines what user data needs to be stored by a
	/// web server that implements SQRL
	/// </summary>
	public interface ISqrlUser
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// This is the base64 encoded public key that identifies which user
		/// this is to the web server.
		/// </value>
		string Id { get; set; }

		/// <summary>
		/// Gets or sets the verify unlock key.
		/// </summary>
		/// <value>
		/// The base64 encoded verify unlock key.
		/// </value>
		string VerifyUnlockKey { get; set; }

		/// <summary>
		/// Gets or sets the server unlock key.
		/// </summary>
		/// <value>
		/// The base64 encoded server unlock key.
		/// </value>
		string ServerUnlockKey { get; set; }
	}
}