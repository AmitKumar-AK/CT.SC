using CT.SC.Feature.PageContent.Models.Promo;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Feature.PageContent.Repositories.Promo
{
	public interface IPromoRepository
	{
		PromoContentModel GetModel(string dataSourceId);
	}
}