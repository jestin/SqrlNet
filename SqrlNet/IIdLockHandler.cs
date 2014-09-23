namespace SqrlNet
{
	/// <summary>
	/// Interface for the identity lock handler
	/// </summary>
	public interface IIdLockHandler
	{
		/// <summary>
		/// Creates the server unlock key and the verify unlock key.
		/// </summary>
		/// <param name="serverUnlockKey">Server unlock key.</param>
		/// <param name="verifyUnlockKey">Verify unlock key.</param>
		/// <param name="identityLockKey">Identity lock key.</param>
		void CreateUnlockKeys(out byte[] serverUnlockKey, out byte[] verifyUnlockKey, byte[] identityLockKey);
	}
}