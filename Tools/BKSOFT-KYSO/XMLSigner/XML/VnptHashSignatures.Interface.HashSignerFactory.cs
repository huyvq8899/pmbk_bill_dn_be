// VnptHashSignatures.Interface.HashSignerFactory
using System;
using VnptHashSignatures.Cms;
using VnptHashSignatures.Common;
using VnptHashSignatures.Interface;
using VnptHashSignatures.Office;
using VnptHashSignatures.Pdf;
using VnptHashSignatures.Xml;

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
		return type switch
		{
			"PDF" => new PdfHashSigner(unsignData, certBase64), 
			"OFFICE" => new OfficeHashSigner(unsignData, certBase64), 
			"XML" => new XmlHashSigner(unsignData, certBase64), 
			"CMS" => new CmsHashSigner(unsignData, certBase64), 
			_ => throw new Exception("Unsuported type"), 
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
		return type switch
		{
			"PDF" => new PdfHashSigner(unsignData, certBase64, tsaUrl), 
			"OFFICE" => new OfficeHashSigner(unsignData, certBase64), 
			"XML" => new XmlHashSigner(unsignData, certBase64), 
			"CMS" => new CmsHashSigner(unsignData, certBase64), 
			_ => throw new Exception("Unsuported type"), 
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
		IHashSigner hashSigner = type switch
		{
			"PDF" => new PdfHashSigner(unsignData, certBase64, tsaUrl), 
			"OFFICE" => new OfficeHashSigner(unsignData, certBase64), 
			"XML" => new XmlHashSigner(unsignData, certBase64), 
			_ => throw new Exception("Unsuported type"), 
		};
		hashSigner.SetHashAlgorithm(alg);
		return hashSigner;
	}
}
