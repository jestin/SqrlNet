using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using SqrlServerExample.DataAccess;

namespace Controllers
{
	public class HomeController : Controller
	{
		#region Dependencies

		private readonly IUserRepository _userRepository;

		#endregion

		#region Constructors

		public HomeController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		#endregion

		#region Actions

		public ActionResult Index()
		{
			ViewData["Message"] = "Welcome to ASP.NET MVC on Mono!";
			return View();
		}

		[Authorize]
		public ActionResult Welcome()
		{
			var user = _userRepository.Retrieve(User.Identity.Name);

			if(!user.Initialized)
			{
				return RedirectToAction("Register", "Login");
			}

			return View(user);
		}

		#endregion
	}
}

