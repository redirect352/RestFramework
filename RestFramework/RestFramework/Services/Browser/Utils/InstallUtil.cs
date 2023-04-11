using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace RestFramework.Services.Browser.Utils
{
    internal static class InstallUtil
    {
        public static IWebProxy? Proxy { get; set; }
        public static string SetupBinary(string url, string zipPath, string binaryPath)
        {
            if (File.Exists(binaryPath))
            {
                return binaryPath;
            }

            string directoryName = Path.GetDirectoryName(zipPath);
            string fileName = Path.GetFileName(binaryPath);
            Directory.CreateDirectory(directoryName);
            zipPath = DownloadZip(url, zipPath);
            string text = Path.Combine(directoryName, "staging");
            string text2 = Path.Combine(text, fileName);
            Directory.CreateDirectory(text);
            if (zipPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(zipPath, text2);
            }
            else if (zipPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                UnZip(zipPath, text2, fileName);
            }
            else if (zipPath.EndsWith(".tar.gz", StringComparison.OrdinalIgnoreCase))
            {
                UnZipTgz(zipPath, text2);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("chmod", "+x " + text2)?.WaitForExit();
            }

            string directoryName2 = Path.GetDirectoryName(binaryPath);
            if (!Directory.Exists(directoryName2))
            {
                Directory.CreateDirectory(directoryName2);
            }

            Exception innerException = null;
            try
            {
                string[] files = Directory.GetFiles(text);
                string[] array = files;
                foreach (string text3 in array)
                {
                    string fileName2 = Path.GetFileName(text3);
                    string destFileName = Path.Combine(directoryName2, fileName2);
                    File.Copy(text3, destFileName, overwrite: true);
                }
            }
            catch (Exception ex)
            {
                innerException = ex;
            }

            try
            {
                if (Directory.Exists(text))
                {
                    Directory.Delete(text, recursive: true);
                }
            }
            catch (Exception ex2)
            {
                Console.Error.WriteLine(ex2.ToString());
            }

            try
            {
                RemoveZip(zipPath);
            }
            catch (Exception ex3)
            {
                Console.Error.WriteLine(ex3.ToString());
            }

            if (!Directory.Exists(directoryName2))
            {
                throw new Exception("Error writing " + directoryName2, innerException);
            }

            return binaryPath;
        }

        private static string DownloadZip(string url, string destination)
        {
            if (File.Exists(destination))
            {
                return destination;
            }

            if (Proxy == null)
            {
                CheckProxySystemVariables();
            }

            if (Proxy != null)
            {
                using WebClient webClient = new WebClient
                {
                    Proxy = Proxy
                };
                webClient.DownloadFile(new Uri(url), destination);
            }
            else
            {
                using WebClient webClient2 = new WebClient();
                webClient2.DownloadFile(new Uri(url), destination);
            }

            return destination;
        }

        private static void CheckProxySystemVariables()
        {
            string environmentVariable = Environment.GetEnvironmentVariable("HTTP_PROXY", EnvironmentVariableTarget.Process);
            string environmentVariable2 = Environment.GetEnvironmentVariable("HTTPS_PROXY", EnvironmentVariableTarget.Process);
            if (!string.IsNullOrEmpty(environmentVariable))
            {
                Proxy = new WebProxy(environmentVariable);
            }
            else if (!string.IsNullOrEmpty(environmentVariable2))
            {
                Proxy = new WebProxy(environmentVariable2);
            }
        }

        private static string UnZip(string path, string destination, string name)
        {
            if (Path.GetFileName(path)?.Equals(name, StringComparison.CurrentCultureIgnoreCase) ?? false)
            {
                File.Copy(path, destination);
                return destination;
            }

            using (ZipArchive zipArchive = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    if (entry.Name == name)
                    {
                        entry.ExtractToFile(destination, overwrite: true);
                    }
                }
            }

            return destination;
        }

        private static void UnZipTgz(string gzArchiveName, string destination)
        {
            using FileStream baseInputStream = File.OpenRead(gzArchiveName);
            using GZipInputStream inputStream = new GZipInputStream(baseInputStream);
            string directoryName = Path.GetDirectoryName(destination);
            using TarArchive tarArchive = TarArchive.CreateInputTarArchive(inputStream, Encoding.Default);
            tarArchive.ExtractContents(directoryName);
        }

        private static void RemoveZip(string path)
        {
            File.Delete(path);
        }

    }
}
