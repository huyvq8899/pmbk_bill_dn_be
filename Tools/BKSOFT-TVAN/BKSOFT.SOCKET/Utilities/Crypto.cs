using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using USB_Token_DLL;

namespace BKSOFT.TCT
{
    public class Crypto
    {
        private static bool GenarateRSAKey()
        {
            try
            {
                // Lets take a new CSP with a new 2048 bit rsa key pair
                var csp = new RSACryptoServiceProvider(2048);

                // How to get the private key
                var privKey = csp.ExportParameters(true);

                // And the public key ...
                var pubKey = csp.ExportParameters(false);

                // Converting the private key into a string representation
                string priKeyString;
                {
                    //we need some buffer
                    var sw = new System.IO.StringWriter();
                    //we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    //serialize the key into the stream
                    xs.Serialize(sw, privKey);
                    //get the string from the stream
                    priKeyString = sw.ToString();
                }

                // Converting the public key into a string representation
                string pubKeyString;
                {
                    // we need some buffer
                    var sw = new System.IO.StringWriter();
                    // we need a serializer
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                    // serialize the key into the stream
                    xs.Serialize(sw, pubKey);
                    // get the string from the stream
                    pubKeyString = sw.ToString();
                }

                // Write private & public key to file
                byte[] bytesPubKey = Encoding.UTF8.GetBytes(pubKeyString);
                string str = Convert.ToBase64String(bytesPubKey);
                File.WriteAllText("pub.key", str);

                byte[] bytesPriKey = Encoding.UTF8.GetBytes(priKeyString);
                str = Convert.ToBase64String(bytesPriKey);
                File.WriteAllText("pri.key", str);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static bool LoadPublicKey(string path, ref RSAParameters pubKey)
        {
            try
            {
                string strData = File.ReadAllText(path);
                byte[] bytes = Convert.FromBase64String(strData);
                // Get public key string
                string pubKeyString = Encoding.UTF8.GetString(bytes);
                // get a stream from the string
                var sr = new System.IO.StringReader(pubKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private static bool LoadPrivateKey(string path, ref RSAParameters priKey)
        {
            try
            {
                string strData = File.ReadAllText(path);
                byte[] bytes = Convert.FromBase64String(strData);
                // Get public key string
                string priKeyString = Encoding.UTF8.GetString(bytes);
                // get a stream from the string
                var sr = new System.IO.StringReader(priKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                priKey = (RSAParameters)xs.Deserialize(sr);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private static string RSAEncodeText(string pubPath, string plainTextData)
        {
            string cypherText = string.Empty;
            try
            {
                // Read file key
                string strData = File.ReadAllText(pubPath);
                // Encode from base 64 string
                byte[] bytes = Convert.FromBase64String(strData);
                // Get public key string
                string pubKeyString = Encoding.UTF8.GetString(bytes);
                // get a stream from the string
                var sr = new System.IO.StringReader(pubKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                var pubKey = (RSAParameters)xs.Deserialize(sr);

                // RSA crypto
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(pubKey);

                //for encryption, always handle bytes...
                var bytesPlainTextData = Encoding.Unicode.GetBytes(plainTextData);

                //apply pkcs#1.5 padding and encrypt our data 
                var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

                //we might want a string representation of our cypher text... base64 will do
                cypherText = Convert.ToBase64String(bytesCypherText);
            }
            catch (Exception)
            {
            }

            return cypherText;
        }

        private static string RSADecodeText(string priPath, string cypherTextData)
        {
            string plainTextData = string.Empty;
            try
            {
                // Read file key
                string strData = File.ReadAllText(priPath);
                // Encode from base 64 string
                byte[] bytes = Convert.FromBase64String(strData);
                // Get public key string
                string priKeyString = Encoding.UTF8.GetString(bytes);
                // get a stream from the string
                var sr = new System.IO.StringReader(priKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                var priKey = (RSAParameters)xs.Deserialize(sr);

                // RSA crypto
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(priKey);

                //for encryption, always handle bytes...
                var bytesCypherText = Convert.FromBase64String(cypherTextData);

                //decrypt and strip pkcs#1.5 padding
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

                //get our original plainText back...
                plainTextData = Encoding.Unicode.GetString(bytesPlainTextData);
            }
            catch (Exception ex)
            {
            }

            return plainTextData;
        }

        private static bool RSAEncodeText(string pubKeyString, string plainTextData, ref string cypherText)
        {
            bool res = true;
            try
            {
                // get a stream from the string
                var sr = new System.IO.StringReader(pubKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                var pubKey = (RSAParameters)xs.Deserialize(sr);

                // RSA crypto
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(pubKey);

                //for encryption, always handle bytes...
                var bytesPlainTextData = Encoding.Unicode.GetBytes(plainTextData);

                //apply pkcs#1.5 padding and encrypt our data 
                var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

                //we might want a string representation of our cypher text... base64 will do
                cypherText = Convert.ToBase64String(bytesCypherText);
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        private static bool RSADecodeText(string priKeyString, string cypherTextData, ref string plainTextData)
        {
            bool res = true;
            try
            {
                // get a stream from the string
                var sr = new System.IO.StringReader(priKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                var priKey = (RSAParameters)xs.Deserialize(sr);

                // RSA crypto
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(priKey);

                //for encryption, always handle bytes...
                var bytesCypherText = Convert.FromBase64String(cypherTextData);

                //decrypt and strip pkcs#1.5 padding
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

                //get our original plainText back...
                plainTextData = Encoding.Unicode.GetString(bytesPlainTextData);
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        private static byte[] Zip(string str)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }
                return mso.ToArray();
            }
        }

        private static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }
                return System.Text.Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static bool EncodeText(string plainTextData, ref string cypherText)
        {
            bool res = true;
            try
            {
                // Encode from base 64 string
                byte[] bytes = Convert.FromBase64String(Encoding.UTF8.GetString(RSAConstants.PUB_KEY));
                // Get public key string
                string pubKeyString = Encoding.UTF8.GetString(bytes);
                // get a stream from the string
                var sr = new System.IO.StringReader(pubKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                var pubKey = (RSAParameters)xs.Deserialize(sr);

                // RSA crypto
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(pubKey);

                // Zip data
                var bytesPlainTextData = Zip(plainTextData);

                //apply pkcs#1.5 padding and encrypt our data 
                var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

                //we might want a string representation of our cypher text... base64 will do
                cypherText = Convert.ToBase64String(bytesCypherText);
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        public static bool DecodeText(string cypherText, ref string plainTextData)
        {
            bool res = true;
            try
            {
                // Encode from base 64 string
                byte[] bytes = Convert.FromBase64String(Encoding.UTF8.GetString(RSAConstants.PRI_KEY));
                // Get public key string
                string priKeyString = Encoding.UTF8.GetString(bytes);

                // get a stream from the string
                var sr = new System.IO.StringReader(priKeyString);
                // we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                // get the object back from the stream
                var priKey = (RSAParameters)xs.Deserialize(sr);

                // RSA crypto
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(priKey);

                //for encryption, always handle bytes...
                var bytesCypherText = Convert.FromBase64String(cypherText);

                //decrypt and strip pkcs#1.5 padding
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

                //get our original plainText back...
                plainTextData = Unzip(bytesPlainTextData);
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }
    }
}
