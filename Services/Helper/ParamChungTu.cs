using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class ParamChungTu
    {
        public string RefId { get; set; }
        public int? RefOfType { get; set; }
        public DateTime? NgayHachToan { get; set; }
        public DateTime? NgayChungTu { get; set; }
        public string SoChungTu { get; set; }
        public string SoChungTuMoi { get; set; }
        public string TienTo { get; set; }
        public string LoaiChungTu { get; set; }
        public string DienGiai { get; set; }
        public string MaChucNang { get; set; }
        public string SoHoaDon { get; set; }
        public int? GiaTriBatDau { get; set; }
        public int? TongSoKyTuPhanSo { set; get; }
        public List<ParamChungTu> List { get; set; }
    }
}
