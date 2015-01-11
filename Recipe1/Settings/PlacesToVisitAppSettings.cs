using System.Collections.Generic;
using ServiceStack.Configuration;

namespace Recipe1.Settings
{
    public class PlacesToVisitAppSettings : AppSettings
    {
        public ApplicationEnvironment Environment
        {
            get { return this.Get("Environment", ApplicationEnvironment.Dev); }
        }

        public List<string> AdministratorEmails
        {
            get
            {
                return this.Get<List<string>>("AdminEmailAddresses",
                    new List<string>());
            }
        }

        public enum ApplicationEnvironment
        {
            Dev,
            Test,
            Prod
        }
    }
}