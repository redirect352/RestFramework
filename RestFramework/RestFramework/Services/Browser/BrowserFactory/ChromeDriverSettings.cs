using Aquality.Selenium.Configurations.WebDriverSettings;
using OpenQA.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.Helpers;

namespace RestFramework.Services.Browser.BrowserFactory
{
    public class ChromeDriverSettings : IDriverSettings
    {
        public string WebDriverVersion 
        {
            get 
            {
                return "112.0.5615.49";
                //return "110.0.5481.30";
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
               return new ChromeOptions();
            }
        }
        public PageLoadStrategy PageLoadStrategy 
        {
            get 
            {
                return PageLoadStrategy.Default;
            } 
        }

        public string DownloadDir 
        {
            get 
            {
                return "D:\\Games";
            }
        }
        public string DownloadDirCapabilityKey
        {
            get
            {
                return "D:\\Games";
            }
        }
    }
}
