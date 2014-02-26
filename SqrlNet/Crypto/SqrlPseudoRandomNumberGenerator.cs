using SqrlNet.Crypto.Sodium;

namespace SqrlNet.Crypto
{
	/// <summary>
	/// Sqrl pseudo random number generator, implemented with the libsodium-net library.
	/// 
	/// The purpose of this "zero calorie" child-class is for users of SqrlNet who wish
	/// to entrust the maintainers with choosing the correct crypto implementations.
	/// All they have to do is use SqrlPseudoRandomNumberGenerator as the default concrete
	/// implementation for ISqrlPseudoRandomNumberGenerator, and they never need to know
	/// about SodiumPseudoRandomNumberGenerator.
	/// </summary>
	public class SqrlPseudoRandomNumberGenerator : SodiumPseudoRandomNumberGenerator, ISqrlPseudoRandomNumberGenerator
	{
	}
}