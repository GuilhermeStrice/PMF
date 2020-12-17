using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace PMF.Managers
{
    /// <summary>
    /// Manages all the local files
    /// </summary>
    internal static class LocalPackageManager
    {
        /// <summary>
        /// Does all the checking locally when the program starts
        /// THIS NEEDS TO BE CALLED!
        /// </summary>
        public static void Start()
        {
            validateManifestFile();
            var json = File.ReadAllText(Config.ManifestFileName);
            PackageManager.PackageList = JsonConvert.DeserializeObject<List<Package>>(json);
            
            //PMF.InvokePackageMessageEvent("Initialized PMF successfully");
        }

        /// <summary>
        /// Saves everything to disk
        /// THIS NEEDS TO BE CALLED!
        /// </summary>
        public static void Stop()
        {
            validateManifestFile();

            var json = JsonConvert.SerializeObject(PackageManager.PackageList);
            File.WriteAllText(Config.ManifestFileName, json);
            Directory.Delete(Config.TemporaryFolder, true);
            
            //PMF.InvokePackageMessageEvent("Successfully cleaned PMF");
        }

        private static void validateManifestFile()
        {
            PMF.InvokePackageMessageEvent("Validating manifest file");
            if (string.IsNullOrEmpty(Config.ManifestFileName))
                throw new ArgumentNullException("");

            if (!File.Exists(Config.ManifestFileName))
                File.Create(Config.ManifestFileName).Close();

            PMF.InvokePackageMessageEvent("Successfully validated manifest file");
        }

        /// <summary>
        /// Checks if a given package is installed
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="package">This value is defined if the package exists, its contents will be the actual package</param>
        /// <param name="packageDirectory">The directory in which the package is installed</param>
        /// <returns>True if package is installed, false otherwise</returns>
        public static bool IsPackageInstalled(string id, out Package package, out string packageDirectory)
        {
            PMF.InvokePackageMessageEvent($"Checking if {id} is installed");
            package = null;

            packageDirectory = Path.Combine(Config.PackageInstallationFolder, id);
            if (!Directory.Exists(packageDirectory))
                return false;

            try
            {
                package = PackageManager.PackageList.GetPackage(id);
                PMF.InvokePackageMessageEvent($"Found {id} with version {package.Assets[0].Version}");
                return true;
            }
            catch
            {
                PMF.InvokePackageMessageEvent($"Couldn't find {id}");
                return false;
            }
        }

        /// <summary>
        /// Uninstalls a package
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <returns>True if uninstalled correctly, false otherwise</returns>
        public static bool RemovePackage(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                PMF.InvokePackageMessageEvent("Package id must be defined");
                return false;
            }

            PMF.InvokePackageMessageEvent($"Removing {id}");

            try
            {
                string packageDirectory = Path.Combine(Config.PackageInstallationFolder, id);
                Directory.Delete(packageDirectory, true);
                PMF.InvokePackageMessageEvent($"Successfully removed package {id}");
            }
            catch
            {
                PMF.InvokePackageMessageEvent($"Couldn't find package with id of: {id}. Removing from package list");
            }

            return PackageManager.PackageList.Remove(id);
        }

        /// <summary>
        /// Extracts zip files and registeres this package as installed
        /// </summary>
        /// <param name="remotePackage">The package which is to be installed</param>
        /// <param name="asset">The version of the asset being installed</param>
        /// <param name="zipPath">The path to the zip file that is to be installed</param>
        /// <returns>State of the installation</returns>
        public static PackageState InstallPackage(Package remotePackage, Asset asset, string zipPath)
        {
            PMF.InvokePackageMessageEvent($"Extracting package {remotePackage.ID}");
            ZipFile.ExtractToDirectory(Path.Combine(zipPath, asset.FileName), Path.Combine(Config.PackageInstallationFolder, remotePackage.ID));
            PMF.InvokePackageMessageEvent($"Finished extracting package {remotePackage.ID}");

            bool error = false;

            // Maybe a library folder and check if is installed
            foreach (var dependency in asset.Dependencies)
            {
                if (dependency.Type == DependencyType.Standalone)
                {
                    PMF.InvokePackageMessageEvent($"Extracting dependency with id: {dependency.ID} of type standalone");
                    ZipFile.ExtractToDirectory(Path.Combine(zipPath, dependency.FileName), Path.Combine(Config.PackageInstallationFolder, remotePackage.ID, "Dependencies", dependency.ID));
                    PMF.InvokePackageMessageEvent($"Finished extracting dependency {dependency.ID}");
                }
                else // DependencyType.Package
                {
                    PMF.InvokePackageMessageEvent($"Downloading dependency with id: {dependency.ID} of type package");

                    var success = PackageManager.Install(dependency.ID, dependency.Version, out Package p);

                    if (success == PackageState.Installed)
                    {
                        PMF.InvokePackageMessageEvent($"Package dependency installed successfully");
                    }
                    else if (success == PackageState.VersionNotFound)
                    {
                        PMF.InvokePackageMessageEvent("Asset not found");
                        error = true;
                    }
                    else if (success == PackageState.NotExisting)
                    {
                        PMF.InvokePackageMessageEvent("Package not found");
                        error = true;
                    }
                    else
                    {
                        PMF.InvokePackageMessageEvent($"Something went wrong installing dependency with id: {dependency.ID}");
                        error = true;
                    }
                }
            }

            remotePackage.Assets.Clear();
            remotePackage.Assets.Add(asset);

            PackageManager.PackageList.Add(remotePackage);

            if (error)
            {
                PMF.InvokePackageMessageEvent($"Couldn't install {remotePackage.ID}@{asset.Version}. See above");
                return PackageState.NotInstalled;
            }
            else
            {
                PMF.InvokePackageMessageEvent($"Successfully installed {remotePackage.ID}@{asset.Version}");
                return PackageState.Installed;
            }
        }
    }
}
