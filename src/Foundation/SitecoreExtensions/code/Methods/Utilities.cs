using System;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace CT.SC.Foundation.SitecoreExtensions.Methods
{
    public static class Utilities
    {
        public static Boolean IsItemNullEmpty(object item)
        {
            Boolean isItemNull = true;

            if (item != null)
            {
                isItemNull = false;
            }

            return isItemNull;
        }

        public static string GetImageUrl(Item item, ID key)
        {
            string imgUrl = string.Empty;

            if (!string.IsNullOrEmpty(System.Convert.ToString(item[key])))
            {
                Sitecore.Data.Fields.ImageField imageField = ((Sitecore.Data.Fields.ImageField)item.Fields[key]);
                if (imageField?.MediaItem != null)
                {
                    var image = new MediaItem(imageField.MediaItem);
                    imgUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(image));
                }
            }

            return imgUrl;
        }

        public static string GetImageUrl(Item item, ID key, ref string altText)
        {
            string imgUrl = string.Empty;
            altText = string.Empty;
            if (!string.IsNullOrEmpty(System.Convert.ToString(item[key])))
            {
                Sitecore.Data.Fields.ImageField imageField = ((Sitecore.Data.Fields.ImageField)item.Fields[key]);
                if (imageField?.MediaItem != null)
                {
                    var image = new MediaItem(imageField.MediaItem);
                    imgUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(image));
                    altText = imageField.Alt;
                }
            }

            return imgUrl;
        }

        public static String LinkUrl(Item item, ID key, ref bool isExternal, ref string altText)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(System.Convert.ToString(item[key])))
            {
                Sitecore.Data.Fields.LinkField linkfield = ((Sitecore.Data.Fields.LinkField)item.Fields[key]);
                if (linkfield != null)
                {
                    url = LinkUrl(linkfield, ref isExternal, ref altText);
                }
            }

            return url;
        }
        public static String LinkUrl(this Sitecore.Data.Fields.LinkField lf, ref bool isExternal, ref string altText)
        {
            isExternal = false;
            altText = string.Empty;
            try
            {
                if (!IsItemNullEmpty(lf.GetAttribute("title")))
                {
                    altText = lf.GetAttribute("title");
                }
            }
            catch
            {

            }

            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(lf.TargetItem) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    isExternal = true;
                    return lf.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return lf.Url;
                case "javascript":
                    // Just return javascript
                    return lf.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return lf.Url;
            }
        }

    }
}