using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamKyBienBanHuyHoaDon
    {
        public BienBanXoaBoViewModel BienBan { get; set; }
        public string DataPDF { set; get; }

        public string DataXML { set; get; }
    }
}
