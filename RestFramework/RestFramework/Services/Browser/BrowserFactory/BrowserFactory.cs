using System;
using OpenQA.Selenium;
using System;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using RestFramework.Services.Browser.Utils;
using RestFramework.ConfigManager;

namespace RestFramework.Services.Browser.BrowserFactory
{
    public class BrowserFactory
    {
        private static readonly object WebDriverDownloadingLock = new object();
        private static readonly object WebDriverInstalLock= new object();
        private static TimeSpan commandTimeout = TimeSpan.FromSeconds(60);
        public static Browser CreateBrowserInstance(string browserName )
        {
            WebDriver driver;
            IBrowserProfile browserProfile = ConfigManager.ConfigManager.BrowserProfile;
            switch (browserProfile.Name)
            {
                case BrowserType.Chrome:
                    //driverSettings
                    var driverSettings = browserProfile.DriverSettings;
                    var path = SetUpDriver(new ChromeConfig(), driverSettings);
                    driver = GetDriver<ChromeDriver>(ChromeDriverService.CreateDefaultService(path),
                        (ChromeOptions)driverSettings.DriverOptions, commandTimeout);
                    break;
                default:
                    throw new NotSupportedException($"Browser [{browserName}] is not supported.");
            }
            return new Browser(driver);
            
        }
        public static string SetUpDriver(IDriverConfig driverConfig, IDriverSettings driverSettings)
        {
            var SystemArchitecture = Environment.Is64BitOperatingSystem ? Architecture.X64: Architecture.X32;
            
            
            
            Architecture architecture = driverSettings.SystemArchitecture.Equals(Architecture.Auto) ?
                SystemArchitecture : 
                driverSettings.SystemArchitecture;
            var version = driverSettings.WebDriverVersion.Equals
                          (DriverVersionStrategy.Latest) ? driverConfig.GetLatestVersion() : driverSettings.WebDriverVersion;
            version = version.Equals(DriverVersionStrategy.MatchingBrowser) ? driverConfig.GetMatchingBrowserVersion() : version;
            var url = architecture.Equals(Architecture.X32) ? driverConfig.GetUrl32() : driverConfig.GetUrl64(); 
            url = url.Replace("<version>", version).Replace("<release>", version.Substring(0, version.LastIndexOf(".", StringComparison.CurrentCulture)));

            var binaryPath = FilePathUtil.GetDriverDestinationPath(driverConfig.GetName(), version, architecture, driverConfig.GetBinaryName());
            
            if (!File.Exists(binaryPath) || !Environment.GetEnvironmentVariable("PATH").Contains(binaryPath))
            {
                lock (WebDriverDownloadingLock)
                {
                    return SetUpDriver(url, binaryPath);
                }
            }
            return binaryPath;
        }

        public static WebDriver GetDriver<T>(DriverService driverService, DriverOptions driverOptions, TimeSpan commandTimeout) where T : WebDriver
        {
            return (T)Activator.CreateInstance(typeof(T), driverService, driverOptions, commandTimeout);
        }

        private static string SetUpDriver(string url, string binaryPath)
        {
            lock (WebDriverInstalLock)
            {
                return SetUpDriverImpl(url, binaryPath);
            }
        }
        private static string SetUpDriverImpl(string url, string binaryPath)
        {
            string zipDestination = FilePathUtil.GetZipDestination(url);
            binaryPath = InstallUtil.SetupBinary(url, zipDestination, binaryPath);
            VariablesUtil.SetupVariable(binaryPath);
            return binaryPath;
        }

    }
}
