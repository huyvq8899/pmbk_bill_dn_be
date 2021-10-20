using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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
                DisplayAttribute dd = (DisplayAttribute)info.GetCustomAttribute(typeof(DisplayAttribute));
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
            return Encoding.UTF8.GetString(base64EncodedBytes);
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
            XmlSerializer serialiser = new XmlSerializer(typeof(HoaDonDienTuViewModel));

            using (TextWriter filestream = new StreamWriter(path))
            {
                serialiser.Serialize(filestream, data, ns);
            }
        }

        public static void GenerateBienBanXML(this BienBanXoaBoViewModel data, string path)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            // XmlSerializer serialiser = new XmlSerializer(typeof(Invoice), new XmlRootAttribute(rootName));
            XmlSerializer serialiser = new XmlSerializer(typeof(BienBanXoaBoViewModel));

            using (TextWriter filestream = new StreamWriter(path))
            {
                serialiser.Serialize(filestream, data, ns);
            }
        }

        public static T ConvertByteXMLToObject<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                XDocument xd = XDocument.Load(ms);

                // convert content xml to object
                XmlSerializer serialiser = new XmlSerializer(typeof(T));
                var model = (T)serialiser.Deserialize(xd.CreateReader());
                return model;
            }
        }

        public static string GetBankNumberFromString(this string sampleString)
        {
            if (string.IsNullOrEmpty(sampleString)) return "";
            string[] words = sampleString.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in words)
            {
                var isNumeric = int.TryParse(item, out _);
                if (item.ToCharArray().Length >= 8 && isNumeric)
                {
                    return item;
                }
            }

            return string.Empty;
        }

        public static T ConvertObjectFromTKhai<T>(ToKhaiDangKyThongTin toKhai, string path)
        {
            if (toKhai == null)
                return default(T);

            string assetsFolder;
            if(toKhai.SignedStatus == true)
            {
                assetsFolder = !toKhai.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_1/signed" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_2/signed";
            }
            else assetsFolder = !toKhai.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_1/unsigned" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_2/unsigned";
            var fullXmlFolder = Path.Combine(path, assetsFolder);
            var xmlPath = Path.Combine(fullXmlFolder, toKhai.FileXMLChuaKy);
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StreamReader sr = new StreamReader(xmlPath))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public static T ConvertObjectFromStringContent<T>(string encodedContent)
        {
            if (string.IsNullOrEmpty(encodedContent))
                return default(T);

            var base64EncodedBytes = System.Convert.FromBase64String(encodedContent);
            string decodedContent = Encoding.UTF8.GetString(base64EncodedBytes);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(decodedContent))
            {
                return (T)xmlSerializer. Deserialize(textReader);
            }
        }
    }
}
