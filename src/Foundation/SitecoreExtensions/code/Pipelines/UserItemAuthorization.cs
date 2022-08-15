using CT.SC.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Managers;
using Sitecore.JavaScriptServices.Configuration;
using Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext;
using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CT.SC.Foundation.SitecoreExtensions.Pipelines
{
    public class UserItemAuthorization : JssGetLayoutServiceContextProcessor
    {
        private bool _isSecureRequest = false;
        private string _itemUrl = string.Empty;
        private const int Allowed = 1;
        public UserItemAuthorization(IConfigurationResolver configurationResolver) : base(configurationResolver)
        {
        }

        protected override void DoProcess(GetLayoutServiceContextArgs args, AppConfiguration application)
        {
            bool isSecureAccessEnabled = false;
            _isSecureRequest = this.IsSecureRequest(ref _itemUrl);

            isSecureAccessEnabled = this.VerifyUserAccessOnSecureItem();

            args.ContextData.Add("userAccess", new
            {
                enabled = isSecureAccessEnabled
            });
        }

        /// <summary>
        /// Is Layout service request for Secure Pages
        /// </summary>
        /// <param name="itemUrl">Get item Url</param>
        /// <returns>true/false</returns>
        private bool IsSecureRequest(ref string itemUrl)
        {
            bool isSecureRequest = false;
            // "item=/home/secure/item-1";
            var itemParam = HttpContext.Current.Request.Params["item"];
            string requestUrl = Convert.ToString(itemParam);
            if (!string.IsNullOrEmpty(requestUrl) && requestUrl.ToLower().Contains(Sitecore.Configuration.Settings.GetSetting(Constants.UserItemAuthorization.SecurePagesPath).ToLower()))
            {
                //-- If "/secure/" string present in the url and we don't want to execute validation if user landing 
                // to "item=/home/secure/"; secure area landing page
                if (requestUrl.IndexOf("/") == 0)
                {
                    var regex = new Regex(Regex.Escape("/"));
                    var newText = regex.Replace(requestUrl, "", 1);
                    requestUrl = newText.ToLower();
                }

                string[] urlDetails = requestUrl.Split(new string[] { Sitecore.Configuration.Settings.GetSetting(Constants.UserItemAuthorization.SecurePagesPath).ToLower() }, StringSplitOptions.None);
                if (urlDetails != null && urlDetails.Length >= 2)
                {
                    string securePagePath = urlDetails[1];
                    string[] securePageDetails = securePagePath.Split('/');
                    if (securePageDetails != null && securePageDetails.Length >= 1)
                    {
                        string itemName = securePageDetails[0];
                        // SC.Context.Site.ContentStartPath => "/sitecore/content/CT"
                        itemUrl = Sitecore.Context.Site.ContentStartPath + "/" + urlDetails[0] + Sitecore.Configuration.Settings.GetSetting(Constants.UserItemAuthorization.SecurePagesPath).ToLower() + itemName;
                        isSecureRequest = true;
                    }
                }

            }
            return isSecureRequest;
        }

        /// <summary>
        /// Get Sitecore Item Context from full URL with protocol and host
        /// </summary>
        /// <param name="url">Item url</param>
        /// <returns>Sitecore Item</returns>
        private Sitecore.Data.Items.Item GetSitecoreItemFromUrl(string url)
        {
            Sitecore.Data.Items.Item item = null;
            string path = new Uri(url).PathAndQuery;

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                item = GetSitecoreItemFromPath(path);
            }
            return item;
        }

        /// <summary>
        /// Get Sitecore Item Context from the path after the hostname
        /// </summary>
        /// <param name="path">Item path</param>
        /// <returns>Sitecore Item</returns>
        private Sitecore.Data.Items.Item GetSitecoreItemFromPath(string path)
        {
            Sitecore.Data.Items.Item item = null;
            // remove query string
            if (path.Contains("?"))
                path = path.Split('?')[0];

            path = path.Replace(".aspx", "");

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                item = Sitecore.Context.Database.GetItem(path);
            }

            return item;
        }

        /// <summary>
        /// Get Sitecore User
        /// </summary>
        /// <param name="domainName">User's domain name</param>
        /// <param name="userName">User's email address</param>
        /// <returns>Sitecore User</returns>
        private Sitecore.Security.Accounts.User GetUser(string domainName, string userName)
        {
            if (Sitecore.Security.Accounts.User.Exists(domainName + @"\" + userName))
            {
                return Sitecore.Security.Accounts.User.FromName(domainName + @"\" + userName, true);
            }
            return null;
        }

        /// <summary>
        /// Check user access on Secure Item
        /// </summary>
        /// <returns></returns>
        private bool VerifyUserAccessOnSecureItem()
        {
            bool isSecureAccessEnabled = false;

            if (_isSecureRequest && !string.IsNullOrEmpty(_itemUrl))
            {
                var userId = Convert.ToString(HttpContext.Current.Request.Params["userid"]);
                if (!string.IsNullOrEmpty(userId))
                {
                    Sitecore.Data.Items.Item secureItem = this.GetSitecoreItemFromPath(_itemUrl);

                    if (secureItem.IsDerived(Templates.SecureItem.ID))
                    {
                        //-- If it's Secure Item then only perform the check
                        Sitecore.Security.Accounts.User currentUser = this.GetUser(Sitecore.Configuration.Settings.GetSetting(Constants.UserItemAuthorization.Publishing.ExtranetDomain).ToLower(),
                            userId.ToLower());

                        if (currentUser != null && !string.Equals(currentUser.LocalName,
                            Sitecore.Configuration.Settings.GetSetting(Constants.UserItemAuthorization.Publishing.AnonymousUser), StringComparison.InvariantCultureIgnoreCase))
                        {
                            using (new Sitecore.Security.Accounts.UserSwitcher(currentUser))
                            {
                                //-- Change context to 'user' context to verify the access
                                if (
                                    (int)secureItem.Security.GetAccessRules().Helper.GetAccessPermission(currentUser, Sitecore.Security.AccessControl.AccessRight.ItemRead, Sitecore.Security.AccessControl.PropagationType.Entity) == Allowed
                                    &&
                                    (int)secureItem.Security.GetAccessRules().Helper.GetAccessPermission(currentUser, Sitecore.Security.AccessControl.AccessRight.ItemRead, Sitecore.Security.AccessControl.PropagationType.Descendants) == Allowed
                                    )
                                {
                                    isSecureAccessEnabled = true;
                                }
                            }
                        }

                    }
                }
            }

            return isSecureAccessEnabled;
        }
    }
}