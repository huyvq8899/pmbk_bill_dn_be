using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Services.Helper
{
    public static class HoaDonHelper
    {
        public static string GetDienGiaiTrangThaiHoaDon(int? hinhThucXoaBo, int? trangThaiGuiHoaDon)
        {
            if (trangThaiGuiHoaDon == (int)Params.HoaDon.LoaiTrangThaiGuiHoaDon.DaGui && (hinhThucXoaBo != null && hinhThucXoaBo != (int)DLL.Enums.HinhThucXoabo.HinhThuc3))
            {
                return "Đã bị xóa bỏ";
            }
            
            if (hinhThucXoaBo == (int)DLL.Enums.HinhThucXoabo.HinhThuc3)
            {
                return "Đã hủy theo lý do phát sinh";
            }
            else if (hinhThucXoaBo == (int)DLL.Enums.HinhThucXoabo.HinhThuc1 ||
                hinhThucXoaBo == (int)DLL.Enums.HinhThucXoabo.HinhThuc4 ||
                hinhThucXoaBo == (int)DLL.Enums.HinhThucXoabo.HinhThuc6)
            {
                return "Đã hủy do sai sót";
            }
            else if (hinhThucXoaBo != null)
            {
                return "Đã bị xóa bỏ";
            }

            return "";
        }
    }
}
