using System;

namespace SqrlNet
{
	/// <summary>
	/// The list of user options the client is providing to the web server.
	/// </summary>
	[Flags]
	public enum SqrlOptions
	{
		/// <summary>
		/// When present, this option requests the web server to set a flag on this user's
		/// account to disable any alternative non-SQRL logon capability, such as weaker
		/// traditional username and password authentication.
		/// 
		/// Users who have become confident of their use of SQRL may ask their client to
		/// include this optional request. The web server should only assume this intention
		/// if the option is present in every transaction. Its absence from any interaction
		/// should immediately reset the flag and prohibition in the web server. The web
		/// server may, at its option, notice when any change has occurred and explicitly
		/// ask the user to affirm their changed intention.
		/// </summary>
		SqrlOnly = 1,

		/// <summary>
		/// When present, this option requests the web server to set a flag on this user's
		/// account to disable any alternative “out of band” change to this user's SQRL
		/// identity, such as traditional and weak “what as your favorite pet's name”
		/// non-SQRL identity authentication.
		/// 
		/// Users who have become confident of their use of SQRL may ask their client to
		/// include this optional request. The web server should only assume this intention
		/// if the option is present in every transaction. Its absence from any interaction
		/// should immediately reset the flag and prohibition in the web server. The web
		/// server may, at its option, notice when any change has occurred and explicitly
		/// ask the user to affirm their changed intention.
		/// </summary>
		HardLock = 1 << 1
	}
}