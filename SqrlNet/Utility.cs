using System;

namespace SqrlNet
{
	internal static class Utility
	{
		#region ISqrlParser implementation

		public static string GetUrlWithoutProtocol(string url)
		{
			// only use this variable for validity checking, never for any cryptographic features because ToLower() will modify nonces
			var lowerUrl = url.ToLower();

			if(lowerUrl.StartsWith("sqrl://"))
			{
				return url.Substring(7);
			}

			if(lowerUrl.StartsWith("qrl://"))
			{
				return url.Substring(6);
			}

			throw new Exception("SQRL urls must begin with 'sqrl://' or 'qrl://'");
		}

		public static string GetDomainFromUrl(string url)
		{
			// strip off scheme
			var domain = GetUrlWithoutProtocol(url);

			var pipeIndex = domain.IndexOf('|');

			if(pipeIndex >= 0)
			{
				return domain.Substring(0, pipeIndex);
			}

			var slashIndex = domain.IndexOf('/');

			if(slashIndex < 0)
			{
				throw new Exception("SQRL urls must contain a '/'");
			}

			return domain.Substring(0, slashIndex);
		}

		#endregion
	}
}

