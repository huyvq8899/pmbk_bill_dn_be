using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.BaoCao
{
    public class BaoCaoTinhHinhSuDungHoaDonChiTiet
    {
        public string BaoCaoTinhHinhSuDungHoaDonChiTietId { get; set; }
        public string BaoCaoTinhHinhSuDungHoaDonId { get; set; }
        public virtual BaoCaoTinhHinhSuDungHoaDon BaoCaoTinhHinhSuDungHoaDon { get; set; }
        public int LoaiHoaDon { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public int TongSo { get; set; }
        public string TonDauKyTu { get; set;}
        public string TonDauKyDen { get; set; }
        public string TrongKyTu { get; set; }
        public string TrongKyDen { get; set; }
        public int TongSoSuDung { get; set; }
        public int DaSuDung { get; set; }
        public string SuDungTu { get; set; }
        public string SuDungDen { get; set; }
        public int DaXoaBo { get; set; }
        public string SoXoaBo { get; set; }
        public int DaMat { get; set; }
        public string SoMat { get; set; }
        public int DaHuy { get; set; }
        public string SoHuy { get; set; }
        public string TonCuoiKyTu { get; set; }
        public string TonCuoiKyDen { get; set; }
        public int SoLuongTon { get; set; }
    }
}
