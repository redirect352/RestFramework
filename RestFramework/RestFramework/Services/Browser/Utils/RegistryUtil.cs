using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;
using WebDriverManager.Helpers;

namespace RestFramework.Services.Browser.Utils
{
    internal static class RegistryUtil
    {
        private static readonly string winRegistryVersionTemplateCurrentUser = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\<executableFileName>";
        private static readonly string winRegistryVersionTemplateLocalMachine = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\<executableFileName>";

        public static string GetInstalledBrowserVersionWindows(string executableFileName)
        {
            object? value = Registry.GetValue(winRegistryVersionTemplateCurrentUser.Replace("<executableFileName>", executableFileName), "", null);
            if (value != null)
            {
                return FileVersionInfo.GetVersionInfo(value.ToString()).FileVersion;
            }

            object value2 = Registry.GetValue(winRegistryVersionTemplateLocalMachine.Replace("<executableFileName>", executableFileName), "", null);
            if (value2 != null)
            {
                return FileVersionInfo.GetVersionInfo(value2.ToString()).FileVersion;
            }

            return null;
        }

        public static string GetInstalledBrowserVersionLinux(params string[] executableAndArgumentsPairs)
        {
            int num = executableAndArgumentsPairs.Length;
            if (num % 2 == 1)
            {
                throw new Exception("Please provide arguments for every executable!");
            }

            for (int i = 0; i < num; i += 2)
            {
                string fileName = executableAndArgumentsPairs[i];
                string arguments = executableAndArgumentsPairs[i + 1];
                string fullPath = GetFullPath(fileName);
                if (fullPath != null)
                {
                    return GetInstalledBrowserVersionLinux(fullPath, arguments);
                }
            }

            throw new Exception($"Unable to locate installed browser for runtime platform {Environment.OSVersion.Platform}");
        }

        private static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
            {
                return Path.GetFullPath(fileName);
            }

            string[] array = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Array.Empty<string>();
            string[] array2 = array;
            foreach (string path in array2)
            {
                string text = Path.Combine(path, fileName);
                if (File.Exists(text))
                {
                    return text;
                }
            }

            return null;
        }
    }
}
