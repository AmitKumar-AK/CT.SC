
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Sitecore.LayoutService.ItemRendering;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Sitecore.Collections;
using System.Collections.Generic;
using Sitecore;
using System;
using Sitecore.LayoutService.Serialization.ItemSerializers;
using Sitecore.Layouts;
using Sitecore.LayoutService.Extensions;
using Sitecore.LayoutService.Placeholders;

namespace CT.SC.Foundation.SitecoreExtensions.LayoutService.ItemRendering.ContentsResolvers
{
	public class ContentResolverPlaceholderContents : RenderingContentsResolver
	{
		public bool IncludeServerUrlInMediaUrls { get; set; }
		public bool UseContextItem { get; set; }
		public string ItemSelectorQuery { get; set; }
		public NameValueCollection Parameters { get; set; }


		protected readonly ILayoutService LayoutService;
		protected readonly IPlaceholderRenderingService PlaceholderService;
		protected readonly ILayoutServiceContext ServiceContext;
		//private ILayoutService LayoutService { get; }

		public ContentResolverPlaceholderContents():this(ServiceLocator.ServiceProvider.GetService<ILayoutService>(),
			ServiceLocator.ServiceProvider.GetService<IPlaceholderRenderingService>(),
			ServiceLocator.ServiceProvider.GetService<ILayoutServiceContext>())
		{

		}

		public ContentResolverPlaceholderContents(ILayoutService layoutService, IPlaceholderRenderingService placeholderService,
			ILayoutServiceContext serviceContext)
		{
			this.LayoutService = layoutService;
			this.PlaceholderService = placeholderService;
			this.ServiceContext = serviceContext;
		}

		protected override JObject ProcessItem(Item item, Rendering rendering, IRenderingConfiguration renderingConfig)
		{

			var jObject = base.ProcessItem(item, rendering, renderingConfig);

			if (item.Children.Count == 0)
			{
				return jObject;
			}

			var childItem = item.GetChildren().Where(x => (x.TemplateID.ToString().Equals("{3853E9D2-FE2D-4933-B24B-EF05B5D84167}"))).ToList();
			if (childItem != null && childItem.Count >0)
			{
				Sitecore.Data.Fields.MultilistField refMultilistField = childItem[0].Fields["Doctors"];
				if (refMultilistField != null)
				{
					Item[] items = refMultilistField.GetItems();

					if (items != null && items.Length >0)
					{
						//RenderedItem rendered = LayoutService.Render(items[0], renderingConfig, GetLayoutServiceRenderOptions(items[0]));

						//--Child Item Layout details - This is the use case
						RenderedItem rendered = Render(items[0], renderingConfig, null);

						//--For testing purpose, trying to see current context item where component added
						RenderedItem renderedParent = Render(item, renderingConfig, GetLayoutServiceRenderOptions(item));
					}
					
				}
			}
			//homeItem.Axes.GetDescendants().Where(x => (x.TemplateID.ToString().Equals(yourId1) || x.TemplateID.ToString().Equals(yourId2));


			jObject["items"] = ProcessItems(item.Children, rendering, renderingConfig);

			return jObject;
		}

		protected RenderOptions GetLayoutServiceRenderOptions(Item item)
		{
			return new RenderOptions
			{
				RootRendering = RenderingContext.Current.Rendering
			};
		}

		public Rendering GetRootRendering(Item item)
		{
			LayoutItem layout = item.Visualization.Layout;
			return new Rendering
			{
				Item = item,
				DeviceId = (Context.Device?.ID.Guid ?? Guid.Empty),
				RenderingItem = new RenderingItem(layout.InnerItem),
				LayoutId = layout.ID.Guid,
				RenderingType = "Layout"
			};
		}

		public IList<RenderedPlaceholderElement> CollectPlaceholderElements(IList<RenderedPlaceholder> placeholders)
		{
			Stack<RenderedPlaceholder> stack = new Stack<RenderedPlaceholder>(placeholders);
			List<RenderedPlaceholderElement> list = new List<RenderedPlaceholderElement>();
			while (stack.Count > 0)
			{
				RenderedPlaceholder renderedPlaceholder = stack.Pop();
				foreach (RenderedPlaceholderElement element in renderedPlaceholder.Elements)
				{
					list.Add(element);
					RenderedJsonRendering renderedJsonRendering = element as RenderedJsonRendering;
					if (renderedJsonRendering?.Placeholders != null)
					{
						foreach (RenderedPlaceholder placeholder in renderedJsonRendering.Placeholders)
						{
							stack.Push(placeholder);
						}
					}
				}
			}
			return list;
		}

		public RenderedItem Render(Item item, IRenderingConfiguration renderingConfiguration, RenderOptions renderOptions = null)
		{
			if (renderOptions == null)
			{
				renderOptions = new RenderOptions();
			}
			if (renderOptions.RootRendering == null)
			{
				renderOptions.RootRendering = GetRootRendering(item);
			}
			if (renderOptions.ContextData == null)
			{
				renderOptions.ContextData = new Dictionary<string, object>();
			}
			RenderedItem renderedItem = new RenderedItem
			{
				ItemId = item.ID.Guid,
				ItemVersion = item.Version.Number,
				DeviceId = renderOptions.RootRendering.DeviceId,
				LayoutId = renderOptions.RootRendering.LayoutId,
				DatabaseName = item.Database.Name,
				ItemLanguage = item.Language.Name,
				TemplateId = item.TemplateID.Guid,
				TemplateName = item.TemplateName
			};
			using (new ContextItemSwitcher(item))
			{
				renderedItem.Name = item.Name;
				renderedItem.DisplayName = item.DisplayName;
				IItemSerializer itemSerializer = renderingConfiguration.ItemSerializer;
				string json = itemSerializer.Serialize(item);
				JObject jObject2 = renderedItem.Fields = JObject.Parse(json);
				//Assert.IsNotNull(renderingConfiguration.PlaceholdersResolver, "renderingConfiguration.PlaceholdersResolver");
				IList<Sitecore.LayoutService.Placeholders.PlaceholderDefinition> placeholderDefinitions = renderingConfiguration.PlaceholdersResolver.ExtractPlaceholders(renderOptions.RootRendering);
				IList<RenderedPlaceholder> placeholders = renderedItem.Placeholders = PlaceholderService.RenderPlaceholders(placeholderDefinitions, renderingConfiguration);
				renderedItem.Elements = CollectPlaceholderElements(placeholders);
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Merge(renderOptions.ContextData);
				dictionary.Merge(ServiceContext.GetContextData(item, renderingConfiguration, renderOptions));
				renderedItem.Context = dictionary;
				return renderedItem;
			}
		}

	}
}
