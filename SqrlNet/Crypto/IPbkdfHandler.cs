namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides all the PBKDF functionality needed by the SQRL process.
	/// </summary>
	public interface IPbkdfHandler
	{
		#region Delegates

		/// <summary>
		/// Gets or sets the iteration complete delegate.
		/// </summary>
		/// <value>
		/// The iteration complete delegate.
		/// </value>
		IterationDelegate OnIterationComplete { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Generates the password key.
		/// </summary>
		/// <returns>
		/// The password key.
		/// </returns>
		/// <param name='password'>
		/// The password.
		/// </param>
		/// <param name='salt'>
		/// The salt.
		/// </param>
		/// <param name='iterations'>
		/// The number of iterations.
		/// </param>
		/// <param name='onIterationComplete'>
		/// A callback delegate that runs at the completion of each iteration
		/// </param>
		byte[] GeneratePasswordKey(string password, byte[] salt, int iterations = 1);

		/// <summary>
		/// Verifies the password.
		/// </summary>
		/// <returns>
		/// Returns true if the password given matches the partial hash given
		/// </returns>
		/// <param name='password'>
		/// The password to be verified.
		/// </param>
		/// <param name='salt'>
		/// The salt added to the password.
		/// </param>
		/// <param name='partialHash'>
		/// The lower 128 bits of a hash of the output of the PBKDF (GeneratePasswordKey).
		/// </param>
		/// <param name='iterations'>
		/// The number of iterations to run through.
		/// </param>
		bool VerifyPassword(string password, byte[] salt, byte[] partialHash, int iterations = 1);

		/// <summary>
		/// Gets the partial hash used for password verification from the password key generated from the PBKDF.
		/// </summary>
		/// <returns>
		/// The partial hash from password key.
		/// </returns>
		/// <param name='passwordKey'>
		/// Password key.
		/// </param>
		byte[] GetPartialHashFromPasswordKey(byte[] passwordKey);

		#endregion
	}

	/// <summary>
	/// A delegate to be called upon the completion of an iteration
	/// </summary>
	public delegate void IterationDelegate(int iteration);
}