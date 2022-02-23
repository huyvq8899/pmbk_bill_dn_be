using System.Security.Cryptography.X509Certificates;

namespace BKSOFT.UTILITY
{
    public interface IHashSigner
    {
        string GetSecondHashAsBase64();

        byte[] GetSecondHashBytes();

        bool CheckHashSignature(string signedHashBase64);

        byte[] Sign(string signedHashBase64);

        string SignHash(X509Certificate2 cert, string hashValues);

        void SetHashAlgorithm(MessageDigestAlgorithm alg);

        bool SetSignerCertchain(string pkcs7Base64);

        string GetSingleNodeValue(string path);
    }
}
