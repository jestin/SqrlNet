using SqrlNet.Crypto.Sodium;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides the cryptographic signature functionality for the SQRL process.  This is
	/// a convenience class that is really just a rename of one library-specific
	/// implementation of the interface.
	/// </summary>
	public class SqrlSigner : SodiumSigner, ISqrlSigner
	{
	}
}