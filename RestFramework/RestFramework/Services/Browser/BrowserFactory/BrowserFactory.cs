using System;
using OpenQA.Selenium;
using System;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using Aquality.Selenium.Configurations.WebDriverSettings;
using WebDriverManager.Helpers;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Aquality.Selenium.Browsers;
using RestFramework.Services.Browser.Utils;

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
            switch (browserName)
            {
                case "chrome":
                    //driverSettings
                    var driverSettings = new ChromeDriverSettings();
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
                          (VersionResolveStrategy.Latest) ? driverConfig.GetLatestVersion() : driverSettings.WebDriverVersion;
            version = version.Equals(VersionResolveStrategy.MatchingBrowser) ? driverConfig.GetMatchingBrowserVersion() : version;
            var url = UrlHelper.BuildUrl(architecture.Equals(Architecture.X32) ? driverConfig.GetUrl32() : driverConfig.GetUrl64(), version);
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
