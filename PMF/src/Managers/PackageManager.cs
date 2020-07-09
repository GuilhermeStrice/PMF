using System;
using System.Collections.Generic;

namespace PMF.Managers
{
    /// <summary>
    /// Global Package manager for PMF
    /// Combines RemotePackageManager and LocalPackageManager
    /// </summary>
    public static class PackageManager
    {
        public static List<Package> PackageList { get; internal set; }

        private static bool initialized = false;

        /// <summary>
        /// Initializes the package manager. Required
        /// </summary>
        public static void Start()
        {
            try
            {
                if (!initialized)
                {
                    LocalPackageManager.Start();
                    initialized = true;
                }
            }
            catch (Exception ex)
            {
                PMF.InvokePackageMessageEvent($"PMF failed to initialize. \n${ex.InnerException.Message}\nClosing.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Cleans up package manager. Required
        /// </summary>
        public static void Stop()
        {
            try
            {
                if (initialized)
                    LocalPackageManager.Stop();
            }
            catch (Exception ex)
            {
                string message = null;
                if (ex.InnerException != null)
                    message = ex.InnerException.Message;
                else
                    message = ex.Message;
                PMF.InvokePackageMessageEvent($"Something failed while cleaning up PMF - \n{message}");
            }
        }

        private static void notInitialized()
        {
            PMF.InvokePackageMessageEvent("You must initialize PMF first before using it.\nClosing");
            Environment.Exit(0);
        }

        /// <summary>
        /// Installs a package given a version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="version">The version of the asset</param>
        /// <returns>true Installation successful, false already installed</returns>
        public static PackageState InstallPackage(Package package, Asset asset)
        {
            if (!initialized)
                notInitialized();

            string zipFile = RemotePackageManager.DownloadAsset(package.ID, asset);
            LocalPackageManager.InstallPackage(package, asset, zipFile);
            return PackageState.Installed;
        }

        /// <summary>
        /// Installs a package given a version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="version">The version of the asset</param>
        /// <param name="package">The package that was installed</param>
        /// <returns>Final package state of installation</returns>
        public static PackageState Install(string id, Version version, out Package package)
        {
            package = null;

            if (!initialized)
                notInitialized();

            // check if is already installed
            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
            {
                Package remotePackage = RemotePackageManager.GetPackageInfo(id);

                if (remotePackage == null)
                    return PackageState.NotExisting;

                Asset asset = remotePackage.GetAssetVersion(version);

                if (asset == null)
                    return PackageState.VersionNotFound;

                package = remotePackage;
                return InstallPackage(remotePackage, asset);
            }
            else
            {
                PMF.InvokePackageMessageEvent("Package already installed");
                return PackageState.AlreadyInstalled;
            }
        }

        /// <summary>
        /// Installs a package given a version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="package">The package that was installed</param>
        /// <returns>true Installation successful, false already installed</returns>
        public static PackageState InstallLatest(string id, out Package package)
        {
            package = null;

            if (!initialized)
                notInitialized();

            // check if is already installed
            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
            {
                // get package info for version
                Package remotePackage = RemotePackageManager.GetPackageInfo(id);

                if (remotePackage == null)
                    return PackageState.NotExisting;

                Asset asset = RemotePackageManager.GetAssetLatestVersion(package);

                if (asset == null)
                    return PackageState.VersionNotFound;

                package = remotePackage;
                return InstallPackage(remotePackage, asset);
            }
            else
            {
                PMF.InvokePackageMessageEvent("Package already installed");
                return PackageState.AlreadyInstalled;
            }
        }

        /// <summary>
        /// Installs a package to the most recent version given an sdk version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="package">The package that was installed</param>
        /// <returns>true update succes, false update failed or cancelled</returns>
        public static PackageState InstallBySdkVersion(string id, out Package package)
        {
            package = null;

            if (!initialized)
                notInitialized();

            // check if is already installed
            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
            {
                Package remotePackage = RemotePackageManager.GetPackageInfo(id);

                if (remotePackage == null)
                    return PackageState.NotExisting;

                Asset asset = RemotePackageManager.GetAssetLatestVersionBySdkVersion(remotePackage);

                if (asset == null)
                    return PackageState.VersionNotFound;

                package = remotePackage;
                return InstallPackage(remotePackage, asset);
            }
            else
            {
                PMF.InvokePackageMessageEvent("Package already installed");
                return PackageState.AlreadyInstalled;
            }
        }

        /// <summary>
        /// Uninstalls a package
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <returns>True if success, false otherwise</returns>
        public static bool Uninstall(string id)
        {
            if (!initialized)
                notInitialized();

            return LocalPackageManager.RemovePackage(id);
        }

        /// <summary>
        /// Updates a package to the most recent version regardless of sdk version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="package">The package that was installed</param>
        /// <returns>true update succes, false update failed or cancelled</returns>
        public static PackageState UpdateLatest(string id, out Package package)
        {
            package = null;

            if (!initialized)
                notInitialized();

            // check if is already installed
            if (LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
            {
                var remotePackage = RemotePackageManager.GetPackageInfo(id);

                if (remotePackage == null)
                    return PackageState.NotExisting;

                var asset = RemotePackageManager.GetAssetLatestVersion(remotePackage);

                // You already have the latest version
                if (localPackage.Assets[0].Version == asset.Version)
                    return PackageState.UpToDate;

                Uninstall(id);
                return InstallPackage(remotePackage, asset);
            }
            else
            {
                PMF.InvokePackageMessageEvent("Package not installed");
                return PackageState.NotInstalled;
            }
        }

        /// <summary>
        /// Updates a package to the most recent version regardless of sdk version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="package">The package that was installed</param>
        /// <returns>True update success, false update failed or cancelled</returns>
        public static PackageState UpdatePackage(string id, Version version, out Package package)
        {
            package = null;

            if (!initialized)
                notInitialized();

            // check if is already installed
            if (LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
            {
                // Up to date
                if (localPackage.Assets[0].Version == version)
                {
                    PMF.InvokePackageMessageEvent("Already up to date");
                    return PackageState.UpToDate;
                }

                var remotePackage = RemotePackageManager.GetPackageInfo(id);

                if (remotePackage == null)
                    return PackageState.NotExisting;

                var asset = remotePackage.GetAssetVersion(version);

                // We don't want to check here if it was success, we just was to remove the package if it is installed
                Uninstall(id);

                return InstallPackage(remotePackage, asset);
            }
            else
            {
                PMF.InvokePackageMessageEvent("Package not installed");
                return PackageState.NotInstalled;
            }
        }

        /// <summary>
        /// Updates a package to the most recent version given an sdk version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="package">The package that was installed</param>
        /// <returns>True if update success, false if package is not installed</returns>
        public static PackageState UpdateBySdkVersion(string id, out Package package)
        {
            package = null;

            if (!initialized)
                notInitialized();

            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string pd))
            {
                PMF.InvokePackageMessageEvent("Already up to date");
                return PackageState.NotInstalled;
            }

            var remotePackage = RemotePackageManager.GetPackageInfo(id);
            
            if (remotePackage == null)
                return PackageState.NotExisting;

            var asset = RemotePackageManager.GetAssetLatestVersionBySdkVersion(remotePackage);

            // doesn't exist for provided sdk version
            if (asset == null)
            {
                PMF.InvokePackageMessageEvent("Package doesn't exist for provided SDK version");
                return PackageState.NotExisting;
            }

            // You already have the latest version
            if (localPackage.Assets[0].Version == asset.Version)
            {
                PMF.InvokePackageMessageEvent("Already up to date");
                return PackageState.UpToDate;
            }

            Uninstall(id);
            return InstallPackage(remotePackage, asset);
        }
    }
}
