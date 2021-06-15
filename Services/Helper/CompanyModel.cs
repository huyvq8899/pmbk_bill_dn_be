using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class CompanyModel
    {
        public string TaxCode { get; set; }

        public string Server { set; get; }

        public string DataBaseName { get; set; }

        public string Password { set; get; }

        public int Type { get; set; }

        public string ConnectionString { get; set; }

        public int TypeDetail { set; get; }

        public string UrlInvoice { set; get; }
    }
}
