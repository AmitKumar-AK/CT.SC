using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CT.SC.Feature.Accounts.Controllers
{
    public class VirtualAccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

		public JsonResult VirtualUserLogin(string email, string fName, string lName, string personaName)
		{
			var virtualUser = (User)null;
			var domainName = "extranet";
			var sitecoreUsername = $"{domainName}\\{email}";

			//--Create virtual user-::Start------//
			virtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
			virtualUser.Profile.Name = fName;
			virtualUser.Profile.FullName = fName + " " + lName;
			virtualUser.Profile.FullName = virtualUser.Profile.FullName.Trim(); //--If last name is empty or null then remove the space from end
			virtualUser.Profile.Email = email;
			virtualUser.Profile.SetCustomProperty("UserPersona", personaName);
			virtualUser.Profile.Save();
			virtualUser.Profile.Reload();
			AuthenticationManager.LoginVirtualUser(virtualUser);
			//--Create virtual user-::End------//

			//--Create Contact-::Start------//
			if (!string.IsNullOrEmpty(email))
			{
				//Tracker.Current.Session.IdentifyAs("IdentifiedEmail", email);
				TrackNewContact(fName, lName, email, personaName);
			}
			//--Create Contact-::End------//

			return Json(true, JsonRequestBehavior.AllowGet);

		}


		public JsonResult VirtualUserLogout(string email)
		{
			Sitecore.Security.Authentication.AuthenticationManager.Logout();

			//--Empty Contact-::Start------//
			if (Tracker.Current != null)
			{
				Tracker.Current.EndVisit(true);
			}

			HttpContext.Session.Abandon();
			//--Empty Contact-::End------//

			return Json(true, JsonRequestBehavior.AllowGet);
		}


		public string TrackNewContact(string firstName, string lastName, string emailAddress, string personaName)
		{
			string contactGuid = string.Empty;
			using (var client = CreateClient())
			{
				try
				{
					ITracker CurrentTracker = Tracker.Current;

					var emailIdent = new IdentifiedContactReference("IdentifiedEmail", emailAddress);

					//Identify with Email
					CurrentTracker.Session.IdentifyAs(emailIdent.Source, emailIdent.Identifier);

					var expandOptions = new ContactExpandOptions(
						CollectionModel.FacetKeys.PersonalInformation,
						CollectionModel.FacetKeys.EmailAddressList);
					var contact = client.Get(emailIdent, expandOptions);
					contactGuid = contact.Id.HasValue ? contact.Id.Value.ToString() : string.Empty;

					//Set Fields
					SetPersonalInformation(firstName, lastName, contact, client, personaName,Tracker.Current.Session);
					SetEmail(emailAddress, contact, client);

					client.Submit();

				}
				catch (Exception ex)
				{

				}
			}
			return contactGuid;
		}
		private static void SetPersonalInformation(string firstName, string lastName, Sitecore.XConnect.Contact contact,
		   IXdbContext client, string personaName, Session userSession)
		{
			if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
				return;
			var personalInfoFacet = contact.Personal() ?? new PersonalInformation();
			//Set persona value
			personalInfoFacet.JobTitle = personaName;
			client.SetPersonal(contact, personalInfoFacet);

			BoostUserPattern(userSession, personaName);

			if (personalInfoFacet.FirstName == firstName && personalInfoFacet.LastName == lastName)
				return;
			personalInfoFacet.FirstName = firstName;
			personalInfoFacet.LastName = lastName;
			
			client.SetPersonal(contact, personalInfoFacet);
		}
		private static void SetEmail(string email, Sitecore.XConnect.Contact contact, IXdbContext client)
		{
			if (string.IsNullOrEmpty(email))
				return;
			var emailFacet = contact.Emails();
			if (emailFacet == null)
			{
				emailFacet = new EmailAddressList(new EmailAddress(email, false), "Preferred");
			}
			else
			{
				if (emailFacet.PreferredEmail?.SmtpAddress == email)
					return;
				emailFacet.PreferredEmail = new EmailAddress(email, false);
			}
			client.SetEmails(contact, emailFacet);
		}

		protected IXdbContext CreateClient()
		{
			return SitecoreXConnectClientConfiguration.GetClient();
		}

		public static void BoostUserPattern(Session userSession, string patternName)
		{
			var patternCards = Context.Database.GetItem("{C46141A2-ED84-4CE2-9DA6-8A9FE2EBE562}").Children.ToList();
			Sitecore.Data.Items.Item patternCard =null;
			//var patternCard = "";// Get the specific pattern you want from the list
			//var patternCards = "";// Get your predefined pattern cards
			/*var patternCard = patternName.ToLower().Equals("Cardiology")
				? patternCards.FirstOrDefault(w => w.Name.ToLower().Equals("Cardiology"))
				: patternCards.FirstOrDefault(w => w.Name.ToLower().Equals("General-Surgery"));
		    */


			foreach (var pCard in patternCards)
			{
			   var patternCardItem = Sitecore.Context.Database.GetItem(pCard.ID);

				if (patternCardItem == null)
				{
					continue;
				}

				if (string.Equals(patternCardItem.Name, patternName, StringComparison.InvariantCultureIgnoreCase))
					{
					patternCard = patternCardItem;
				}

			}


				if (patternCard == null)
					{
						return;
					}

			var profile = Tracker.Current.Interaction.Profiles[patternCard.Parent.Parent.Name];

			BoostUserPattern(userSession, patternCard, profile);
		}

		public static void BoostUserPattern(Session session, Item patternCard, Profile profile)
		{
			if (patternCard != null && !patternCard.Name.Equals(profile.PatternLabel))
			{
				Sitecore.Data.Fields.XmlField xmlData = patternCard.Fields["Pattern"];
				XmlDocument xmlDoc = xmlData.Xml;

				XmlNodeList parentNode = xmlDoc.GetElementsByTagName("key");
				//var scores = new Dictionary<string, float>();
				var scores = new Dictionary<string, double>();

				foreach (XmlNode childrenNode in parentNode)
				{
					if (childrenNode.Attributes != null)
					{
						scores.Add(childrenNode.Attributes["name"].Value, 0);
					}
				}

				// Set a score value here
				scores[patternCard.Name] = 5;

				profile.Score(scores);

				profile.PatternId = patternCard.ID.ToGuid();
				profile.PatternLabel = patternCard.Name;

				UpdateBehaviorProfile(session);
			}
		}

		private static void UpdateBehaviorProfile(Session session)
		{
			var profileConverterBase = BehaviorProfileConverterBase.Create();

			if (session?.Contact == null || Tracker.Current.Interaction == null)
			{
				return;
			}

			session.Contact.BehaviorProfiles.RemoveAll();

			foreach (var profileName in session.Interaction.Profiles.GetProfileNames())
			{
				var profile = session.Interaction.Profiles[profileName];

				if (!IgnoreInteractionProfile(profile))
				{
					var matchedBehaviorProfile = profileConverterBase.Convert(profile);

					session.Contact.BehaviorProfiles.Add(matchedBehaviorProfile.Id, matchedBehaviorProfile);
				}
			}
		}

		private static bool IgnoreInteractionProfile(Profile profile)
		{
			Assert.ArgumentNotNull(profile, "profile");

			return false;
		}


	}
}