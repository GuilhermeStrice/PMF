using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace PMF
{
    /// <summary>
    /// Asset version of a Package
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// The version of this asset
        /// </summary>
        [JsonConverter(typeof(VersionConverter))]
        public Version Version { get; set; }

        /// <summary>
        /// The SDK version that this asset is for
        /// </summary>
        public string SdkVersion { get; set; }

        /// <summary>
        /// Checksum //TODO: implement
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// The filename of the asset
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Download link
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Dependencies of this asset
        /// </summary>
        public List<Dependency> Dependencies { get; set; }
    }
}
