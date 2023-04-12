using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rublon.Sdk.TwoFactor.API
{
    public class AppInit : APIMethod
    {
        public const string REQUEST_URI_PATH = "/api/app/init";
        public const string FIELD_APP_VERSION = "appVer";
        public const string FIELD_SDK_VERSION = "systemToken";

        protected string AppVersion { get; set; } = "";
        public string CurrentVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public AppInit(IRublon rublon, string appVersion) : base(rublon)
        {
            AppVersion = appVersion;
        }     

        protected override string getUrl()
        {
            return rublon.APIServer + REQUEST_URI_PATH;
        }

        protected override JObject prepareRequestBody()
        {
            var parameters = base.prepareRequestBody();
            parameters.Add(FIELD_APP_VERSION, AppVersion);
            parameters.Add(FIELD_SDK_VERSION, CurrentVersion);
            return parameters;
        }
    }
}
