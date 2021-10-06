using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class WardsParam
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Slug { get; set; }
        public string Name_with_type { get; set; }
        public string Path { get; set; }
        public string Path_with_type { get; set; }
        public int? Code { get; set; }
        public int? Parent_code { get; set; }
    }
}
