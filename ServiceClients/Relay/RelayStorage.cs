// -
// <copyright file="RelayStorage.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
#if WINDOWS_PHONE || SILVERLIGHT
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO.IsolatedStorage;
    using System.Text;
    /// <summary>
    /// Helper class to store Endpoint id and secret key into the mobile's 
    /// isolated storage for endpoint persistence purpose.
    /// </summary>
    public static class RelayStorage
    {
        /// <summary>Value separator </summary>
        private const string ValueSeperator = "||";

        /// <summary>Object separator </summary>
        private const string ObjectSeperator = "|col|";

        /// <summary>Endpoint key</summary>
        private const string EndpointKey = "Endpoint";

        /// <summary>Created group key</summary>
        private const string CreatedGroupsKey = "CreatedGroups";

        /// <summary>Joined groups key</summary>
        private const string JoinedGroupsKey = "JoinedGroups";

        /// <summary>Messages key</summary>
        private const string MessagesKey = "Messages";

        /// <summary>Container name for App Relay Endpoint ids</summary>
        private const string ContainerName = "Microsoft.Hawaii.Relay.SampleAppWinRT";

        /// <summary>
        /// Helper method to store the endpoint.
        /// </summary>
        /// <param name="endpoint">A valid endpoint.</param>
        public static void SaveEndpoint(Endpoint endpoint)
        {
            if (endpoint == null)
            {
                RelayStorage.RemoveKey(RelayStorage.EndpointKey);
                RelayStorage.RemoveKey(RelayStorage.JoinedGroupsKey);
            }
            else
            {
                string endpointValue = string.Format(
                    "{0}{1}{2}",
                    endpoint.RegistrationId,
                    RelayStorage.ValueSeperator,
                    endpoint.SecretKey);

                RelayStorage.SetValue(RelayStorage.EndpointKey, endpointValue);
                RelayStorage.SaveGroups(RelayStorage.JoinedGroupsKey, endpoint.Groups);
            }
        }

        /// <summary>
        /// Hepler method to create endpoint using stored endpoint values.
        /// </summary>
        /// <returns>An endpoint.</returns>
        public static Endpoint ReadEndpoint()
        {
            string endpointValue = RelayStorage.GetValue(RelayStorage.EndpointKey);
            if (string.IsNullOrEmpty(endpointValue))
            {
                return null;
            }

            Endpoint endpoint = null;
            string[] values = endpointValue.Split(
                new string[] { RelayStorage.ValueSeperator },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (values == null || values.Length != 2)
            {
                return null;
            }

            endpoint = new Endpoint()
            {
                RegistrationId = values[0],
                SecretKey = values[1],
                Groups = RelayStorage.ReadGroups(RelayStorage.JoinedGroupsKey)
            };

            return endpoint;
        }

        /// <summary>
        /// Method to read groups.
        /// </summary>
        /// <param name="groups">Specifies a groups object.</param>
        public static void SaveGroups(Groups groups)
        {
            RelayStorage.SaveGroups(RelayStorage.CreatedGroupsKey, groups);
        }

        /// <summary>
        /// Method to read groups.
        /// </summary>
        /// <returns>A valid groups object.</returns>
        public static Groups ReadGroups()
        {
            return RelayStorage.ReadGroups(RelayStorage.CreatedGroupsKey);
        }

        /// <summary>
        /// Method to save messages.
        /// </summary>
        /// <param name="messages">Specifies a list of messages.</param>
        public static void SaveMessages(List<Message> messages)
        {
            RelayStorage.SaveMessages(RelayStorage.MessagesKey, messages);
        }

        /// <summary>
        /// Method to read messages.
        /// </summary>
        /// <returns>List of messages.</returns>
        public static List<Message> ReadMessages()
        {
            return RelayStorage.ReadMessages(RelayStorage.MessagesKey);
        }

        /// <summary>
        /// Method saves the groups into the storage.
        /// </summary>
        /// <param name="key">Specifies a key to store.</param>
        /// <param name="groups">Specifies a groups object.</param>
        private static void SaveGroups(string key, Groups groups)
        {
            // If the groups is null, remove the entry.
            if (groups == null)
            {
                RelayStorage.RemoveKey(key);
                return;
            }

            List<string> groupItems = new List<string>();
            foreach (Group group in groups)
            {
                groupItems.Add(
                    string.Format(
                    "{0}{1}{2}{3}{4}",
                    group.Name,
                    RelayStorage.ValueSeperator,
                    group.RegistrationId,
                    RelayStorage.ValueSeperator,
                    group.SecretKey));
            }

            string groupsValue = string.Join(RelayStorage.ObjectSeperator, groupItems.ToArray());

            RelayStorage.SetValue(key, groupsValue);
        }

        /// <summary>
        /// Method reads groups from storage.
        /// </summary>
        /// <param name="key">Specifies a key to read.</param>
        /// <returns>A Groups object.</returns>
        private static Groups ReadGroups(string key)
        {
            Groups groups = new Groups();

            string value = RelayStorage.GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return groups;
            }

            string[] groupsItems = value.Split(
                new string[] { RelayStorage.ObjectSeperator },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (groupsItems == null || groupsItems.Length == 0)
            {
                return groups;
            }

            foreach (string item in groupsItems)
            {
                string[] groupItems = item.Split(
                    new string[] { RelayStorage.ValueSeperator },
                    System.StringSplitOptions.None);

                if (groupItems == null || groupItems.Length != 3)
                {
                    continue;
                }

                Group group = new Group()
                {
                    Name = groupItems[0],
                    RegistrationId = groupItems[1],
                    SecretKey = groupItems[2]
                };

                groups.Add(group);
            }

            return groups;
        }

        /// <summary>
        /// Method saves string values of messages into the storage.
        /// </summary>
        /// <param name="key">Specifies a key to store value.</param>
        /// <param name="messages">Specifies a list of messages.</param>
        private static void SaveMessages(string key, List<Message> messages)
        {
            if (messages == null)
            {
                RelayStorage.RemoveKey(key);
                return;
            }

            List<string> messageItems = new List<string>();
            foreach (Message message in messages)
            {
                messageItems.Add(
                    string.Format(
                    "{0}{1}{2}{3}{4}",
                    message.From,
                    RelayStorage.ValueSeperator,
                    message.To,
                    RelayStorage.ValueSeperator,
                    Encoding.Unicode.GetString(message.Body, 0, message.Body.Length)));
            }

            string messagesValue = string.Join(RelayStorage.ObjectSeperator, messageItems.ToArray());

            RelayStorage.SetValue(key, messagesValue);
        }

        /// <summary>
        /// Method read messages from storage.
        /// </summary>
        /// <param name="key">Specifies a key to read.</param>
        /// <returns>List of message object.</returns>
        private static List<Message> ReadMessages(string key)
        {
            List<Message> messages = new List<Message>();

            string value = RelayStorage.GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return messages;
            }

            string[] messagesItems = value.Split(
                new string[] { RelayStorage.ObjectSeperator },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (messagesItems == null || messagesItems.Length == 0)
            {
                return messages;
            }

            foreach (string item in messagesItems)
            {
                string[] messageItems = item.Split(
                    new string[] { RelayStorage.ValueSeperator },
                    System.StringSplitOptions.None);

                if (messageItems == null || messageItems.Length != 3)
                {
                    continue;
                }

                Message message = new Message()
                {
                    From = messageItems[0],
                    To = messageItems[1],
                    Body = Encoding.Unicode.GetBytes(messageItems[2]),
                    Valid = true
                };

                messages.Add(message);
            }

            return messages;
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


