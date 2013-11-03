using System;
using SqrlNet;

namespace SqrlGtk
{
	public partial class PasswordDialog : Gtk.Dialog
	{
		#region Properties

		public string Password { get; set; }
		public SqrlIdentity Identity { get; set; }

		#endregion

		public PasswordDialog(SqrlIdentity identity)
		{
			this.Build();

			Identity = identity;

			this.Title = string.Format("Enter Password for {0}", identity.Name);
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			if(!string.IsNullOrEmpty(passwordEntry.Text))
			{
				Password = passwordEntry.Text;
				Respond(Gtk.ResponseType.Ok);
			}
		}
	}
}

