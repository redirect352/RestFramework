using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Diagnostics;
using RestFramework.Services.Browser;
using RestFramework.Services.Browser.BrowserFactory;

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

            Browser browser = BrowserService.Browser;  
            browser.GoTo("https://google.com");
            browser.Quit();
            Assert.Pass();
        }
    }
}