using System;
using System.Linq;
using Gtk;
using SqrlNet.Client;
using SqrlNet.Crypto;
using System.IO;
using System.Text;
using SqrlNet;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using SqrlGtk;

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

		var identities = GetIdentities();

		if(identities.Count() <= 0)
		{
			identities.Add(CreateNewIdentity());
			SaveIdentities(identities);
		}

		// for testing, select the first identity in the list
		var identity = identities.First();

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

	private SqrlIdentity CreateNewIdentity()
	{
		SqrlIdentity identity = null;
		var dlg = new CreateIdentityDialog();

		var response = (ResponseType) dlg.Run();

		if(response == ResponseType.Ok)
		{
			identity = _sqrlClient.CreateIdentity(dlg.Password, Encoding.UTF8.GetBytes(DateTime.Now.ToLongDateString()));
			identity.Name = dlg.IdentityName;
		}

		dlg.Destroy();

		return identity;
	}

	private ICollection<SqrlIdentity> GetIdentities()
	{
		var identitiesFile = GetFilePath();
		ICollection<SqrlIdentity> identities = new Collection<SqrlIdentity>();

		if(string.IsNullOrEmpty(identitiesFile) || !File.Exists(identitiesFile))
		{
			return identities;
		}

		using(var fs = File.OpenText(identitiesFile))
		{
			var serializer = new JsonSerializer();
			identities = (ICollection<SqrlIdentity>) serializer.Deserialize(fs, typeof(ICollection<SqrlIdentity>));
		}

		return identities;
	}

	private void SaveIdentities(IEnumerable<SqrlIdentity> identities)
	{
		var identitiesFile = GetFilePath();

		using(var fs = File.Open(identitiesFile, FileMode.OpenOrCreate))
		using(var sw = new StreamWriter(fs))
		using(var jw = new JsonTextWriter(sw))
		{
			jw.Formatting = Formatting.Indented;

			var serializer = new JsonSerializer();
			serializer.Serialize(jw, identities);
		}
	}

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
		var workingDirFile = "." + pathChar + "identities";

		if(File.Exists(workingDirFile))
		{
			return workingDirFile;
		}

		var configPath = homePath + pathChar + folderName;

		if(!Directory.Exists(configPath))
		{
			return string.Empty;
		}

		return configPath + pathChar + "identities";
	}

	#endregion
}
