using System;
using OpenQA.Selenium;

namespace RestFramework.Services.Browser.Navigation
{
    public class BrowserNavigation : INavigation
    {
        private WebDriver driver;
        internal BrowserNavigation(WebDriver driver)
        {
            this.driver = driver;
        }

        public void Back() 
        {
            driver.Navigate().Back();
        }
        public void Forward()
        {
            driver.Navigate().Forward();
        }
        public void Refresh() 
        {
            driver.Navigate ().Refresh();
        }

        public void GoToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public void GoToUrl(Uri url)
        {
            driver.Navigate().GoToUrl(url);
        }

    }
}
