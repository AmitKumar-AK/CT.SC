using CT.SC.Foundation.SitecoreExtensions.Methods;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Feature.PageContent.Models.Promo
{
	public class PromoContentFactory
	{
		public static PromoContentModel GetPromoItemDetails(Sitecore.Data.Items.Item item)
		{
			PromoContentModel promo = null;
			if (!Utilities.IsItemNullEmpty(item))
			{
				try
				{
					#region Promo Item Details
					promo = new PromoContentModel();
					promo.ItemUrl = Sitecore.Links.LinkManager.GetItemUrl(item);
					promo.DataSourceItem = item;

					if (!Utilities.IsItemNullEmpty(System.Convert.ToString(item[Templates._PromoItem.Fields.PromoText])))
					{
						promo.PromoText = System.Convert.ToString(item[Templates._PromoItem.Fields.PromoText]);
					}

					promo.PromoIcon = Utilities.GetImageUrl(item, Templates._PromoItem.Fields.PromoIcon);

					bool isExtLink = false;
					string altText = string.Empty;
					promo.PromoLink = Utilities.LinkUrl(item, Templates._PromoItem.Fields.PromoLink, ref isExtLink, ref altText);

					if (!Utilities.IsItemNullEmpty(System.Convert.ToString(item[Templates._PromoItem.Fields.PromoText2])))
					{
						promo.PromoText2 = System.Convert.ToString(item[Templates._PromoItem.Fields.PromoText2]);
					}
					promo.PromoIcon2 = Utilities.GetImageUrl(item, Templates._PromoItem.Fields.PromoIcon2);
					isExtLink = false;
					altText = string.Empty;
					promo.PromoLink2 = Utilities.LinkUrl(item, Templates._PromoItem.Fields.PromoLink2, ref isExtLink, ref altText);

					promo.AdvanceImage = Utilities.GetAdvancedImageUrl(Templates._PromoItem.Fields.AdvanceImage, item, 300,150);
					#endregion
				}
				catch (Exception e)
				{
					Log.Error(e.Message, e, item);
				}
			}

			return promo;
		}
	}
}