using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.DanhMuc
{
    public class CoQuanThue
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string MaCQTCapCuc { get; set; }
    }
}
