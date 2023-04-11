
namespace RestFramework.Services.Browser
{
    public class BrowserService
    {
        private static Browser browser = null;
        public static Browser Browser
        {
            get
            {
                if (browser == null)
                {
                    browser = BrowserFactory.BrowserFactory.CreateBrowserInstance("chrome");
                }
                return browser;
            }
        }
    }
}