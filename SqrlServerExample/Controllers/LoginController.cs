using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using SqrlNet.Server;
using ZXing;
using System.IO;
using System.Drawing.Imaging;
using SqrlServerExample.Models;
using ZXing.Common;
using System.Net;
using System.Security.Cryptography;

namespace SqrlServerExample.Controllers
{
	public class LoginController : Controller
	{
		#region Dependencies

		private readonly ISqrlServer _sqrlServer;

		#endregion

		#region Constructors

		public LoginController(ISqrlServer sqrlServer)
		{
			_sqrlServer = sqrlServer;
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

			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(nutData.Entropy);

			var nut = _sqrlServer.GenerateNut(Globals.AesKey, Globals.AesIV, nutData);
			var url = string.Format("{0}/{1}", Url.Action("Sqrl", "Login", new {}, "sqrl"), HttpServerUtility.UrlTokenEncode(nut));
			ViewData["Message"] = url;
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

		public ActionResult Sqrl(string id)
		{
			return View();
		}

		#endregion
	}
}

