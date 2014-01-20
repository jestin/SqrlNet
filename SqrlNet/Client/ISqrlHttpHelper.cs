using System.Net;

namespace SqrlNet.Client
{
	public interface ISqrlHttpHelper
	{
		HttpWebRequest GetRequest(SqrlData data);
	}
}