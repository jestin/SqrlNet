using SqrlNet;

namespace SqrlServerExample.Data
{
	public class SqrlUser : ISqrlUser
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public bool Initialized { get; set; }
		public string VerifyUnlockKey { get; set; }
		public string ServerUnlockKey { get; set; }
	}
}