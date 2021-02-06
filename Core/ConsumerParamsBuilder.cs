using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Rublon.Sdk.Core
{
    public class ConsumerParamsBuilder
    {
        private String logoutUrl = null;
        private Dictionary<String, String> customEntries = new Dictionary<String,String>();

        public static readonly String LOGOUT_URL_KEY = "logoutUrl";
        public static readonly String CONSUMER_PARAMS_KEY = "consumerParams";

        public ConsumerParamsBuilder() { }

        public ConsumerParamsBuilder withLogoutUrl(string logoutUrl)
        {
            this.logoutUrl = logoutUrl;
            return this;
        }

        public ConsumerParamsBuilder withCustomEntry(string key, string value)
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
            foreach(var customEntry in customEntries)
            {
                result.Add(customEntry.Key, customEntry.Value);
            }
            if (result.HasValues)
            {
                var consumerParams = new JObject();
                consumerParams.Add(CONSUMER_PARAMS_KEY, result);
                return consumerParams;
            }

            return result;
        }


    }
}
