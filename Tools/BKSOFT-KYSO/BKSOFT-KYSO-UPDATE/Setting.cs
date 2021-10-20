using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYSO_UPDATE
{
    public class Setting
    {
        public string WebRoot { set; get; }

        public int Port { set; get; }

        public string CertificateFile { set; get; }

        public string CertificatePassword { set; get; }

        public string URL { set; get; }

        public string Date { set; get; }

        public string Version { set; get; }
    }
}
