using System;

namespace SqrlNet
{
	/// <summary>
	/// A static class with useful utility methods that are useful to both client
	/// and server applications.  Please be careful not to bloat this class.
	/// </summary>
	public static class Utility
	{
		#region ISqrlParser implementation

		/// <summary>
		/// Gets the URL without protocol.
		/// </summary>
		/// <returns>
		/// The URL without protocol.
		/// </returns>
		/// <param name='url'>
		/// The URL.
		/// </param>
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

		/// <summary>
		/// Gets the domain from the URL.
		/// </summary>
		/// <returns>
		/// The domain from the URL.
		/// </returns>
		/// <param name='url'>
		/// The URL.
		/// </param>
		public static string GetDomainFromUrl(string url)
		{
			// convert to lower case
			url = url.ToLower();

			// strip off scheme
			var domain = GetUrlWithoutProtocol(url);

			var atIndex = domain.IndexOf('@');

			if(atIndex >= 0)
			{
				domain = domain.Substring(atIndex + 1);
			}

			var colonIndex = domain.IndexOf(':');

			if(colonIndex >= 0)
			{
				var nextSlash = domain.IndexOf('/');
				domain = domain.Remove(colonIndex, nextSlash - colonIndex);
			}

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

		/// <summary>
		/// Converts to a base64 string, but making it URL friendly for use with SQRL.
		/// </summary>
		/// <returns>
		/// The base64 string.
		/// </returns>
		/// <param name='bytes'>
		/// The data to convert.
		/// </param>
		public static string ConvertToSqrlBase64String(byte[] bytes)
		{
			var result = Convert.ToBase64String(bytes);

			result = result.Replace('+', '-');
			result = result.Replace('/', '_');
			result = result.TrimEnd(new[] { '=' });

			return result;
		}

		/// <summary>
		/// Converts from a base64 string that has been made URL friendly.
		/// </summary>
		/// <returns>
		/// The data as it was before encoding.
		/// </returns>
		/// <param name='data'>
		/// The base64 encoded string.
		/// </param>
		public static byte[] ConvertFromSqrlBase64String(string data)
		{
			data = data.Replace('-', '+');
			data = data.Replace('_', '/');

			// re-add padding
			var m = data.Length % 4;

			if(m != 0)
			{
				for(int i = 0; i < 4 - m; i++)
				{
					data = data + "=";
				}
			}

			return Convert.FromBase64String(data);
		}

		#endregion
	}
}