using SqrlNet.Crypto.CryptSharp;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides all the PBKDF functionality needed by the SQRL process.  This is
	/// a convenience class that is really just a rename of one library-specific
	/// implementation of the interface.
	/// 
	/// The purpose of this "zero calorie" child-class is for users of SqrlNet who wish
	/// to entrust the maintainers with choosing the correct crypto implementations.
	/// All they have to do is use PbkdfHandler as the default concrete implementation
	/// for IPbkdfHandler, and they never need to know about CryptSharpPbkdfHandler.
	/// </summary>
	public class PbkdfHandler : CryptSharpPbkdfHandler, IPbkdfHandler
	{
	}
}