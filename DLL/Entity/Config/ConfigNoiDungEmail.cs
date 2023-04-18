using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.Config
{
    public class ConfigNoiDungEmail
    {
        public string Id { get; set; }
        public bool IsDefault { get; set; }
        public int LoaiEmail { get; set; }
        public string TieuDeEmail { get; set; }
        public string NoiDungEmail { get; set; }
    }
}
