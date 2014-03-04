using System.Net;

namespace SqrlNet.Client
{
	/// <summary>
	/// Interface for a helper class that takes SQRL data and forms various http requests.
	/// </summary>
	/// <remarks>
	/// When implemented, this interface will create the actual web requests that make up
	/// the SQRL protocol.  It is intended to be used aside an implementation of
	/// ISqrlClient.
	/// </remarks>
	public interface ISqrlHttpHelper
	{
		/// <summary>
		/// Gets the SQRL HTTP request for a login.
		/// </summary>
		/// <returns>
		/// The login request.
		/// </returns>
		/// <param name='data'>
		/// The SQRL data to be included in the request.
		/// </param>
		HttpWebRequest GetLoginRequest(SqrlLoginData data);
	}
}