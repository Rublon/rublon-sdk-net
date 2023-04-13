﻿using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using System.Reflection;

namespace Rublon.Sdk.TwoFactor.API
{
    public class AppInit : APIMethod
    {
        public const string REQUEST_URI_PATH = "/api/app/init";
        public const string FIELD_APP_VERSION = "appVer";
        public const string FIELD_PARAMS = "params";
        public const string FIELD_SDK_VERSION = "sdkVer";
        public const string FIELD_SYSTEM_VERSION = "systemVersion";
        public const string FIELD_SYSTEM_BUILD = "systemBuild";
        public static readonly string BASE_REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";

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
            string systemName = "";
            string systemBuild = "";
            using (RegistryKey keyHandle = Registry.LocalMachine.OpenSubKey(BASE_REGISTRY_PATH))
            { 
                systemName = keyHandle.GetValue("ProductName").ToString();
                systemBuild = keyHandle.GetValue("CurrentBuildNumber").ToString();
            }

            var parameters = base.prepareRequestBody();
            parameters.Add(FIELD_APP_VERSION, AppVersion);
            string[,] requestParameters = new string[,] { { FIELD_SDK_VERSION, CurrentVersion },
                                                          { FIELD_SYSTEM_VERSION, systemName },
                                                          { FIELD_SYSTEM_BUILD, systemBuild } 
                                                        };
            string param = JsonConvert.SerializeObject(requestParameters);
            parameters.Add(FIELD_PARAMS, param);
            return parameters;
        }
    }
}