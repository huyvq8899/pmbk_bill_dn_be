using System;

namespace Services.ViewModels.Config
{
    [Serializable]
    public class TuyChinhChiTietModel
    {
        public int? DoRong { get; set; }
        public int? CoChu { get; set; }
        public int? CanTieuDe { get; set; }  // 1: căn theo tiêu đề, 2: căn theo tiêu đề dài nhất, 3: căn theo tiêu đề dài nhất bao 
        public bool? ChuDam { get; set; }
        public bool? ChuNghieng { get; set; }
        public bool? MaSoThue { get; set; }
        public string MauNenTieuDeBang { get; set; }
        public string MauChu { get; set; }
        public int? CanChu { get; set; } // 1: trái, 2: giữa, 3: phải
        public bool? IsTuyChinhGiaTri { get; set; }
    }
}
