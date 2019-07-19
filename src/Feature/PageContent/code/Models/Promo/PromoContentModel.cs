using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Feature.PageContent.Models.Promo
{
	public class PromoContentModel
	{
		public Item DataSourceItem { get; set; }
		public string PromoText { get; set; }
		public string PromoIcon { get; set; }
		public string PromoLink { get; set; }
		public string PromoText2 { get; set; }
		public string PromoIcon2 { get; set; }
		public string PromoLink2 { get; set; }
		public string AdvanceImage { get; set; }
		public string ItemUrl { get; set; }
	}
}