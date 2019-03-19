using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Foundation.SitecoreExtensions.Repositories
{
    public interface IDictionaryPhraseRepository
    {
        string Get(string key);
    }
}