﻿using DLL.Entity.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Services.Helper
{
    public static class DataHelper
    {
        public static DataTable CreateDataTable<T>(this IEnumerable<T> list)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            DataTable dataTable = new DataTable();

            foreach (PropertyInfo info in properties)
            {
                DisplayAttribute dd = info.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                if (dd != null)
                {
                    dataTable.Columns.Add(new DataColumn(dd.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                }
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static void GenerateBillXML(this HoaDonDienTuViewModel data, string path)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            // XmlSerializer serialiser = new XmlSerializer(typeof(Invoice), new XmlRootAttribute(rootName));
            XmlSerializer serialiser = new XmlSerializer(typeof(HoaDonDienTu));

            using (TextWriter filestream = new StreamWriter(path))
            {
                serialiser.Serialize(filestream, data, ns);
            }
        }

        public static string GetBankNumberFromString(this string sampleString)
        {
            string result = "";
            try
            {
                if (string.IsNullOrEmpty(sampleString)) return "";
                string[] words = sampleString.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in words)
                {
                    var isNumeric = int.TryParse(item, out int n);
                    if (item.ToCharArray().Length >= 8 && isNumeric)
                    {
                        result = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
    }
}
