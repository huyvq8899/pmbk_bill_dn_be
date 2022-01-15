using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO
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
