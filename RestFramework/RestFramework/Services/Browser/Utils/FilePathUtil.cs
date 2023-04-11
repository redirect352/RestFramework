using RestFramework.Services.Browser.BrowserFactory;
using System.Net;

namespace RestFramework.Services.Browser.Utils
{
    internal static class FilePathUtil
    {
        public static IWebProxy Proxy { get; set; }

        public static string GetDriverDestinationPath(string driverName, string version, Architecture architecture, string driverFileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return Path.Combine(currentDirectory, driverName, version, architecture.ToString(), driverFileName);
        }

        public static void CreateDestinationDirectory(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName != null)
            {
                Directory.CreateDirectory(directoryName);
            }
        }
        public static string GetZipDestination(string url)
        {
            string tempPath = Path.GetTempPath();
            string path = Guid.NewGuid().ToString();
            string fileName = Path.GetFileName(url);
            if (fileName == null)
            {
                throw new ArgumentNullException("Can't get zip name from URL: " + url);
            }

            return Path.Combine(tempPath, path, fileName);
        }
    }
}
