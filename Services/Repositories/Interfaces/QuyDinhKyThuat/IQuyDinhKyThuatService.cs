﻿using DLL.Entity.QuyDinhKyThuat;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.ViewModels.Params;
using Services.ViewModels.QuyDinhKyThuat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IQuyDinhKyThuatService
    {
        Task<ToKhaiDangKyThongTinViewModel> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai);
        Task<string> GetNoiDungThongDiepXMLChuaKy(string thongDiepId);
        Task<string> GetXmlContentThongDiepAsync(string maThongDiep);
        Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id);
        Task<bool> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> XoaToKhai(string Id);
        Task<bool> GuiToKhai(string XMLUrl, string idThongDiep, string maThongDiep, string mst);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep ConvertToThongDiepTiepNhan(string encodedContent);
        Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhaiDuocChapNhan();
        Task<ThongDiepChungViewModel> GetThongDiepChungById(string Id);
        Task<ThongDiepChungViewModel> InsertThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> UpdateThongDiepChung(ThongDiepChungViewModel model);
        Task<bool> DeleteThongDiepChung(string Id);
        Task<List<ThongDiepChungViewModel>> GetAllThongDiepTraVe(string ThongDiepGocId);
        Task<int> GetLanThuMax(int MaLoaiThongDiep);
        Task<ThongDiepChungViewModel> GetThongDiepByThamChieu(string ThamChieuId);
        Task<List<string>> GetAllListCTS();
        List<LoaiThongDiep> GetListLoaiThongDiepNhan();
        List<LoaiThongDiep> GetListLoaiThongDiepGui();
        Task<int> GetLanGuiMax(ThongDiepChungViewModel td);
        int GetTrangThaiPhanHoiThongDiepNhan(ThongDiepChungViewModel tdn);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep ConvertToThongDiepKUNCQT(string encodedContent);
        ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep ConvertToThongDiepUNCQT(string encodedContent);
        Task<bool> ThongDiepDaGui(ThongDiepChungViewModel td);
        Task<bool> InsertThongDiepNhanAsync(ThongDiepPhanHoiParams @params);
        List<EnumModel> GetTrangThaiGuiPhanHoiTuCQT(int maLoaiThongDiep);
        Task<string> GetXMLDaKy(string ToKhaiId);
        Task<ThongDiepChiTiet> ShowThongDiepFromFileByIdAsync(string id);
        Task<FileReturn> ExportBangKeAsync(ThongDiepChungParams @params);
        Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhai();
        Task<List<ToKhaiForBoKyHieuHoaDonViewModel>> GetListToKhaiFromBoKyHieuHoaDonAsync(ToKhaiParams toKhaiParams);
        Task<bool> AddRangeDangKyUyNhiem(List<DangKyUyNhiemViewModel> listDangKyUyNhiems);
        Task<List<DangKyUyNhiemViewModel>> GetListDangKyUyNhiem(string idToKhai);
        List<DangKyUyNhiemViewModel> GetListTrungKyHieuTrongHeThong(List<DangKyUyNhiemViewModel> data);
        Task<FileReturn> GetLinkFileXml(ThongDiepChungViewModel model, bool signed = false);
        Task<bool> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models);
        Task<bool> DeleteRangeChungThuSo(List<string> Ids);
        List<EnumModel> GetListTimKiemTheoThongDiep();
        Task<ThongDiepChiTiet> GetAllThongDiepTraVeV2(string giaTriTimKiem, string phanLoai);
        Task<int> GetSoBangTongHopDuLieu(BangTongHopParams2 @params);
        Task<KetQuaConvertPDF> ConvertThongDiepToFilePDF(ThongDiepChungViewModel td);
        Task<ThongKeSoLuongThongDiepViewModel> ThongKeSoLuongThongDiepAsync(int trangThaiGuiThongDiep, byte coThongKeSoLuong);
        Task<int> UpdateNgayThongBaoToKhaiAsync();
    }
}
