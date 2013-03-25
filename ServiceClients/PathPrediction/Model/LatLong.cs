// -----------------------------------------------------------------------
// <copyright file="LatLong.cs" company="Microsoft Research">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Hawaii.PathPrediction.Client
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Represents a latitude and longitude
    /// </summary>
    [DataContract]
    public struct LatLong
    {
        /// <summary>
        /// The average radius of the earth in meters
        /// value taken from http://en.wikipedia.org/wiki/Earth_radius#Mean_radius
        /// </summary>
        private const float EarthRadiusMeters = 6371009;

        /// <summary>
        /// Initializes a new instance of the LatLong struct
        /// </summary>
        /// <param name="latitudeDegrees">The latitude in degrees of the location</param>
        /// <param name="longitudeDegrees">The longitude in degrees of the location</param>
        public LatLong(float latitudeDegrees, float longitudeDegrees)
            : this()
        {
            this.Latitude = latitudeDegrees;
            this.Longitude = longitudeDegrees;
        }

        /// <summary>
        /// Gets or sets latitude of this location
        /// </summary>
        [DataMember(Name = "Lat")]
        public float Latitude
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the longitude of this location
        /// </summary>
        [DataMember(Name = "Long")]
        public float Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if two latlongs refer to the same location
        /// </summary>
        /// <param name="llA">the first location</param>
        /// <param name="llB">the second location</param>
        /// <returns>true if the two latlongs are the same, false otherwise</returns>
        public static bool operator ==(LatLong llA, LatLong llB)
        {
            if (float.IsNaN(llA.Latitude)
                && float.IsNaN(llB.Latitude)
                && float.IsNaN(llA.Longitude)
                && float.IsNaN(llB.Longitude))
            {
                return true;
            }

            return llA.Latitude == llB.Latitude && llA.Longitude == llB.Longitude;
        }

        /// <summary>
        /// Determines if two latlongs are NOT the same location
        /// </summary>
        /// <param name="llA">the first location</param>
        /// <param name="llB">the second location</param>
        /// <returns>true if the two latlongs differ, false otherwise</returns>
        public static bool operator !=(LatLong llA, LatLong llB)
        {
            if (float.IsNaN(llA.Latitude)
                && float.IsNaN(llB.Latitude)
                && float.IsNaN(llA.Longitude)
                && float.IsNaN(llB.Longitude))
            {
                return false;
            }

            return llA.Latitude != llB.Latitude || llA.Longitude != llB.Longitude;
        }

        /// <summary>
        /// Calculates the distance between the current latlong and the supplied latlong.
        /// Uses Haversine formula from http://mathforum.org/library/drmath/view/51879.html 
        /// </summary>
        /// <param name="latLong">The remote location to calculate the distance to</param>
        /// <returns>The distance in meters between the two locations</returns>
        public float DistanceMeters(LatLong latLong)
        {
            // Convert to radians
            float latitude1 = (float)(Math.PI * this.Latitude / 180.0);
            float longitude1 = (float)(Math.PI * this.Longitude / 180.0);
            float latitude2 = (float)(Math.PI * latLong.Latitude / 180.0);
            float longitude2 = (float)(Math.PI * latLong.Longitude / 180.0);

            // do the formula
            float dlon = longitude2 - longitude1;
            float dlat = latitude2 - latitude1;
            float sinedlatdiv2 = (float)Math.Sin(dlat / 2.0);
            float sinedlondiv2 = (float)Math.Sin(dlon / 2.0);
            float a = (float)((sinedlatdiv2 * sinedlatdiv2) + (Math.Cos(latitude1) * Math.Cos(latitude2) * sinedlondiv2 * sinedlondiv2));
            float c = (float)(2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
            float distMeters = c * EarthRadiusMeters;

            return distMeters;
        }

        /// <summary>
        /// determines if this instance is the same as the supplied instance
        /// </summary>
        /// <param name="obj">the object to compare ourselves with</param>
        /// <returns>True if it is the same object, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets a hash code for this object
        /// </summary>
        /// <returns>THe object's hashcode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}