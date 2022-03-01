using System.IO;

namespace BKSOFT.UTILITY
{
    public class BaseHashSigner
    {
        protected string HASH_ALGORITHM = "SHA256";

        protected static readonly string ENCRYPT_ALGORITHM = "RSA";

        protected byte[] _unsignData = null;

        protected byte[] _hashOnlyBytes = null;

        protected byte[] _secondHash = null;

        protected string _signerCert = null;

        protected string _tsaRul = null;

        protected MessageDigestAlgorithm _hashAlgorithm = MessageDigestAlgorithm.SHA256;

        public BaseHashSigner()
        {
        }

        public BaseHashSigner(byte[] unsignData, string certBase64)
        {
            _unsignData = unsignData;
            _signerCert = certBase64;
        }

        public BaseHashSigner(byte[] unsignData, string certBase64, string tsaUrl)
        {
            _unsignData = unsignData;
            _signerCert = certBase64;
            _tsaRul = tsaUrl;
        }

        public static byte[] FileToByteArray(string fileName)
        {
            byte[] result = null;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            try
            {
                long length = new FileInfo(fileName).Length;
                result = binaryReader.ReadBytes((int)length);
            }
            finally
            {
                fileStream.Close();
                binaryReader.Close();
            }
            return result;
        }
    }
}