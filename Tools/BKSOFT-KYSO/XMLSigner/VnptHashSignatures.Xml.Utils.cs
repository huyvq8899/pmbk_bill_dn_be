// VnptHashSignatures.Xml.Utils
using System;
using System.Xml;
using VnptHashSignatures.Xml;

public class Utils
{
	internal static CanonicalXmlNodeList GetPropagatedAttributes(XmlElement elem)
	{
		if (elem == null)
		{
			return null;
		}
		CanonicalXmlNodeList canonicalXmlNodeList = new CanonicalXmlNodeList();
		XmlNode xmlNode = elem;
		if (xmlNode == null)
		{
			return null;
		}
		bool flag = true;
		while (xmlNode != null)
		{
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement == null)
			{
				xmlNode = xmlNode.ParentNode;
				continue;
			}
			XmlElement xmlElement2 = xmlElement;
			if (!IsCommittedNamespace(xmlElement2, xmlElement2.Prefix, xmlElement.NamespaceURI))
			{
				XmlElement xmlElement3 = xmlElement;
				if (!IsRedundantNamespace(xmlElement3, xmlElement3.Prefix, xmlElement.NamespaceURI))
				{
					string name = ((xmlElement.Prefix.Length > 0) ? ("xmlns:" + xmlElement.Prefix) : "xmlns");
					XmlAttribute xmlAttribute = elem.OwnerDocument.CreateAttribute(name);
					xmlAttribute.Value = xmlElement.NamespaceURI;
					canonicalXmlNodeList.Add(xmlAttribute);
				}
			}
			if (xmlElement.HasAttributes)
			{
				foreach (XmlAttribute attribute in xmlElement.Attributes)
				{
					if (flag && attribute.LocalName == "xmlns")
					{
						XmlAttribute xmlAttribute3 = elem.OwnerDocument.CreateAttribute("xmlns");
						xmlAttribute3.Value = attribute.Value;
						canonicalXmlNodeList.Add(xmlAttribute3);
						flag = false;
					}
					else if (attribute.Prefix == "xmlns" || attribute.Prefix == "xml")
					{
						canonicalXmlNodeList.Add(attribute);
					}
					else if (attribute.NamespaceURI.Length > 0 && !IsCommittedNamespace(xmlElement, attribute.Prefix, attribute.NamespaceURI) && !IsRedundantNamespace(xmlElement, attribute.Prefix, attribute.NamespaceURI))
					{
						string name2 = ((attribute.Prefix.Length > 0) ? ("xmlns:" + attribute.Prefix) : "xmlns");
						XmlAttribute xmlAttribute4 = elem.OwnerDocument.CreateAttribute(name2);
						xmlAttribute4.Value = attribute.NamespaceURI;
						canonicalXmlNodeList.Add(xmlAttribute4);
					}
				}
			}
			xmlNode = xmlNode.ParentNode;
		}
		return canonicalXmlNodeList;
	}

	internal static bool IsCommittedNamespace(XmlElement element, string prefix, string value)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		string name = ((prefix.Length > 0) ? ("xmlns:" + prefix) : "xmlns");
		return element.HasAttribute(name) && element.GetAttribute(name) == value;
	}

	internal static bool IsRedundantNamespace(XmlElement element, string prefix, string value)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		for (XmlNode parentNode = element.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
		{
			XmlElement xmlElement = parentNode as XmlElement;
			if (xmlElement != null && HasNamespace(xmlElement, prefix, value))
			{
				return true;
			}
		}
		return false;
	}

	internal static bool HasNamespace(XmlElement element, string prefix, string value)
	{
		return IsCommittedNamespace(element, prefix, value) || (element.Prefix == prefix && element.NamespaceURI == value);
	}

	internal static void AddNamespaces(XmlElement elem, CanonicalXmlNodeList namespaces)
	{
		if (namespaces == null)
		{
			return;
		}
		foreach (XmlNode @namespace in namespaces)
		{
			string text = ((@namespace.Prefix.Length > 0) ? (@namespace.Prefix + ":" + @namespace.LocalName) : @namespace.LocalName);
			if (!elem.HasAttribute(text) && (!text.Equals("xmlns") || elem.Prefix.Length != 0))
			{
				XmlAttribute xmlAttribute = elem.OwnerDocument.CreateAttribute(text);
				xmlAttribute.Value = @namespace.Value;
				elem.SetAttributeNode(xmlAttribute);
			}
		}
	}
}
