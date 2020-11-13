using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace PMF.Managers
{
    internal static class RemotePackageManager
    {
        /// <summary>
        /// Gets package info from the server along with all the assets in the json
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <returns>The package downloaded</returns>
        public static Package GetPackageInfo(string id)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    PMF.InvokePackageMessageEvent("Downloading package information");
                    string json = client.DownloadString($"{Config.RepositoryEndpoint}/{id}");
                    PMF.InvokePackageMessageEvent("Parsing package information");
                    return JsonConvert.DeserializeObject<Package>(json);
                }
            }
            catch (WebException)
            {
                PMF.InvokePackageMessageEvent("Couldn't download information from the server");
                return null;
            }
        }

        /// <summary>
        /// Downloads a specific version of a certain package
        /// </summary>
        /// <param name="id">The id of the package</param>
        /// <param name="asset">The asset that is to be downloaded</param>
        /// <returns>The zip file path which was downloaded</returns>
        public static string DownloadAsset(string id, Asset asset)
        {
            using (WebClient client = new WebClient())
            {
                PMF.InvokePackageMessageEvent("Downloading asset");

                var zipPath = Path.Combine(Config.TemporaryFolder, id);
                client.DownloadFile(asset.Url, Path.Combine(zipPath, asset.FileName));

                foreach (var dependency in asset.Dependencies)
                {
                    PMF.InvokePackageMessageEvent($"Downloading dependency with id: {dependency.ID}");
                    client.DownloadFile(dependency.Url, Path.Combine(zipPath, dependency.FileName));
                }

                PMF.InvokePackageMessageEvent("Finished downloading all required files");

                return zipPath;
            }
        }
    }
}
