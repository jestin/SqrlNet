using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using SqrlNet.Server;
using SqrlServerExample.Models;
using SqrlNet;
using SqrlServerExample.DataAccess;
using SqrlServerExample.Data;

namespace SqrlServerExample.Controllers
{
	public class LoginController : Controller
	{
		#region Dependencies

		private readonly ISqrlServer _sqrlServer;
		private readonly INutRepository _nutRepository;
		private readonly IUserRepository _userRepository;

		#endregion

		#region Constructors

		public LoginController(
			ISqrlServer sqrlServer,
			INutRepository nutRepository,
			IUserRepository userRepository)
		{
			_sqrlServer = sqrlServer;
			_nutRepository = nutRepository;
			_userRepository = userRepository;
		}

		#endregion

		#region Actions

		public ActionResult Index()
		{
			var nutData = new NutData
			{
				Timestamp = DateTime.Now,
				Address = IPAddress.Parse(Request.ServerVariables["REMOTE_ADDR"]),
				Counter = ++Globals.Counter,
				Entropy = new byte[4]
			};

			// just to keep the database size down, delete all nuts older than 2 minutes, but only every 10 page requests
			if(Globals.Counter % 10 == 0)
			{
				var deleted = _nutRepository.DeleteOlderThan(DateTime.Now.AddMinutes(-2));
				Console.WriteLine("{0} nuts deleted", deleted);
			}

			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(nutData.Entropy);

			var nut = HttpServerUtility.UrlTokenEncode(_sqrlServer.GenerateNut(Globals.AesKey, Globals.AesIV, nutData));
			var url = string.Format("{0}/{1}",
			                        Url.Action("Sqrl",
												"Login",
												null,
												"sqrl",
												Request.Url.Host + ":" + Request.Url.Port),
			                        nut);

			_nutRepository.Create(nut);

			var barcodeWriter = new BarcodeWriter
			{
				Format = BarcodeFormat.QR_CODE,
				Options = new EncodingOptions
				{
					Width = 400,
					Height = 400
				}
			};

			var image = barcodeWriter.Write(url);
			MemoryStream stream = new System.IO.MemoryStream();
			image.Save(stream, ImageFormat.Png);
			byte[] imageBytes = stream.ToArray();

			var model = new SqrlLoginModel
			{
				Url = url,
				QrCode = Convert.ToBase64String(imageBytes)
			};

			return View(model);
		}

		[HttpPost]
		public ActionResult Sqrl(string id, string publickey, string signature, string url)
		{
			var data = new SqrlData
			{
				PublicKey = HttpServerUtility.UrlTokenDecode(publickey),
				Signature = HttpServerUtility.UrlTokenDecode(signature),
				Url = url
			};

			var expected = string.Format("{0}/{1}",
			                             Url.Action("Sqrl",
													"Login",
													null,
													"sqrl",
													Request.Url.Host + ":" + Request.Url.Port),
			                             id);

			if(_sqrlServer.VerifySqrlRequest(data, expected) && _nutRepository.IsNutActive(id))
			{
				var user = _userRepository.Retrieve(publickey);

				if(user == null)
				{
					// register user
					user = new SqrlUser
					{
						Id = publickey,
						Initialized = false
					};

					_userRepository.Create(user);
				}

				_nutRepository.Validate(id);

				return Content("valid");
			}

			return Content("invalid");
		}

		#endregion
	}
}

