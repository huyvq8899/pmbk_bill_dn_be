using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_SmartCA_P12
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "-sign")
            {
                string encode = TextHelper.Base64Decode(args[1]);
                if (!string.IsNullOrWhiteSpace(encode))
                {
                    MessageObj msg = JsonConvert.DeserializeObject<MessageObj>(encode);

                    // Handler XML
                    if (!string.IsNullOrWhiteSpace(msg.PathXMLOriginal) && !File.Exists(msg.PathXMLOriginal))
                    {
                        // Sign XML
                    }
                    else
                    {
                         
                    }
                }
            }
        }
    }
}
