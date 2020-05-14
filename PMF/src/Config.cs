using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PMF
{
    public static class Config
    {
        /// <summary>
        /// Manifest file name, the library will look for it in the directory that is being run
        /// </summary>
        public static string ManifestFileName { get; set; } = "manifest.json";

        /// <summary>
        /// Installation folder containing the packages
        /// </summary>
        public static string PackageInstallationFolder { get; set; }

        /// <summary>
        /// Repository endpoint for the database of packages
        /// </summary>
        public static string RepositoryEndpoint { get; set; }

        /// <summary>
        /// The current SDK version
        /// </summary>
        public static string CurrentSdkVersion { get; set; }

        /// <summary>
        /// Internal flag to spit out debug info
        /// </summary>
        public static bool IsDebugging { get; set; }

        /// <summary>
        /// Temporary folder where downloads will go to
        /// </summary>
        public static string TemporaryFolder { get; set; } = ".pmf-temp";
    }
}
