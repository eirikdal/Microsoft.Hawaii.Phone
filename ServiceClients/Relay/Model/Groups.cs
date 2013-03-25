// -
// <copyright file="Groups.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    
    /// <summary>
    /// A container class for Group objects.
    /// </summary>
    public class Groups 
    {
        /// <summary>
        /// List field to store the Group items.
        /// </summary>
        private List<Group> items;

        /// <summary>
        /// Initializes a new instance of the Groups class.
        /// </summary>
        public Groups()
        {
            this.items = new List<Group>();
        }

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        /// <returns>No of items (groups).</returns>
        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        /// <summary>
        /// Gets the index group item.
        /// </summary>
        /// <param name="index">A Index index.</param>
        /// <returns>A valid group item.</returns>
        public Group this[int index]
        {
            get
            {
                return this.GetGroup(index);
            }
        }

        /// <summary>
        /// Method top add a Group object into the container.
        /// </summary>
        /// <param name="group">
        /// A valid group object.
        /// </param>
        public void Add(Group group)
        {
            Debug.Assert(group != null, "Group object is null");

            this.items.Add(group);
        }

        /// <summary>
        /// Method to check whether a group exists in the container.
        /// </summary>
        /// <param name="groupId">
        /// Id of the Group object to find.
        /// </param>
        /// <returns>
        /// Returns 'true' if the container contains group item, other 'false'.
        /// </returns>
        public bool Exists(string groupId)
        {
            Debug.Assert(!string.IsNullOrEmpty(groupId), "Group Id is null/empty.");

            return this.Find(groupId) != null;
        }

        /// <summary>
        /// Method to retrieve a Group item from the container.
        /// </summary>
        /// <param name="groupId">
        /// Specifies a group id.
        /// </param>
        /// <returns>
        /// A valid Group object.
        /// </returns>
        public Group Find(string groupId)
        {
            Debug.Assert(!string.IsNullOrEmpty(groupId), "GroupId is null/empty.");

            foreach (Group group in this.items)
            {
                if (group.RegistrationId == groupId)
                {
                    return group;
                }
            }

            return null;
        }

        /// <summary>
        /// Method to remove a group from the container based on its id.
        /// </summary>
        /// <param name="groupId">
        /// Id of the Group object to be removed.
        /// </param>
        public void Remove(string groupId)
        {
            Debug.Assert(!string.IsNullOrEmpty(groupId), "GroupId is null/empty.");

            Group group = this.Find(groupId);
            if (group != null)
            {
                this.items.Remove(group);
            }
        }

        /// <summary>
        /// Clears all item from the container.
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Enumerator for groups class.
        /// </summary>
        /// <returns>
        /// IEnumerator object.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            foreach (Group group in this.items)
            {
                yield return group;
            }
        }

        /// <summary>
        /// Gets a group object from the list of items
        /// </summary>
        /// <param name="index">index of the object in the items list to get</param>
        /// <returns>a group object</returns>
        private Group GetGroup(int index)
        {
            if (index >= this.items.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return this.items[index];
        }
    }
}
