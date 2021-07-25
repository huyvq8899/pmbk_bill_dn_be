using ManagementServices.Helper;
using Services.Helper.Params.BaoCao;
using Services.ViewModels.BaoCao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.BaoCao
{
    public interface IBaoCaoService
    {
        Task<List<SoLuongHoaDonDaPhatHanhViewModel>> ThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params);
        Task<string> ExportExcelThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params);
        Task<List<BaoCaoBangKeChiTietHoaDonViewModel>> BangKeChiTietHoaDonAsync(BaoCaoParams @params);
        Task<string> ExportExcelBangKeChiTietHoaDonAsync(BaoCaoParams @params);
        Task<List<TongHopGiaTriHoaDonDaSuDung>> TongHopGiaTriHoaDonDaSuDungAsync(BaoCaoParams @params);
        Task<bool> ThemBaoCaoTinhHinhSuDungHoaDon(ChonKyTinhThueParams @params);
        Task<bool> CapNhatChiTietBaoCaoTinhHinhSuDungHoaDon(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao);
        Task<List<BaoCaoTinhHinhSuDungHoaDonViewModel>> GetListTinhHinhSuDungHoaDon(PagingParams @params);
        Task<BaoCaoTinhHinhSuDungHoaDonViewModel> GetById(string baoCaoId);
        Task<ChonKyTinhThueParams> CheckNgayThangBaoCaoTinhHinhSuDungHD(ChonKyTinhThueParams @params);
        Task<bool> XoaBaoCaoTinhHinhSuDungHoaDon(string BaoCaoId);
        Task<BaoCaoBangKeChiTietHoaDonViewModel> GetBaoCaoByKyTinhThue(ChonKyTinhThueParams @params);
    }
}
