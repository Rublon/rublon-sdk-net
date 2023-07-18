using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rublon.Sdk.TwoFactor
{
    public class ProxySettingsProvider
    {
        public string baseRegistryPath = "SOFTWARE\\Rublon\\RublonProxySettings";

        public string BaseRegistryPath
        {
            get
            {
                return baseRegistryPath;
            }

            set
            {
                baseRegistryPath = value;
            }
        }

        public ProxySettings LoadSettings()
        {
            ProxySettings result = new ProxySettings();
            try
            {
                using (RegistryKey keyHandle = Registry.LocalMachine.OpenSubKey(BaseRegistryPath))
                {
                    if (keyHandle != null)
                    {

                        result.ProxyHost = keyHandle.GetValue("httpProxyHost", "").ToString();
                        result.ProxyPort = (int)UInt32.Parse(keyHandle.GetValue("httpProxyPort", 0).ToString());
                        result.ProxyUsername = keyHandle.GetValue("httpProxyUser", "").ToString();
                        result.ProxyPassword = keyHandle.GetValue("httpProxyPassword", "").ToString();
                    }
                }
            }
            catch (Exception ex)
            { }
            return result;
        }
    }
}
