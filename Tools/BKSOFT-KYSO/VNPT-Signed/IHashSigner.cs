// VnptHashSignatures.Interface.IHashSigner
using VnptHashSignatures.Common;
namespace VnptHashSignatures.Interface
{
	public interface IHashSigner
	{
		string GetSecondHashAsBase64();

		byte[] GetSecondHashBytes();

		bool CheckHashSignature(string signedHashBase64);

		byte[] Sign(string signedHashBase64);

		void SetHashAlgorithm(MessageDigestAlgorithm alg);

		bool SetSignerCertchain(string pkcs7Base64);

		string GetSignerSubjectDN();
	}
}