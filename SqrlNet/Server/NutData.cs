using System;
using System.Net;

namespace SqrlNet.Server
{
	public class NutData
	{
		#region constructors

		public NutData()
		{
		}

		public NutData(NutStruct nutStruct)
		{
			// TODO: Create a NutData object from a NutStruct
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>
		/// The IP address of the request to the login page.
		/// </value>
		public IPAddress Address { get; set; }

		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		/// <value>
		/// The timestamp.
		/// </value>
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the counter.
		/// </summary>
		/// <value>
		/// A rolling up-counter to encrypt into the nut.
		/// </value>
		public Int32 Counter { get; set; }

		/// <summary>
		/// Gets or sets the entropy.
		/// </summary>
		/// <value>
		/// Pseudo-random noise to encrypt into the nut.
		/// </value>
		public byte[] Entropy { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the nut struct.
		/// </summary>
		/// <returns>
		/// The nut struct.
		/// </returns>
		public NutStruct GetNutStruct()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

