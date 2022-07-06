using DLL.Entity.QuyDinhKyThuat;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.ViewModels.Params;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IQuyDinhKyThuatService
    {
        Task<string> GetNoiDungThongDiepXMLChuaKy(string thongDiepId);
        Task<string> GetXmlContentThongDiepAsync(string maThongDiep);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep ConvertToThongDiepTiepNhan(string encodedContent);
        Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhaiDuocChapNhan();
        Task<ThongDiepChungViewModel> GetThongDiepChungById(string Id);
        Task<ThongDiepChungViewModel> InsertThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> UpdateThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> DeleteThongDiepChung(string Id);
        Task<List<ThongDiepChungViewModel>> GetAllThongDiepTraVe(string ThongDiepGocId);
        Task<ThongDiepChungViewModel> GetThongDiepByThamChieu(string ThamChieuId);
        Task<List<string>> GetAllListCTS();
        List<LoaiThongDiep> GetListLoaiThongDiepNhan();
        List<LoaiThongDiep> GetListLoaiThongDiepGui();
        int GetTrangThaiPhanHoiThongDiepNhan(ThongDiepChungViewModel tdn);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep ConvertToThongDiepKUNCQT(string encodedContent);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep ConvertToThongDiepUNCQT(string encodedContent);
        Task<bool> InsertThongDiepNhanAsync(ThongDiepPhanHoiParams @params);
        List<EnumModel> GetTrangThaiGuiPhanHoiTuCQT(int maLoaiThongDiep);
        Task<ThongDiepChiTiet> ShowThongDiepFromFileByIdAsync(string id);
        Task<FileReturn> ExportBangKeAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhai();
        Task<bool> CheckChuaLapToKhaiAsync();
        Task<List<ToKhaiForBoKyHieuHoaDonViewModel>> GetListToKhaiFromBoKyHieuHoaDonAsync(ToKhaiParams toKhaiParams);
        List<DangKyUyNhiemViewModel> GetListTrungKyHieuTrongHeThong(List<DangKyUyNhiemViewModel> data);
        Task<FileReturn> GetLinkFileXml(ThongDiepChungViewModel model, bool signed = false);
        List<EnumModel> GetListTimKiemTheoThongDiep();
        Task<ThongDiepChiTiet> GetAllThongDiepTraVeV2(string giaTriTimKiem, string phanLoai);
        Task<ThongKeSoLuongThongDiepViewModel> ThongKeSoLuongThongDiepAsync(int trangThaiGuiThongDiep, byte coThongKeSoLuong);
        Task<ThongKeSoLuongThongDiepViewModel> ThongKeSoLuongThongDiepRaSoatAsync(byte coThongKeSoLuong);
        Task<int> UpdateNgayThongBaoToKhaiAsync();
        Task<KetQuaConvertPDF> ConvertThongDiepToFilePDF(ThongDiepChungViewModel td);
        Task<ThongDiepChungViewModel> GetThongDiepChungByMaThongDiep(string maThongDiep);

    }
}
