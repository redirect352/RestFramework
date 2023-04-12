using System;
using RestFramework.Services.Browser;
using RestFramework.Services.Browser.BrowserFactory;

namespace RestFramework.Services.Browser
{
    public interface IBrowserProfile
    {
        public BrowserType Name{ get; }
        public IDriverSettings DriverSettings { get; }
    }

    public enum BrowserType
    {
        Chrome
    }
}
