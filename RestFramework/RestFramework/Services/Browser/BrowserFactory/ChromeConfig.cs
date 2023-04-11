using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.Helpers;
using RestFramework.Services.Browser.Utils;

namespace RestFramework.Services.Browser.BrowserFactory
{
    public class ChromeConfig : IDriverConfig
    {
        private const string BaseVersionPatternUrl = "https://chromedriver.storage.googleapis.com/<version>/";

        private const string LatestReleaseVersionUrl = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE";

        private const string ExactReleaseVersionPatternUrl = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_<version>";

        public virtual string GetName()
        {
            return "Chrome";
        }

        public virtual string GetUrl32()
        {
            return GetUrl();
        }

        public virtual string GetUrl64()
        {
            return GetUrl();
        }

        private string GetUrl()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return BaseVersionPatternUrl + "chromedriver_linux64.zip";
            }

            return BaseVersionPatternUrl+"chromedriver_win32.zip";
        }

        public virtual string GetBinaryName()
        {
            string text = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty);
            return "chromedriver" + text;
        }

        public virtual string GetLatestVersion()
        {
            return GetLatestVersion(LatestReleaseVersionUrl);
        }

        private static string GetLatestVersion(string url)
        {
            Uri uri = new Uri(url);
            WebRequest webRequest = WebRequest.Create(uri);
            using WebResponse webResponse = webRequest.GetResponse();
            using Stream stream = webResponse.GetResponseStream();
            if (stream == null)
            {
                throw new ArgumentNullException($"Can't get content from URL: {uri}");
            }

            using StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd().Trim();
        }

        public virtual string GetMatchingBrowserVersion()
        {
            string rawBrowserVersion = GetRawBrowserVersion();
            if (string.IsNullOrEmpty(rawBrowserVersion))
            {
                throw new Exception("Not able to get chrome version or not installed");
            }
            
            Version version2 = Version.Parse(rawBrowserVersion);
            string versionWithoutRevision = $"{version2.Major}.{version2.Minor}.{version2.Build}"; ;
            
            string url = ExactReleaseVersionPatternUrl.Replace("<version>", versionWithoutRevision);
            return GetLatestVersion(url);
        }

        private string GetRawBrowserVersion()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return RegistryUtil.GetInstalledBrowserVersionLinux("google-chrome", "--product-version", "chromium", "--version", "chromium-browser", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RegistryUtil.GetInstalledBrowserVersionWindows("chrome.exe");
            }
            throw new PlatformNotSupportedException("Your operating system is not supported");
        }

    }
}
