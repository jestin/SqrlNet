using SqrlNet.Crypto;
using System;

namespace SqrlNet.Client
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

		/// <summary>
		/// Initializes a new instance of the <see cref="SqrlNet.Client.IdLockHandler"/> class.
		/// </summary>
		/// <param name="prng">Pseudo random number generator.</param>
		/// <param name="diffieHellmanHandler">Diffie hellman handler.</param>
		/// <param name="signer">Signer.</param>
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

			Array.Clear(randomLockKey, 0, randomLockKey.Length);

			verifyUnlockKey = _signer.MakePublicKey(privateVerifyUnlockKey);

			Array.Clear(privateVerifyUnlockKey, 0, privateVerifyUnlockKey.Length);
		}

		#endregion
	}
}