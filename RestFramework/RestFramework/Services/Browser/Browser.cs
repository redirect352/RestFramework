using System;
using System.Drawing;
using OpenQA.Selenium;
using RestFramework.Services.Browser.Navigation;

namespace RestFramework.Services.Browser
{
    public class Browser
    {
        private WebDriver driver;
        private TimeSpan implicitWaitTimeout = TimeSpan.FromSeconds(10);
        private TimeSpan pageLoadTimeout = TimeSpan.FromSeconds(100);
       

        internal Browser (WebDriver driver)
        {
            this.driver = driver;
        }

        public WebDriver Driver 
        {
            get 
            {
                return driver;
            } 
        }

        public void Quit()
        {
            Driver?.Quit();
        }

        public void SetImplicitWaitTimeout(TimeSpan timeout)
        {
            if (!timeout.Equals(implicitWaitTimeout))
            {
                Driver.Manage().Timeouts().ImplicitWait = timeout;
                implicitWaitTimeout = timeout;
            }
        }



        public void SetPageLoadTimeout(TimeSpan timeout)
        {
            pageLoadTimeout = timeout;
            Driver.Manage().Timeouts().PageLoad = timeout;
        }

        public void SetScriptTimeout(TimeSpan timeout)
        {
            Driver.Manage().Timeouts().AsynchronousJavaScript = timeout;
        }

        //Navigation-----------------------------
        public void GoTo(string url)
        {
            Navigation.GoToUrl(url);
        }

        public void GoTo(Uri url)
        {
            Navigation.GoToUrl(url);
        }

        public void GoBack()
        {
            Navigation.Back();
        }

        public void GoForward()
        {
            Navigation.Forward();
        }

        public void Refresh()
        {
            Navigation.Refresh();
        }

        private INavigation Navigation
        {
            get
            {
                return new BrowserNavigation(Driver);
            }
        }
        //---------------------------------------

        public void Maximize()
        {
            Driver.Manage().Window.Maximize();
        }

        public byte[] GetScreenshot()
        {
            return Driver.GetScreenshot().AsByteArray;
        }

        //Scripts-------------------------------------------------
        public void ExecuteScript(string script, params object[] arguments)
        {
            Driver.ExecuteScript(script, arguments);
        }

        public object ExecuteAsyncScript(string script, params object[] arguments)
        {
            return Driver.ExecuteAsyncScript(script, arguments);
        }

        public void SetWindowSize(int width, int height)
        {
            Driver.Manage().Window.Size = new Size(width, height);
        }

        //---------------------------------------
    }
    public enum AlertConfirmation
    {
        Accept,
        Decline
    }
}