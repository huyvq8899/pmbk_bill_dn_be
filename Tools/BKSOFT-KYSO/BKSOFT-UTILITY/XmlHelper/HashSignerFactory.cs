using System;

namespace BKSOFT.UTILITY
{
    public class HashSignerFactory
    {
        public const string PDF = "PDF";

        public const string OFFICE = "OFFICE";

        public const string XML = "XML";

        public const string CMS = "CMS";

        public static IHashSigner GenerateSigner(byte[] unsignData, string certBase64, string type)
        {
            if (string.IsNullOrEmpty(certBase64))
            {
                throw new FormatException("Bas64 must not be null");
            }
            try
            {
                Convert.FromBase64String(certBase64);
            }
            catch (FormatException ex)
            {
                throw ex;
            }
            switch (type)
            {
                case "XML":
                    return new XmlHashSigner(unsignData, certBase64);
                default:
                    throw new Exception("Unsuported type");
            };
        }

        public static IHashSigner GenerateSigner(byte[] unsignData, string certBase64, string tsaUrl, string type)
        {
            if (string.IsNullOrEmpty(certBase64))
            {
                throw new FormatException("Bas64 must not be null");
            }
            try
            {
                Convert.FromBase64String(certBase64);
            }
            catch (FormatException ex)
            {
                throw ex;
            }
            switch (type)
            {
                case "XML":
                    return new XmlHashSigner(unsignData, certBase64);
                default:
                    throw new Exception("Unsuported type");
            };
        }

        public static IHashSigner GenerateSigner(byte[] unsignData, string certBase64, string tsaUrl, string type, MessageDigestAlgorithm alg)
        {
            if (string.IsNullOrEmpty(certBase64))
            {
                throw new FormatException("Bas64 must not be null");
            }
            try
            {
                Convert.FromBase64String(certBase64);
            }
            catch (FormatException ex)
            {
                throw ex;
            }
            IHashSigner hashSigner = null;
            switch (type)
            {
                case "XML":
                    hashSigner = new XmlHashSigner(unsignData, certBase64);
                    break;
                default:
                    throw new Exception("Unsuported type");
            };

            hashSigner.SetHashAlgorithm(alg);
            return hashSigner;
        }
    }
}