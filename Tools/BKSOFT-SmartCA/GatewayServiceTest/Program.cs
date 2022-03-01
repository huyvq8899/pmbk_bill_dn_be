using GatewayServiceTest.Signature;
using iTextSharp.text;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.ModelBinding;
using System.Xml;
using VnptHashSignatures.Common;
using VnptHashSignatures.Interface;
using VnptHashSignatures.Office;
using VnptHashSignatures.Pdf;
using VnptHashSignatures.Xml;

namespace GatewayServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "-sign")
            {
                string encode = TextHelper.Base64Decode(args[1]);
                if (!string.IsNullOrWhiteSpace(encode))
                {
                    MessageObj msg = JsonConvert.DeserializeObject<MessageObj>(encode);

                    // Check compress
                    if (!string.IsNullOrEmpty(msg.PathXML) && File.Exists(msg.PathXML))
                    {
                        msg.DataXML = File.ReadAllText(msg.PathXML);
                    }
                    else
                    {
                        msg.DataXML = TextHelper.Base64Decode(msg.DataXML);
                    }

                    // Handler xml
                    string xmlSigned = SmartCAHandler.SignSmartCAXML(msg.UID, msg.Password, msg.MLTDiep, msg.DataXML, msg.Description);
                    if (!string.IsNullOrWhiteSpace(msg.PathXMLSigned) && !File.Exists(msg.PathXMLSigned))
                    {
                        File.WriteAllText(msg.PathXMLSigned, xmlSigned);

                        // Delete file
                        File.Delete(msg.PathXML);

                        // Write path signed
                        Console.WriteLine(TextHelper.Base64Encode(msg.PathXMLSigned));
                    }
                    else
                    {
                        Console.WriteLine(TextHelper.Base64Encode(xmlSigned));
                    }
                }
            }
            else if (args.Length == 2 && args[0] == "-token")
            {
                string encode = TextHelper.Base64Decode(args[1]);
                if (!string.IsNullOrWhiteSpace(encode))
                {
                    MessageObj msg = JsonConvert.DeserializeObject<MessageObj>(encode);

                    // Handler xml
                    string token = SmartCAHandler.GetAccessToken(msg.UID, msg.Password);

                    // Return value
                    Console.WriteLine(token);
                }
            }
            else if (args.Length == 2 && args[0] == "-info")
            {
                string encode = TextHelper.Base64Decode(args[1]);
                if (!string.IsNullOrWhiteSpace(encode))
                {
                    MessageObj msg = JsonConvert.DeserializeObject<MessageObj>(encode);

                    // Handler xml
                    var cert = SmartCAHandler.SmartCAInfoFull(msg.UID, msg.Password);
                    if (cert != null)
                    {
                        string strJson = JsonConvert.SerializeObject(cert);
                        Console.WriteLine(TextHelper.Base64Encode(strJson));
                    }
                    else
                    {
                        Console.WriteLine(string.Empty);
                    }
                }
            }

            // TestSmartCAXML();

            //    Console.WriteLine(string.Empty);
        }

        public static void TestSmartCAXML()
        {
            string dataXML = File.ReadAllText("HDBH.xml");
            string json = JsonConvert.SerializeObject(
                   new
                   {
                       UID = "8640714455",
                       Password = "BachKhoa@2021",
                       MLTDiep = 200,
                       Description = "Hoa Don",
                       DataXML = TextHelper.Base64Encode(dataXML),
                       PathXMLSigned = "D:\\ToKhai.xml"
                   }, Newtonsoft.Json.Formatting.Indented);

            string encode = TextHelper.Base64Encode(json);
            if (!string.IsNullOrWhiteSpace(encode))
            {
                encode = TextHelper.Base64Decode(encode);
                MessageObj msg = JsonConvert.DeserializeObject<MessageObj>(encode);

                // Decode xml
                msg.DataXML = TextHelper.Base64Decode(msg.DataXML);
                // Handler xml
                string xmlSigned = SmartCAHandler.SignSmartCAXML(msg.UID, msg.Password, msg.MLTDiep, msg.DataXML, msg.Description);
                if (!string.IsNullOrWhiteSpace(xmlSigned))
                {
                    File.WriteAllText(msg.PathXMLSigned, xmlSigned);
                }
                Console.WriteLine(xmlSigned);

                //string xmlSigned = SmartCAHandler.GetAccessToken(msg.UID, msg.Password);

                //var cert = SmartCAHandler.SmartCAInfoFull(msg.UID, msg.Password);
            }
        }
    }
}
