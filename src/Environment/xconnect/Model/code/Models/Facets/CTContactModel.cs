using Sitecore.XConnect;
using Sitecore.XConnect.Schema;

namespace CT.Environment.XConnect.Model.Models.Facets
{
    public class CTContactModel
    {
        public static XdbModel Model { get; } = BuildModel();

        private static XdbModel BuildModel()
        {
            var builder = new XdbModelBuilder(typeof(CTContactModel).FullName, new XdbModelVersion(1, 0));

            builder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            builder.DefineFacet<Contact, EmployerFacet>(EmployerFacet.DefaultFacetKey);
            builder.DefineFacet<Contact, PreferenceFacet>(PreferenceFacet.DefaultFacetKey);

            return builder.BuildModel();
        }
    }
}
