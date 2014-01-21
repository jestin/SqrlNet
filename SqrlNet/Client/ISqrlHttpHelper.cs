using System.Net;

namespace SqrlNet.Client
{
	/// <summary>
	/// Interface for a helper class that takes SQRL data and forms various http requests.
	/// </summary>
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
		HttpWebRequest GetLoginRequest(SqrlData data);
	}
}