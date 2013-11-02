using System;
using System.Net;
using System.Runtime.InteropServices;

namespace SqrlNet.Server
{
	[StructLayout(LayoutKind.Explicit)]
	public struct NutStruct
	{
		/// <summary>
		/// The bytes.
		/// </summary>
		[FieldOffset(0)]
		public byte[] Bytes;

		/// <summary>
		/// The address.
		/// </summary>
		[FieldOffset(0)]
		public UInt32 Address;

		/// <summary>
		/// The timestamp.
		/// </summary>
		[FieldOffset(32)]
		public UInt32 Timestamp;

		/// <summary>
		/// The counter.
		/// </summary>
		[FieldOffset(64)]
		public UInt32 Counter;

		/// <summary>
		/// The entropy.
		/// </summary>
		[FieldOffset(96)]
		public byte[] Entropy;
	}
}

