using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.Services;

namespace RestFramework.Services.Browser.Utils
{
    public static class VariablesUtil
    {
        public static void SetupVariable(string path)
        {
            UpdatePath(path);
        }

        private static void UpdatePath(string path)
        {
            string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            if (environmentVariable == null)
            {
                throw new ArgumentNullException("Can't get PATH variable");
            }

            path = Path.GetDirectoryName(path);
            string value = $"{path}{Path.PathSeparator}{environmentVariable}";
            if (path != null && !environmentVariable.Contains(path))
            {
                Environment.SetEnvironmentVariable("PATH", value, EnvironmentVariableTarget.Process);
            }
        }
    }
}
