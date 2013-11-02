using System;
using Gtk;
using SqrlNet.Client;
using SqrlNet.Crypto;
using System.IO;
using System.Text;

public partial class MainWindow: Gtk.Window
{	
	#region Dependencies

	private readonly ISqrlClient _sqrlClient;
	private readonly IPbkdfHandler _pbkdfHandler;
	private readonly IHmacGenerator _hmacGenerator;
	private readonly ISqrlSigner _sqrlSigner;

	#endregion

	public MainWindow(string url): base (Gtk.WindowType.Toplevel)
	{
		Build();
		Url = url;

		_pbkdfHandler = new PbkdfHandler();
		_hmacGenerator = new HmacGenerator();
		_sqrlSigner = new SqrlSigner();
		_sqrlClient = new SqrlClient(_pbkdfHandler, _hmacGenerator, _sqrlSigner);

		// very insecure way of gathering entropy, but good enough for testing temporary identities
		var identity = _sqrlClient.CreateIdentity("Test", Encoding.UTF8.GetBytes(DateTime.Now.ToLongDateString()));

		var data = _sqrlClient.GetSqrlDataForLogin(identity, "Test", Url);

		this.domainLabel.Text = string.Format("Do you want to log in to {0}", data.Domain);

		dataView.Buffer.Text = string.Format("{0}\n{1}\n{2}",
		                                     data.Url,
		                                     Convert.ToBase64String(data.PublicKey),
		                                     Convert.ToBase64String(data.Signature));
	}

	#region Public Properties

	public string Url { get; set; }

	#endregion
	
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnLoginButtonClicked(object sender, EventArgs e)
	{
		throw new System.NotImplementedException();
	}

	protected void OnCancelButtonClicked(object sender, EventArgs e)
	{
		throw new System.NotImplementedException();
	}

	#region Private Methods

	private string GetFilePath()
	{
		string homePath;
		string pathChar;
		string folderName;

		if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
		{
			homePath = Environment.GetEnvironmentVariable("HOME");
			pathChar = "/";
			folderName = ".Sqrl";
		}
		else
		{
			homePath = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%");
			pathChar = "\\";
			folderName = "Sqrl";
		}

		// check working directory first
		var workingDirFile = "." + pathChar + "identity";

		if(File.Exists(workingDirFile))
		{
			return workingDirFile;
		}

		var configPath = homePath + pathChar + folderName;

		if(!Directory.Exists(configPath))
		{
			return string.Empty;
		}

		return configPath + pathChar + "identity";
	}

	#endregion
}
