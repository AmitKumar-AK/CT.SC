namespace CT.SC.Foundation.Theming.Extensions
{
  using System.Web.Mvc;
  using Sitecore.Data;
  using CT.SC.Foundation.Theming.Extensions.Controls;
  using Sitecore.Mvc.Presentation;
    using Sitecore;

    public static class RenderingExtensions
  {


    public static string GetBackgroundClass([NotNull] this Rendering rendering)
    {
      var id = MainUtil.GetID(rendering.Parameters[CT.SC.Foundation.Theming.Constants.BackgroundLayoutParameters.Background] ?? "", null);
      if (ID.IsNullOrEmpty(id))
        return "";
      var item = rendering.RenderingItem.Database.GetItem(id);
      return item?[Templates.Style.Fields.Class] ?? "";
    }

    public static bool IsFixedHeight([NotNull] this Rendering rendering)
    {
      var isFixed = MainUtil.GetBool(rendering.Parameters[CT.SC.Foundation.Theming.Constants.IsFixedHeightLayoutParameters.FixedHeight] ?? "", false);
      return isFixed;
    }

    public static int GetHeight([NotNull] this Rendering rendering)
    {
      return MainUtil.GetInt(rendering.Parameters[CT.SC.Foundation.Theming.Constants.IsFixedHeightLayoutParameters.Height] ?? "", 0);
    }

    public static string GetContainerClass([NotNull] this Rendering rendering)
    {
      return rendering.IsContainerFluid() ? "container-fluid" : "container";
    }

    public static bool IsContainerFluid([NotNull] this Rendering rendering)
    {
      return MainUtil.GetBool(rendering.Parameters[CT.SC.Foundation.Theming.Constants.HasContainerLayoutParameters.IsFluid], false);
    }

    public static BackgroundRendering RenderBackground([NotNull] this Rendering rendering, HtmlHelper helper)
    {
      return new BackgroundRendering(helper.ViewContext.Writer, rendering.GetBackgroundClass());
    }
  }
}