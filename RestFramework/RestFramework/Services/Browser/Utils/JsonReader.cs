using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace RestFramework.Services.Browser.Utils
{
    public class JsonReader 
    {
        //public static string[] GetNestedFieldsNames(string fieldName)
        //{
        //    CheckPathSet(testDataPath, "Test data file path");
        //    StreamReader reader = File.OpenText(testDataPath);
        //    JsonTextReader jsonTextReader = new JsonTextReader(reader);
        //    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
        //    List<string> result = new List<string>();
        //    foreach (var jToken in jsonObject[fieldName].Children())
        //    {
        //        result.Add(jToken.Path.Substring(jToken.Path.LastIndexOf('.') + 1));
        //    }
        //    return result.ToArray();
        //}

        public static string ReadDataField(string jsonText,string JsonPath)
        {
            TextReader reader = new StringReader(jsonText);
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            JToken result = jsonObject;
            string[] path = JsonPath.Split('.');
            foreach (string fieldName in path)
            {
                result = result[fieldName];
                if (result == null)
                    throw new ArgumentException($"Json path {JsonPath} doesnt exists");
            }

            return result.ToString();
        }
        public static string[] ReadArrayDataField(string jsonText, string JsonPath)
        {
            TextReader reader = new StringReader(jsonText);
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            string path = JsonPath;
            List<string> result = new List<string>();
 
            foreach (var jToken in jsonObject.SelectToken(path))
            {
                result.Add(jToken.ToString());
            }
            return result.ToArray();
        }

        public static Dictionary<string,string> ReadDictionaryDataField(string jsonText, string JsonPath)
        {
            TextReader reader = new StringReader(jsonText);
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            string path = JsonPath;
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (KeyValuePair<string, JToken> pair in (JObject)jsonObject.SelectToken(path))
            {

                //var key = jToken.Path.Remove(0, jToken.Path.LastIndexOf('.'));
                var key = pair.Key;
                var value = pair.Value.ToString();
                if(value != null)
                {
                    result.Add(key, value);
                }
            }
            return result;
        }

    }
}
