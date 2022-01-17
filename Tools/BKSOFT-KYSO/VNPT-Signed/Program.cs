using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnptHashSignatures.Common;
using VnptHashSignatures.Interface;
using VnptHashSignatures.Xml;

namespace VNPT_Signed
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlData = "";

            string certBase64 = string.Empty;

            byte[] unsignData = Encoding.UTF8.GetBytes(xmlData);

            // Step 4: Sign XML
            IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
            ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

            // Set reference đến id
            ((XmlHashSigner)signers).SetReferenceId("#SigningData");

            // Set thời gian ký
            ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");

            hashValues = signers.GetSecondHashAsBase64();

            byte[] signed = signers.Sign(datasigned);
        }
    }
}
