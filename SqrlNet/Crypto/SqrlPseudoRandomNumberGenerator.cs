using SqrlNet.Crypto.Sodium;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Sqrl pseudo random number generator, implemented with the libsodium-net library.
	/// </summary>
	public class SqrlPseudoRandomNumberGenerator : SodiumPseudoRandomNumberGenerator, ISqrlPseudoRandomNumberGenerator
	{
	}
}