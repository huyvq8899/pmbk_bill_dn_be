using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class WardsParam
    {
        public string name { get; set; }
        public string type { get; set; }
        public string slug { get; set; }
        public string name_with_type { get; set; }
        public string path { get; set; }
        public string path_with_type { get; set; }
        public int? code { get; set; }
        public int? parent_code { get; set; }
    }
}
