using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace PMF.Managers
{
    public static class PackageManager
    {
        /// <summary>
        /// Installs a package given a version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="version">The version of the asset</param>
        /// <returns>true Installation successful, false already installed</returns>
        public static PackageState InstallPackage(Package package, Asset asset)
        {
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
        /// <returns>true Installation successful, false already installed</returns>
        public static PackageState Install(string id, Version version, out Package package)
        {
            package = null;

            // check if is already installed
            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
            {
                // get package info for version
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

            // check if is already installed
            if (LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
                return PackageState.AlreadyInstalled;

            Package remotePackage = RemotePackageManager.GetPackageInfo(id);

            if (remotePackage == null)
                return PackageState.NotExisting;

            Asset asset = RemotePackageManager.GetAssetLatestVersionBySdkVersion(remotePackage);

            if (asset == null)
                return PackageState.VersionNotFound;

            package = remotePackage;
            return InstallPackage(remotePackage, asset);
        }

        /// <summary>
        /// Uninstalls a package
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <returns>True if success, false otherwise</returns>
        public static bool Uninstall(string id)
        {
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

            // check if is already installed
            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
                return PackageState.NotInstalled;

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

        /// <summary>
        /// Updates a package to the most recent version regardless of sdk version
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="package">The package that was installed</param>
        /// <returns>True update success, false update failed or cancelled</returns>
        public static PackageState UpdatePackage(string id, Version version, out Package package)
        {
            package = null;

            // check if is already installed
            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string packageDirectory))
                return PackageState.NotInstalled;

            // You already have the latest version
            if (localPackage.Assets[0].Version == version)
                return PackageState.UpToDate;

            var remotePackage = RemotePackageManager.GetPackageInfo(id);

            if (remotePackage == null)
                return PackageState.NotExisting;

            var asset = remotePackage.GetAssetVersion(version);

            // We don't want to check here if it was success, we just was to remove the package if it is installed
            Uninstall(id);

            return InstallPackage(remotePackage, asset);
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

            if (!LocalPackageManager.IsPackageInstalled(id, out Package localPackage, out string pd))
                return PackageState.NotInstalled;

            var remotePackage = RemotePackageManager.GetPackageInfo(id);
            
            if (remotePackage == null)
                return PackageState.NotExisting;

            var asset = RemotePackageManager.GetAssetLatestVersionBySdkVersion(remotePackage);

            // doesn't exist for provided sdk version
            if (asset == null)
                return PackageState.NotExisting;

            // You already have the latest version
            if (localPackage.Assets[0].Version == asset.Version)
                return PackageState.UpToDate;

            Uninstall(id);
            return InstallPackage(remotePackage, asset);
        }
    }
}
