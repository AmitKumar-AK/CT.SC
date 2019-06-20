using Sitecore.Analytics;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Foundation.SitecoreExtensions.Rules
{
	public class CheckUserPersonaStringNotNull<T> : StringOperatorCondition<T> where T : RuleContext
	{
		protected override bool Execute(T ruleContext)
		{
			bool isNotNull = false;
			string personaNameFromUserProfile = string.Empty;
			Assert.ArgumentNotNull(ruleContext, "ruleContext");

			//Anonymous User flow
			if (Sitecore.Context.User != null && !Sitecore.Context.User.IsAuthenticated)
			{
				if (Sitecore.Analytics.Tracker.Current.Contact != null)
				{
					var personalFacet = Tracker.Current.Contact.GetFacet<IContactPersonalInfo>("Personal");
					personaNameFromUserProfile = personalFacet.JobTitle;
					if (string.IsNullOrEmpty(personaNameFromUserProfile))
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}

			//For logged-in user
			if (Sitecore.Context.User != null && Sitecore.Context.User.IsAuthenticated)
			{
				personaNameFromUserProfile = Sitecore.Context.User.Profile.GetCustomProperty("UserPersona");

			}
			if (string.IsNullOrEmpty(personaNameFromUserProfile))
			{
				isNotNull = false;
			}
			else
			{
				isNotNull = true;
			}

			return isNotNull;
		}
	}
}