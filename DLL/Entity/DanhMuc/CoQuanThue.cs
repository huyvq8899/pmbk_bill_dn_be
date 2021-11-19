using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.DanhMuc
{
    public class CoQuanThue : ICloneable
    {
        public string Id { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string MaCQTCapCuc { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public List<CoQuanThue> InitData()
        {
            List<CoQuanThue> datas = new List<CoQuanThue>()
            {
                new CoQuanThue
                {
                    Id = Guid.NewGuid().ToString(),
                    Ma = "10100",
                    Ten = ""
                }
            };

            return datas;
        }
    }
}
