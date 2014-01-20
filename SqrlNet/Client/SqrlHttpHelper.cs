using System.Net;
using System.Text;

namespace SqrlNet.Client
{
	public class SqrlHttpHelper : ISqrlHttpHelper
	{
		#region ISqrlHttpHelper implementation

		public HttpWebRequest GetRequest(SqrlData data)
		{
			var request = (HttpWebRequest) WebRequest.Create("http://" + data.Url);
			request.Method = "POST";

			string postData = string.Format("publickey={0}&signature={1}&url={2}",
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
	}
}