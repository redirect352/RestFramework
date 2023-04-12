using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestFramework.Services.Browser.BrowserFactory
{
    public interface IDriverSettings
    {
        //     Gets version of web driver for WebDriverManager.
        string WebDriverVersion { get; }

        //     Gets target system architecture for WebDriverManager.
        Architecture SystemArchitecture { get; }

        //     Gets desired options for web driver.
        DriverOptions DriverOptions { get; }

        //
        // Сводка:
        //     WebDriver page load strategy.
        PageLoadStrategy PageLoadStrategy { get; }

        string DownloadDirectory { get; }
    }
}
