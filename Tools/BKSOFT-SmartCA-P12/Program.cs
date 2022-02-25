using BKSoft;
using BKSoft.Utils.Common;
using BKSoft.Utils.Interface;
using BKSoft.Utils.Xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_SmartCA_P12
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

                    HandleSign(msg);

                    string strJson = JsonConvert.SerializeObject(msg);

                    Console.WriteLine(TextHelper.Base64Encode(strJson));
                }
            }
        }

        public static MessageObj HandleSign(MessageObj msg)
        {
            try
            {
                // Handler XML
                string sParentNode = string.Empty;
                if (!string.IsNullOrWhiteSpace(msg.PathXMLOriginal) && File.Exists(msg.PathXMLOriginal))
                {
                    // Get loại thông điệp
                    switch (msg.MLTDiep)
                    {
                        case MLTDiep.TDGToKhai:                     // I.1 Định dạng dữ liệu tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử
                        case MLTDiep.TDGToKhaiUN:                   // I.2 Định dạng dữ liệu tờ khai đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn
                        case MLTDiep.TDDNCHDDT:                     // I.7 Định dạng dữ liệu đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh
                            sParentNode = "/TDiep/DLieu/TKhai/DSCKS/NNT";
                            break;
                        case MLTDiep.TDCDLHDKMDCQThue:              // II.1 Định dạng chung của hóa đơn điện tử
                        case MLTDiep.TDGHDDTTCQTCapMa:
                        case MLTDiep.TDGHDDTTCQTCMTLPSinh:
                        case MLTDiep.TBKQCMHDon:
                            sParentNode = "/TDiep/DLieu/HDon/DSCKS/NBan";
                            break;
                        case MLTDiep.TDTBHDDLSSot:                  // III.3 Định dạng dữ liệu thông báo hóa đơn điện tử có sai sót
                            sParentNode = "/TDiep/DLieu/TBao/DSCKS/NNT";
                            break;
                        case MLTDiep.TDCBTHDLHDDDTDCQThue:          // 4. Bảng tổng hợp dữ liệu
                            sParentNode = "/TDiep/DLieu/BTHDLieu/DSCKS/NNT";
                            break;
                        default:
                            break;
                    }

                    // Check parent node
                    if (string.IsNullOrWhiteSpace(sParentNode))
                    {
                        msg.Description = $"MLTDiep = {msg.MLTDiep} không tồn tại.";
                        msg.Status = false;

                        return msg;
                    }

                    // Get file Certificate
                    X509Certificate2 cert = null;
                    if (!string.IsNullOrWhiteSpace(msg.PathCert) && File.Exists(msg.PathCert))
                    {
                        cert = new X509Certificate2(msg.PathCert, msg.Password);
                    }

                    // Sign XML
                    string dataXmlOri = File.ReadAllText(msg.PathXMLOriginal);

                    // Config
                    byte[] unsignData = Encoding.UTF8.GetBytes(dataXmlOri);
                    string certBase64 = Convert.ToBase64String(cert.RawData);
                    IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
                    ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

                    // Signing XML
                    ((XmlHashSigner)signers).SetReferenceId("#SigningData");
                    ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");
                    ((XmlHashSigner)signers).SetParentNodePath(sParentNode);
                    var hashValues = signers.GetSecondHashAsBase64();
                    var datasigned = signers.SignHash(cert, hashValues);
                    byte[] signData = signers.Sign(datasigned);

                    // Write xml signed
                    if (signData != null && signData.Length > 0)
                    {
                        string dataXmlSigned = Encoding.UTF8.GetString(signData);
                        File.WriteAllText(msg.PathXMLSigned, dataXmlSigned);
                        msg.Status = true;
                    }
                }
                else
                {
                    msg.Description = $"FILE {msg.PathXMLOriginal} NOT EXIST.";
                    msg.Status = false;
                }
            }
            catch (Exception ex)
            {
                msg.Description = ex.ToString();
                msg.Status = false;

                LogFile.WriteLog(string.Empty, ex);
            }

            return msg;
        }
    }
}
