using Newtonsoft.Json;

namespace BKSOFT.UTILITY
{
    public class XMLSignauterParam
    {
        [JsonProperty("namespace")]
        public string Namespace
        {
            get;
            set;
        }

        [JsonProperty("namespaceRef")]
        public string NamespaceRef
        {
            get;
            set;
        }

        [JsonProperty("parentNodePath")]
        public string ParentNodePath
        {
            get;
            set;
        }

        [JsonProperty("referenceId")]
        public string ReferenceId
        {
            get;
            set;
        }

        [JsonProperty("signatureId")]
        public string SignatureId
        {
            get;
            set;
        }
    }
}