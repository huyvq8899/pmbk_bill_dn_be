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
using Services.Helper;
using Services.Helper.Params.Filter;
using Services.Helper.XmlModel;
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
        private readonly IQuyDinhKyThuatService _IQuyDinhKyThuatService;
        private readonly int MaLoaiThongDiep = 300;

        public ThongDiepGuiNhanCQTService(Datacontext db,
            IMapper mp,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IQuyDinhKyThuatService quyDinhKyThuatService
        )
        {
            _db = db;
            _mp = mp;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _IQuyDinhKyThuatService = quyDinhKyThuatService;
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
            if (!string.IsNullOrWhiteSpace(@params.KyHieuHoaDon))
            {
                kyHieuHoaDons = @params.KyHieuHoaDon.Split(',');
            }

            var query = from hoaDon in _db.HoaDonDienTus
                        where DateTime.Parse(hoaDon.NgayLap.Value.ToString("yyyy-MM-dd")) >= fromDate
                        && DateTime.Parse(hoaDon.NgayLap.Value.ToString("yyyy-MM-dd")) <= toDate
                        && (@params.LoaiHoaDon == 0 || (@params.LoaiHoaDon != 0 && hoaDon.LoaiHoaDon == @params.LoaiHoaDon))
                        && (@params.HinhThucHoaDon == 0 || (@params.HinhThucHoaDon == 1 && !string.IsNullOrWhiteSpace(hoaDon.MaCuaCQT)) || (@params.HinhThucHoaDon == 2 && string.IsNullOrWhiteSpace(hoaDon.MaCuaCQT)))
                        && (kyHieuHoaDons == null || (kyHieuHoaDons != null && kyHieuHoaDons.Contains(hoaDon.MauHoaDonId))) 
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
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/unsigned";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }

                //ghi ra các file XML, Word, PDF sau khi lưu thành công
                var tenFile = Guid.NewGuid().ToString();
                var tDiepXML = CreateXMLThongDiepGuiCQT(fullFolder + "/" + tenFile + ".xml", model);
                var tenFileWordPdf = CreateWordAndPdfFile(tenFile, model);
                string filePath = assetsFolder + "/" + tenFile + ".xml" + ";" + tenFileWordPdf;

                //cập nhật lại file xml vào trường file đính kèm
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (entityToUpdate != null)
                {
                    entityToUpdate.FileDinhKem = filePath;
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //khai báo biến kết quả lưu dữ liệu
                var ketQuaLuuDuLieu = new KetQuaLuuThongDiep { Id = model.Id, FilePath = filePath, MaThongDiep = tDiepXML.TTChung.MTDiep, CreatedDate = model.CreatedDate };

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
                        return await _db.SaveChangesAsync() > 0;
                    }
                    else
                    {
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
                    MNGui = "V0202029650",
                    MNNhan = "TCT",
                    MLTDiep = MaLoaiThongDiep,
                    MTDiep = "V0202029650" + string.Join("", Guid.NewGuid().ToString().Split("-")).ToUpper(),
                    MTDTChieu = "", //đọc từ thông điệp nhận
                    MST = model.MaSoThue,
                    SLuong = model.ThongDiepChiTietGuiCQTs.Count
                };

                List<HDon> listHDon = new List<HDon>();
                for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count; i++)
                {
                    var item = model.ThongDiepChiTietGuiCQTs[i];
                    HDon hoaDon = new HDon
                    {
                        STT = i + 1,
                        MCQTCap = item.MaCQTCap, //đọc từ thông điệp nhận (đọc từ API đọc danh sách hóa đơn sai sót)
                        KHMSHDon = item.MauHoaDon ?? "",
                        KHHDon = item.KyHieuHoaDon ?? "",
                        SHDon = item.SoHoaDon ?? "",
                        Ngay = item.NgayLapHoaDon.Value.ToString("yyyy-MM-dd"),
                        LADHĐĐT = item.LoaiApDungHoaDon.GetValueOrDefault(),
                        TCTBao = item.PhanLoaiHDSaiSot.GetValueOrDefault(),
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
                    So = model.LoaiThongBao == 2 ? "SoThongBaoCuaCQT" : "", //đọc từ thông điệp nhận
                    NTBCCQT = model.LoaiThongBao == 2 ? model.NgayLap.ToString("yyyy-MM-dd") : "",
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
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/signed";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }

                var tenFile = Guid.NewGuid().ToString();
                string xmlDeCode = DataHelper.Base64Decode(@params.DataXML);
                var fullDuongDanXML = fullFolder + "/" + tenFile + ".xml";
                File.WriteAllText(fullDuongDanXML, xmlDeCode);

                //lưu đường dẫn file vào database
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == @params.ThongDiepGuiCQTId);
                if (entityToUpdate != null)
                {
                    entityToUpdate.FileXMLDaKy = assetsFolder + "/" + tenFile + ".xml";
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
                var signedXmlFileFolder = Path.Combine(_hostingEnvironment.WebRootPath, @params.XMLFilePath);
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
                    //đọc ra tên file xml đã ký
                    var xmlFile = @params.XMLFilePath.Split('/');

                    entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiToKhaiDenCQT.DaGui;
                    entityBangThongDiepChungToUpdate.NgayGui = DateTime.Now;
                    entityBangThongDiepChungToUpdate.FileXML = xmlFile[xmlFile.Length-1];
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
                    if (item.LoaiApDungHoaDon.GetValueOrDefault() == 1)
                    {
                        mauHoaDon = item.MauHoaDon ?? "" + item.KyHieuHoaDon ?? "";
                    }
                    else
                    {
                        mauHoaDon = item.MauHoaDon ?? "" + "-" + item.KyHieuHoaDon ?? "";
                        mauHoaDon = mauHoaDon.Trim('-');
                    }

                    paragraph = row.Cells[2].Paragraphs[0];
                    paragraph.Text = mauHoaDon;

                    paragraph = row.Cells[3].Paragraphs[0];
                    paragraph.Text = item.SoHoaDon;

                    paragraph = row.Cells[4].Paragraphs[0];
                    paragraph.Text = item.NgayLapHoaDon.Value.ToString("dd/MM/yyyy");

                    paragraph = row.Cells[5].Paragraphs[0];
                    paragraph.Text = item.LoaiApDungHoaDon.GetValueOrDefault().ToString();

                    paragraph = row.Cells[6].Paragraphs[0];
                    paragraph.Text = HienThiPhanLoaiHoaDonSaiSot(item.PhanLoaiHDSaiSot.GetValueOrDefault());

                    paragraph = row.Cells[7].Paragraphs[0];
                    paragraph.Text = item.LyDo;
                }

                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);

                //lưu file word
                var tenFileWord = fullFolder + "/" + fileName + ".docx";
                doc.SaveToFile(tenFileWord, FileFormat.Docx);

                //lưu file pdf
                var tenFilePdf = fullFolder + "/" + fileName + ".pdf";
                doc.SaveToFile(tenFilePdf, FileFormat.PDF);

                return assetsFolder + "/" + fileName + ".docx" + ";" + assetsFolder + "/" + fileName + ".pdf";
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
                PhienBan = tDiep.TTChung.PBan,
                MaThongDiep = tDiep.TTChung.MTDiep,
                ThongDiepGuiDi = true,
                MaLoaiThongDiep = tDiep.TTChung.MLTDiep,
                HinhThuc = (int)HThuc.ChinhThuc,
                TrangThaiGui = TrangThaiGuiToKhaiDenCQT.ChuaGui,
                MaNoiGui = tDiep.TTChung.MNGui,
                MaNoiNhan = tDiep.TTChung.MNNhan,
                MaSoThue = tDiep.TTChung.MST,
                SoLuong = tDiep.TTChung.SLuong,
                NgayGui = null,
                CreatedDate = thongDiepChung != null ? thongDiepChung.CreatedDate: DateTime.Now,
                ModifyDate = DateTime.Now,
                IdThamChieu = ketQuaLuuThongDiep.Id
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
    }
}
