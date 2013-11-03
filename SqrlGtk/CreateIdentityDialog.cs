using System;

namespace SqrlGtk
{
	public partial class CreateIdentityDialog : Gtk.Dialog
	{
		#region Public Properties

		public string IdentityName { get; set; }
		public string Password { get; set; }

		#endregion

		public CreateIdentityDialog()
		{
			this.Build();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			if((!string.IsNullOrEmpty(nameEntry.Text)) && (passwordEntry.Text == confirmPasswordEntry.Text))
			{
				IdentityName = nameEntry.Text;
				Password = passwordEntry.Text;
				Respond(Gtk.ResponseType.Ok);
			}
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			//throw new System.NotImplementedException();
		}
	}
}

