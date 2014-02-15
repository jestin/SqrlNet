using System.Net;
using System.Text;

namespace SqrlNet.Client
{
	/// <summary>
	/// A helper class that takes SQRL data and forms various http requests.
	/// </summary>
	public class SqrlHttpHelper : ISqrlHttpHelper
	{
		#region ISqrlHttpHelper implementation

		/// <summary>
		///  Gets the SQRL HTTP request for a login. 
		/// </summary>
		/// <returns>
		///  The login request. 
		/// </returns>
		/// <param name='data'>
		///  The SQRL data to be included in the request. 
		/// </param>
		public HttpWebRequest GetLoginRequest(SqrlData data)
		{
			var request = (HttpWebRequest) WebRequest.Create("http://" + data.Url);
			request.Method = "POST";

			string postData = string.Format("client={0}&server={1}&ids={2}&pids={3}",
			                                GetClientParameter(data, SqrlCommand.Login),
			                                GetServerParameter(data),
			                                Utility.ConvertToSqrlBase64String(data.PublicKey),
			                                Utility.ConvertToSqrlBase64String(data.Signature),
			                                data.Url);

			var byteArray = Encoding.UTF8.GetBytes(postData);
			request.UserAgent = "SQRL/1";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = byteArray.Length;
			var dataStream = request.GetRequestStream();
			dataStream.Write(byteArray, 0, byteArray.Length);
			dataStream.Close();

			return request;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the client parameter.
		/// </summary>
		/// <returns>
		/// The client parameter.
		/// </returns>
		/// <param name='data'>
		/// The SQRL data.
		/// </param>
		/// <param name='cmd'>
		/// The command.
		/// </param>
		public string GetClientParameter(SqrlData data, SqrlCommand cmd)
		{
			string result = string.Empty;

			switch(cmd)
			{
				case SqrlCommand.SetKey:
					break;
				case SqrlCommand.SetLock:
					break;
				case SqrlCommand.Create:
					break;
				case SqrlCommand.Login:
					result = string.Format("ver={0}\r\ncmd={1}\r\nidk={2}",
				                       1,
				                       cmd.ToString().ToLower(),
				                       data.PublicKey);
					break;
				default:
					throw new System.ArgumentOutOfRangeException();
			}

			return Utility.ConvertToSqrlBase64String(ASCIIEncoding.ASCII.GetBytes(result));
		}

		/// <summary>
		/// Gets the server parameter.
		/// </summary>
		/// <returns>
		/// The server parameter.
		/// </returns>
		/// <param name='data'>
		/// The SQRL data.
		/// </param>
		public string GetServerParameter(SqrlData data)
		{
			return string.Empty;
		}

		#endregion
	}
}