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
using RestFramework.Services;

namespace RestFramework.ConfigManager
{
    public static class ConfigManager
    {
        private static string settingsFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\settings.json";
        private static DateTime lastUpdateTime = DateTime.MinValue;
        private static string settingsFileContent = "";
        
        private static readonly string chromeJsonPath = "driverSettings.chrome";
        private static readonly string retryJsonPath = "retry";

        public static IBrowserProfile BrowserProfile 
        {
            get 
            {
                GetSettingsFilePath();
                string fileContent = SettingsFileContent;
                BrowserType type;
                IDriverSettings driverSettings;
                var browserName = JsonReader.ReadDataField(fileContent, "browserName");
                switch (browserName.ToLower().Trim())
                {
                    case "chrome":
                        type = BrowserType.Chrome;
                        string version = JsonReader.ReadDataField(fileContent, $"{chromeJsonPath}.webDriverVersion"),
                               pageLoadStrategy = JsonReader.ReadDataField(fileContent, $"{chromeJsonPath}.pageLoadStrategy"),
                               downloadDirectory = JsonReader.ReadDataField(fileContent, $"{chromeJsonPath}.options.download_defaultDirectory");
                        var options = new ChromeOptions();
                        var startArguments = JsonReader.ReadArrayDataField(fileContent, $"{chromeJsonPath}.startArguments");
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

        public static IRetryPolicy GetRetryPolicy() 
        {
            GetSettingsFilePath();
            string fileContent = SettingsFileContent;
            string number = JsonReader.ReadDataField(fileContent, $"{retryJsonPath}.number"),
                   pollingInterval = JsonReader.ReadDataField(fileContent, $"{retryJsonPath}.pollingInterval");
            return new RetryPolicy(int.Parse(number),TimeSpan.FromMilliseconds(double.Parse(pollingInterval));
        }

        private static string SettingsFileContent 
        {
            get 
            {
                var newUpdateTime = File.GetLastWriteTime(settingsFilePath);
                if (settingsFileContent == string.Empty || newUpdateTime != lastUpdateTime)
                {
                    settingsFileContent = File.ReadAllText(settingsFilePath); 
                    lastUpdateTime = newUpdateTime;
                }
                return settingsFileContent;
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
            settingsFilePath = settinsFilePath;
        }

        private static void AddAdditionalOptions(DriverOptions options, string jsonContent) 
        {
            var additionaloptions = JsonReader.ReadDictionaryDataField(jsonContent, $"{chromeJsonPath}.options");
            foreach( var key in additionaloptions.Keys ) 
            {
                options.AddAdditionalOption( key, additionaloptions[key] );
            }
        }
    }
}
