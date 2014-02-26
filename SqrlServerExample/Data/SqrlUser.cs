namespace SqrlServerExample.Data
{
	public class SqrlUser
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public bool Initialized { get; set; }
		public byte[] VerifyUnlockKey { get; set; }
		public byte[] ServerUnlockKey { get; set; }
	}
}