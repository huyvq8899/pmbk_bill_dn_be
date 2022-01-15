using System.Security.Cryptography;

namespace BKSOFT_UTILITY
{
    public class RSAPKCS1SHA256SignatureDescription : SignatureDescription
    {
        private string _hashAlgorithm;

        public RSAPKCS1SHA256SignatureDescription()
        {
            KeyAlgorithm = "System.Security.Cryptography.RSA";
            DigestAlgorithm = "System.Security.Cryptography.SHA256Cng";
            FormatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureFormatter";
            DeformatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureDeformatter";
            _hashAlgorithm = "SHA256";
        }

        public sealed override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
        {
            AsymmetricSignatureDeformatter item = base.CreateDeformatter(key);
            item.SetHashAlgorithm(_hashAlgorithm);
            return item;
        }

        public sealed override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
        {
            AsymmetricSignatureFormatter item = base.CreateFormatter(key);
            item.SetHashAlgorithm(_hashAlgorithm);
            return item;
        }
    }
}
