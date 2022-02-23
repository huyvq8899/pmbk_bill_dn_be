using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static T RemoveTrailingZeros<T>(this T data)
        {
            var json = JsonConvert.SerializeObject(data, new DecimalFormatConverter());
            data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }

    public class DecimalFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal)) || (objectType == typeof(decimal?));
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            //writer.WriteValue(string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:G29}", value));
            //writer.WriteValue(((decimal)value).Normalize().ToString(CultureInfo.CreateSpecificCulture("en-US")));
            var conv = ((decimal)value).ToString("0.######", CultureInfo.CreateSpecificCulture("en-US"));
            writer.WriteValue(conv);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType,
                                     object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
