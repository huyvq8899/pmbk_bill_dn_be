using DLL.Entity.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

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

        public static string Base64Encode(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string EncodeFile(this string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(bytes);
        }

        public static string EncodeString(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
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

        public static T ConvertBase64ToObject<T>(string base64)
        {
            byte[] encodedString = Encoding.UTF8.GetBytes(base64);
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            XDocument xd = XDocument.Load(ms);

            // convert content xml to object
            if (xd.XPathSelectElement("/TDiep/DLieu/HDon/DSCKS/NBan") != null)
                xd.XPathSelectElement("/TDiep/DLieu/HDon/DSCKS/NBan").Remove();
            else if (xd.XPathSelectElement("/TDiep/DLieu/BTHDLieu/DSCKS/NNT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/BTHDLieu/DSCKS/NNT").Remove();
            }
            else if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CQT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CQT").Remove();
            }
            XmlSerializer serialiser = new XmlSerializer(typeof(T));
            var model = (T)serialiser.Deserialize(xd.CreateReader());
            return model;
        }

        public static T ConvertFileToObject<T>(string file)
        {
            XDocument xd = XDocument.Load(file);
            // convert content xml to object
            if (xd.XPathSelectElement("/TDiep/DLieu/HDon/DSCKS/NBan") != null)
                xd.XPathSelectElement("/TDiep/DLieu/HDon/DSCKS/NBan").Remove();
            else if (xd.XPathSelectElement("/TDiep/DLieu/BTHDLieu/DSCKS/NNT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/BTHDLieu/DSCKS/NNT").Remove();
            }
            else if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CQT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CQT").Remove();
            }
            XmlSerializer serialiser = new XmlSerializer(typeof(T));
            var model = (T)serialiser.Deserialize(xd.CreateReader());
            return model;
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

            string assetsFolder = !toKhai.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_1/unsigned" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_2/unsigned";
            var fullXmlFolder = Path.Combine(path, assetsFolder);
            var xmlPath = Path.Combine(fullXmlFolder, toKhai.FileXMLChuaKy);
            XmlSerializer ser = new XmlSerializer(typeof(T));
            if (File.Exists(xmlPath))
            {
                using (StreamReader sr = new StreamReader(xmlPath))
                {
                    return (T)ser.Deserialize(sr);
                }
            }
            else
            {
                string decodedContent = Encoding.UTF8.GetString(toKhai.ContentXMLChuaKy);
                using (StringReader textReader = new StringReader(decodedContent))
                {
                    return (T)ser.Deserialize(textReader);
                }
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
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static T ConvertObjectFromPlainContent<T>(string plainContent)
        {
            if (string.IsNullOrEmpty(plainContent))
                return default(T);
            using (StringReader textReader = new StringReader(plainContent))
            {
                XDocument xd = XDocument.Load(textReader);
                // convert content xml to object
                // do có file xml có nhiều chữ ký số nên tách ra các if khác nhau, ko đưa vào elseif
                // vì có trường hợp 301 cho vào elseif đã ko Remove được hết các chữ ký số đi nên ko ra nội dung

                if (xd.XPathSelectElement("/TDiep/DLieu/HDon/DSCKS/NBan") != null)
                {
                    xd.XPathSelectElement("/TDiep/DLieu/HDon/DSCKS/NBan").Remove();
                }
                    
                if (xd.XPathSelectElement("/TDiep/DLieu/BTHDLieu/DSCKS/NNT") != null)
                {
                    xd.XPathSelectElement("/TDiep/DLieu/BTHDLieu/DSCKS/NNT").Remove();
                }

                if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CQT") != null)
                {
                    xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CQT").Remove();
                }

                if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT") != null)
                {
                    xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT").Remove();
                }

                if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/TTCQT") != null)
                {
                    xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/TTCQT").Remove();
                }

                if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CCKSKhac") != null)
                {
                    xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/CCKSKhac").Remove();
                }

                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

                xd.WriteTo(xmlTextWriter);
                plainContent = stringWriter.ToString();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (StringReader txtReader = new StringReader(plainContent))
                {
                    return (T)xmlSerializer.Deserialize(txtReader);
                }
            }
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
}
