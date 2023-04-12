using RestFramework.Services.Browser.BrowserFactory;

namespace RestFramework.Services.Browser
{
    public class BrowserProfile : IBrowserProfile
    {
        public BrowserType Name { get; }
        public IDriverSettings DriverSettings { get; }

        public BrowserProfile (BrowserType name, IDriverSettings driverSettings)
        {
            Name = name;
            DriverSettings = driverSettings;
        }
    }
}
