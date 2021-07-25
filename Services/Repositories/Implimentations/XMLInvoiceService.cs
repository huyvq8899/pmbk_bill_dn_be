using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations
{
    public class XMLInvoiceService : IXMLInvoiceService
    {
        private readonly Datacontext _dataContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public XMLInvoiceService(Datacontext dataContext, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            try
            {
                string linkSearch = _configuration["Config:LinkSearchInvoice"];
                var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

                LoaiTien loaiTien = await _dataContext.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == model.LoaiTienId);
                var hoSoHDDT = await _dataContext.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
                if (hoSoHDDT == null)
                {
                    hoSoHDDT = new HoSoHDDT { MaSoThue = taxCode };
                }

                //TTChung
                TTChung _ttChung = new TTChung
                {
                    PBan = "1.1.0",
                    THDon = ((LoaiHoaDon)model.LoaiHoaDon).GetDescription(),
                    KHMSHDon = model.MauSo,
                    KHHDon = model.KyHieu,
                    SHDon = model.SoHoaDon,
                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                    DVTTe = loaiTien != null ? loaiTien.Ma : string.Empty,
                    TGia = model.TyGia.Value,
                    TTNCC = "",
                    DDTCuu = linkSearch,
                    MTCuu = linkSearch,
                    HTTToan = 1,
                    THTTTKhac = string.Empty
                };
                #region NDHDon
                //NBan

                NBan _nBan = new NBan
                {
                    Ten = hoSoHDDT.TenDonVi,
                    MST = hoSoHDDT.MaSoThue,
                    DChi = hoSoHDDT.DiaChi,
                    SDThoai = hoSoHDDT.SoDienThoaiLienHe,
                    DCTDTu = hoSoHDDT.EmailLienHe,
                    STKNHang = hoSoHDDT.SoTaiKhoanNganHang,
                    TNHang = hoSoHDDT.TenNganHang,
                    Fax = hoSoHDDT.Fax,
                    Website = hoSoHDDT.Website,
                };
                //NMua
                NMua _nMua = new NMua
                {
                    Ten = model.TenKhachHang,
                    MST = model.MaSoThue,
                    DChi = model.DiaChi,
                    SDThoai = model.SoDienThoaiNguoiMuaHang,
                    DCTDTu = model.EmailNguoiMuaHang,
                    HVTNMHang = model.HoTenNguoiMuaHang,
                    STKNHang = model.SoTaiKhoanNganHang,
                };

                //HHDVus
                List<HHDVu> _dSHHDVu = new List<HHDVu>();
                int _STT = 1;
                foreach (var item in model.HoaDonChiTiets)
                {
                    HHDVu _hhdv = new HHDVu
                    {
                        STT = _STT,
                        TChat = 1,
                        THHDVu = item.TenHang,
                        DVTinh = item.DonViTinh != null ? item.DonViTinh.Ten : string.Empty,
                        SLuong = item.SoLuong.Value,
                        DGia = item.DonGia.Value,
                        ThTien = item.ThanhTien.Value,
                        TSuat = item.ThueGTGT,
                    };
                    _STT += 1;
                    _dSHHDVu.Add(_hhdv);
                }

                //TToan
                List<LTSuat> listTLSuat = new List<LTSuat>();
                LTSuat tLSuat = new LTSuat
                {
                    TSuat = model.HoaDonChiTiets.FirstOrDefault().ThueGTGT,
                    ThTien = model.TongTienHangQuyDoi.Value,
                    TThue = model.TongTienThueGTGTQuyDoi.Value
                };
                listTLSuat.Add(tLSuat);

                TToan _TToan = new TToan
                {
                    TgTCThue = model.TongTienHangQuyDoi.Value,
                    TgTThue = model.TongTienThueGTGTQuyDoi.Value,
                    TgTTTBSo = model.TongTienThanhToanQuyDoi.Value,
                    TgTTTBChu = model.SoTienBangChu,
                    THTTLTSuat = listTLSuat
                };

                NDHDon _nDHDon = new NDHDon
                {
                    NBan = _nBan,
                    NMua = _nMua,
                    DSHHDVu = _dSHHDVu,
                    TToan = _TToan,
                };

                ///

                HDon _hDon = new HDon();
                _hDon.DLHDon = new DLHDon
                {
                    TTChung = _ttChung,
                    NDHDon = _nDHDon,
                };
                _hDon.DSCKS = new DSCKS
                {
                    NBan = "",
                    NMua = "",
                };
                #endregion

                GenerateBillXML2(_hDon, xmlFilePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void GenerateBillXML2(HDon data, string path)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer serialiser = new XmlSerializer(typeof(HDon));

                using (TextWriter filestream = new StreamWriter(path))
                {
                    serialiser.Serialize(filestream, data, ns);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
