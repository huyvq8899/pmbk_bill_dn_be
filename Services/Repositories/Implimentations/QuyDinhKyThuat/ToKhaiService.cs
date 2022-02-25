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

        public ToKhaiService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMapper mp,
            IXMLInvoiceService xmlInvoiceService,
            ITVanService ITVanService,
            IQuyDinhKyThuatService quyDinhKyThuatService
            )
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mp = mp;
            _xmlInvoiceService = xmlInvoiceService;
            _ITVanService = ITVanService;
            _quyDinhKyThuatService = quyDinhKyThuatService;
        }

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
                            PPTinh = tk.PPTinh
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
                            PPTinh = x.First().PPTinh
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
        /// <summary>
        /// Lưu các dòng chứng thư số trên tờ khai
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models)
        {
            var listValidToAdd = models.Where(x => _dataContext.ChungThuSoSuDungs.Any(o => string.IsNullOrEmpty(o.Id))).ToList();
            var listValidToEdit = models.Where(x => _dataContext.ChungThuSoSuDungs.Any(o => !string.IsNullOrEmpty(o.Id))).ToList();
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

        #endregion
    }
}
