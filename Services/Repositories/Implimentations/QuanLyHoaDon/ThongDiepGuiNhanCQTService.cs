using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
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

            var query = from hoaDon in _db.HoaDonDienTus
                        where DateTime.Parse(hoaDon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                       DateTime.Parse(hoaDon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate
                        select new HoaDonSaiSotViewModel
                        {
                            HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                            MaCQTCap = "",
                            MauHoaDon = hoaDon.MauSo ?? "",
                            KyHieuHoaDon = hoaDon.KyHieu ?? "",
                            SoHoaDon = hoaDon.SoHoaDon ?? "",
                            NgayLapHoaDon = hoaDon.NgayLap
                        };

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
                }
            }

            //thêm thông điệp gửi hóa đơn sai sót (đây là trường hợp thêm mới)
            model.Id = Guid.NewGuid().ToString();
            model.CreatedDate = model.ModifyDate = model.NgayGui = DateTime.Now;
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
                CreateXMLThongDiepGuiCQT(fullFolder + "/" + tenFile + ".xml", model);
                var tenFileWordPdf = CreateWordAndPdfFile(tenFile, model);

                //var maThongDiep = "V0202029650" + string.Join("", Guid.NewGuid().ToString().Split("-")).ToUpper();
                //var ketqua = GuiThongDiepToiCQTAsync(fullFolder + "/" + tenFile + ".xml", model.MaSoThue, maThongDiep);
                string filePath = assetsFolder + "/" + tenFile + ".xml" + ";" + tenFileWordPdf;
                return new KetQuaLuuThongDiep { Id = model.Id, FilePath = filePath, PhanHoi = true };
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
                var thongDiepGuiCQT = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == id);
                _db.ThongDiepGuiCQTs.Remove(thongDiepGuiCQT);
                return await _db.SaveChangesAsync() > 0;
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
        private bool CreateXMLThongDiepGuiCQT(string xmlFilePath, ThongDiepGuiCQTViewModel model)
        {
            try
            {
                TTChung ttChung = new TTChung
                {
                    PBan = "2.0.0",
                    MNGui = "V0202029650",
                    MNNhan = "TCT",
                    MLTDiep = 300,
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
                        KHMSHDon = item.MauHoaDon,
                        KHHDon = item.KyHieuHoaDon,
                        SHDon = item.SoHoaDon,
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
                    So = model.LoaiThongBao == 2 ? "" : "", //đọc từ thông điệp nhận
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

                List<TBao> listTBao = new List<TBao>();
                listTBao.Add(tBao);

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

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> GateForWebSocket(FileXMLThongDiepGuiParams @params)
        {
            string newSignedXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/xml/signed/");
            var tenFile = Guid.NewGuid().ToString() + ".xml";
            string xmlDeCode = DataHelper.Base64Decode(@params.DataXML);
            File.WriteAllText(newSignedXmlFolder + tenFile, xmlDeCode);

            return false;
        }

        /// <summary>
        /// GuiThongDiepToiCQTAsync gửi dữ liệu tới cơ quan thuế
        /// </summary>
        /// <param name="urlXMLFile"></param>
        /// <param name="maSoThue"></param>
        /// <param name="maThongDiep"></param>
        /// <returns></returns>
        private bool GuiThongDiepToiCQTAsync(string urlXMLFile, string maSoThue, string maThongDiep)
        {
            var data = new GuiThongDiepData
            {
                MST = maSoThue,
                MTDiep = maThongDiep,
                DataXML = urlXMLFile.EncodeFile()
            };
            var ketQua = TextHelper.SendViaSocketConvert("192.168.2.2", 35000, DataHelper.EncodeString(JsonConvert.SerializeObject(data)));

            return ketQua != string.Empty;
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
                    paragraph.Text = item.PhanLoaiHDSaiSot.GetValueOrDefault().ToString();

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
    }
}
