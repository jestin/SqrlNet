using SqrlNet.Crypto;

namespace SqrlNet
{
	/// <summary>
	/// Identifier lock handler.
	/// </summary>
	public class IdLockHandler : IIdLockHandler
	{
		#region Dependencies

		private readonly ISqrlPseudoRandomNumberGenerator _prng;
		private readonly IDiffieHellmanHandler _diffieHellmanHandler;
		private readonly ISqrlSigner _signer;

		#endregion

		#region Constructors

		public IdLockHandler(
			ISqrlPseudoRandomNumberGenerator prng,
			IDiffieHellmanHandler diffieHellmanHandler,
			ISqrlSigner signer)
		{
			_prng = prng;
			_diffieHellmanHandler = diffieHellmanHandler;
			_signer = signer;
		}

		#endregion

		#region IIdLockHandler implementation

		/// <summary>
		/// Creates the server unlock key and the verify unlock key.
		/// </summary>
		/// <param name="serverUnlockKey">Server unlock key.</param>
		/// <param name="verifyUnlockKey">Verify unlock key.</param>
		/// <param name="identityLockKey">Identity lock key.</param>
		public void CreateUnlockKeys(out byte[] serverUnlockKey, out byte[] verifyUnlockKey, byte[] identityLockKey)
		{
			var randomLockKey = new byte[32];

			_prng.GetBytes(randomLockKey);

			serverUnlockKey = _signer.MakePublicKey(randomLockKey);

			var privateVerifyUnlockKey = _diffieHellmanHandler.CreateKey(identityLockKey, randomLockKey);

			verifyUnlockKey = _signer.MakePublicKey(privateVerifyUnlockKey);
		}

		#endregion
	}
}