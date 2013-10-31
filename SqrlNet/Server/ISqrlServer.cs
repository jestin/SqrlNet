using System;
using System.Net;

namespace SqrlNet.Server
{
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
		byte[] GenerateNut(byte[] key);

		/// <summary>
		/// Generates the nut.
		/// </summary>
		/// <returns>
		/// The nut.
		/// </returns>
		/// <param name='key'>
		/// An encryption key that is used on the given data to return and encrypted nut.
		/// </param>
		/// <param name='data'>
		/// Data to encrypt into the nut.
		/// </param>
		byte[] GenerateNut(byte[] key, NutData data);

		/// <summary>
		/// Dycrypts a nut.
		/// </summary>
		/// <returns>
		/// The data encrypted into the nut.
		/// </returns>
		/// <param name='key'>
		/// The encryption key.
		/// </param>
		/// <param name='nut'>
		/// The nut.
		/// </param>
		NutData DycryptNut(byte[] key, byte[] nut);

		/// <summary>
		/// Verifies the sqrl request.
		/// </summary>
		/// <returns>
		/// Whether the public key provided decrypts the signature provided.
		/// </returns>
		/// <param name='data'>
		/// The data contained in the SQRL request.
		/// </param>
		bool VerifySqrlRequest(SqrlData data);
	}
}

