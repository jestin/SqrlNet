using SqrlNet.Crypto.CryptSharp;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Provides all the PBKDF functionality needed by the SQRL process.  This is
	/// a convenience class that is really just a rename of one library-specific
	/// implementation of the interface.
	/// </summary>
	public class PbkdfHandler : CryptSharpPbkdfHandler, IPbkdfHandler
	{
	}
}