using System;

namespace PMF
{
    /// <summary>
    /// Files that an asset depends on
    /// </summary>
    public class Dependency
    {
        /// <summary>
        /// The id of the dependency
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Checksum of the dependency // TODO: implement
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Type of dependency
        /// </summary>
        public DependencyType Type { get; set; }

        /// <summary>
        /// Version of the dependency if type is Package
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// The file name of the dependecy
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Link where the dependecy is located
        /// </summary>
        public string Url { get; set; }
    }
}
