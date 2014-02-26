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
using System.Net;
using System.Web;

public partial class MainWindow: Gtk.Window
{	
	#region Dependencies

	private readonly ISqrlClient _sqrlClient;
	private readonly IPbkdfHandler _pbkdfHandler;
	private readonly IHmacGenerator _hmacGenerator;
	private readonly ISqrlSigner _sqrlSigner;
	private readonly IDiffieHellmanHandler _diffieHellmanHandler;
	private readonly ISqrlPseudoRandomNumberGenerator _prng;

	#endregion

	#region Private Variables

	private ICollection<SqrlIdentity> _identities;

	#endregion

	public MainWindow(string url): base (Gtk.WindowType.Toplevel)
	{
		Build();
		Url = url;

		_pbkdfHandler = new PbkdfHandler();
		_hmacGenerator = new HmacGenerator();
		_sqrlSigner = new SqrlSigner();
		_diffieHellmanHandler = new DiffieHellmanHandler();
		_prng = new SqrlPseudoRandomNumberGenerator();
		_sqrlClient = new SqrlClient(_pbkdfHandler, _hmacGenerator, _sqrlSigner, _diffieHellmanHandler, _prng);

		this.domainLabel.Text = string.Format("Do you want to log in to {0}", _sqrlClient.GetDomainFromUrl(url));

		_identities = GetIdentities();

		if(_identities.Count() <= 0)
		{
			var newIdentity = CreateNewIdentity();
			_identities.Add(newIdentity);
			SaveIdentities(_identities);
		}

		var comboList = new ListStore(typeof(int), typeof(string));
		var textRenderer = new CellRendererText();
		identityCombo.PackStart(textRenderer, false);
		identityCombo.AddAttribute(textRenderer, "text", 1);

		var identitiesArray = _identities.ToArray();

		for(var i = 0; i < identitiesArray.Length; i++)
		{
			comboList.AppendValues(i, identitiesArray[i].Name);
		}

		identityCombo.Model = comboList;
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
		TreeIter activeIter;
		int index = 0;
		if(identityCombo.GetActiveIter(out activeIter))
		{
			index = (int) identityCombo.Model.GetValue(activeIter, 0);
		}

		var identity = _identities.ToArray()[index];

		var passwordDlg = new PasswordDialog(identity);
		var response = (ResponseType) passwordDlg.Run();

		if(response != ResponseType.Ok)
		{
			// ???
		}

		passwordDlg.Destroy();

		if(_sqrlClient.VerifyPassword(passwordDlg.Password, identity))
		{
			var data = _sqrlClient.GetSqrlDataForLogin(identity, passwordDlg.Password, Url);

			dataView.Buffer.Text = string.Format("{0}\n{1}\n{2}",
			                                     data.Url,
			                                     Convert.ToBase64String(data.PublicKey),
			                                     Convert.ToBase64String(data.Signature));

			SendSqrlData(data);

			//Destroy();
		}
		else
		{
			// ???
		}
	}

	protected void OnCancelButtonClicked(object sender, EventArgs e)
	{
		throw new System.NotImplementedException();
	}

	#region Private Methods

	private void SendSqrlData(SqrlLoginData data)
	{
		var request = (HttpWebRequest) WebRequest.Create("http://" + data.Url);
		request.Method = "POST";

		string postData = string.Format("publickey={0}&signature={1}&url={2}",
		                                HttpServerUtility.UrlTokenEncode(data.PublicKey),
		                                HttpServerUtility.UrlTokenEncode(data.Signature),
		                                data.Url);

		var byteArray = Encoding.UTF8.GetBytes(postData);
		request.UserAgent = "SQRL/1";
		request.ContentType = "application/x-www-form-urlencoded";
		request.ContentLength = byteArray.Length;
		var dataStream = request.GetRequestStream();
		dataStream.Write(byteArray, 0, byteArray.Length);
		dataStream.Close();

		var response = request.GetResponse();

		dataStream = response.GetResponseStream();
		var reader = new StreamReader(dataStream);
		var responseFromServer = reader.ReadToEnd();

		dataView.Buffer.Text = responseFromServer;

		reader.Close();
		dataStream.Close();
		response.Close();
	}

	private SqrlIdentity CreateNewIdentity()
	{
		SqrlIdentity identity = null;
		var dlg = new CreateIdentityDialog();

		var response = (ResponseType) dlg.Run();

		if(response == ResponseType.Ok)
		{
			byte[] identityUnlockKey;
			// TODO: use better entropy
			identity = _sqrlClient.CreateIdentity(dlg.Password, Encoding.UTF8.GetBytes(DateTime.Now.ToLongDateString()), out identityUnlockKey);
			identity.Name = dlg.IdentityName;

			// this is likely not the right time to clear this memory, but
			// we need to ensure that the user prints out a copy of this key
			// before we erase it forever.
			Array.Clear(identityUnlockKey, 0, identityUnlockKey.Length);
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
