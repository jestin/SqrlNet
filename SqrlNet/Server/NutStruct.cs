using System;
using System.Net;
using System.Runtime.InteropServices;

namespace SqrlNet.Server
{
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct NutStruct
	{
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
		public UInt32 Entropy;

		/// <summary>
		/// The bytes.
		/// </summary>
		[FieldOffset(0)]
		public fixed byte ByteArray[16];

		#region Methods

		public byte[] GetBytes()
		{
			var data = new byte[16];
			fixed(byte* ptr = ByteArray)
			{
				for (int i = 0; i < 16; i++)
				{
					data[i] = ptr[i];
				}
			}

			return data;
		}

		public void SetBytes(byte[] bytes)
		{
			fixed(byte* ptr = ByteArray)
			{
				for (int i = 0; i < 16; i++)
				{
					ptr[i] = bytes[i];
				}
			}
		}

		#endregion
	}
}

