using System.Collections.Generic;
using System.Web.Mvc;

namespace code.Controllers
{
	public class SearchController : Controller
	{
		[ValidateInput(false)]
		public ActionResult Index(string userinput,string sanitized)
		{
			ViewBag.Userinput = userinput;
			ViewBag.Sanitized = sanitized;
			return View(new EmptyResult());
		}
	}
}