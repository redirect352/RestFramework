using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Services.Browser.BrowserFactory
{
    public interface IDriverConfig
    {
        string GetName();

        string GetUrl32();

        string GetUrl64();

        string GetBinaryName();

        string GetLatestVersion();

        string GetMatchingBrowserVersion();
    }

    public enum Architecture
    {
        X32 = 0x20,
        X64 = 0x40,
        Auto = 65
    }
}
