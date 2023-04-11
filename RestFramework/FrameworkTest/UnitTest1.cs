
using RestFramework.Services.Browser.BrowserFactory;
using OpenQA.Selenium.Chrome;
using Aquality.Selenium.Configurations.WebDriverSettings;
using OpenQA.Selenium;
using System.Diagnostics;

namespace FrameworkTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            string path = BrowserFactory.SetUpDriver(new ChromeConfig(),new ChromeDriverSettings());

            var t =ChromeDriverService.CreateDefaultService(path);
            var opt = new ChromeOptions();
            


            var driver = BrowserFactory.GetDriver<ChromeDriver>
                            (ChromeDriverService.CreateDefaultService(path),
                            opt,TimeSpan.FromSeconds(20));
           

            driver.Navigate().GoToUrl("https://google.com");
            driver.Quit();
            Assert.Pass();
        }
    }
}