using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Params
{
    public class TrangThai
    {
        public int TrangThaiId { get; set; }
        public string Ten { get; set; }
        public int? TrangThaiChaId { get; set; }
        public int? Key { get; set; }
        public int Level { get; set; } = 0;
        public bool IsParent { get; set; } = false;
        public List<TrangThai> Children { get; set; } = new List<TrangThai>();
        public int SoLuong { get; set; } = 0;
    }
}
