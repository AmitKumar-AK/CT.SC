using System;
using Sitecore.XConnect;

namespace CT.Environment.XConnect.Model.Models.Facets
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class EmployerFacet : Sitecore.XConnect.Facet
    {
        public const string DefaultFacetKey = nameof(EmployerFacet);

        public string SubTitle { get; set; }

        public string Company { get; set; }

        public bool Consent { get; set; }

        public string PreferredOfficeLocation { get; set; }

    }
}
