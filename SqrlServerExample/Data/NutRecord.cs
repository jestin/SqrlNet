using System;

namespace SqrlServerExample.Data
{
	public class NutRecord
	{
		public string Id { get; set; }
		public DateTime Timestamp { get; set; }
		public bool Validated { get; set; }
		public string UserId { get; set; }
	}
}

