using CT.SC.Feature.PageContent.Models.Promo;
using CT.SC.Feature.PageContent.Repositories.Promo;
using Sitecore.Collections;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CT.SC.Feature.PageContent.Controllers
{
    public class PageContentController : Controller
    {
		private IPromoRepository PromoRepository  { get; }

		public PageContentController(IPromoRepository promoRepository)
		{
			this.PromoRepository = promoRepository;
		}


		// GET: PageContent
		public ActionResult PromoContent()
        {
			PromoContentModel promoContentModel = this.PromoRepository.GetModel(RenderingContext.Current.Rendering.DataSource);
			return View("~/Views/PageContent/PromoContent.cshtml",promoContentModel);
        }
    }
}