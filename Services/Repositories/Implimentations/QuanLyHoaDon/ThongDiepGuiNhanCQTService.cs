using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper;
using Services.Helper.HoaDonSaiSot;
using Services.Helper.Params.DanhMuc;
using Services.Helper.Params.Filter;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces.QuanLyHoaDon;
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
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class ThongDiepGuiNhanCQTService : IThongDiepGuiNhanCQTService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly int MaLoaiThongDiep = 300;

        public ThongDiepGuiNhanCQTService(Datacontext db,
            IMapper mp,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment hostingEnvironment
        )
        {
            _db = db;
            _mp = mp;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
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
                && DateTime.Parse(x.NgayXoaBo.Value.ToString("yyyy-MM-dd")) >= fromDate
                && DateTime.Parse(x.NgayXoaBo.Value.ToString("yyyy-MM-dd")) <= toDate).Select(y => y.HoaDonDienTuId);

            var queryHoaDonBiDieuChinh = from hoaDon in _db.HoaDonDienTus
                                         join bbdc in _db.BienBanDieuChinhs on hoaDon.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId
                                         join hddc in _db.HoaDonDienTus on bbdc.HoaDonDieuChinhId equals hddc.HoaDonDienTuId
                                         where
                                         DateTime.Parse(hddc.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate
                                         && DateTime.Parse(hddc.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate
                                         select hoaDon.HoaDonDienTuId;
            var listIdHoaDonSaiSot = queryHoaDonXoaBo.Union(queryHoaDonBiDieuChinh);

            var query = from hoaDon in _db.HoaDonDienTus
                        where
                        /*
                        DateTime.Parse(hoaDon.NgayLap.Value.ToString("yyyy-MM-dd")) >= fromDate 
                        && DateTime.Parse(hoaDon.NgayLap.Value.ToString("yyyy-MM-dd")) <= toDate 
                        */

                        listIdHoaDonSaiSot.Contains(hoaDon.HoaDonDienTuId) 
                        && (loaiHoaDons == null || (loaiHoaDons != null && loaiHoaDons.Contains(TachKyTuDauTien(hoaDon.MauSo)))) 
                        && (string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) || (!string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) && @params.HinhThucHoaDon.ToUpper() == TachKyTuDauTien(hoaDon.KyHieu).ToUpper())) 
                        && (kyHieuHoaDons == null || (kyHieuHoaDons != null && kyHieuHoaDons.Contains(string.Format("{0}{1}", hoaDon.MauSo ?? "", hoaDon.KyHieu ?? "")))) 
                        /*
                        && (hoaDon.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo || (hoaDon.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe && hoaDon.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh) || (hoaDon.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh && hoaDon.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh))
                        */

                        orderby hoaDon.MaCuaCQT ascending, hoaDon.MauHoaDon descending, hoaDon.KyHieu descending, hoaDon.SoHoaDon descending 
                        select new HoaDonSaiSotViewModel
                        {
                            HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                            MaCQTCap = hoaDon.MaCuaCQT ?? "",
                            MauHoaDon = hoaDon.MauSo ?? "",
                            KyHieuHoaDon = hoaDon.KyHieu ?? "",
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
        /// GetDanhSachDiaDanh trả về danh sách các địa danh theo Thông tư số 78/2021/TT-BTC 
        /// </summary>
        /// <returns></returns>
        public List<DiaDanhParam> GetDanhSachDiaDanh()
        {
            string path = _hostingEnvironment.WebRootPath + "\\jsons\\dia-danh.json";
            var list = new List<DiaDanhParam>().Deserialize(path).ToList();
            return list;
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
                }
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.Id = Guid.NewGuid().ToString();
                model.MaThongDiep = "V0200784873" + string.Join("", Guid.NewGuid().ToString().Split("-")).ToUpper();
            }

            //thêm thông điệp gửi hóa đơn sai sót (đây là trường hợp thêm mới)
            model.ModifyDate = model.NgayGui = DateTime.Now;
            model.DaKyGuiCQT = false;
            ThongDiepGuiCQT entity = _mp.Map<ThongDiepGuiCQT>(model);
            await _db.ThongDiepGuiCQTs.AddAsync(entity);
            var ketQua = await _db.SaveChangesAsync();
            if (ketQua > 0)
            {
                //thêm thông điệp gửi hóa đơn chi tiết bị sai sót
                foreach (var item in model.ThongDiepChiTietGuiCQTs)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.ThongDiepGuiCQTId = model.Id;
                    item.CreatedDate = item.ModifyDate = DateTime.Now;
                }

                List<ThongDiepChiTietGuiCQT> children = _mp.Map<List<ThongDiepChiTietGuiCQT>>(model.ThongDiepChiTietGuiCQTs);
                await _db.ThongDiepChiTietGuiCQTs.AddRangeAsync(children);
                await _db.SaveChangesAsync();

                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/unsigned/{model.Id}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                try
                {
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                    else
                    {
                        //xóa các file đã có trong đó đi để lưu các file khác vào
                        DirectoryInfo di = new DirectoryInfo(fullFolder);
                        FileInfo[] files = di.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }
                    }
                }
                catch (Exception) {}

                //ghi ra các file XML, Word, PDF sau khi lưu thành công
                var tenFile = Guid.NewGuid().ToString();
                var tDiepXML = CreateXMLThongDiepGuiCQT(fullFolder + "/" + tenFile + ".xml", model);
                var tenFileWordPdf = CreateWordAndPdfFile(tenFile, model);
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
                var ketQuaLuuDuLieu = new KetQuaLuuThongDiep { Id = model.Id, FileNames = fileNames,
                    FileContainerPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}",
                    MaThongDiep = tDiepXML.TTChung.MTDiep, CreatedDate = model.CreatedDate };

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
                            XoaThuMucChuaFileTheoId(id);
                        }

                        return ketQuaXoa3;
                    }
                    else
                    {
                        //xóa các file word, pdf, xml chưa ký đi
                        XoaThuMucChuaFileTheoId(id);

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
        private TDiep CreateXMLThongDiepGuiCQT(string xmlFilePath, ThongDiepGuiCQTViewModel model)
        {
            try
            {
                TTChung ttChung = new TTChung
                {
                    PBan = "2.0.0",
                    MNGui = "V0200784873", // "V0202029650",
                    MNNhan = "TCT",
                    MLTDiep = MaLoaiThongDiep,
                    MTDiep = model.MaThongDiep ?? "",
                    MTDTChieu = model.LoaiThongBao == 2 ? (model.MaTDiepThamChieu ?? "" ) : "", //đọc từ thông điệp nhận
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
                    NTBCCQT = model.LoaiThongBao == 2 ? model.NTBCCQT.Value.ToString("yyyy-MM-dd"): "",
                    MCQT = "", //đọc sau khi bên thuế cung cấp giá trị
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
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/signed/{@params.ThongDiepGuiCQTId}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                try
                {
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                    else
                    {
                        //xóa các file đã có trong đó đi để lưu các file khác vào
                        DirectoryInfo di = new DirectoryInfo(fullFolder);
                        FileInfo[] files = di.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }
                    }
                }
                catch (Exception) { }

                var tenFile = Guid.NewGuid().ToString();
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
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/signed/{@params.ThongDiepGuiCQTId}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);

                //đường dẫn đến file xml đã ký
                var signedXmlFileFolder = fullFolder + "/" + @params.XMLFileName;

                var data = new GuiThongDiepData
                {
                    MST = @params.MaSoThue,
                    MTDiep = @params.MaThongDiep,
                    DataXML = signedXmlFileFolder.EncodeFile()
                };

                var phanHoi = TextHelper.SendViaSocketConvert("192.168.2.2", 35000, DataHelper.EncodeString(JsonConvert.SerializeObject(data)));
                var ketQua = phanHoi != string.Empty;

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
                var entityBangThongDiepChungToUpdate = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == @params.ThongDiepGuiCQTId && x.MaLoaiThongDiep == MaLoaiThongDiep && x.TrangThaiGui == (int)TrangThaiGuiToKhaiDenCQT.ChuaGui);
                if (entityBangThongDiepChungToUpdate != null)
                {
                    entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiToKhaiDenCQT.ChoPhanHoi;
                    entityBangThongDiepChungToUpdate.NgayGui = DateTime.Now;
                    entityBangThongDiepChungToUpdate.FileXML = @params.XMLFileName;
                    _db.ThongDiepChungs.Update(entityBangThongDiepChungToUpdate);
                    await _db.SaveChangesAsync();
                }

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
        private string CreateWordAndPdfFile(string fileName, ThongDiepGuiCQTViewModel model)
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

                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/word_pdf/{model.Id}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                try
                {
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                    else
                    {
                        //xóa các file đã có trong đó đi để lưu các file khác vào
                        DirectoryInfo di = new DirectoryInfo(fullFolder);
                        FileInfo[] files = di.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }
                    }
                }
                catch (Exception) { }

                //lưu file word
                var tenFileWord = fullFolder + "/" + fileName + ".docx";
                doc.SaveToFile(tenFileWord, FileFormat.Docx);

                //lưu file pdf
                var tenFilePdf = fullFolder + "/" + fileName + ".pdf";
                doc.SaveToFile(tenFilePdf, FileFormat.PDF);

                return fileName + ".docx" + ";" + fileName + ".pdf";
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
        private void XoaThuMucChuaFileTheoId(string id)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}";
            var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            var fullFolderWordPdf = fullFolder + $"/word_pdf/{id}"; //đường dẫn chứa các file word/pdf
            var fullFolderUnsignedXML = fullFolder + $"/xml/unsigned/{id}"; //đường dẫn chứa file xml chưa ký
            Directory.Delete(fullFolderWordPdf, true);
            Directory.Delete(fullFolderUnsignedXML, true);
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
            ThongDiepChungViewModel model = new ThongDiepChungViewModel
            {
                ThongDiepChungId = thongDiepChung != null ? thongDiepChung.ThongDiepChungId : Guid.NewGuid().ToString(),
                MaThongDiepThamChieu = DateTime.Now.ToString("yyyy/MM/DD HH:MM:SS"),
                ThongDiepGuiDi = true,
                MaLoaiThongDiep = tDiep.TTChung.MLTDiep,
                HinhThuc = (int)HThuc.ChinhThuc,
                TrangThaiGui = TrangThaiGuiToKhaiDenCQT.ChuaGui,
                SoLuong = tDiep.TTChung.SLuong,
                NgayGui = null,
                CreatedDate = thongDiepChung != null ? thongDiepChung.CreatedDate: DateTime.Now,
                ModifyDate = DateTime.Now,
                IdThamChieu = ketQuaLuuThongDiep.Id

                //lúc lưu thì ko cần lưu các trường này, chỉ có lúc gửi thành công mới lưu
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
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/hoadonrasoat/";

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
                            FileUploadPath = assetsFolder + hoaDon.Id
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
            var fileNameGuid = Guid.NewGuid().ToString();
            var xmlFileName = fileNameGuid + ".xml";
            var pdfFileName = fileNameGuid + ".pdf";
            string fullFolder = "";
            try
            {
                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/hoadonrasoat/{thongBaoHoaDonRaSoatId}";
                fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }
                else
                {
                    //xóa các file đã có trong đó đi để lưu các file khác vào
                    DirectoryInfo di = new DirectoryInfo(fullFolder);
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        file.Delete();
                    }
                }

                //lưu file xml
                XmlSerializerNamespaces xmlSerializingNameSpace = new XmlSerializerNamespaces();
                xmlSerializingNameSpace.Add("", "");

                XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep));

                using (TextWriter fileStream = new StreamWriter(fullFolder + "/" + xmlFileName))
                {
                    serialiser.Serialize(fileStream, tDiep, xmlSerializingNameSpace);
                }
            }
            catch(Exception)
            {
                xmlFileName = "";
            }

            //Lưu ra file PDF
            CreatePdfFileThongBaoRaSoat(fullFolder + "/" + pdfFileName, tDiep);

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
    }
}
