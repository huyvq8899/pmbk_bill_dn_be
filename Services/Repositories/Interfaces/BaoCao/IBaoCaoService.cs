using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.BaoCao;
using Services.ViewModels.BaoCao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.BaoCao
{
    public interface IBaoCaoService
    {
        Task<List<SoLuongHoaDonDaPhatHanhViewModel>> ThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params);
        string ExportExcelThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params);
        string PrintThongKeSoLuongHoaDonDaPhatHanh(BaoCaoParams @params);
        Task<List<BaoCaoBangKeChiTietHoaDonViewModel>> BangKeChiTietHoaDonAsync(BaoCaoParams @params);
        string ExportExcelBangKeChiTietHoaDonAsync(BaoCaoParams @params);
        string PrintBangKeChiTietHoaDonAsync(BaoCaoParams @params);
        Task<List<TongHopGiaTriHoaDonDaSuDung>> TongHopGiaTriHoaDonDaSuDungAsync(BaoCaoParams @params);
        Task<FileReturn> ExportExcelTongHopGiaTriHoaDonDaSuDungAsync(BaoCaoParams @params);
        Task<bool> ThemBaoCaoTinhHinhSuDungHoaDon(ChonKyTinhThueParams @params);
        Task<bool> CapNhatChiTietBaoCaoTinhHinhSuDungHoaDon(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao);
        Task<string> ExportExcelBaoCaoTinhHinhSuDungHoaDonAsync(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao);
        Task<string> PrintChiTietBaoCaoTinhHinhSuDungHoaDonAsync(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao);
        Task<List<BaoCaoTinhHinhSuDungHoaDonViewModel>> GetListTinhHinhSuDungHoaDon(PagingParams @params);
        Task<BaoCaoTinhHinhSuDungHoaDonViewModel> GetById(string baoCaoId);
        Task<ChonKyTinhThueParams> CheckNgayThangBaoCaoTinhHinhSuDungHD(ChonKyTinhThueParams @params);
        Task<bool> XoaBaoCaoTinhHinhSuDungHoaDon(string BaoCaoId);
        Task<BaoCaoBangKeChiTietHoaDonViewModel> GetBaoCaoByKyTinhThue(ChonKyTinhThueParams @params);
        Task<string> ExportExcelBangKeHangHoaBanRa(PagingParams @params);
    }
}
