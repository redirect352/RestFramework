using Aquality.Selenium.Configurations.WebDriverSettings;
using OpenQA.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V109.Network;
using WebDriverManager.Helpers;

namespace RestFramework.Services.Browser.BrowserFactory
{
    public class ChromeDriverSettings : IDriverSettings
    {
        private string webDriverVersion = "MatchingBrowser";
        private DriverOptions  driverOptions = new ChromeOptions();
        private PageLoadStrategy strategy = PageLoadStrategy.Default;
        private string directory = "./downloads";

        public string WebDriverVersion 
        {
            get 
            {
                return webDriverVersion;
            }
        }

        public Architecture SystemArchitecture 
        {
            get 
            {
                return Architecture.Auto;
            } 
        }

        public DriverOptions DriverOptions 
        {
            get
            {
               return driverOptions;
            }
        }
        public PageLoadStrategy PageLoadStrategy 
        {
            get 
            {
                return strategy;
            } 
        }

        public string DownloadDirectory 
        {
            get 
            {
                return directory;
            }
        }

        public ChromeDriverSettings(string version,DriverOptions driverOptions1,PageLoadStrategy loadStrategy, string downloadDirectory)
        {
            webDriverVersion = version;
            driverOptions = driverOptions1;
            strategy = loadStrategy;
            directory = downloadDirectory;
           
        }
    }
}
