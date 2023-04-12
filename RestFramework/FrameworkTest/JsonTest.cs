using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestFramework.Services.Browser.Utils;
using System.IO;

namespace FrameworkTest
{
    public class JsonTest
    {
        private readonly string jsonTestFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\test.json";
        [Test]
        public void ReadNotNestedFieldTest() 
        {
            string res = JsonReader.ReadDataField(File.ReadAllText(jsonTestFilePath), "Level1");
            Assert.IsTrue(res == "confirmed", "field read result wrong");
        }

        [Test]
        public void ReadNestedFieldTest()
        {
            string res = JsonReader.ReadDataField(File.ReadAllText(jsonTestFilePath), "Level2.Level21");
            Assert.IsTrue(res == "confirmed", "field read result wrong");
        }

        [Test]
        public void ReadNotNestedArrayFieldTest()
        {
            string[] res = JsonReader.ReadArrayDataField(File.ReadAllText(jsonTestFilePath), "Level3");
            Assert.IsTrue(res.Contains("Ford") && res.Contains("BMW"), "field read result wrong");
        }
        [Test]
        public void ReadNestedArrayFieldTest()
        {
            string[] res = JsonReader.ReadArrayDataField(File.ReadAllText(jsonTestFilePath), "Level4.Level41");
            Assert.IsTrue(res.Contains("Ford") && res.Contains("BMW"), "field read result wrong");
        }

        [Test]
        public void ReadArrayKeysFieldTest()
        {
            var res = JsonReader.ReadDictionaryDataField(File.ReadAllText(jsonTestFilePath),
                "driverSettings.chrome.options");
            Assert.IsTrue(res.Keys.Count == 6, "Wrong items count");
        }
    }
}
