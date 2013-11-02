using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SqrlServerExample.Overrides;
using StructureMap;

namespace SqrlServerExample
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);

			ObjectFactory.Initialize(x => x.AddRegistry(new DependencyRegistry()));

			ControllerBuilder.Current.SetControllerFactory(new DependencyControllerFactory());

			GenerateAes();
		}

		void GenerateAes()
		{
			var rng = new RNGCryptoServiceProvider();

			Globals.AesKey = new byte[16];
			Globals.AesIV = new byte[16];

			rng.GetBytes(Globals.AesKey);
			rng.GetBytes(Globals.AesIV);
		}

	}
}
