using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMF
{
    public class Asset
    {
        // This ensures the version object is correctly converted
        [JsonConverter(typeof(VersionConverter))]
        public Version Version { get; set; }

        public string SdkVersion { get; set; }

        public string Checksum { get; set; }

        public string FileName { get; set; }

        public string Url { get; set; }

        public List<Dependency> Dependencies { get; set; }
    }
}
