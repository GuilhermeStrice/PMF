using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace PMF
{
    public class Package
    {
        /// <summary>
        /// The id of the package
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The type of this package
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType Type { get; set; }

        /// <summary>
        /// The full name of the package
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The author of the package
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Full description of this package
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of assets that can be downloaded
        /// </summary>
        public List<Asset> Assets { get; set; } // If the package is a local one the list will only have one asset which is the version installed

        /// <summary>
        /// Gets an asset of the package given a version
        /// </summary>
        /// <param name="version">The version which we want to get</param>
        /// <returns>The asset of that version or null</returns>
        public Asset GetAssetVersion(Version version)
        {
            if (version == null)
                throw new ArgumentNullException();

            foreach (var asset in Assets)
            {
                if (asset.Version == version)
                    return asset;
            }

            return null;
        }

        /// <summary>
        /// Gets you the latest version of a package
        /// </summary>
        /// <param name="package">The package object to get the latest version</param>
        /// <returns>The latest asset version of a given package</returns>
        public Asset GetAssetLatestVersion()
        {
            if (Assets.Count == 0)
                return null;

            Asset ret_asset = null;
            foreach (var asset in Assets)
            {
                if (ret_asset == null || ret_asset.Version < asset.Version)
                    ret_asset = asset;
            }

            return ret_asset;
        }

        /// <summary>
        /// Gets you the latest version of a package given an SDK version
        /// </summary>
        /// <param name="package">The package object to get the asset</param>
        /// <returns>The latest asset version of a given package and given SDK version</returns>
        public Asset GetAssetLatestVersionBySdkVersion()
        {
            if (Assets.Count == 0)
                return null;

            Asset ret_asset = null;
            foreach (var asset in Assets)
            {
                if (asset.SdkVersion == Config.CurrentSdkVersion)
                {
                    if (ret_asset == null || ret_asset.Version < asset.Version)
                        ret_asset = asset;
                }
            }

            return ret_asset;
        }

        // A valid package must have:
        //      - an id
        //      - a type
        //      - a name
        //      - an author
        //      - a description
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ID) &&
                   Type != PackageType.None &&
                   !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Author) &&
                   !string.IsNullOrEmpty(Description);
        }
    }
}
