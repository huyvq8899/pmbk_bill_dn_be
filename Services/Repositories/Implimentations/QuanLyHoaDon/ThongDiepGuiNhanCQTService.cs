using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.HoaDonSaiSot;
using Services.Helper.Params.Filter;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.ThongDiepGuiNhanCQT;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class ThongDiepGuiNhanCQTService : IThongDiepGuiNhanCQTService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITVanService _ITVanService;
        private readonly int MaLoaiThongDiep = 300;

        public ThongDiepGuiNhanCQTService(Datacontext db,
            IMapper mp,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            ITVanService tvanService
        )
        {
            _db = db;
            _mp = mp;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _ITVanService = tvanService;
        }

        /// <summary>
        /// GetThongDiepGuiCQTByIdAsync trả về bản ghi thông điệp gửi CQT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ThongDiepGuiCQTViewModel> GetThongDiepGuiCQTByIdAsync(string id)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string fileContainerPath = $"FilesUpload/{databaseName}";

            var queryDetail = _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == id);

            var queryDetailThongBaoRaSoat = from chiTiet in _db.ThongBaoChiTietHoaDonRaSoats
                                            select new ThongBaoChiTietHoaDonRaSoatViewModel
                                            {
                                                Id = chiTiet.Id,
                                                LoaiApDungHD = chiTiet.LoaiApDungHD,
                                                LyDoRaSoat = chiTiet.LyDoRaSoat
                                            };

            var query = from thongDiep in _db.ThongDiepGuiCQTs
                        join raSoat in _db.ThongBaoHoaDonRaSoats on thongDiep.ThongBaoHoaDonRaSoatId equals raSoat.Id into
                        tmpRaSoat
                        from raSoat in tmpRaSoat.DefaultIfEmpty()
                        where thongDiep.Id == id
                        select new ThongDiepGuiCQTViewModel
                        {
                            Id = thongDiep.Id,
                            DaiDienNguoiNopThue = thongDiep.DaiDienNguoiNopThue,
                            DaKyGuiCQT = thongDiep.DaKyGuiCQT,
                            DiaDanh = thongDiep.DiaDanh,
                            FileDinhKem = thongDiep.FileDinhKem,
                            FileXMLDaKy = thongDiep.FileXMLDaKy,
                            LoaiThongBao = (byte)(string.IsNullOrWhiteSpace(thongDiep.ThongBaoHoaDonRaSoatId) ? 1 : 2),
                            MaCoQuanThue = thongDiep.MaCoQuanThue,
                            MaDiaDanh = thongDiep.MaDiaDanh,
                            MaSoThue = thongDiep.MaSoThue,
                            MaThongDiep = thongDiep.MaThongDiep,
                            NgayGui = thongDiep.NgayGui,
                            NgayLap = thongDiep.NgayLap,
                            NguoiNopThue = thongDiep.NguoiNopThue,
                            NTBCCQT = raSoat.NgayThongBao,
                            SoTBCCQT = raSoat.SoThongBaoCuaCQT,
                            TenCoQuanThue = thongDiep.TenCoQuanThue,
                            ThongBaoHoaDonRaSoatId = thongDiep.ThongBaoHoaDonRaSoatId,
                            CreatedDate = thongDiep.CreatedDate,
                            FileContainerPath = fileContainerPath,
                            ThongDiepChiTietGuiCQTs = (from chiTiet in queryDetail
                                                       orderby chiTiet.STT
                                                       select new ThongDiepChiTietGuiCQTViewModel
                                                       {
                                                           Id = chiTiet.Id,
                                                           ThongDiepGuiCQTId = chiTiet.ThongDiepGuiCQTId,
                                                           HoaDonDienTuId = chiTiet.HoaDonDienTuId,
                                                           MaCQTCap = chiTiet.MaCQTCap,
                                                           MauHoaDon = chiTiet.MauHoaDon,
                                                           KyHieuHoaDon = chiTiet.KyHieuHoaDon,
                                                           SoHoaDon = chiTiet.SoHoaDon,
                                                           NgayLapHoaDon = chiTiet.NgayLapHoaDon,
                                                           LoaiApDungHoaDon = chiTiet.LoaiApDungHoaDon,
                                                           PhanLoaiHDSaiSot = chiTiet.PhanLoaiHDSaiSot,
                                                           LyDo = chiTiet.LyDo,
                                                           STT = chiTiet.STT,
                                                           ThongBaoChiTietHDRaSoatId = chiTiet.ThongBaoChiTietHDRaSoatId,
                                                           CreatedDate = chiTiet.CreatedDate,
                                                           CreatedBy = chiTiet.CreatedBy,
                                                           ModifyDate = chiTiet.ModifyDate,
                                                           ModifyBy = chiTiet.ModifyBy,
                                                           LoaiApDungHD = (string.IsNullOrWhiteSpace(chiTiet.ThongBaoChiTietHDRaSoatId) == false) ? queryDetailThongBaoRaSoat.FirstOrDefault(x => x.Id == chiTiet.ThongBaoChiTietHDRaSoatId).LoaiApDungHD : ((byte)0),
                                                           LyDoRaSoat = (string.IsNullOrWhiteSpace(chiTiet.ThongBaoChiTietHDRaSoatId) == false) ? queryDetailThongBaoRaSoat.FirstOrDefault(x => x.Id == chiTiet.ThongBaoChiTietHDRaSoatId).LyDoRaSoat : string.Empty
                                                       }
                                                      ).ToList()
                        };

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetListHoaDonSaiSotAsync trả về danh sách các hóa đơn sai sót
        /// </summary>
        /// <param name="params"></param>
        /// <returns>List<HoaDonSaiSotViewModel></returns>
        public async Task<List<HoaDonSaiSotViewModel>> GetListHoaDonSaiSotAsync(HoaDonSaiSotParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);
            string[] kyHieuHoaDons = null;
            string[] loaiHoaDons = null;

            if (!string.IsNullOrWhiteSpace(@params.KyHieuHoaDon))
            {
                kyHieuHoaDons = @params.KyHieuHoaDon.Split(';').Where(x => x != "").ToArray();
            }

            if (!string.IsNullOrWhiteSpace(@params.LoaiHoaDon))
            {
                //ko tính đến giá trị tất cả
                loaiHoaDons = @params.LoaiHoaDon.Split(';').Where(x => x != "0").ToArray();
            }

            var queryHoaDonXoaBo = _db.HoaDonDienTus.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo
                && x.NgayXoaBo != null
                && DateTime.Parse(x.NgayXoaBo.Value.ToString("yyyy-MM-dd")) >= fromDate
                && DateTime.Parse(x.NgayXoaBo.Value.ToString("yyyy-MM-dd")) <= toDate
                ).Select(y => y.HoaDonDienTuId);

            var queryHoaDonBiDieuChinh = from hoaDon in _db.HoaDonDienTus
                                         join bbdc in _db.BienBanDieuChinhs on hoaDon.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId
                                         join hddc in _db.HoaDonDienTus on bbdc.HoaDonDieuChinhId equals hddc.HoaDonDienTuId
                                         where
                                         DateTime.Parse(hddc.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate
                                         && DateTime.Parse(hddc.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate
                                         select hoaDon.HoaDonDienTuId;

            //tạm thời comment để hiện tất cả
            //var listIdHoaDonSaiSot = queryHoaDonXoaBo.Union(queryHoaDonBiDieuChinh);

            var query = from hoaDon in _db.HoaDonDienTus 
                        join bkhhd in _db.BoKyHieuHoaDons on hoaDon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId 
                        where 
                        //listIdHoaDonSaiSot.Contains(hoaDon.HoaDonDienTuId)
                        //&&
                        (loaiHoaDons == null || (loaiHoaDons != null && loaiHoaDons.Contains(TachKyTuDauTien(hoaDon.MauSo))))
                        && (string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) || (!string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) && @params.HinhThucHoaDon.ToUpper() == TachKyTuDauTien(hoaDon.KyHieu).ToUpper()))
                        && (kyHieuHoaDons == null || (kyHieuHoaDons != null && kyHieuHoaDons.Contains(string.Format("{0}{1}", bkhhd.KyHieuMauSoHoaDon.ToString(), bkhhd.KyHieuHoaDon ?? ""))))

                        orderby hoaDon.MaCuaCQT ascending, hoaDon.MauHoaDon descending, hoaDon.KyHieu descending, hoaDon.SoHoaDon descending
                        select new HoaDonSaiSotViewModel
                        {
                            HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                            MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoaDon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                            MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                            KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                            SoHoaDon = hoaDon.SoHoaDon ?? "",
                            NgayLapHoaDon = hoaDon.NgayLap
                        };

            if (@params.FilterColumns != null)
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                for (int i = 0; i < @params.FilterColumns.Count; i++)
                {
                    var item = @params.FilterColumns[i];
                    if (item.ColKey == "maCQTCap")
                    {
                        query = GenericFilterColumn<HoaDonSaiSotViewModel>.Query(query, x => x.MaCQTCap, item, FilterValueType.String);
                    }
                    if (item.ColKey == "soHoaDon")
                    {
                        query = GenericFilterColumn<HoaDonSaiSotViewModel>.Query(query, x => x.SoHoaDon, item, FilterValueType.String);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(@params.SortKey))
            {
                if (@params.SortKey == "MaCQTCap" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MaCQTCap);
                }
                if (@params.SortKey == "MaCQTCap" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MaCQTCap);
                }

                if (@params.SortKey == "MauHoaDon" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MauHoaDon + x.KyHieuHoaDon);
                }
                if (@params.SortKey == "MauHoaDon" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MauHoaDon + x.KyHieuHoaDon);
                }

                if (@params.SortKey == "SoHoaDon" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoHoaDon);
                }
                if (@params.SortKey == "SoHoaDon" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoHoaDon);
                }

                if (@params.SortKey == "NgayLapHoaDon" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.NgayLapHoaDon);
                }
                if (@params.SortKey == "NgayLapHoaDon" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.NgayLapHoaDon);
                }

            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// GetDanhSachDiaDanhAsync trả về danh sách các địa danh theo Thông tư số 78/2021/TT-BTC 
        /// </summary>
        /// <returns></returns>
        public async Task<List<DiaDanhParam>> GetDanhSachDiaDanhAsync()
        {
            var query = (from diaDanh in _db.DiaDanhs
                         orderby ConvertToNumber(diaDanh.Ma)
                         select new DiaDanhParam
                         {
                             code = diaDanh.Ma,
                             name = diaDanh.Ten
                         }).ToListAsync();
            /*
            string path = _hostingEnvironment.WebRootPath + "\\jsons\\dia-danh.json";
            var list = new List<DiaDanhParam>().Deserialize(path).ToList();
            */

            return await query;
        }

        /// <summary>
        /// InsertThongBaoGuiHoaDonSaiSotAsync thêm bản ghi thông điệp gửi cơ quan thuế
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ThongDiepGuiCQTViewModel</returns>
        public async Task<KetQuaLuuThongDiep> InsertThongBaoGuiHoaDonSaiSotAsync(ThongDiepGuiCQTViewModel model)
        {
            ThongDiepChung thongDiepChung = null;
            if (string.IsNullOrWhiteSpace(model.Id) == false)
            {
                //nếu đã có bản ghi thì xóa trước khi lưu (đây là trường hợp sửa và lưu)
                var thongDiepChiTietGuiCQTs = await _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == model.Id).ToListAsync();
                _db.ThongDiepChiTietGuiCQTs.RemoveRange(thongDiepChiTietGuiCQTs);
                var ketQuaXoa = await _db.SaveChangesAsync();
                if (ketQuaXoa > 0)
                {
                    var thongDiepGuiCQT = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == model.Id);
                    _db.ThongDiepGuiCQTs.Remove(thongDiepGuiCQT);
                    await _db.SaveChangesAsync();

                    //xóa bản ghi ở bảng thông điệp chung
                    thongDiepChung = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == model.Id);
                    _db.ThongDiepChungs.Remove(thongDiepChung);
                    await _db.SaveChangesAsync();

                    //xóa file ở bảng filedata
                    if (thongDiepChung != null)
                    {
                        var fileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == thongDiepChung.ThongDiepChungId);
                        _db.FileDatas.Remove(fileData);
                        await _db.SaveChangesAsync();
                    }
                }
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.Id = Guid.NewGuid().ToString();
                //model.MaThongDiep = "V0200784873" + string.Join("", Guid.NewGuid().ToString().Split("-")).ToUpper();
                model.MaThongDiep = "0200784873" + string.Join("", Guid.NewGuid().ToString().Split("-")).ToUpper();
            }

            //thêm thông điệp gửi hóa đơn sai sót (đây là trường hợp thêm mới)
            model.ModifyDate = model.NgayGui = DateTime.Now;
            model.DaKyGuiCQT = false;
            ThongDiepGuiCQT entity = _mp.Map<ThongDiepGuiCQT>(model);
            await _db.ThongDiepGuiCQTs.AddAsync(entity);
            model.Id = entity.Id;
            var ketQua = await _db.SaveChangesAsync();
            if (ketQua > 0)
            {
                //thêm thông điệp gửi hóa đơn chi tiết bị sai sót
                var STT = 1;
                foreach (var item in model.ThongDiepChiTietGuiCQTs)
                {
                    item.STT = STT;
                    item.Id = Guid.NewGuid().ToString();
                    item.ThongDiepGuiCQTId = model.Id;
                    item.ThongBaoChiTietHDRaSoatId = item.ThongBaoChiTietHDRaSoatId;
                    item.CreatedDate = item.ModifyDate = DateTime.Now;
                    STT += 1;
                }

                List<ThongDiepChiTietGuiCQT> children = _mp.Map<List<ThongDiepChiTietGuiCQT>>(model.ThongDiepChiTietGuiCQTs);
                await _db.ThongDiepChiTietGuiCQTs.AddRangeAsync(children);
                await _db.SaveChangesAsync();

                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                try
                {
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                }
                catch (Exception) { }

                //ghi ra các file XML, Word, PDF sau khi lưu thành công
                var tenFile = "TD-" + Guid.NewGuid().ToString();
                var tDiepXML = await CreateXMLThongDiepGuiCQT(fullFolder + "/" + tenFile + ".xml", model);
                var tenFileWordPdf = await CreateWordAndPdfFile(tenFile, model);
                string fileNames = tenFile + ".xml" + ";" + tenFileWordPdf;

                //cập nhật lại file xml vào trường file đính kèm
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (entityToUpdate != null)
                {
                    entityToUpdate.FileDinhKem = fileNames;
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //khai báo biến kết quả lưu dữ liệu
                var ketQuaLuuDuLieu = new KetQuaLuuThongDiep
                {
                    Id = model.Id,
                    FileNames = fileNames,
                    FileContainerPath = $"FilesUpload/{databaseName}",
                    MaThongDiep = tDiepXML.TTChung.MTDiep,
                    CreatedDate = model.CreatedDate
                };

                //thêm bản ghi vào bảng thông điệp chung để hiển thị ra bảng kê
                await ThemDuLieuVaoBangThongDiepChung(tDiepXML, ketQuaLuuDuLieu, thongDiepChung);

                return ketQuaLuuDuLieu;
            }

            return null;
        }

        /// <summary>
        /// DeleteAsync xóa bản ghi thông báo hóa đơn sai sót
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var thongDiepChiTietGuiCQTs = await _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == id).ToListAsync();
            _db.ThongDiepChiTietGuiCQTs.RemoveRange(thongDiepChiTietGuiCQTs);
            var ketQuaXoa = await _db.SaveChangesAsync();
            if (ketQuaXoa > 0)
            {
                //xóa bản ghi ở bảng ThongDiepGuiCQTs
                var thongDiepGuiCQT = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == id);
                _db.ThongDiepGuiCQTs.Remove(thongDiepGuiCQT);
                var ketQuaXoa2 = await _db.SaveChangesAsync() > 0;

                if (ketQuaXoa2)
                {
                    //xóa bản ghi ở bảng thông điệp chung
                    var thongDiepChung = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == id);
                    if (thongDiepChung != null)
                    {
                        _db.ThongDiepChungs.Remove(thongDiepChung);
                        var ketQuaXoa3 = await _db.SaveChangesAsync() > 0;
                        if (ketQuaXoa3)
                        {
                            //xóa các file word, pdf, xml chưa ký đi
                            await XoaThuMucChuaFileTheoId(id);
                        }

                        return ketQuaXoa3;
                    }
                    else
                    {
                        //xóa các file word, pdf, xml chưa ký đi
                        await XoaThuMucChuaFileTheoId(id);

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// CreateXMLThongDiepGuiCQT tạo file XML chưa được ký để gửi lên cục thuế
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<TDiep> CreateXMLThongDiepGuiCQT(string xmlFilePath, ThongDiepGuiCQTViewModel model)
        {
            try
            {
                TTChung ttChung = new TTChung
                {
                    PBan = "2.0.0",
                    MNGui = "0200784873", // "V0200784873", // "V0202029650",
                    MNNhan = "0105987432", //"TCT",
                    MLTDiep = MaLoaiThongDiep,
                    MTDiep = model.MaThongDiep ?? "",
                    MTDTChieu = model.LoaiThongBao == 2 ? (model.MaTDiepThamChieu ?? "") : "", //đọc từ thông điệp nhận
                    MST = model.MaSoThue ?? "",
                    SLuong = 1
                };

                List<HDon> listHDon = new List<HDon>();
                for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count; i++)
                {
                    var item = model.ThongDiepChiTietGuiCQTs[i];
                    HDon hoaDon = new HDon
                    {
                        STT = i + 1,
                        MCQTCap = item.MaCQTCap ?? "", //giá trị này ở bên hóa đơn điện tử
                        KHMSHDon = item.MauHoaDon ?? "",
                        KHHDon = item.KyHieuHoaDon ?? "",
                        SHDon = item.SoHoaDon ?? "",
                        Ngay = item.NgayLapHoaDon.Value.ToString("yyyy-MM-dd"),
                        LADHDDT = item.LoaiApDungHoaDon,
                        TCTBao = item.PhanLoaiHDSaiSot,
                        LDo = item.LyDo ?? ""
                    };
                    listHDon.Add(hoaDon);
                }

                DLTBao dLTBao = new DLTBao
                {
                    PBan = "2.0.0",
                    MSo = "04/SS-HĐĐT",
                    Ten = "Thông báo hóa đơn điện tử có sai sót",
                    Loai = model.LoaiThongBao,
                    So = model.LoaiThongBao == 2 ? (model.SoTBCCQT ?? "") : "", //đọc từ thông điệp nhận
                    NTBCCQT = model.LoaiThongBao == 2 ? model.NTBCCQT.Value.ToString("yyyy-MM-dd") : "",
                    MCQT = model.MaCoQuanThue,
                    TCQT = model.TenCoQuanThue ?? "",
                    TNNT = model.NguoiNopThue ?? "",
                    MST = model.MaSoThue ?? "",
                    MDVQHNSach = "", //đọc từ thông điệp nhận sau
                    DDanh = model.DiaDanh ?? "",
                    NTBao = model.NgayLap.ToString("yyyy-MM-dd"),
                    DSHDon = listHDon
                };

                DSCKS dSCKS = new DSCKS
                {
                    NNT = ""
                };

                TBao tBao = new TBao
                {
                    DLTBao = dLTBao,
                    DSCKS = dSCKS
                };

                DLieu DLieu = new DLieu
                {
                    TBao = tBao
                };

                TDiep tDiep = new TDiep
                {
                    TTChung = ttChung,
                    DLieu = DLieu
                };

                //sau khi có các dữ liệu trên, thì lưu dữ liệu đó vào file XML
                XmlSerializerNamespaces xmlSerializingNameSpace = new XmlSerializerNamespaces();
                xmlSerializingNameSpace.Add("", "");

                XmlSerializer serialiser = new XmlSerializer(typeof(TDiep));

                using (TextWriter fileStream = new StreamWriter(xmlFilePath))
                {
                    serialiser.Serialize(fileStream, tDiep, xmlSerializingNameSpace);
                }

                string content = File.ReadAllText(xmlFilePath);
                await ThemDuLieuVaoBangFileData(model.Id, content, Path.GetFileName(xmlFilePath));

                return tDiep;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GateForWebSocket sẽ lưu file XML đã ký, và trả về đường dẫn file XML đó
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<string> GateForWebSocket(FileXMLThongDiepGuiParams @params)
        {
            try
            {
                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                try
                {
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                }
                catch (Exception) { }

                var tenFile = "TD-" + Guid.NewGuid().ToString();
                string xmlDeCode = DataHelper.Base64Decode(@params.DataXML);
                var fullDuongDanXML = fullFolder + "/" + tenFile + ".xml";
                File.WriteAllText(fullDuongDanXML, xmlDeCode);

                //lưu tên file vào database
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == @params.ThongDiepGuiCQTId);
                if (entityToUpdate != null)
                {
                    entityToUpdate.FileXMLDaKy = tenFile + ".xml";
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                return entityToUpdate.FileXMLDaKy;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// GuiThongDiepToiCQTAsync gửi dữ liệu tới cơ quan thuế
        /// </summary>
        /// <param name="DuLieuXMLGuiCQTParams"></param>
        /// <returns></returns>
        public async Task<bool> GuiThongDiepToiCQTAsync(DuLieuXMLGuiCQTParams @params)
        {
            try
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);

                //đường dẫn đến file xml đã ký
                var signedXmlFileFolder = fullFolder + "/" + @params.XMLFileName;

                bool ketQua = false;

                // Gửi dữ liệu tới TVan
                var xmlContent = File.ReadAllText(signedXmlFileFolder);
                var responce999 = await _ITVanService.TVANSendData("api/error-invoice/send", xmlContent);
                var thongDiep999 = ConvertXMLDataToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhan999.TDiep>(responce999);
                ketQua = (thongDiep999.DLieu.TBao.TTTNhan == 0);

                //lưu trạng thái đã ký gửi thành công tới cơ quan thuế hay chưa
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == @params.ThongDiepGuiCQTId);
                if (entityToUpdate != null)
                {
                    entityToUpdate.NgayGui = DateTime.Now;
                    entityToUpdate.DaKyGuiCQT = ketQua;
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //lưu thông tin ký gửi vào bảng thông điệp chung
                var entityBangThongDiepChungToUpdate = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == @params.ThongDiepGuiCQTId && x.MaLoaiThongDiep == MaLoaiThongDiep && x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChuaGui);
                if (entityBangThongDiepChungToUpdate != null)
                {
                    //cập nhật dữ liệu xml vào đây
                    var tDiep300 = ConvertXMLFileToObject<TDiep>(signedXmlFileFolder);
                    if (tDiep300 != null)
                    {
                        entityBangThongDiepChungToUpdate.MaThongDiep = tDiep300.TTChung.MTDiep;
                        entityBangThongDiepChungToUpdate.MaThongDiepThamChieu = tDiep300.TTChung.MTDTChieu;
                        entityBangThongDiepChungToUpdate.PhienBan = tDiep300.TTChung.PBan;
                        entityBangThongDiepChungToUpdate.MaNoiGui = tDiep300.TTChung.MNGui;
                        entityBangThongDiepChungToUpdate.MaNoiNhan = tDiep300.TTChung.MNNhan;
                        entityBangThongDiepChungToUpdate.MaSoThue = tDiep300.TTChung.MST;
                    }

                    if (ketQua)
                    {
                        entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiKhongLoi;
                    }
                    else
                    {
                        entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                    }

                    entityBangThongDiepChungToUpdate.NgayGui = DateTime.Now;
                    entityBangThongDiepChungToUpdate.FileXML = @params.XMLFileName;
                    entityBangThongDiepChungToUpdate.NgayThongBao = DateTime.Now;
                    entityBangThongDiepChungToUpdate.MaThongDiepPhanHoi = thongDiep999.TTChung.MTDiep;

                    _db.ThongDiepChungs.Update(entityBangThongDiepChungToUpdate);
                    await _db.SaveChangesAsync();

                    // Cập nhật lại dữ liệu xml đã ký vào bảng filedatas
                    await ThemDuLieuVaoBangFileData(entityBangThongDiepChungToUpdate.ThongDiepChungId, xmlContent, @params.XMLFileName, 1, ketQua);
                }

                //lưu thông điệp nhận 999 từ TVAN
                ThongDiepChung tdc999 = new ThongDiepChung
                {
                    ThongDiepChungId = Guid.NewGuid().ToString(),
                    PhienBan = thongDiep999.TTChung.PBan,
                    MaNoiGui = thongDiep999.TTChung.MNGui,
                    MaNoiNhan = thongDiep999.TTChung.MNNhan,
                    MaLoaiThongDiep = 999,
                    MaThongDiep = thongDiep999.TTChung.MTDiep,
                    MaThongDiepThamChieu = thongDiep999.TTChung.MTDTChieu,
                    MaSoThue = thongDiep999.TTChung.MST,
                    SoLuong = 0,
                    ThongDiepGuiDi = false,
                    TrangThaiGui = (ketQua)? (int)TrangThaiGuiThongDiep.GuiKhongLoi: (int)TrangThaiGuiThongDiep.GuiLoi,
                    HinhThuc = 0,
                    NgayThongBao = DateTime.Now,
                    FileXML = $"TD-{Guid.NewGuid()}.xml"
                };
                await _db.ThongDiepChungs.AddAsync(tdc999);
                await _db.SaveChangesAsync();

                //thêm nội dung file xml 999 vào bảng file data
                await ThemDuLieuVaoBangFileData(tdc999.ThongDiepChungId, responce999, tdc999.FileXML, 1, true, 1);

                return ketQua;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// CreateWordAndPdfFile tạo file Word và file PDF
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<string> CreateWordAndPdfFile(string fileName, ThongDiepGuiCQTViewModel model, bool saveToDatabase = false)
        {
            try
            {
                Document doc = new Document();
                string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/QuanLy/thong-bao-hoa-don-dien-tu-co-sai-sot.docx");
                doc.LoadFromFile(docFolder);

                doc.Replace("<CoQuanThue>", model.TenCoQuanThue, true, true);
                doc.Replace("<TenNguoiNopThue>", model.NguoiNopThue, true, true);
                doc.Replace("<MaSoThue>", model.MaSoThue, true, true);
                doc.Replace("<DiaDanh>", model.DiaDanh ?? "", true, true);
                var ngayThangNam = model.NgayLap;
                doc.Replace("<NgayThangNam>", string.Format("ngày {0} tháng {1} năm {2}", ngayThangNam.Day, ngayThangNam.Month, ngayThangNam.Year), true, true);
                doc.Replace("<DaiDienNguoiNopThue>", model.DaiDienNguoiNopThue, true, true);

                //thao tác với bảng dữ liệu đầu tiên
                var bangDuLieu = doc.Sections[0].Tables[0];
                for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count - 1; i++) //-1 vì đã có sẵn 1 dòng rồi
                {
                    // Clone row
                    TableRow newRow = bangDuLieu.Rows[2].Clone();
                    bangDuLieu.Rows.Insert(2, newRow);
                }

                //điền dữ liệu vào bảng
                TableRow row = null;
                Paragraph paragraph = null;
                ThongDiepChiTietGuiCQTViewModel item = null;
                for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count; i++)
                {
                    item = model.ThongDiepChiTietGuiCQTs[i];

                    row = bangDuLieu.Rows[i + 2]; // +2 vì ko tính 2 dòng đầu
                    paragraph = row.Cells[0].Paragraphs[0];
                    paragraph.Text = (i + 1).ToString();

                    paragraph = row.Cells[1].Paragraphs[0];
                    paragraph.Text = item.MaCQTCap;

                    var mauHoaDon = "";
                    if (item.LoaiApDungHoaDon == 1)
                    {
                        mauHoaDon = (item.MauHoaDon ?? "") + (item.KyHieuHoaDon ?? "");
                    }
                    else
                    {
                        mauHoaDon = (item.MauHoaDon ?? "") + "-" + (item.KyHieuHoaDon ?? "");
                        mauHoaDon = mauHoaDon.Trim('-');
                    }

                    paragraph = row.Cells[2].Paragraphs[0];
                    paragraph.Text = mauHoaDon;

                    paragraph = row.Cells[3].Paragraphs[0];
                    paragraph.Text = item.SoHoaDon;

                    paragraph = row.Cells[4].Paragraphs[0];
                    paragraph.Text = item.NgayLapHoaDon.Value.ToString("dd/MM/yyyy");

                    /*
                    paragraph = row.Cells[5].Paragraphs[0];
                    paragraph.Text = item.LoaiApDungHoaDon.GetDescription();

                    paragraph = row.Cells[6].Paragraphs[0];
                    paragraph.Text = item.PhanLoaiHDSaiSot.GetDescription();
                    */

                    paragraph = row.Cells[5].Paragraphs[0];
                    paragraph.Text = item.LoaiApDungHoaDon.ToString();

                    paragraph = row.Cells[6].Paragraphs[0];
                    paragraph.Text = HienThiPhanLoaiHoaDonSaiSot(item.PhanLoaiHDSaiSot);

                    paragraph = row.Cells[7].Paragraphs[0];
                    paragraph.Text = item.LyDo;
                }

                //nếu lưu nội dung vào các file vật lý (.docx, .xml)
                if (saveToDatabase == false)
                {
                    //tạo thư mục để lưu các file dữ liệu
                    var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                    string assetsFolder = $"FilesUpload/{databaseName}";
                    var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.FILE_ATTACH);
                    try
                    {
                        if (!Directory.Exists(fullFolder))
                        {
                            Directory.CreateDirectory(fullFolder);
                        }
                    }
                    catch (Exception) { }

                    fileName = fileName.Replace("TD", "TB");
                    //lưu file word
                    var duongDanFileWord = fullFolder + "/" + fileName + ".docx";
                    doc.SaveToFile(duongDanFileWord, FileFormat.Docx);

                    //lưu file pdf
                    var duongDanFilePdf = fullFolder + "/" + fileName + ".pdf";
                    doc.SaveToFile(duongDanFilePdf, FileFormat.PDF);

                    doc.Close();

                    await ThemAttachVaoBangFileData(model.Id, duongDanFileWord);
                    await ThemAttachVaoBangFileData(model.Id, duongDanFilePdf);

                    await ThemAttachVaoBangTaiLieuDinhKem(model.Id, duongDanFileWord);
                    await ThemAttachVaoBangTaiLieuDinhKem(model.Id, duongDanFilePdf);

                    return fileName + ".docx" + ";" + fileName + ".pdf";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// HienThiPhanLoaiHoaDonSaiSot sẽ hiển thị chữ phân loại hóa đơn sai sót
        /// </summary>
        /// <param name="GiaTri"></param>
        /// <returns></returns>
        private string HienThiPhanLoaiHoaDonSaiSot(byte? GiaTri)
        {
            string ketQua;
            switch (GiaTri)
            {
                case 1:
                    ketQua = "Hủy";
                    break;
                case 2:
                    ketQua = "Điều chỉnh";
                    break;
                case 3:
                    ketQua = "Thay thế";
                    break;
                case 4:
                    ketQua = "Giải trình";
                    break;
                default:
                    ketQua = "";
                    break;
            }

            return ketQua;
        }

        /// <summary>
        /// XoaThuMucChuaFileTheoId sẽ xóa thư mục chứa các file word, pdf, xml chưa ký theo id bản ghi
        /// </summary>
        /// <param name="id"></param>
        private async Task XoaThuMucChuaFileTheoId(string id)
        {
            //var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
            //string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}";
            //var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            //var fullFolderWordPdf = fullFolder + $"/word_pdf/{id}"; //đường dẫn chứa các file word/pdf
            //var fullFolderUnsignedXML = fullFolder + $"/xml/unsigned/{id}"; //đường dẫn chứa file xml chưa ký
            //Directory.Delete(fullFolderWordPdf, true);
            //Directory.Delete(fullFolderUnsignedXML, true);

            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _IHttpContextAccessor);
            await uploadFile.DeleteInFileDataByRefIdAsync(id, _db);
        }

        #region Phần thêm dữ liệu vào bảng thông điệp chung để hiển thị ra bảng kê thông điệp
        /// <summary>
        /// ThemDuLieuVaoBangThongDiepChung sẽ thêm bản ghi vào bảng thông điệp chung
        /// </summary>
        /// <param name="tDiep"></param>
        /// <param name="ketQuaLuuThongDiep"></param>
        /// <returns></returns>
        private async Task<string> ThemDuLieuVaoBangThongDiepChung(TDiep tDiep, KetQuaLuuThongDiep ketQuaLuuThongDiep, ThongDiepChung thongDiepChung)
        {
            var createdDate = (thongDiepChung != null) ? thongDiepChung.CreatedDate.Value : DateTime.Now;

            ThongDiepChungViewModel model = new ThongDiepChungViewModel
            {
                ThongDiepChungId = thongDiepChung != null ? thongDiepChung.ThongDiepChungId : Guid.NewGuid().ToString(),
                MaThongDiepThamChieu = thongDiepChung != null ? thongDiepChung.MaThongDiepThamChieu : DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                ThongDiepGuiDi = true,
                MaLoaiThongDiep = tDiep.TTChung.MLTDiep,
                HinhThuc = (int)HThuc.ChinhThuc,
                TrangThaiGui = TrangThaiGuiThongDiep.ChuaGui,
                SoLuong = tDiep.TTChung.SLuong,
                NgayGui = null,
                CreatedDate = createdDate,
                ModifyDate = DateTime.Now,
                IdThamChieu = ketQuaLuuThongDiep.Id

                //lúc lưu thì ko cần lưu các trường này, chỉ có lúc ký gửi mới lưu
                //PhienBan = tDiep.TTChung.PBan,
                //MaThongDiep = tDiep.TTChung.MTDiep,
                //MaNoiGui = tDiep.TTChung.MNGui,
                //MaNoiNhan = tDiep.TTChung.MNNhan,
                //MaSoThue = tDiep.TTChung.MST,
            };

            var entity = _mp.Map<ThongDiepChung>(model);
            await _db.ThongDiepChungs.AddAsync(entity);
            var ketQua = await _db.SaveChangesAsync() > 0;

            if (ketQua)
            {
                //update lại ngày tạo CreatedDate
                var entityToUpdate = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == model.ThongDiepChungId);
                if (entityToUpdate != null)
                {
                    entityToUpdate.CreatedDate = createdDate;
                    _db.ThongDiepChungs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //thêm dữ liệu xml vào bảng filedatas
                XmlSerializer serialiser = new XmlSerializer(typeof(TDiep));
                using (var stringWriter = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(stringWriter))
                    {
                        serialiser.Serialize(writer, tDiep);
                        await ThemDuLieuVaoBangFileData(model.ThongDiepChungId, stringWriter.ToString(), null);
                    }
                }

                return model.ThongDiepChungId;
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// GetDSMauKyHieuHoaDon trả về danh sách mẫu ký hiệu hóa đơn
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<string>> GetDSMauKyHieuHoaDon(MauKyHieuHoaDonParams @params)
        {
            string[] loaiHoaDons = null;
            if (!string.IsNullOrWhiteSpace(@params.LoaiHoaDon))
            {
                //ko tính đến giá trị tất cả
                loaiHoaDons = @params.LoaiHoaDon.Split(';').Where(x => x != "0").ToArray();
            }

            var query = _db.HoaDonDienTus.Where(y => (string.IsNullOrWhiteSpace(y.MauSo) == false || string.IsNullOrWhiteSpace(y.KyHieu) == false)
             && (loaiHoaDons == null || (loaiHoaDons != null && loaiHoaDons.Contains(TachKyTuDauTien(y.MauSo))))
             && (string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) || (!string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) && @params.HinhThucHoaDon.ToUpper() == TachKyTuDauTien(y.KyHieu).ToUpper()))
             ).Select(x => string.Format("{0}{1}", x.MauSo ?? "", x.KyHieu ?? "")).Distinct().OrderBy(z => z);

            return await query.ToListAsync();
        }

        //Method này để tách ra ký tự đầu tiên trong chuỗi
        private string TachKyTuDauTien(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.ToCharArray()[0].ToString();
        }

        #region Phần code cho trường hợp thông báo hóa đơn sai sót theo mẫu của CQT
        /// <summary>
        /// GetListHoaDonRaSoatAsync trả về danh sách các bản ghi thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<ThongBaoHoaDonRaSoatViewModel>> GetListHoaDonRaSoatAsync(HoaDonRaSoatParams @params)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}";

            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;

            if (string.IsNullOrWhiteSpace(@params.ThongBaoHoaDonRaSoatId) == true)
            {
                fromDate = DateTime.Parse(@params.FromDate);
                toDate = DateTime.Parse(@params.ToDate);
            }

            var query = from hoaDon in _db.ThongBaoHoaDonRaSoats
                        where
                        (
                            string.IsNullOrWhiteSpace(@params.ThongBaoHoaDonRaSoatId) == false
                            && hoaDon.Id == @params.ThongBaoHoaDonRaSoatId
                        )
                        ||
                        (
                            string.IsNullOrWhiteSpace(@params.ThongBaoHoaDonRaSoatId) == true
                            && DateTime.Parse(hoaDon.NgayThongBao.ToString("yyyy-MM-dd")) >= fromDate
                            && DateTime.Parse(hoaDon.NgayThongBao.ToString("yyyy-MM-dd")) <= toDate
                        )
                        orderby hoaDon.NgayThongBao, hoaDon.SoThongBaoCuaCQT
                        select new ThongBaoHoaDonRaSoatViewModel
                        {
                            Id = hoaDon.Id,
                            SoThongBaoCuaCQT = hoaDon.SoThongBaoCuaCQT,
                            NgayThongBao = hoaDon.NgayThongBao,
                            TenCQTCapTren = hoaDon.TenCQTCapTren,
                            TenCQTRaThongBao = hoaDon.TenCQTRaThongBao,
                            TenNguoiNopThue = hoaDon.TenNguoiNopThue,
                            MaSoThue = hoaDon.MaSoThue,
                            NgayThoiHan = hoaDon.NgayThongBao.AddDays(hoaDon.ThoiHan),
                            Lan = hoaDon.Lan,
                            TinhTrang = hoaDon.NgayThongBao.AddDays(hoaDon.ThoiHan) > DateTime.Now,
                            //nếu tình trạng = true thì là trong hạn, ngược lại là quá hạn
                            FileDinhKem = hoaDon.FileDinhKem,
                            FileUploadPath = assetsFolder
                        };

            if (@params.FilterColumns != null)
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                for (int i = 0; i < @params.FilterColumns.Count; i++)
                {
                    var item = @params.FilterColumns[i];
                    if (item.ColKey == "soThongBaoCuaCQT")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.SoThongBaoCuaCQT, item, FilterValueType.String);
                    }
                    if (item.ColKey == "tenCQTCapTren")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.TenCQTCapTren, item, FilterValueType.String);
                    }
                    if (item.ColKey == "tenCQTRaThongBao")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.TenCQTRaThongBao, item, FilterValueType.String);
                    }
                    if (item.ColKey == "tenNguoiNopThue")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.TenNguoiNopThue, item, FilterValueType.String);
                    }
                    if (item.ColKey == "maSoThue")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.MaSoThue, item, FilterValueType.String);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(@params.SortKey))
            {
                if (@params.SortKey == "SoThongBaoCuaCQT" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoThongBaoCuaCQT);
                }
                if (@params.SortKey == "SoThongBaoCuaCQT" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoThongBaoCuaCQT);
                }

                if (@params.SortKey == "TenCQTCapTren" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenCQTCapTren);
                }
                if (@params.SortKey == "TenCQTCapTren" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenCQTCapTren);
                }

                if (@params.SortKey == "TenCQTRaThongBao" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenCQTRaThongBao);
                }
                if (@params.SortKey == "TenCQTRaThongBao" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenCQTRaThongBao);
                }

                if (@params.SortKey == "NgayThongBao" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.NgayThongBao);
                }
                if (@params.SortKey == "NgayThongBao" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.NgayThongBao);
                }

                if (@params.SortKey == "TenNguoiNopThue" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenNguoiNopThue);
                }
                if (@params.SortKey == "TenNguoiNopThue" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenNguoiNopThue);
                }

                if (@params.SortKey == "MaSoThue" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MaSoThue);
                }
                if (@params.SortKey == "MaSoThue" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MaSoThue);
                }
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// GetListChiTietHoaDonRaSoatAsync sẽ đọc ra danh sách chi tiết thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="thongBaoHoaDonRaSoatId"></param>
        /// <returns></returns>
        public async Task<List<ThongBaoChiTietHoaDonRaSoatViewModel>> GetListChiTietHoaDonRaSoatAsync(string thongBaoHoaDonRaSoatId)
        {
            var query = from hoaDon in _db.ThongBaoChiTietHoaDonRaSoats
                        where hoaDon.ThongBaoHoaDonRaSoatId == thongBaoHoaDonRaSoatId
                        orderby hoaDon.CreatedDate
                        select new ThongBaoChiTietHoaDonRaSoatViewModel
                        {
                            Id = hoaDon.Id,
                            ThongBaoHoaDonRaSoatId = hoaDon.ThongBaoHoaDonRaSoatId,
                            MauHoaDon = hoaDon.MauHoaDon,
                            KyHieuHoaDon = hoaDon.KyHieuHoaDon,
                            SoHoaDon = hoaDon.SoHoaDon,
                            NgayLapHoaDon = hoaDon.NgayLapHoaDon,
                            LoaiApDungHD = hoaDon.LoaiApDungHD,
                            LyDoRaSoat = hoaDon.LyDoRaSoat
                        };

            return await query.ToListAsync();
        }

        /// <summary>
        /// ThemThongBaoHoaDonRaSoat sẽ thêm bản ghi thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="tDiep"></param>
        /// <returns></returns>
        public async Task<string> ThemThongBaoHoaDonRaSoat(ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep tDiep)
        {
            //Id bản ghi thông báo hóa đơn rà soát
            var thongBaoHoaDonRaSoatId = Guid.NewGuid().ToString();

            //Lưu ra file xml nội dung file đã nhận
            var fileNameGuid = "TD-" + Guid.NewGuid().ToString();
            var xmlFileName = fileNameGuid + ".xml";
            var pdfFileName = fileNameGuid + ".pdf";
            string pdfFullFolder = "";
            try
            {
                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}";
                string xmlFullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.XML_SIGNED);
                if (!Directory.Exists(xmlFullFolder))
                {
                    Directory.CreateDirectory(xmlFullFolder);
                }
                pdfFullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.PDF_UNSIGN);
                if (!Directory.Exists(pdfFullFolder))
                {
                    Directory.CreateDirectory(pdfFullFolder);
                }

                //lưu file xml
                XmlSerializerNamespaces xmlSerializingNameSpace = new XmlSerializerNamespaces();
                xmlSerializingNameSpace.Add("", "");

                XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep));

                using (TextWriter fileStream = new StreamWriter(xmlFullFolder + "/" + xmlFileName))
                {
                    serialiser.Serialize(fileStream, tDiep, xmlSerializingNameSpace);
                }

                //thêm dữ liệu xml vào bảng filedatas
                //vì đã thêm xml vào filedatas ở API InsertThongDiepNhanAsync nên bỏ qua ở đây
                /*
                XmlSerializer serialiserRaSoat = new XmlSerializer(typeof(TDiep));
                using (var stringWriter = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(stringWriter))
                    {
                        serialiserRaSoat.Serialize(writer, tDiep);
                        await ThemDuLieuVaoBangFileData(thongBaoHoaDonRaSoatId, stringWriter.ToString(), xmlFileName);
                    }
                }
                */
            }
            catch (Exception)
            {
                xmlFileName = "";
            }

            //Lưu ra file PDF
            CreatePdfFileThongBaoRaSoat(pdfFullFolder + "/" + pdfFileName, tDiep);

            //Lưu dữ liệu vào database
            ThongBaoHoaDonRaSoatViewModel model = new ThongBaoHoaDonRaSoatViewModel
            {
                Id = thongBaoHoaDonRaSoatId,
                SoThongBaoCuaCQT = tDiep.DLieu.TBao.STBao.So,
                NgayThongBao = DateTime.Parse(tDiep.DLieu.TBao.STBao.NTBao),
                TenCQTCapTren = tDiep.DLieu.TBao.DLTBao.TCQTCTren,
                TenCQTRaThongBao = tDiep.DLieu.TBao.DLTBao.TCQT,
                TenNguoiNopThue = tDiep.DLieu.TBao.DLTBao.TNNT,
                MaSoThue = tDiep.DLieu.TBao.DLTBao.MST,
                ThoiHan = tDiep.DLieu.TBao.DLTBao.THan,
                Lan = tDiep.DLieu.TBao.DLTBao.Lan,
                HinhThuc = tDiep.DLieu.TBao.DLTBao.HThuc,
                ChucDanh = tDiep.DLieu.TBao.DLTBao.CDanh,
                FileDinhKem = xmlFileName + ";" + pdfFileName,
                CreatedDate = DateTime.Now,
                CreatedBy = "",
                ModifyDate = DateTime.Now,
                ModifyBy = "",
                MaThongDiep = tDiep.TTChung.MTDiep
            };

            var entity = _mp.Map<ThongBaoHoaDonRaSoat>(model);
            await _db.ThongBaoHoaDonRaSoats.AddAsync(entity);
            var ketQua = await _db.SaveChangesAsync() > 0;

            if (ketQua)
            {
                List<ThongBaoChiTietHoaDonRaSoat> children = new List<ThongBaoChiTietHoaDonRaSoat>();
                //lưu chi tiết thông báo hóa đơn rà soát
                foreach (var item in tDiep.DLieu.TBao.DLTBao.DSHDon)
                {
                    children.Add(
                        new ThongBaoChiTietHoaDonRaSoat
                        {
                            Id = Guid.NewGuid().ToString(),
                            ThongBaoHoaDonRaSoatId = model.Id,
                            MauHoaDon = item.KHMSHDon,
                            KyHieuHoaDon = item.KHHDon,
                            SoHoaDon = item.SHDon,
                            NgayLapHoaDon = DateTime.Parse(item.NLap),
                            LoaiApDungHD = item.LADHDDT,
                            LyDoRaSoat = item.LDo,
                            DaGuiThongBao = false,
                            CreatedDate = DateTime.Now,
                            CreatedBy = "",
                            ModifyDate = DateTime.Now,
                            ModifyBy = ""
                        });
                }
                await _db.ThongBaoChiTietHoaDonRaSoats.AddRangeAsync(children);
                await _db.SaveChangesAsync();

                return model.Id;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// CreatePdfFileThongBaoRaSoat sẽ lưu file pdf của thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="pdfFilePath"></param>
        /// <param name="tDiep"></param>
        /// <returns></returns>
        private string CreatePdfFileThongBaoRaSoat(string pdfFilePath, ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep tDiep)
        {
            try
            {
                Document doc = new Document();
                string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/QuanLy/thong-bao-hoa-don-dien-tu-can-ra-soat.docx");
                doc.LoadFromFile(docFolder);

                doc.Replace("<TenCQTCapTren>", tDiep.DLieu.TBao.DLTBao.TCQTCTren, true, true);
                doc.Replace("<TenCQT>", tDiep.DLieu.TBao.DLTBao.TCQT, true, true);
                doc.Replace("<TenNguoiNopThue>", tDiep.DLieu.TBao.DLTBao.TNNT ?? "", true, true);
                doc.Replace("<MaSoThue>", tDiep.DLieu.TBao.DLTBao.MST ?? "", true, true);

                doc.Replace("<DiaChiLienHe>", tDiep.DLieu.TBao.DLTBao.DCNNT ?? "", true, true);
                doc.Replace("<DiaChiThuDienTu>", tDiep.DLieu.TBao.DLTBao.DCTDTu ?? "", true, true);

                var ngayThangNam = DateTime.Parse(tDiep.DLieu.TBao.STBao.NTBao);
                doc.Replace("<NgayThangNam>", string.Format("Ngày {0} tháng {1} năm {2}", ngayThangNam.Day, ngayThangNam.Month, ngayThangNam.Year), true, true);

                //thao tác với bảng dữ liệu thứ 2 (bảng chi tiết)
                var bangDuLieu = doc.Sections[0].Tables[2];
                for (int i = 0; i < tDiep.DLieu.TBao.DLTBao.DSHDon.Count - 1; i++) //-1 vì đã có sẵn 1 dòng rồi
                {
                    // Clone row
                    TableRow newRow = bangDuLieu.Rows[2].Clone();
                    bangDuLieu.Rows.Insert(2, newRow);
                }

                //điền dữ liệu vào bảng
                TableRow row = null;
                Paragraph paragraph = null;
                ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.HDon item = null;
                for (int i = 0; i < tDiep.DLieu.TBao.DLTBao.DSHDon.Count; i++)
                {
                    item = tDiep.DLieu.TBao.DLTBao.DSHDon[i];

                    row = bangDuLieu.Rows[i + 2]; // +2 vì ko tính 2 dòng đầu
                    paragraph = row.Cells[0].Paragraphs[0];
                    paragraph.Text = (i + 1).ToString();

                    var mauHoaDon = "";
                    if (item.LADHDDT == 1)
                    {
                        mauHoaDon = (item.KHMSHDon ?? "") + (item.KHHDon ?? "");
                    }
                    else
                    {
                        mauHoaDon = (item.KHMSHDon ?? "") + "-" + (item.KHHDon ?? "");
                        mauHoaDon = mauHoaDon.Trim('-');
                    }

                    paragraph = row.Cells[1].Paragraphs[0];
                    paragraph.Text = mauHoaDon;

                    paragraph = row.Cells[2].Paragraphs[0];
                    paragraph.Text = item.SHDon ?? "";

                    paragraph = row.Cells[3].Paragraphs[0];
                    paragraph.Text = DateTime.Parse(item.NLap).ToString("dd/MM/yyyy");

                    paragraph = row.Cells[4].Paragraphs[0];
                    paragraph.Text = item.LADHDDT.ToString();

                    paragraph = row.Cells[5].Paragraphs[0];
                    paragraph.Text = item.LDo;
                }

                //lưu file pdf
                doc.SaveToFile(pdfFilePath, FileFormat.PDF);

                return pdfFilePath;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        /// <summary>
        /// XuLyDuLieuNhanVeTuCQT xử lý dữ liệu nhận về từ cơ quan thuế
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<bool> XuLyDuLieuNhanVeTuCQT(ThongDiepPhanHoiParams @params)
        {
            var ketQua = true;

            switch (@params.MLTDiep)
            {
                case (int)ViewModels.XML.MLTDiep.TDTBHDDTCRSoat: // 302
                    var tDiep302 = DataHelper.ConvertBase64ToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep>(@params.DataXML);

                    ketQua = await ThemThongBaoHoaDonRaSoat(tDiep302) != null;
                    break;
                default:
                    break;
            }

            return ketQua;
        }

        /// <summary>
        /// GetListChungThuSoAsync trả về danh sách các chứng thư số liên quan đến hóa đơn
        /// </summary>
        /// <param name="ThongDiepGuiCQTId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetListChungThuSoAsync(string thongDiepGuiCQTId)
        {
            var query0 = from thongDiep in _db.ThongDiepGuiCQTs
                        join thongDiepChiTiet in _db.ThongDiepChiTietGuiCQTs on thongDiep.Id equals thongDiepChiTiet.ThongDiepGuiCQTId
                        join hoaDon in _db.HoaDonDienTus on thongDiepChiTiet.HoaDonDienTuId equals hoaDon.HoaDonDienTuId
                        join bkhhd in _db.NhatKyXacThucBoKyHieus on hoaDon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                        where thongDiep.Id == thongDiepGuiCQTId && !string.IsNullOrWhiteSpace(bkhhd.SoSeriChungThu)
                        select bkhhd.SoSeriChungThu;
            var listSerial0 = await query0.Distinct().ToListAsync();

            var result = new List<ChungThuSoSuDungViewModel>();
            IQueryable<ToKhaiDangKyThongTinViewModel> query = from tdc in _db.ThongDiepChungs
                                                              join tk in _db.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id
                                                              join hs in _db.HoSoHDDTs on tdc.MaSoThue equals hs.MaSoThue
                                                              where tdc.MaLoaiThongDiep == 100 && tdc.HinhThuc == (int)HThuc.DangKyMoi && tdc.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
                                                              select new ToKhaiDangKyThongTinViewModel
                                                              {
                                                                  Id = tk.Id,
                                                                  NgayTao = tk.NgayTao,
                                                                  IsThemMoi = tk.IsThemMoi,
                                                                  FileXMLChuaKy = tk.FileXMLChuaKy,
                                                                  ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                                                  ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                                                  NhanUyNhiem = tk.NhanUyNhiem,
                                                                  LoaiUyNhiem = tk.LoaiUyNhiem,
                                                                  SignedStatus = tk.SignedStatus,
                                                                  NgayGui = tdc != null ? tdc.NgayGui : null,
                                                                  ModifyDate = tk.ModifyDate,
                                                                  PPTinh = tk.PPTinh
                                                              };
            var toKhai = await query.FirstOrDefaultAsync();
            if (toKhai != null)
            {
                result = toKhai.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung
                    .Select(x => new ChungThuSoSuDungViewModel
                    {
                        TTChuc = x.TTChuc,
                        Seri = x.Seri,
                        TNgay = x.TNgay,
                        DNgay = x.DNgay
                    })
                    .ToList();
            }

            List<string> listSerial = new List<string>();
            listSerial = result.Select(x => x.Seri).ToList();

            return (listSerial.Union(listSerial0)).ToList();
        }


        //Các phương thức private ==============================================================

        //Method này sẽ thêm bản ghi vào bảng FileDatas
        private async Task ThemDuLieuVaoBangFileData(string refId, string data, string fileName, int type = 1, bool isSigned = false, byte bothCheckUpdateAndInsert = 3)
        {
            // Ghi chú: bothCheckUpdateAndInsert = 1 là thêm mới; 2 là update; 3 là vừa kiểm tra update và insert
            if (bothCheckUpdateAndInsert == 3)
            {
                var entityFileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == refId);
                if (entityFileData != null)
                {
                    //nếu đã có bản ghi thì cập nhật
                    entityFileData.Content = data;
                    entityFileData.FileName = fileName;
                    entityFileData.DateTime = DateTime.Now;
                    entityFileData.IsSigned = isSigned;
                    _db.FileDatas.Update(entityFileData);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //thêm bản ghi vào nếu chưa có
                    FileData fileData = new FileData
                    {
                        FileDataId = Guid.NewGuid().ToString(),
                        RefId = refId,
                        Type = type,
                        DateTime = DateTime.Now,
                        Content = data,
                        IsSigned = isSigned,
                        FileName = fileName
                    };
                    await _db.FileDatas.AddAsync(fileData);
                    await _db.SaveChangesAsync();
                }
            }
            else if (bothCheckUpdateAndInsert == 1)
            {
                //thêm bản ghi vào nếu chưa có
                FileData fileData = new FileData
                {
                    FileDataId = Guid.NewGuid().ToString(),
                    RefId = refId,
                    Type = type,
                    DateTime = DateTime.Now,
                    Content = data,
                    IsSigned = isSigned,
                    FileName = fileName
                };
                await _db.FileDatas.AddAsync(fileData);
                await _db.SaveChangesAsync();
            }
        }

        private async Task ThemAttachVaoBangTaiLieuDinhKem(string refId, string path)
        {
            TaiLieuDinhKem taiLieuDinhKem = new TaiLieuDinhKem
            {
                TaiLieuDinhKemId = Guid.NewGuid().ToString(),
                LoaiNghiepVu = RefType.ThongDiepGuiNhanCQT,
                NghiepVuId = refId,
                TenGoc = Path.GetFileName(path),
                TenGuid = Path.GetFileName(path),
                CreatedDate = DateTime.Now,
                Status = true
            };
            await _db.TaiLieuDinhKems.AddAsync(taiLieuDinhKem);
            await _db.SaveChangesAsync();
        }

        private async Task ThemAttachVaoBangFileData(string refId, string path)
        {
            FileData fileData = new FileData
            {
                FileDataId = Guid.NewGuid().ToString(),
                RefId = refId,
                Type = 4,
                DateTime = DateTime.Now,
                Binary = File.ReadAllBytes(path),
                FileName = Path.GetFileName(path),
            };
            await _db.FileDatas.AddAsync(fileData);
            await _db.SaveChangesAsync();
        }

        //Method này để chuyển nội dung file XML sang popco
        private T ConvertXMLFileToObject<T>(string xmlFilePath)
        {
            XDocument xd = XDocument.Load(xmlFilePath);
            if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT").Remove();
            }

            XmlSerializer serialiser = new XmlSerializer(typeof(T));
            var model = (T)serialiser.Deserialize(xd.CreateReader());
            return model;
        }

        //Method này để chuyển nội dung chuỗi sang popco
        private T ConvertXMLDataToObject<T>(string xmlData)
        {
            XDocument xd = XDocument.Parse(xmlData);
            if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT").Remove();
            }

            XmlSerializer serialiser = new XmlSerializer(typeof(T));
            var model = (T)serialiser.Deserialize(xd.CreateReader());
            return model;
        }

        //Method này để đọc thông tin người dùng đã đăng nhập
        private string GetUserId()
        {
            string nameIdentifier = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return nameIdentifier;
        }

        //Hàm này để convert chuỗi sang số
        private int ConvertToNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            var giaTri = 0;
            var isNumeric = int.TryParse(value, out giaTri);
            if (isNumeric)
            {
                return giaTri;
            }
            else
            {
                return 0;
            }
        }
    }
}
