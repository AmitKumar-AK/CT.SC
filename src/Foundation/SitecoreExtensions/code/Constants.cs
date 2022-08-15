namespace CT.SC.Foundation.SitecoreExtensions
{
    public struct Constants
    {
        public struct DynamicPlaceholdersLayoutParameters
        {
            public static string UseStaticPlaceholderNames => "UseStaticPlaceholderNames";
        }

        public struct UserItemAuthorization
        {
            public static string SecurePagesPath => "SecurePagesPath";

            public struct Publishing
            {
                public static string ExtranetDomain => "ExtranetDomain";
                public static string MasterDatabase => "MasterDatabase";
                public static string AnonymousUser => "AnonymousUser";
            }

        }
    }
}