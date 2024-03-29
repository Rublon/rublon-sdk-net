﻿using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using Rublon.Sdk.Core.Validation;
using System;
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
        protected string FailMode { get; set; } = "";
        protected string SendUPN { get; set; }
        protected string Token { get; set; }
        public string CurrentVersion
        {
            get
            {
                return string.Format("{0}.{1}.{2}", Assembly.GetExecutingAssembly().GetName().Version.Major,
                    Assembly.GetExecutingAssembly().GetName().Version.Minor,
                    Assembly.GetExecutingAssembly().GetName().Version.Build);
            }
        }

        public AppInit(IRublon rublon, string appVersion, string failMode, string sendUPN, string token, string key) : base(rublon, key)
        {
            AppVersion = appVersion;
            FailMode = failMode;
            SendUPN = sendUPN;
            Token = token;
            ResponseValidator = new CheckApplicationAPIResponseValidator();
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
            parameters["systemToken"] = Token;
            parameters.Add(FIELD_APP_VERSION, AppVersion);           

            var requestParameters = new 
            {
                sdkVer = CurrentVersion,
                systemVersion = systemName,
                systemBuild = systemBuild,

                config = new
                {
                    FailMode = FailMode,
                    SendUPN = SendUPN
                }
            };

            parameters.Add(FIELD_PARAMS, JToken.FromObject(requestParameters));
            return parameters;
        }
    }
}
