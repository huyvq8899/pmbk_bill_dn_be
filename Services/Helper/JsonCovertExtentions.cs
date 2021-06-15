using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ManagementServices.Helper
{
    public static class JsonCovertExtentions
    {
        public static List<T> Deserialize<T>(this List<T> list, string pathFile)
        {
            string strJson = File.ReadAllText(pathFile);
            var dictionaries = JsonConvert.DeserializeObject<Dictionary<string, T>>(strJson);

            foreach (var item in dictionaries)
            {
                list.Add(item.Value);
            }
            return list;
        }

        public static bool IsValidJson(this string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
