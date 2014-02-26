using SqrlNet.Crypto.Sodium;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides the cryptographic signature functionality for the SQRL process.  This is
	/// a convenience class that is really just a rename of one library-specific
	/// implementation of the interface.
	/// 
	/// The purpose of this "zero calorie" child-class is for users of SqrlNet who wish
	/// to entrust the maintainers with choosing the correct crypto implementations. All
	/// they have to do is use SqrlSigner as the default concrete implementation for
	/// ISqrlSigner, and they never need to know about SodiumSigner.
	/// </summary>
	public class SqrlSigner : SodiumSigner, ISqrlSigner
	{
	}
}