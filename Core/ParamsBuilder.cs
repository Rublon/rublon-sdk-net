using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Rublon.Sdk.Core
{
    public class ParamsBuilder
    {
        private String logoutUrl = null;
        private Dictionary<String, String> customEntries = new Dictionary<String,String>();
        private string userIp = null;
        private string connectorVersion = null;
        public static readonly String CONNECTOR_VERSION_KEY = "appVer";
        public static readonly String LOGOUT_URL_KEY = "logoutUrl";
        public static readonly String PARAMS_KEY = "params";
        public static readonly String USER_IP_KEY = "userIP";

        public ParamsBuilder() { }

        public ParamsBuilder withLogoutUrl(string logoutUrl)
        {
            this.logoutUrl = logoutUrl;
            return this;
        }

        public ParamsBuilder withUserIp(string ip)
        {
            this.userIp = ip;
            return this;
        }

        public ParamsBuilder withCustomEntry(string key, string value)
        {
            this.customEntries[key] = value;
            return this;
        }

        public JObject build()
        {
            var result = new JObject();
            if (!string.IsNullOrEmpty(logoutUrl))
            {
                result.Add(LOGOUT_URL_KEY, logoutUrl);
            }
            if (!string.IsNullOrEmpty(userIp))
            {
                result.Add(USER_IP_KEY, userIp);
            }
            if (!string.IsNullOrEmpty(connectorVersion))
            {
                result.Add(CONNECTOR_VERSION_KEY, connectorVersion);
            }
            foreach(var customEntry in customEntries)
            {
                result.Add(customEntry.Key, customEntry.Value);
            }
            if (result.HasValues)
            {
                var consumerParams = new JObject();
                consumerParams.Add(PARAMS_KEY, result);
                return consumerParams;
            }

            return result;
        }

        public ParamsBuilder withConnectorVersion(string connectorVersion)
        {
            this.connectorVersion = connectorVersion;
            return this;
        }
    }
}
