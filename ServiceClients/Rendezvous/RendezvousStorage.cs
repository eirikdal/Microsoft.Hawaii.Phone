// -
// <copyright file="RendezvousStorage.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Rendezvous.Client
{
#if WINDOWS_PHONE || SILVERLIGHT
    using System.Diagnostics;
    using System.IO.IsolatedStorage;

    /// <summary>
    /// Helper class to store names and its secret key pair into the mobile's 
    /// isolated storage.
    /// </summary>
    public static class RendezvousStorage
    {
        /// <summary>Container name for App Relay Endpoint ids</summary>
        private const string ContainerName = "Microsoft.Hawaii.Rendezvous.SampleAppWinRT";

        /// <summary>
        /// Helper method to retrieve the secret key.
        /// </summary>
        /// <param name="registrationId">Specifies a registration id.</param>
        /// <returns>String value for specified key.</returns>
        public static string GetSecretKey(string registrationId)
        {
            return RendezvousStorage.GetValue(registrationId);
        }

        /// <summary>
        /// Helper method to store the secret key.
        /// </summary>
        /// <param name="registrationId">Registration key.</param>
        /// <param name="secretKey">Secret key.</param>
        public static void SetSecretKey(string registrationId, string secretKey)
        {
            RendezvousStorage.SetValue(registrationId, secretKey);
        }

        /// <summary>
        /// Gets a value for specified key from isolated storage.
        /// </summary>
        /// <returns>An endpoint object.</returns>
        private static string GetValue(string key)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains(key))
            {
                return settings[key].ToString();
            }

            return null;
        }

        /// <summary>
        /// Stores a value into Isolated storage.
        /// </summary>
        /// <param name="key">Specifies a key.</param>
        /// <param name="value">Specifies a value.</param>
        private static void SetValue(string key, string value)
        {
            Debug.Assert(!string.IsNullOrEmpty(key), "key is empty or null");

            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
        }

        /// <summary>
        /// Helper method removes the key.
        /// </summary>
        /// <param name="key">Specifies a key.</param>
        private static void RemoveKey(string key)
        {
            Debug.Assert(!string.IsNullOrEmpty(key), "key is empty or null");

            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(key))
            {
                settings.Remove(key);
            }
        }
    }
#endif
}


