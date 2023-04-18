using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Ocsp;

namespace Services.Repositories.Implimentations.ESignCloud
{
    class MakeSignature
    {
        private string data;
        private string key;
        private string passKey;

        public MakeSignature(string dataInput, string PriKeyPath, string PriKeyPass)
        {
            data = dataInput;
            key = PriKeyPath;
            passKey = PriKeyPass;
        }

        public string getSignature()
        {
            RSACng key = GetKey();
            return Sign(data, key);
        }

        public static string Sign(string content, RSACng rsa)
        {
            RSACng crsa = rsa;
            byte[] Data = Encoding.UTF8.GetBytes(content);
            //byte[] signData = crsa.SignData(Data, "sha1");
            byte[] signData = crsa.SignData(Data, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signData);
        }
        private RSACng GetKey()
        {
            X509Certificate2 cert2 = new X509Certificate2(key, passKey, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            //RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert2.PrivateKey;
            RSACng rSACng = (RSACng)cert2.PrivateKey;
            return rSACng;
        }
    }
}
