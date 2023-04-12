using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestFramework.Services.Browser;
using RestFramework.Services.Browser.Utils;
using RestFramework.Services.Browser.BrowserFactory;
using OpenQA.Selenium.DevTools.V109.Debugger;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace RestFramework.ConfigManager
{
    public static class ConfigManager
    {
        private static string SettingsFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\settings.json";
        private static readonly string chromePath = "driverSettings.chrome"; 

        public static IBrowserProfile BrowserProfile 
        {
            get 
            {
                GetSettingsFilePath();
                string fileContent = File.ReadAllText(SettingsFilePath);
                BrowserType type;
                IDriverSettings driverSettings;
                var browserName = JsonReader.ReadDataField(fileContent, "browserName");
                switch (browserName.ToLower().Trim())
                {
                    case "chrome":
                        type = BrowserType.Chrome;
                        string version = JsonReader.ReadDataField(fileContent, $"{chromePath}.webDriverVersion"),
                               pageLoadStrategy = JsonReader.ReadDataField(fileContent, $"{chromePath}.pageLoadStrategy"),
                               downloadDirectory = JsonReader.ReadDataField(fileContent, $"{chromePath}.options.download_defaultDirectory");
                        var options = new ChromeOptions();
                        var startArguments = JsonReader.ReadArrayDataField(fileContent, $"{chromePath}.startArguments");
                        options.AddArguments(startArguments);
                        AddAdditionalOptions(options, fileContent);
                        driverSettings = new ChromeDriverSettings(version, options, PageLoadStrategy.Parse<PageLoadStrategy>(pageLoadStrategy, true),
                            downloadDirectory);
                        break;
                    default: throw new ArgumentException($"Browser {browserName} not supported");
                }
                return new BrowserProfile(type,driverSettings) ;
            }
        }

        private static void GetSettingsFilePath() 
        {
            //find settins file
        }
        private static void ChangeSettingsFilePath(string settinsFilePath) 
        {
            if(File.Exists(settinsFilePath)) 
            {
                throw new ArgumentException(settinsFilePath,$"File {settinsFilePath} doesnt exists");
            }
            SettingsFilePath = settinsFilePath;
        }

        private static void AddAdditionalOptions(DriverOptions options, string jsonContent) 
        {
            var additionaloptions = JsonReader.ReadDictionaryDataField(jsonContent, $"{chromePath}.options");
            foreach( var key in additionaloptions.Keys ) 
            {
                options.AddAdditionalOption( key, additionaloptions[key] );
            }
        }
    }
}
