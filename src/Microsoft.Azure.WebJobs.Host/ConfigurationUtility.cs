// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
#if !NETSTANDARD1_4
using System.Configuration;
#endif

namespace Microsoft.Azure.WebJobs.Host
{
    internal static class ConfigurationUtility
    {
        public static string GetSettingFromConfigOrEnvironment(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
            {
                return null;
            }

            string configValue = null;

#if !NETSTANDARD1_4
            configValue = ConfigurationManager.AppSettings[settingName];
            if (!string.IsNullOrEmpty(configValue))
            {
                // config values take precedence over environment values
                return configValue;
            }
#endif

            return Environment.GetEnvironmentVariable(settingName) ?? configValue;
        }

        public static string GetConnectionFromConfigOrEnvironment(string connectionName)
        {
            string configValue = null;

#if !NETSTANDARD1_4
            var connectionStringEntry = ConfigurationManager.ConnectionStrings[connectionName];
            if (connectionStringEntry != null)
            {
                configValue = connectionStringEntry.ConnectionString;
            }
#endif

            if (!string.IsNullOrEmpty(configValue))
            {
                // config values take precedence over environment values
                return configValue;
            }

            return Environment.GetEnvironmentVariable(connectionName) ?? configValue;
        }
    }
}
