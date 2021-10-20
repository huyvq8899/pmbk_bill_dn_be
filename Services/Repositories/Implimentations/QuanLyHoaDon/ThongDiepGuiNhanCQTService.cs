using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.QuanLyHoaDon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
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
                            MauHoaDon = hoaDon.MauSo,
                            KyHieuHoaDon = hoaDon.KyHieu,
                            SoHoaDon = hoaDon.SoHoaDon,
                            NgayLapHoaDon = hoaDon.NgayLap
                        };

            return await query.ToListAsync();
        }

        /// <summary>
        /// InsertThongBaoGuiHoaDonSaiSotAsync thêm bản ghi thông điệp gửi cơ quan thuế
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ThongDiepGuiCQTViewModel</returns>
        public async Task<string> InsertThongBaoGuiHoaDonSaiSotAsync(ThongDiepGuiCQTViewModel model)
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

                return model.Id;
            }

            return null;
        }

        private async Task<bool> CreateXMLThongDiepGuiCQT(string xmlFilePath, ThongDiepGuiCQTViewModel model)
        {
            var taxCode = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;
            var hoSoHDDT = await _db.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
            if (hoSoHDDT == null)
            {
                hoSoHDDT = new DLL.Entity.DanhMuc.HoSoHDDT { MaSoThue = taxCode };
            }

            ViewModels.XML.ThongDiepGuiNhanCQT.TTChung ttChung = new ViewModels.XML.ThongDiepGuiNhanCQT.TTChung
            {
                PBan = "2.0.0",
                MNGui = "",
                MNNhan = "",
                MLTDiep = "",
                MTDTChieu = "",
                MST = "",
                SLuong = 0
            };

            List<ViewModels.XML.ThongDiepGuiNhanCQT.HDon> listHDon = new List<ViewModels.XML.ThongDiepGuiNhanCQT.HDon>();
            for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count; i++)
            {
                var item = model.ThongDiepChiTietGuiCQTs[i];
                ViewModels.XML.ThongDiepGuiNhanCQT.HDon hoaDon = new ViewModels.XML.ThongDiepGuiNhanCQT.HDon
                {
                    STT = (i + 1),
                    MCQTCap = "",
                    KHMSHDon = item.MauHoaDon,
                    KHHDon = item.KyHieuHoaDon,
                    SHDon = item.SoHoaDon,
                    Ngay = item.NgayLapHoaDon.Value.ToString("dd/MM/yyyy"),
                    LADHĐĐT = "",
                    TCTBao = "",
                    LDo = ""
                };
                listHDon.Add(hoaDon);
            }

            ViewModels.XML.ThongDiepGuiNhanCQT.DSHDon dSHDon = new ViewModels.XML.ThongDiepGuiNhanCQT.DSHDon
            {
                HDon = listHDon
            };

            ViewModels.XML.ThongDiepGuiNhanCQT.DLTBao dLTBao = new ViewModels.XML.ThongDiepGuiNhanCQT.DLTBao
            {
                PBan = "",
                MSo = "",
                Ten = "",
                Loai = "",
                So = "",
                NTBCCQT = "",
                MCQT = "",
                TCQT = "",
                TNNT = "",
                MST = "",
                MDVQHNSach = "",
                DDanh = "",
                NTBao = "",
                DSHDon = dSHDon
            };

            ViewModels.XML.ThongDiepGuiNhanCQT.DSCKS dSCKS = new ViewModels.XML.ThongDiepGuiNhanCQT.DSCKS
            {
                NNT = ""
            };

            ViewModels.XML.ThongDiepGuiNhanCQT.TBao tBao = new ViewModels.XML.ThongDiepGuiNhanCQT.TBao
            {
                DLTBao = dLTBao,
                DSCKS = dSCKS
            };

            List<ViewModels.XML.ThongDiepGuiNhanCQT.TBao> listTBao = new List<ViewModels.XML.ThongDiepGuiNhanCQT.TBao>();
            listTBao.Add(tBao);

            ViewModels.XML.ThongDiepGuiNhanCQT.DLieu DLieu = new ViewModels.XML.ThongDiepGuiNhanCQT.DLieu
            {
                TBao = listTBao
            };

            ViewModels.XML.ThongDiepGuiNhanCQT.TDiep tDiep = new ViewModels.XML.ThongDiepGuiNhanCQT.TDiep
            {
                TTChung = ttChung,
                DLieu = DLieu
            };

            //sau khi có các dữ liệu trên, thì lưu dữ liệu vào file XML
            XmlSerializerNamespaces xmlSerializingNameSpace = new XmlSerializerNamespaces();
            xmlSerializingNameSpace.Add("", "");

            XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.ThongDiepGuiNhanCQT.TDiep));

            using (TextWriter fileStream = new StreamWriter(xmlFilePath))
            {
                serialiser.Serialize(fileStream, tDiep, xmlSerializingNameSpace);
            }

            return false;
        }
    }
}
