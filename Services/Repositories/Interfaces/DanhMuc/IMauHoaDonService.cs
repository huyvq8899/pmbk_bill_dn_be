using DLL.Enums;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.TienIch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IMauHoaDonService
    {
        List<ImageParam> GetMauHoaDonBackgrounds();
        Task<List<MauHoaDonViewModel>> GetAllAsync(MauHoaDonParams @params = null);
        Task<PagedList<MauHoaDonViewModel>> GetAllPagingAsync(MauHoaDonParams @params);
        Task<MauHoaDonViewModel> GetByIdAsync(string id);
        Task<MauHoaDonViewModel> GetByIdBasicAsync(string id);
        Task<MauHoaDonViewModel> GetNgayKyByIdAsync(string id);
        List<MauParam> GetListMauHoaDon(MauHoaDonParams @params);
        Task<List<string>> GetAllMauSoHoaDon();
        Task<List<string>> GetAllKyHieuHoaDon(string ms = "");
        Task<List<MauHoaDonViewModel>> GetListMauDaDuocChapNhanAsync();
        List<EnumModel> GetListQuyDinhApDung();
        List<EnumModel> GetListLoaiHoaDon();
        List<EnumModel> GetListLoaiMau();
        List<EnumModel> GetListLoaiThueGTGT();
        List<EnumModel> GetListLoaiNgonNgu();
        List<EnumModel> GetListLoaiKhoGiay();
        Task<FileReturn> PreviewPdfAsync(MauHoaDonFileParams @params);
        Task<FileReturn> DownloadFileAsync(MauHoaDonFileParams @params);
        Task<string> CheckAllowUpdateAsync(MauHoaDonViewModel model); // 0 allow
        Task<FileReturn> ExportMauHoaDonAsync(ExportMauHoaDonParams @params);
        Task<List<NhatKyTruyCapViewModel>> GetListNhatKyHoaDonAsync(string id);
        List<ImageParam> GetBackgrounds();
        List<ImageParam> GetBorders();
        Task<List<MauHoaDonTuyChinhChiTietViewModel>> GetTruongMoRongByLoaiHoaDonAsync(LoaiHoaDon loaiHoaDon);
        Task<List<MauHoaDonViewModel>> GetListFromBoKyHieuHoaDonAsync(MauHoaDonParams @params);
        string GetFileToSign();

        Task<MauHoaDonViewModel> InsertAsync(MauHoaDonViewModel model);
        Task<bool> UpdateAsync(MauHoaDonViewModel model);
        Task<bool> UpdateNgayKyAsync(MauHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungTenMauHoaDonAsync(MauHoaDonViewModel model);
        Task<ChiTietMauHoaDon> GetChiTietByMauHoaDon(string mauHoaDonId);
        Task<int> UpdateMauTuyChonChiTietBanHangAsync();
        Task<bool> CheckXoaKyDienTuAsync(string mauHoaDonId);
    }
}
