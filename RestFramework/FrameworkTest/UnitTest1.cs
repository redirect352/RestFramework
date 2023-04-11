
using RestFramework.Services.Browser.BrowserFactory;
using OpenQA.Selenium.Chrome;
using Aquality.Selenium.Configurations.WebDriverSettings;
using OpenQA.Selenium;
using System.Diagnostics;
using RestFramework.Services.Browser; 

namespace FrameworkTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void BrowserCreateTest()
        {
            //string path = BrowserFactory.SetUpDriver(new ChromeConfig(),new ChromeDriverSettings());

            //var t =ChromeDriverService.CreateDefaultService(path);
            //var opt = new ChromeOptions();


            Browser browser = BrowserService.Browser;
            //var driver = BrowserFactory.GetDriver<ChromeDriver>
            //                (ChromeDriverService.CreateDefaultService(path),
            //                opt,TimeSpan.FromSeconds(20));
           
            browser.GoTo("https://google.com");
            browser.Quit();
            Assert.Pass();
        }
    }
}