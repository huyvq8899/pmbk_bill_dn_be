using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuyDinhKyThuat;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Constants;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Services.Helper;
using Services.Helper.XmlModel;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.Repositories.Interfaces.QuanLy;
using DLL.Enums;
using Services.ViewModels.XML;
using System.Xml.Linq;
using TDiep103 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep;
using TBao103 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.TBao;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;


namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class ToKhaiService : IToKhaiService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mp;
        private readonly IXMLInvoiceService _xmlInvoiceService;
        private readonly ITVanService _ITVanService;
        private readonly IQuyDinhKyThuatService _quyDinhKyThuatService;
        private readonly IBoKyHieuHoaDonService _boKyHieuHoaDonService;
        private readonly IHttpContextAccessor _IHttpContextAccessor;

        public ToKhaiService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMapper mp,
            IXMLInvoiceService xmlInvoiceService,
            ITVanService ITVanService,
            IQuyDinhKyThuatService quyDinhKyThuatService,
            IBoKyHieuHoaDonService boKyHieuHoaDonService,
            IHttpContextAccessor IHttpContextAccessor
            )
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mp = mp;
            _xmlInvoiceService = xmlInvoiceService;
            _ITVanService = ITVanService;
            _quyDinhKyThuatService = quyDinhKyThuatService;
            _boKyHieuHoaDonService = boKyHieuHoaDonService;
            _IHttpContextAccessor = IHttpContextAccessor;
        }

        #region Check
        /// <summary>
        /// Kiểm tra xem hệ thống đã có tờ khai (thông điệp 100) được cơ quan thuế chấp nhận chưa
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsHasToKhaiDuocChapNhan()
        {
            return await _dataContext.ThongDiepChungs.AnyAsync(x => x.MaLoaiThongDiep == (int)MLTDiep.TDGToKhai && x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan);
        }

        #endregion

        #region CRUD
        /// <summary>
        /// Tạo mới tờ khai
        /// </summary>
        /// <param name="tKhai"></param>
        /// <returns></returns>
        public async Task<ToKhaiDangKyThongTinViewModel> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai)
        {
            var _entity = _mp.Map<ToKhaiDangKyThongTin>(tKhai);
            //var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}";
            //var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            var fullXmlName = Path.Combine(_hostingEnvironment.WebRootPath, tKhai.FileXMLChuaKy);

            if (!File.Exists(fullXmlName))
            {
                var bin = await _dataContext.FileDatas.Where(x => x.RefId == tKhai.Id && x.Type == 1 && x.IsSigned == false).Select(x => x.Binary).FirstOrDefaultAsync();
                if(bin != null)
                {
                    File.WriteAllBytes(fullXmlName, bin);
                }
                else
                {
                    var files = tKhai.FileXMLChuaKy.Split("\\");
                    if (tKhai.ToKhaiKhongUyNhiem != null)
                    {
                        _xmlInvoiceService.CreateFileXML(tKhai.ToKhaiKhongUyNhiem, ManageFolderPath.XML_UNSIGN, files[files.Length - 1]);
                    }
                    else
                    {
                        _xmlInvoiceService.CreateFileXML(tKhai.ToKhaiUyNhiem, ManageFolderPath.XML_UNSIGN, files[files.Length - 1]);
                    }
                }
            }
            //string xmlDeCode = DataHelper.Base64Decode(fullXmlName);
            byte[] byteXML = File.ReadAllBytes(fullXmlName);
            string strXML = File.ReadAllText(fullXmlName);
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayTao = DateTime.Now;
            _entity.ModifyDate = DateTime.Now;
            await _dataContext.ToKhaiDangKyThongTins.AddAsync(_entity);

            var fileData = new FileData
            {
                RefId = _entity.Id,
                Type = 1,
                Binary = byteXML,
                Content = strXML,
                DateTime = DateTime.Now,
                FileName = Path.GetFileName(fullXmlName),
                IsSigned = false
            };
            await _dataContext.FileDatas.AddAsync(fileData);

            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return _mp.Map<ToKhaiDangKyThongTinViewModel>(_entity);
            }
            else return null;
        }

        /// <summary>
        /// Sửa tờ khai 
        /// </summary>
        /// <param name="tKhai"></param>
        /// <returns></returns>
        public async Task<bool> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai)
        {
            var _entity = _mp.Map<ToKhaiDangKyThongTin>(tKhai);
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var fullXmlName = Path.Combine(fullXmlFolder, fileName);

            if (!File.Exists(fullXmlName))
            {
                if (tKhai.ToKhaiKhongUyNhiem != null)
                {
                    _xmlInvoiceService.CreateFileXML(tKhai.ToKhaiKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
                }
                else
                {
                    _xmlInvoiceService.CreateFileXML(tKhai.ToKhaiUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
                }
            }
            //string xmlDeCode = DataHelper.Base64Decode(fullXmlName);
            byte[] byteXML = Encoding.UTF8.GetBytes(fullXmlName);
            string strXML = File.ReadAllText(fullXmlName);

            var fileData = await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == _entity.Id);
            if (fileData != null)
            {
                fileData.Content = strXML;
                fileData.Binary = byteXML;
                fileData.FileName = Path.GetFileName(fullXmlName);
            }

            _dataContext.ToKhaiDangKyThongTins.Update(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Xóa tờ khai
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> XoaToKhai(string Id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteInFileDataByRefIdAsync(Id, _dataContext);

            var entity = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == Id);
            _dataContext.ToKhaiDangKyThongTins.Remove(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region Lấy dữ liệu tờ khai
        /// <summary>
        /// Lấy thông tin tờ khai với id bất kỳ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id)
        {
            var query = from tk in _dataContext.ToKhaiDangKyThongTins
                        join tdc in _dataContext.ThongDiepChungs on tk.Id equals tdc.IdThamChieu into tmpTDC
                        from tdc in tmpTDC.DefaultIfEmpty()
                        where tk.Id == Id
                        select new ToKhaiDangKyThongTinViewModel
                        {
                            Id = tk.Id,
                            NgayTao = tk.NgayTao,
                            IsThemMoi = tk.IsThemMoi,
                            FileXMLChuaKy = tk.FileXMLChuaKy,
                            ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_dataContext.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                            ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_dataContext.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                            NhanUyNhiem = tk.NhanUyNhiem,
                            LoaiUyNhiem = tk.LoaiUyNhiem,
                            SignedStatus = tk.SignedStatus,
                            NgayGui = tdc != null ? tdc.NgayGui : null,
                            ModifyDate = tk.ModifyDate,
                            PPTinh = tk.PPTinh,
                            CreatedDate = tk.CreatedDate,
                            CreatedBy = tk.CreatedBy
                        };

            query = query.GroupBy(x => new { x.Id })
                        .Select(x => new ToKhaiDangKyThongTinViewModel
                        {
                            Id = x.Key.Id,
                            NgayTao = x.First().NgayTao,
                            IsThemMoi = x.First().IsThemMoi,
                            NhanUyNhiem = x.First().NhanUyNhiem,
                            LoaiUyNhiem = x.First().LoaiUyNhiem,
                            SignedStatus = x.First().SignedStatus,
                            FileXMLChuaKy = x.First().FileXMLChuaKy,
                            ToKhaiKhongUyNhiem = x.First().ToKhaiKhongUyNhiem,
                            ToKhaiUyNhiem = x.First().ToKhaiUyNhiem,
                            NgayGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.NgayGui).FirstOrDefault(),
                            ModifyDate = x.First().ModifyDate,
                            PPTinh = x.First().PPTinh,
                            CreatedBy = x.First().CreatedBy,
                            CreatedDate = x.First().CreatedDate
                        });

            var data = await query.FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }
        #endregion

        #region Ký và gửi
        /// <summary>
        /// Lưu dữ liệu ký vào fileDatas
        /// </summary>
        /// <param name="kTKhai"></param>
        /// <returns></returns>
        public async Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai)
        {
            var _entityTDiep = await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == kTKhai.IdToKhai);
            var _entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == kTKhai.IdToKhai);
            var base64EncodedBytes = System.Convert.FromBase64String(kTKhai.Content);
            byte[] byteXML = Encoding.UTF8.GetBytes(kTKhai.Content);
            string dataXML = Encoding.UTF8.GetString(base64EncodedBytes);
            var ttChung = Helper.XmlHelper.GetTTChungFromStringXML(dataXML);

            if (_entityTDiep.MaThongDiep != ttChung.MTDiep)
            {
                _entityTDiep.MaThongDiep = ttChung.MTDiep;
                _dataContext.Update(_entityTDiep);
            }

            if (!_entityTK.SignedStatus)
            {
                _entityTK.SignedStatus = true;
                _dataContext.Update(_entityTK);
            }

            var fileData = new FileData
            {
                RefId = _entityTDiep.ThongDiepChungId,
                Type = 1,
                IsSigned = true,
                DateTime = DateTime.Now,
                Content = dataXML,
                Binary = byteXML,
            };

            var entity = await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == _entityTDiep.ThongDiepChungId);
            if (entity != null) _dataContext.FileDatas.Remove(entity);
            await _dataContext.FileDatas.AddAsync(fileData);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Gửi tờ khai đến TVAN
        /// </summary>
        /// <param name="idThongDiep"></param>
        /// <param name="maThongDiep"></param>
        /// <param name="mst"></param>
        /// <returns></returns>
        public async Task<bool> GuiToKhai(string idThongDiep, string maThongDiep, string mst)
        {
            var entity = await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == idThongDiep);
            var entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == entity.IdThamChieu);
            var dataXML = (await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == idThongDiep)).Content;
            var data = new GuiThongDiepData
            {
                MST = mst,
                MTDiep = maThongDiep,
                DataXML = dataXML
            };

            // Send to TVAN
            string strContent = await _ITVanService.TVANSendData("api/register/send", data.DataXML);

            if (string.IsNullOrEmpty(strContent))
            {
                return false;
            }

            var @params = new ThongDiepPhanHoiParams()
            {
                ThongDiepId = idThongDiep,
                DataXML = strContent,
                MST = mst,
                MLTDiep = 999,
                MTDiep = maThongDiep
            };

            return await _quyDinhKyThuatService.InsertThongDiepNhanAsync(@params);
        }

        /// <summary>
        /// Kiểm tra tờ khai thay đổi thông tin trước khi ký và gửi
        /// </summary>
        /// <param name="toKhaiId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> CheckToKhaiThayDoiThongTinTruocKhiKyVaGuiAsync(string toKhaiId)
        {
            string result = null;

            // get file data của tờ khai hiện tại
            var fileData = await _dataContext.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.RefId == toKhaiId);
            if (fileData != null)
            {
                // get tờ khai mới nhất
                var toKhaiMoiNhatDuocChapNhan = await _boKyHieuHoaDonService.GetThongTinTuToKhaiMoiNhatAsync();

                if (toKhaiMoiNhatDuocChapNhan?.ToKhaiForBoKyHieuHoaDon?.ToKhaiKhongUyNhiem != null)
                {
                    var toKhaiMoiNhat = toKhaiMoiNhatDuocChapNhan?.ToKhaiForBoKyHieuHoaDon?.ToKhaiKhongUyNhiem;
                    var toKhaiHienTai = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(fileData.Content);

                    // lấy thông tin hình thức hóa đơn và loại hóa đơn không sử dụng
                    List<string> loaiHoaDonThayDoi = new List<string>();
                    List<string> hinhThucHoaDonThayDoi = new List<string>();

                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.HTHDon.CMa != toKhaiHienTai.DLTKhai.NDTKhai.HTHDon.CMa )
                    {
                        if (toKhaiMoiNhat.DLTKhai.NDTKhai.HTHDon.CMa == 1)
                        {
                            hinhThucHoaDonThayDoi.Add("Có mã của cơ quan thuế");

                        }
                    }
                    if( toKhaiMoiNhat.DLTKhai.NDTKhai.HTHDon.CMTMTTien != toKhaiHienTai.DLTKhai.NDTKhai.HTHDon.CMTMTTien)
                    {
                        if (toKhaiMoiNhat.DLTKhai.NDTKhai.HTHDon.CMTMTTien == 1)
                        {
                            hinhThucHoaDonThayDoi.Add("Có mã của cơ quan thuế khởi tạo từ máy tính tiền");
                        }
                    }
                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.HTHDon.KCMa != toKhaiHienTai.DLTKhai.NDTKhai.HTHDon.KCMa)
                    {
                        if(toKhaiMoiNhat.DLTKhai.NDTKhai.HTHDon.KCMa == 1)
                        {
                            hinhThucHoaDonThayDoi.Add("Không có mã của cơ quan thuế");
                        }
                    }
                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1 && toKhaiHienTai.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 0)
                    {
                        loaiHoaDonThayDoi.Add(LoaiHoaDon.HoaDonGTGT.GetDescription());
                    }
                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1 && toKhaiHienTai.DLTKhai.NDTKhai.LHDSDung.HDBHang == 0)
                    {
                        loaiHoaDonThayDoi.Add(LoaiHoaDon.HoaDonBanHang.GetDescription());
                    }
                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1 && toKhaiHienTai.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 0)
                    {
                        loaiHoaDonThayDoi.Add(LoaiHoaDon.HoaDonBanTaiSanCong.GetDescription());
                    }
                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1 && toKhaiHienTai.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 0)
                    {
                        loaiHoaDonThayDoi.Add(LoaiHoaDon.HoaDonBanHangDuTruQuocGia.GetDescription());
                    }
                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1 && toKhaiHienTai.DLTKhai.NDTKhai.LHDSDung.HDKhac == 0)
                    {
                        loaiHoaDonThayDoi.Add(LoaiHoaDon.CacLoaiHoaDonKhac.GetDescription());
                    }
                    if (toKhaiMoiNhat.DLTKhai.NDTKhai.LHDSDung.CTu == 1 && toKhaiHienTai.DLTKhai.NDTKhai.LHDSDung.CTu == 0)
                    {
                        loaiHoaDonThayDoi.Add(LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD.GetDescription());
                    }

                    if (hinhThucHoaDonThayDoi.Any() || loaiHoaDonThayDoi.Any())
                    {
                        result = "<div>Bạn đang thực hiện thay đổi thông tin sử dụng hóa đơn điện tử. Hệ thống nhận thấy bạn không tiếp tục đăng ký sử dụng:</div>";
                        if (hinhThucHoaDonThayDoi.Any())
                        {
                            result += $"<div>- Hình thức hóa đơn: <b>{string.Join(", ", hinhThucHoaDonThayDoi)}</b></div>";
                        }
                        if (loaiHoaDonThayDoi.Any())
                        {
                            result += $"<div>- Loại hóa đơn: <b>{string.Join(", ", loaiHoaDonThayDoi)}</b></div>";
                        }
                        result += "<br/><div>Kể từ thời điểm CQT chấp nhận tờ khai thay đổi thông tin này, hệ thống sẽ không cho phép lập hóa đơn có hình thức hóa đơn và loại hóa đơn đã không tiếp tục đăng ký sử dụng.</div><br/>" +
                            "<div>Bạn có chắc chắn muốn tiếp tục <b class=\"cssred\"> Ký và Gửi</b> tờ khai này không?</div>";
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Lấy thông tin chi tiết thông điệp 103 từ file xml tải lên
        /// </summary>
        /// <param name="formData">file xml tải lên từ người dùng</param>
        /// <returns>chi tiết thông điệp 103</returns>
        public async Task<TBao103> GetThongDiep103FromFile(IFormFile file)
        {
            try
            {
                TBao103 result = null;
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var strXML = stream.ReadToEnd();
                    stream.BaseStream.Position = 0;
                    XDocument xd = XDocument.Load(stream);
                    // convert content xml to object
                    // do có file xml có nhiều chữ ký số nên tách ra các if khác nhau, ko đưa vào elseif
                    // vì có trường hợp 301 cho vào elseif đã ko Remove được hết các chữ ký số đi nên ko ra nội dung

                    bool IsThongDiep = xd.XPathSelectElement("TDiep")   != null;

                    if (IsThongDiep)
                    {
                        if (xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/CQT") != null)
                        {
                            xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/CQT").Remove();
                        }

                        if (xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/NNT") != null)
                        {
                            xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/NNT").Remove();
                        }

                        if (xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/TTCQT") != null)
                        {
                            xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/TTCQT").Remove();
                        }

                        if (xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/CCKSKhac") != null)
                        {
                            xd.XPathSelectElement("TDiep/DLieu/TBao/DSCKS/CCKSKhac").Remove();
                        }
                        XmlSerializer serialiser = new XmlSerializer(typeof(TDiep103));
                        var model = (TDiep103)serialiser.Deserialize(xd.CreateReader());
                        result = model.DLieu.TBao;

                    } else {
                        if (xd.XPathSelectElement("TBao/DSCKS/CQT") != null)
                        {
                            xd.XPathSelectElement("TBao/DSCKS/CQT").Remove();
                        }

                        if (xd.XPathSelectElement("TBao/DSCKS/NNT") != null)
                        {
                            xd.XPathSelectElement("TBao/DSCKS/NNT").Remove();
                        }

                        if (xd.XPathSelectElement("TBao/DSCKS/TTCQT") != null)
                        {
                            xd.XPathSelectElement("TBao/DSCKS/TTCQT").Remove();
                        }

                        if (xd.XPathSelectElement("TBao/DSCKS/CCKSKhac") != null)
                        {
                            xd.XPathSelectElement("TBao/DSCKS/CCKSKhac").Remove();
                        }

                        XmlSerializer serialiser = new XmlSerializer(typeof(TBao103));
                        var model = (TBao103)serialiser.Deserialize(xd.CreateReader());
                        result = model;

                    }

                    return result;


                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Xác nhận tờ khai 01 dựa trên thông điệp 103 lấy từ file người dùng tải lên
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public async Task<bool> XacNhanToKhai01(string idThongDiep, string mst, IFormFile file)
        {
            try
            {
                var xmlToKhai = await _dataContext.FileDatas.Where(x => x.RefId == idThongDiep && x.IsSigned == true).Select(x => x.Content).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(xmlToKhai))
                {
                    await _dataContext.AddTransferLog(xmlToKhai, 1, idThongDiep);
                }

                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var dataXML = stream.ReadToEnd();

                    var @params = new ThongDiepPhanHoiParams()
                    {
                        ThongDiepId = idThongDiep,
                        DataXML = dataXML,
                        MST = mst,
                        MLTDiep = 103,
                        SelfConfirm = true
                    };

                    var res = await _quyDinhKyThuatService.InsertThongDiepNhanAsync(@params);
                    return res;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

            #endregion

            #region Đăng ký ủy nhiệm
            /// <summary>
            /// Lưu các dòng đăng ký ủy nhiệm trên tờ khai 101
            /// </summary>
            /// <param name="listDangKyUyNhiems"></param>
            /// <returns></returns>
            public async Task<bool> AddRangeDangKyUyNhiem(List<DangKyUyNhiemViewModel> listDangKyUyNhiems)
        {
            var toKhaiId = listDangKyUyNhiems.FirstOrDefault()?.IdToKhai;
            if (!string.IsNullOrEmpty(toKhaiId))
            {
                var oldEntities = await _dataContext.DangKyUyNhiems.Where(x => x.IdToKhai == toKhaiId).ToListAsync();
                if (oldEntities.Any())
                {
                    _dataContext.DangKyUyNhiems.RemoveRange(oldEntities);
                }

                var entities = _mp.Map<List<DangKyUyNhiem>>(listDangKyUyNhiems);
                var idx = 1;
                foreach (var entity in entities)
                {
                    entity.Id = Guid.NewGuid().ToString();
                    entity.STT = idx;
                    idx++;
                }
                await _dataContext.DangKyUyNhiems.AddRangeAsync(entities);
                return await _dataContext.SaveChangesAsync() == listDangKyUyNhiems.Count;
            }

            return false;
        }

        /// <summary>
        /// Lấy các dòng đăng ký ủy nhiệm để hiển thị trên giao diện tờ khai
        /// </summary>
        /// <param name="idToKhai"></param>
        /// <returns></returns>
        public async Task<List<DangKyUyNhiemViewModel>> GetListDangKyUyNhiem(string idToKhai)
        {
            IQueryable<DangKyUyNhiemViewModel> query = from dkun in _dataContext.DangKyUyNhiems
                                                       where dkun.IdToKhai == idToKhai
                                                       select new DangKyUyNhiemViewModel
                                                       {
                                                           Id = dkun.Id,
                                                           STT = dkun.STT,
                                                           IdToKhai = dkun.IdToKhai,
                                                           TLHDon = dkun.TLHDon,
                                                           KHMSHDon = dkun.KHMSHDon,
                                                           KHHDon = dkun.KHHDon,
                                                           KyHieu1 = dkun.KyHieu1,
                                                           KyHieu23 = dkun.KyHieu23,
                                                           KyHieu4 = dkun.KyHieu4,
                                                           KyHieu56 = dkun.KyHieu56,
                                                           MST = dkun.MST,
                                                           MDich = dkun.MDich,
                                                           TTChuc = dkun.TTChuc,
                                                           TNgay = dkun.TNgay,
                                                           DNgay = dkun.DNgay,
                                                           PThuc = dkun.PThuc,
                                                           TenPThuc = ((HTTToan)dkun.PThuc).GetDescription()
                                                       };

            return await query.ToListAsync();
        }
        #endregion

        #region Chứng thư số
        public bool VerifyFile103(IFormFile file)
        {
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var strXML = stream.ReadToEnd();
                    stream.BaseStream.Position = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.LoadXml(strXML);
                    SignedXml signedXml = new SignedXml(xmlDoc);
                    XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");
                    XmlNodeList certificates = xmlDoc.GetElementsByTagName("X509Certificate");

                    bool passes = false;
                    for (var i = 0; i < nodeList.Count; i++)
                    {
                        X509Certificate2 dcert2 = new X509Certificate2(Convert.FromBase64String(certificates[i].InnerText));
                        signedXml.LoadXml((XmlElement)nodeList[i]);
                        passes = signedXml.CheckSignature(dcert2, true);
                        if (passes == false)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Lưu các dòng chứng thư số trên tờ khai
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models)
        {
            var listValidToAdd = models.Where(x => string.IsNullOrEmpty(x.Id)).ToList();
            var listValidToEdit = models.Where(x => !string.IsNullOrEmpty(x.Id)).ToList();
            var entities = _mp.Map<List<ChungThuSoSuDung>>(listValidToAdd);
            foreach (var item in entities)
            {
                item.Id = Guid.NewGuid().ToString();
            }
            await _dataContext.ChungThuSoSuDungs.AddRangeAsync(entities);
            var entitiesEdit = _mp.Map<List<ChungThuSoSuDung>>(listValidToEdit);
            foreach (var item in entitiesEdit)
            {
                item.Id = _dataContext.ChungThuSoSuDungs.Where(x => x.Seri == item.Seri && x.HThuc == item.HThuc).Select(x => x.Id).FirstOrDefault();
            }
            _dataContext.UpdateRange(entitiesEdit);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Xóa chứng thư số
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRangeChungThuSo(List<string> Ids)
        {
            var entities = await _dataContext.ChungThuSoSuDungs.Where(x => Ids.Contains(x.Id)).ToListAsync();
            _dataContext.ChungThuSoSuDungs.RemoveRange(entities);
            return await _dataContext.SaveChangesAsync() == entities.Count;
        }

        public async Task<int> CheckThongDiep103(IFormFile file)
        {
            TBao103 DuLieuFormFile103 = await GetThongDiep103FromFile(file);
            if(DuLieuFormFile103 != null)
            {
                var ss = await SoSanhNgayKyVoiToKhaiGanNhat(file);
                if (!ss)
                {
                    return 1;
                }

                var hoSoHDDT = await _dataContext.HoSoHDDTs.FirstOrDefaultAsync();
                if (hoSoHDDT.MaSoThue != DuLieuFormFile103.DLTBao.MST)
                {
                    return 2;
                }
            } else
            {

            }


            return 0;
        }
        /// <summary>
        /// Insert file 103 and update mcqt in hosohđt
        /// </summary>
        /// <param name="files"></param>
        /// <param name="MaCQT"></param>
        /// <returns></returns>
        public async Task<string> UpdateFile103AndMCQTToHoSo(IList<IFormFile> files, string MaCQT)
        {
            bool result = false;
            FileData fileData = new FileData();
            var upload = new UploadFile(_hostingEnvironment, _IHttpContextAccessor);
            var fileUrl = upload.InsertFileAttach(out string name, files);
            var entity = await _dataContext.HoSoHDDTs.FirstOrDefaultAsync();


            if (!String.IsNullOrEmpty(fileUrl))
            {
                fileData = new FileData
                {
                    RefId = Guid.NewGuid().ToString(),
                    Type = 2,
                    DateTime = DateTime.Now,
                    FileName = name,
                    IsSigned = true,
                    Binary = File.ReadAllBytes(fileUrl),
                    Content = File.ReadAllText(fileUrl),
                    FileDataId = Guid.NewGuid().ToString(),
                };
                await _dataContext.FileDatas.AddAsync(fileData);
                entity.IsNhapKhauMaCQT = true;
                entity.FileId103Import = fileData.FileDataId;
                entity.MaCuaCQTToKhaiChapNhan = MaCQT;
                 result = await _dataContext.SaveChangesAsync() > 0;
            }
            return result == true ? fileData.FileDataId : "";
        }
        /// <summary>
        /// DownLoad File 103 Imported
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<string> GetFile103Imported(string fileId)
        {
            var taiLieuDinhKem = await _dataContext.FileDatas.Where(x => x.FileDataId == fileId).FirstOrDefaultAsync();
            if (taiLieuDinhKem != null)
            {
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach";
                string folder = _hostingEnvironment.WebRootPath + rootFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

              string  filePath = Path.Combine(folder, taiLieuDinhKem.FileName);
                if (!File.Exists(filePath))
                {
                    File.WriteAllBytes(filePath, taiLieuDinhKem.Binary);
                }
            }
            return GetLinkFile(taiLieuDinhKem.FileName);
        }

        private async Task<bool> SoSanhNgayKyVoiToKhaiGanNhat(IFormFile file)
        {
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var strXML = stream.ReadToEnd();
                    stream.BaseStream.Position = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.LoadXml(strXML);
                    XmlNodeList nodeList = xmlDoc.GetElementsByTagName("SigningTime");

                    var newest103InSys = await _dataContext.ThongDiepChungs.Where(x => x.MaLoaiThongDiep == (int)MLTDiep.TBCNToKhai && x.TrangThaiGui != (int)TrangThaiGuiThongDiep.TuChapNhan).OrderByDescending(x => x.NgayThongBao).FirstOrDefaultAsync();
                    var fileDataNewest103 = await _dataContext.FileDatas.Where(x => x.RefId == newest103InSys.ThongDiepChungId).FirstOrDefaultAsync();
                    if (fileDataNewest103 != null)
                    {
                        XmlDocument xmlDocThamChieu = new XmlDocument();
                        xmlDocThamChieu.PreserveWhitespace = true;
                        xmlDocThamChieu.LoadXml(fileDataNewest103.Content);
                        XmlNodeList nodeListThamChieu = xmlDocThamChieu.GetElementsByTagName("SigningTime");

                        if (DateTime.Parse(nodeList[1].InnerText) <= DateTime.Parse(nodeListThamChieu[1].InnerText))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion

        private string GetLinkFile(string link)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var filename = $@"FilesUpload/{databaseName}/FileAttach/" + link;
            string url;
            if (_IHttpContextAccessor.HttpContext.Request.IsHttps)
            {
                url = "https://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            url = url + "/" + filename;
            return url;
        }
    }
}
