using System;
using System.Collections.Generic;
using CT.Environment.XConnect.Model.Models.Preference;
using Sitecore.XConnect;

namespace CT.Environment.XConnect.Model.Models.Facets
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class PreferenceFacet : Sitecore.XConnect.Facet
    {
        public const string DefaultFacetKey = nameof(PreferenceFacet);

        public List<Communication> CommunicationPreferences { get; set; }

        public List<AreaOfInterest> AreaOfInterestPreferences { get; set; }
    }
}