using CT.SC.Feature.PageContent.Models.Promo;
using CT.SC.Foundation.DependencyInjection;
using CT.SC.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Feature.PageContent.Repositories.Promo
{
	[Service(typeof(IPromoRepository))]
	public class PromoRepository: IPromoRepository
	{
		public PromoContentModel GetModel(string dataSourceId)
		{
			PromoContentModel promo = new PromoContentModel();

			if (string.IsNullOrEmpty(dataSourceId))
			{
				throw new ArgumentNullException(nameof(dataSourceId));
			}

			Item contextItem = Sitecore.Context.Database.GetItem(dataSourceId);

			if (contextItem == null)
			{
				throw new ArgumentNullException(nameof(contextItem));
			}
			if (!contextItem.IsDerived(CT.SC.Feature.PageContent.Templates._PromoItem.ID))
			{
				Sitecore.Diagnostics.Log.Error("Item must derive from Promo", nameof(contextItem));
				throw new ArgumentException("Item must derive from Promo", nameof(contextItem));
			}

			if (!Foundation.SitecoreExtensions.Methods.Utilities.IsItemNullEmpty(contextItem))
			{
				promo = PromoContentFactory.GetPromoItemDetails(contextItem);
			}
			return promo;
		}
	}
}