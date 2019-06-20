using System;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace CT.SC.Foundation.SitecoreExtensions.LayoutService.ItemRendering.ContentsResolvers
{
	public class CustomResolverDoctorRelatedContents : RenderingContentsResolver
	{
		public bool IncludeServerUrlInMediaUrls { get; set; }
		public bool UseContextItem { get; set; }
		public string ItemSelectorQuery { get; set; }
		public NameValueCollection Parameters { get; set; }

		/*
		public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
		{
			
			//var datasource = !string.IsNullOrEmpty(rendering.DataSource)
			//	? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
			//	: null;

			//return new
			//{
			//	name = datasource.Name,
			//	date = DateTime.Now,
			//	hello = "world"
			//};
			
			Item contextItem = rendering.Item;

			var jObject = base.ProcessItem(contextItem, renderingConfig);

			if (contextItem == null)
			{
				return jObject;
			}

			Sitecore.Collections.ChildList relatedItem = contextItem.GetChildren();

			if (relatedItem == null)
			{
				return jObject;
			}

			//foreach (Item item in relatedItem)
			//{

			//}

			jObject["items"] = base.ProcessItems(relatedItem, renderingConfig);

			return new
			{
				name = contextItem.Name,
				date = DateTime.Now,
				items = jObject
			};

			//return new
			//{
			//	name = contextItem.Name,
			//	date = DateTime.Now,
			//	hello = "world"
			//};
			//return jObject;


		}
		*/
		protected override JObject ProcessItem(Item item, IRenderingConfiguration renderingConfig)
		{

			var jObject = base.ProcessItem(item, renderingConfig);

			if (item.Children.Count == 0)
			{
				return jObject;
			}

			jObject["items"] = ProcessItems(item.Children, renderingConfig);

			return jObject;
		}
	}
}
