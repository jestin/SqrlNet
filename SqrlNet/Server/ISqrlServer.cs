namespace SqrlNet.Server
{
	/// <summary>
	/// An interface that contains all the functionality to create the server-side
	/// implentation of the SQRL protocol.
	/// </summary>
	/// <remarks>
	/// When implemented, this interface will provide all the required functionality of
	/// the server-side responsibilities of the SQRL protocol.
	/// </remarks>
	public interface ISqrlServer
	{
		/// <summary>
		/// Generates a nut.
		/// </summary>
		/// <returns>
		/// The nut.
		/// </returns>
		/// <param name='key'>
		/// An encryption key that is used on the generated data to return and encrypted nut.
		/// </param>
		/// <param name='iv'>
		/// The initialization vector for the Rijndael cipher.
		/// </param>
		byte[] GenerateNut(byte[] key, byte[] iv);

		/// <summary>
		/// Generates the nut.
		/// </summary>
		/// <returns>
		/// The nut.
		/// </returns>
		/// <param name='key'>
		/// An encryption key that is used on the given data to return and encrypted nut.
		/// </param>
		/// <param name='iv'>
		/// The initialization vector for the Rijndael cipher.
		/// </param>
		/// <param name='data'>
		/// Data to encrypt into the nut.
		/// </param>
		byte[] GenerateNut(byte[] key, byte[] iv, NutData data);

		/// <summary>
		/// Dycrypts a nut.
		/// </summary>
		/// <returns>
		/// The data encrypted into the nut.
		/// </returns>
		/// <param name='key'>
		/// The encryption key.
		/// </param>
		/// <param name='iv'>
		/// The initialization vector for the Rijndael cipher.
		/// </param>
		/// <param name='nut'>
		/// The nut.
		/// </param>
		NutData DecryptNut(byte[] key, byte[] iv, byte[] nut);

		/// <summary>
		/// Verifies the sqrl request.
		/// </summary>
		/// <returns>
		/// Whether the public key provided decrypts the signature provided.
		/// </returns>
		/// <param name='data'>
		/// The data contained in the SQRL request.
		/// </param>
		/// <param name="expectedUrl">
		/// The URL that is expected from the request.
		/// </param>
		bool VerifySqrlRequest(SqrlLoginData data, string expectedUrl);
	}
}