﻿namespace CT.SC.Foundation.Assets.Pipelines.GetPageRendering
{
    using CT.SC.Foundation.Assets.Repositories;
    using Sitecore.Mvc.Pipelines.Response.GetPageRendering;

    public class ClearAssets : GetPageRenderingProcessor
    {
        public override void Process(GetPageRenderingArgs args)
        {
            AssetRepository.Current.Clear();
        }
    }
}