using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuanLy;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.Filter;
using Services.Helper.Params.HoaDon;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class QuyDinhKyThuatService : IQuyDinhKyThuatService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mp;
        private readonly IXMLInvoiceService _xmlInvoiceService;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly IThongDiepGuiNhanCQTService _thongDiepGuiNhanCQTService;
        private readonly IMauHoaDonService _mauHoaDonService;

        private readonly List<LoaiThongDiep> TreeThongDiepNhan = new List<LoaiThongDiep>()
        {
            new LoaiThongDiep(){ LoaiThongDiepId = -1, MaLoaiThongDiep = -1, Ten = "Tất cả", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 0, MaLoaiThongDiep = 1, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ đăng ký, thay đổi thông tin sử dụng hóa đơn điện tử, đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 1, MaLoaiThongDiep = 102, Ten = "102 - Thông điệp thông báo về việc tiếp nhận/không tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HĐĐT, tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 2, MaLoaiThongDiep = 103, Ten = "103 - Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 3, MaLoaiThongDiep = 104, Ten = "104 - Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 4, MaLoaiThongDiep = 105, Ten = "105 - Thông điệp thông báo về việc hết thời gian sử dụng hóa đơn điện tử có mã qua cổng thông tin điện tử Tổng cục Thuế/qua ủy thác tổ chức cung cấp dịch vụ về hóa đơn điện tử; không thuộc trường hợp sử dụng hóa đơn điện tử không có mã", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 5, MaLoaiThongDiep = 2, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ lập và gửi hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 6, MaLoaiThongDiep = 202, Ten = "202 - Thông điệp thông báo kết quả cấp mã hóa đơn điện tử của cơ quan thuế", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 7, MaLoaiThongDiep = 204, Ten = "204 - Thông điệp thông báo mẫu số 01/TB-KTDL về việc kết quả kiểm tra dữ liệu hóa đơn điện tử", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 8, MaLoaiThongDiep = 205, Ten = "205 - Thông điệp phản hồi về hồ sơ đề nghị cấp hóa đơn điện tử có mã của cơ quan thuế theo từng lần pháp sinh", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 9, MaLoaiThongDiep = 3, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ xử lý hóa đơn có sai sót", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 10, MaLoaiThongDiep = 301, Ten = "301 - Thông điệp gửi thông báo về việc tiếp nhận và kết quả xử lý về việc hóa đơn điện tử đã lập có sai sót", LoaiThongDiepChaId = 3, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 11, MaLoaiThongDiep = 302, Ten = "302 - Thông điệp thông báo về hóa đơn điện tử cần rà soát", LoaiThongDiepChaId = 3, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 12, MaLoaiThongDiep = 9, Ten = "Nhóm thông điệp khác", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 13, MaLoaiThongDiep = 999, Ten = "999 - Thông điệp phản hồi kỹ thuật", LoaiThongDiepChaId = 9, Level = 1 },
        };

        private readonly List<LoaiThongDiep> TreeThongDiepGui = new List<LoaiThongDiep>()
        {
            new LoaiThongDiep(){ LoaiThongDiepId = -1, MaLoaiThongDiep = -1, Ten = "Tất cả", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 0, MaLoaiThongDiep = 1, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ đăng ký, thay đổi thông tin sử dụng hóa đơn điện tử, đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 1, MaLoaiThongDiep = 100, Ten = "100 - Thông điệp gửi tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 2, MaLoaiThongDiep = 101, Ten = "101 - Thông điệp gửi tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 3, MaLoaiThongDiep = 106, Ten = "106 - Thông điệp gửi Đơn đề nghị cấp hóa đơn điện tử có mã của CQT theo từng lần phát sinh", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 4, MaLoaiThongDiep = 2, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ lập và gửi hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 5, MaLoaiThongDiep = 200, Ten = "200 - Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 6, MaLoaiThongDiep = 201, Ten = "201 - Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã theo từng lần phát sinh", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 7, MaLoaiThongDiep = 203, Ten = "203 - Thông điệp chuyển dữ liệu hóa đơn điện tử không mã đến cơ quan thuế", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 8, MaLoaiThongDiep = 3, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ xử lý hóa đơn có sai sót", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 9, MaLoaiThongDiep = 300, Ten = "300 - Thông điệp thông báo về hóa đơn điện tử đã lập có sai sót", LoaiThongDiepChaId = 3, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 10, MaLoaiThongDiep = 4, Ten = "Nhóm thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 11, MaLoaiThongDiep = 400, Ten = "400 - Thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = 4, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 12, MaLoaiThongDiep = 5, Ten = "Nhóm thông điệp chuyển dữ liệu hóa đơn điện tử do TCTN ủy quyền cấp mã đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 13, MaLoaiThongDiep = 500, Ten = "500 - Thông điệp chuyển dữ liệu hóa đơn điện tử do TCTN ủy quyền cấp mã đến cơ quan thuế", LoaiThongDiepChaId = 5, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 14, MaLoaiThongDiep = 9, Ten = "Nhóm thông điệp khác", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 15, MaLoaiThongDiep = 999, Ten = "999 - Thông điệp phản hồi kỹ thuật", LoaiThongDiepChaId = 9, Level = 1 },
        };

        public QuyDinhKyThuatService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMapper mp,
            IXMLInvoiceService xmlInvoiceService,
            IHoSoHDDTService hoSoHDDTService,
            IHoaDonDienTuService hoaDonDienTuService,
            IThongDiepGuiNhanCQTService thongDiepGuiNhanCQTService,
            IMauHoaDonService mauHoaDonService
            )
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mp = mp;
            _xmlInvoiceService = xmlInvoiceService;
            _hoSoHDDTService = hoSoHDDTService;
            _hoaDonDienTuService = hoaDonDienTuService;
            _thongDiepGuiNhanCQTService = thongDiepGuiNhanCQTService;
            _mauHoaDonService = mauHoaDonService;
        }

        public List<EnumModel> GetTrangThaiGuiPhanHoiTuCQT(int maLoaiThongDiep)
        {
            var result = new List<EnumModel>();
            switch (maLoaiThongDiep)
            {
                case (int)MLTDiep.TDGToKhai:
                case (int)MLTDiep.TDGToKhaiUN:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChuaGui, Name = TrangThaiGuiThongDiep.ChuaGui.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChoPhanHoi, Name = TrangThaiGuiThongDiep.ChoPhanHoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiKhongLoi, Name = TrangThaiGuiThongDiep.GuiKhongLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiLoi, Name = TrangThaiGuiThongDiep.GuiLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.DaTiepNhan, Name = "CQT tiếp nhận" });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.TuChoiTiepNhan, Name = "CQT không tiếp nhận" });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChapNhan, Name = TrangThaiGuiThongDiep.ChapNhan.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.KhongChapNhan, Name = TrangThaiGuiThongDiep.KhongChapNhan.GetDescription() });
                        if (maLoaiThongDiep == (int)MLTDiep.TDGToKhaiUN)
                            result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoUNCQTKhongChapNhan, Name = TrangThaiGuiThongDiep.CoUNCQTKhongChapNhan.GetDescription() });

                        break;
                    }
                case (int)MLTDiep.TBTNToKhai:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.DaTiepNhan, Name = "CQT tiếp nhận" });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.TuChoiTiepNhan, Name = "CQT không tiếp nhận" });
                        break;
                    }
                case (int)MLTDiep.TBCNToKhai:
                case (int)MLTDiep.TBCNToKhaiUN:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.DaTiepNhan, Name = "CQT tiếp nhận" });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.TuChoiTiepNhan, Name = "CQT không tiếp nhận" });
                        if (maLoaiThongDiep == (int)MLTDiep.TBCNToKhaiUN)
                            result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoUNCQTKhongChapNhan, Name = TrangThaiGuiThongDiep.CoUNCQTKhongChapNhan.GetDescription() });

                        break;
                    }
                case (int)MLTDiep.TDCDLTVANUQCTQThue:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiKhongLoi, Name = TrangThaiGuiThongDiep.GuiKhongLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiLoi, Name = TrangThaiGuiThongDiep.GuiLoi.GetDescription() });

                        break;
                    }
                case (int)MLTDiep.TDTBHDDLSSot:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChuaGui, Name = TrangThaiGuiThongDiep.ChuaGui.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChoPhanHoi, Name = TrangThaiGuiThongDiep.ChoPhanHoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiKhongLoi, Name = TrangThaiGuiThongDiep.GuiKhongLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiLoi, Name = TrangThaiGuiThongDiep.GuiLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon, Name = TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan, Name = TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoHDKhongHopLe, Name = TrangThaiGuiThongDiep.CoHDKhongHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiTCTNLoi, Name = TrangThaiGuiThongDiep.GuiTCTNLoi.GetDescription() });
                        break;
                    }
                case (int)MLTDiep.TDTBKQKTDLHDon:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.KhongDuDieuKienCapMa, Name = TrangThaiGuiThongDiep.KhongDuDieuKienCapMa.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoHDKhongHopLe, Name = TrangThaiGuiThongDiep.CoHDKhongHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe.GetDescription() });
                        break;
                    }
                case (int)MLTDiep.TDCBTHDLHDDDTDCQThue:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChuaGui, Name = TrangThaiGuiThongDiep.ChuaGui.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChoPhanHoi, Name = TrangThaiGuiThongDiep.ChoPhanHoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiKhongLoi, Name = TrangThaiGuiThongDiep.GuiKhongLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiLoi, Name = TrangThaiGuiThongDiep.GuiLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoHDKhongHopLe, Name = TrangThaiGuiThongDiep.CoHDKhongHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe.GetDescription() });
                        break;
                    }
                default: break;
            }

            result.Insert(0, new EnumModel { Value = -99, Name = "Tất cả" });
            return result;
        }

        public async Task<string> GetNoiDungThongDiepXMLChuaKy(string thongDiepId)
        {
            var data = await _dataContext.FileDatas.Where(x => x.RefId == thongDiepId && x.IsSigned == false).FirstOrDefaultAsync();
            return data.Content;
        }

        public async Task<List<string>> GetAllListCTS()
        {
            return await _dataContext.ChungThuSoSuDungs.Where(x => x.HThuc != (int)HThuc2.NgungSuDung).Select(x => x.Seri).ToListAsync();
        }

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

        public async Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhai()
        {
            IQueryable<ThongDiepChungViewModel> query = from tdc in _dataContext.ThongDiepChungs
                                                        join tk in _dataContext.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id into tmpToKhai
                                                        from tk in tmpToKhai.DefaultIfEmpty()
                                                        select new ThongDiepChungViewModel
                                                        {
                                                            ThongDiepChungId = tdc.ThongDiepChungId,
                                                            MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                                                            MaThongDiep = tdc.MaThongDiep,
                                                            TenLoaiThongDiep = ((MLTDiep)tdc.MaLoaiThongDiep).GetDescription(),
                                                            MaNoiGui = tdc.MaNoiGui,
                                                            MaNoiNhan = tdc.MaNoiNhan,
                                                            MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                                                            MaSoThue = tdc.MaSoThue,
                                                            ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                                                            HinhThuc = tdc.HinhThuc,
                                                            TenHinhThuc = ((HThuc)tdc.HinhThuc).GetDescription(),
                                                            TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                                                            TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                                                            NgayGui = tdc.NgayGui ?? null,
                                                            IdThamChieu = tk.Id,
                                                            CreatedDate = tdc.CreatedDate,
                                                            ModifyDate = tdc.ModifyDate
                                                        };

            return await query.FirstOrDefaultAsync(x => x.MaLoaiThongDiep == 100 && x.HinhThuc == (int)HThuc.DangKyMoi && x.TrangThaiGui != TrangThaiGuiThongDiep.ChapNhan);
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhaiDuocChapNhan()
        {
            IQueryable<ThongDiepChungViewModel> query = from tdc in _dataContext.ThongDiepChungs
                                                        join tk in _dataContext.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id into tmpToKhai
                                                        from tk in tmpToKhai.DefaultIfEmpty()
                                                        select new ThongDiepChungViewModel
                                                        {
                                                            ThongDiepChungId = tdc.ThongDiepChungId,
                                                            MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                                                            MaThongDiep = tdc.MaThongDiep,
                                                            TenLoaiThongDiep = ((MLTDiep)tdc.MaLoaiThongDiep).GetDescription(),
                                                            MaNoiGui = tdc.MaNoiGui,
                                                            MaNoiNhan = tdc.MaNoiNhan,
                                                            MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                                                            MaSoThue = tdc.MaSoThue,
                                                            ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                                                            HinhThuc = tdc.HinhThuc,
                                                            TenHinhThuc = ((HThuc)tdc.HinhThuc).GetDescription(),
                                                            TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                                                            TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                                                            NgayGui = tdc.NgayGui ?? null,
                                                            IdThamChieu = tk.Id,
                                                            CreatedDate = tdc.CreatedDate,
                                                            ModifyDate = tdc.ModifyDate
                                                        };
            return await query.FirstOrDefaultAsync(x => x.MaLoaiThongDiep == 100 && x.TrangThaiGui == TrangThaiGuiThongDiep.ChapNhan);
        }

        public async Task<FileReturn> GetLinkFileXml(ThongDiepChungViewModel model, bool signed = false)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            switch (model.MaLoaiThongDiep)
            {
                case (int)MLTDiep.TDGToKhai:
                    if (!string.IsNullOrEmpty(model.MaThongDiep) && signed == true)
                    {
                        var dataXML = (await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == model.ThongDiepChungId)).Content;
                        var fileName = $"{Guid.NewGuid()}.xml";
                        var fullXMLFolder = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
                        if (!Directory.Exists(fullXMLFolder))
                        {
                            Directory.CreateDirectory(fullXMLFolder);
                        }
                        var fullXMLPath = Path.Combine(fullXMLFolder, fileName);
                        File.WriteAllText(fullXMLPath, dataXML);

                        byte[] fileByte = File.ReadAllBytes(fullXMLPath);
                        File.Delete(fullXMLPath);

                        return new FileReturn
                        {
                            Bytes = fileByte,
                            ContentType = MimeTypes.GetMimeType(fullXMLPath),
                            FileName = fileName
                        };
                    }
                    else
                    {
                        var entityTK = _dataContext.ToKhaiDangKyThongTins.FirstOrDefault(x => x.Id == model.IdThamChieu);
                        var fullXMLPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}/{entityTK.FileXMLChuaKy}");

                        byte[] fileByte = File.ReadAllBytes(fullXMLPath);
                        File.Delete(fullXMLPath);

                        return new FileReturn
                        {
                            Bytes = fileByte,
                            ContentType = MimeTypes.GetMimeType(fullXMLPath),
                            FileName = entityTK.FileXMLChuaKy
                        };
                    }
                case (int)MLTDiep.TDGToKhaiUN:
                    if (!string.IsNullOrEmpty(model.MaThongDiep) && signed == true)
                    {
                        var dataXML = (await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == model.ThongDiepChungId)).Content;
                        string fileName = $"{Guid.NewGuid()}.xml";
                        var fullXMLFolder = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
                        if (!Directory.Exists(fullXMLFolder))
                        {
                            Directory.CreateDirectory(fullXMLFolder);
                        }

                        var fullXMLPath = Path.Combine(fullXMLFolder, fileName);
                        File.WriteAllText(fullXMLPath, dataXML);

                        byte[] fileByte = File.ReadAllBytes(fullXMLPath);
                        File.Delete(fullXMLPath);

                        return new FileReturn
                        {
                            Bytes = fileByte,
                            ContentType = MimeTypes.GetMimeType(fullXMLPath),
                            FileName = fileName
                        };
                    }
                    else
                    {
                        var entityTK = _dataContext.ToKhaiDangKyThongTins.FirstOrDefault(x => x.Id == model.IdThamChieu);
                        var fullXMLPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}/{entityTK.FileXMLChuaKy}");

                        byte[] fileByte = File.ReadAllBytes(fullXMLPath);
                        File.Delete(fullXMLPath);

                        return new FileReturn
                        {
                            Bytes = fileByte,
                            ContentType = MimeTypes.GetMimeType(fullXMLPath),
                            FileName = entityTK.FileXMLChuaKy
                        };
                    }
                case (int)MLTDiep.TDTBHDDLSSot:
                    var entityTC = _dataContext.ThongDiepGuiCQTs.FirstOrDefault(x => x.Id == model.IdThamChieu);
                    string folderPath;
                    if (entityTC.DaKyGuiCQT == true && signed == true)
                    {
                        folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{entityTC.FileXMLDaKy}";
                    }
                    else
                    {
                        var files = entityTC.FileDinhKem.Split(';');
                        var file = files.FirstOrDefault(x => x.Contains(".xml"));
                        folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}/{file}";
                    }

                    return new FileReturn
                    {
                        Bytes = File.ReadAllBytes(folderPath),
                        ContentType = MimeTypes.GetMimeType(folderPath),
                        FileName = Path.GetFileName(folderPath)
                    };
                case (int)MLTDiep.TDGHDDTTCQTCapMa:
                case (int)MLTDiep.TDCDLHDKMDCQThue:
                    return null;
                default:
                    return null;
            }
        }

        public async Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungAsync(ThongDiepChungParams @params)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            try
            {
                IQueryable<ThongDiepChungViewModel> query = from tdc in _dataContext.ThongDiepChungs
                                                            where tdc.ThongDiepGuiDi == @params.IsThongDiepGui
                                                            select new ThongDiepChungViewModel
                                                            {
                                                                ThongDiepChungId = tdc.ThongDiepChungId,
                                                                PhienBan = tdc.PhienBan,
                                                                MaNoiGui = tdc.MaNoiGui,
                                                                MaNoiNhan = tdc.MaNoiNhan,
                                                                MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                                                                MaThongDiep = !string.IsNullOrEmpty(tdc.MaThongDiep) ? tdc.MaThongDiep : string.Empty,
                                                                MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                                                                MaSoThue = tdc.MaSoThue,
                                                                SoLuong = tdc.SoLuong,
                                                                TenLoaiThongDiep = ((MLTDiep)tdc.MaLoaiThongDiep).GetDescription(),
                                                                ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                                                                HinhThuc = tdc.HinhThuc ?? 0,
                                                                TenHinhThuc = tdc.HinhThuc.HasValue ? ((HThuc)tdc.HinhThuc).GetDescription() : string.Empty,
                                                                TrangThaiGui = tdc.TrangThaiGui.HasValue ? (TrangThaiGuiThongDiep)tdc.TrangThaiGui : TrangThaiGuiThongDiep.ChuaGui,
                                                                TenTrangThaiGui = tdc.TrangThaiGui.HasValue ? ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription() : TrangThaiGuiThongDiep.ChuaGui.GetDescription(),
                                                                NgayGui = tdc.NgayGui,
                                                                NgayThongBao = tdc.NgayThongBao,
                                                                IdThamChieu = tdc.IdThamChieu,
                                                                CreatedDate = tdc.CreatedDate,
                                                                ModifyDate = tdc.ModifyDate,
                                                                TaiLieuDinhKems = (from tldk in _dataContext.TaiLieuDinhKems
                                                                                   where tldk.NghiepVuId == tdc.ThongDiepChungId
                                                                                   orderby tldk.CreatedDate
                                                                                   select new TaiLieuDinhKemViewModel
                                                                                   {
                                                                                       TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                                                       NghiepVuId = tldk.NghiepVuId,
                                                                                       LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                                                       TenGoc = tldk.TenGoc,
                                                                                       TenGuid = tldk.TenGuid,
                                                                                       CreatedDate = tldk.CreatedDate,
                                                                                       Link = _httpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                                                       Status = tldk.Status
                                                                                   })
                                                                                   .ToList(),
                                                            };

                if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
                {
                    DateTime fromDate = DateTime.ParseExact(@params.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(@params.ToDate + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    query = query.Where(x => (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) >= fromDate &&
                                            (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) <= toDate);
                }

                // thông điệp nhận
                if (@params.IsThongDiepGui != true && @params.LoaiThongDiep != -1)
                {
                    var loaiThongDiep = TreeThongDiepNhan.FirstOrDefault(x => x.MaLoaiThongDiep == @params.LoaiThongDiep);
                    if (loaiThongDiep.IsParent == true)
                    {
                        var maLoaiThongDieps = TreeThongDiepNhan.Where(x => x.LoaiThongDiepChaId == @params.LoaiThongDiep).Select(x => x.MaLoaiThongDiep).ToList();
                        query = query.Where(x => maLoaiThongDieps.Contains(x.MaLoaiThongDiep));
                    }
                    else
                    {
                        query = query.Where(x => x.MaLoaiThongDiep == loaiThongDiep.MaLoaiThongDiep);
                    }
                }

                if (@params.IsThongDiepGui == true && @params.LoaiThongDiep != -1)
                {
                    var loaiThongDiep = TreeThongDiepGui.FirstOrDefault(x => x.MaLoaiThongDiep == @params.LoaiThongDiep);
                    if (loaiThongDiep.IsParent == true)
                    {
                        var maLoaiThongDieps = TreeThongDiepGui.Where(x => x.LoaiThongDiepChaId == @params.LoaiThongDiep).Select(x => x.MaLoaiThongDiep).ToList();
                        query = query.Where(x => maLoaiThongDieps.Contains(x.MaLoaiThongDiep));
                    }
                    else
                    {
                        query = query.Where(x => x.MaLoaiThongDiep == loaiThongDiep.MaLoaiThongDiep);
                    }
                }

                if (@params.TimKiemTheo != null)
                {
                    var timKiemTheo = @params.TimKiemTheo;
                    if (!string.IsNullOrEmpty(timKiemTheo.PhienBan))
                    {
                        var keyword = timKiemTheo.PhienBan.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.PhienBan) && x.PhienBan.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaNoiGui))
                    {
                        var keyword = timKiemTheo.MaNoiGui.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaNoiGui) && x.MaNoiGui.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaNoiNhan))
                    {
                        var keyword = timKiemTheo.MaNoiNhan.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaNoiNhan) && x.MaNoiNhan.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (timKiemTheo.MaLoaiThongDiep != null)
                    {
                        var keyword = timKiemTheo.MaLoaiThongDiep;
                        query = query.Where(x => x.MaLoaiThongDiep == keyword.Value);
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiep))
                    {
                        var keyword = timKiemTheo.MaThongDiep.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaThongDiep) && x.MaThongDiep.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiepThamChieu))
                    {
                        var keyword = timKiemTheo.MaThongDiepThamChieu.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaThongDiepThamChieu) && x.MaThongDiepThamChieu.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                    {
                        var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaSoThue) && x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (timKiemTheo.SoLuong != null)
                    {
                        var keyword = timKiemTheo.SoLuong;
                        query = query.Where(x => x.SoLuong == keyword.Value);
                    }
                }
                #region Filter and Sort
                if (@params.FilterColumns != null && @params.FilterColumns.Any())
                {
                    @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                    foreach (var filterCol in @params.FilterColumns)
                    {
                        switch (filterCol.ColKey)
                        {
                            case nameof(@params.Filter.PhienBan):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.PhienBan, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaNoiGui):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaNoiGui, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaNoiNhan):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaNoiNhan, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaLoaiThongDiep):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaLoaiThongDiep, filterCol, FilterValueType.Decimal);
                                break;
                            case nameof(@params.Filter.MaThongDiep):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaThongDiep, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaThongDiepThamChieu):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaThongDiepThamChieu, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaSoThue):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaSoThue, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.SoLuong):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.SoLuong, filterCol, FilterValueType.Decimal);
                                break;
                            case nameof(@params.Filter.NgayGui):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.NgayGui, filterCol, FilterValueType.DateTime);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(@params.SortKey))
                {
                    if (@params.SortKey == "PhienBan" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.PhienBan);
                    }
                    if (@params.SortKey == "PhienBan" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.PhienBan);
                    }

                    if (@params.SortKey == "MaNoiGui" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaNoiGui);
                    }
                    if (@params.SortKey == "MaNoiGui" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaNoiGui);
                    }


                    if (@params.SortKey == "MaNoiNhan" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaNoiNhan);
                    }
                    if (@params.SortKey == "MaNoiNhan" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaNoiNhan);
                    }

                    if (@params.SortKey == "MaLoaiThongDiep" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaLoaiThongDiep);
                    }
                    if (@params.SortKey == "MaLoaiThongDiep" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaLoaiThongDiep);
                    }

                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaThongDiep);
                    }
                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaThongDiep);
                    }

                    if (@params.SortKey == "MaThongDiepThamChieu" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaThongDiepThamChieu);
                    }
                    if (@params.SortKey == "MaThongDiepThamChieu" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaThongDiepThamChieu);
                    }

                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaSoThue);
                    }
                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaSoThue);
                    }

                    if (@params.SortKey == "SoLuong" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoLuong);
                    }
                    if (@params.SortKey == "SoLuong" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoLuong);
                    }

                    if (@params.SortKey == "NgayGui" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayGui);
                    }
                    if (@params.SortKey == "NgayGui" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayGui);
                    }

                }
                else
                {
                    query = query.OrderByDescending(x => x.CreatedDate);
                }
                #endregion

                //var list = await query.ToListAsync();

                if (@params.PageSize == -1)
                {
                    @params.PageSize = await query.CountAsync();
                }

                return await PagedList<ThongDiepChungViewModel>
                     .CreateAsync(query, @params.PageNumber, @params.PageSize);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return null;
            }
        }

        public int GetTrangThaiPhanHoiThongDiepNhan(ThongDiepChungViewModel tdn)
        {
            var tdData = _dataContext.FileDatas.FirstOrDefault(x => x.RefId == tdn.ThongDiepChungId);
            if (tdData == null) return -99;
            var contentXML = tdData.Content;
            switch (tdn.MaLoaiThongDiep)
            {
                case (int)MLTDiep.TDCDLTVANUQCTQThue:
                    var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(contentXML);
                    if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi) return (int)TrangThaiGuiThongDiep.GuiKhongLoi;
                    else return (int)TrangThaiGuiThongDiep.GuiLoi;
                case (int)MLTDiep.TBTNToKhai:
                    var tDiep102 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(contentXML);
                    if (tDiep102.DLieu.TBao.DLTBao.THop == THop.TruongHop1 || tDiep102.DLieu.TBao.DLTBao.THop == THop.TruongHop3)
                    {
                        return (int)TrangThaiGuiThongDiep.DaTiepNhan;
                    }
                    else
                    {
                        return (int)TrangThaiGuiThongDiep.TuChoiTiepNhan;
                    };
                case (int)MLTDiep.TBCNToKhai:
                    var tDiep103 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(contentXML);
                    if (tDiep103.DLieu.TBao.DLTBao.TTXNCQT == TTXNCQT.ChapNhan)
                    {
                        return (int)TrangThaiGuiThongDiep.ChapNhan;
                    }
                    else
                    {
                        return (int)TrangThaiGuiThongDiep.KhongChapNhan;
                    }
                case (int)MLTDiep.TBCNToKhaiUN:
                    var tDiep104 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(contentXML);
                    if (!tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem.Any(x => x.DSLDKCNhan.Count > 0))
                    {
                        return (int)(TrangThaiGuiThongDiep.ChapNhan);
                    }
                    else if (tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem.All(x => x.DSLDKCNhan.Count > 0))
                    {
                        return (int)TrangThaiGuiThongDiep.KhongChapNhan;
                    }
                    else return (int)TrangThaiGuiThongDiep.CoUNCQTKhongChapNhan;
                case (int)MLTDiep.TBKQCMHDon:
                    var tDiep202 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(contentXML);
                    return (int)(TrangThaiGuiThongDiep.CQTDaCapMa);
                case (int)MLTDiep.TDTBKQKTDLHDon:
                    var tDiep204 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(contentXML);
                    if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao1)
                    {
                        return (int)(TrangThaiGuiThongDiep.KhongDuDieuKienCapMa);
                    }
                    else
                    {
                        if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao2)
                        {
                            return (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe;
                        }
                        else if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao3)
                        {
                            return (int)TrangThaiGuiThongDiep.CoHDKhongHopLe;
                        }
                        else if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao9)
                        {
                            return (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe;
                        }
                        else
                        {
                            return (int)TrangThaiGuiThongDiep.GuiLoi;
                        }
                    }
                default: return -99;
            }
        }

        public List<EnumModel> GetListTimKiemTheoThongDiep()
        {
            ThongDiepSearch search = new ThongDiepSearch();
            var result = search.GetType().GetProperties()
                .Select(x => new EnumModel
                {
                    Value = x.Name,
                    Name = (x.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute).Name
                })
                .ToList();

            return result;
        }

        public async Task<ThongDiepChungViewModel> InsertThongDiepChung(ThongDiepChungViewModel model)
        {
            var _entity = _mp.Map<ThongDiepChung>(model);
            _entity.ThongDiepChungId = Guid.NewGuid().ToString();
            _entity.CreatedDate = DateTime.Now;
            _entity.ModifyDate = DateTime.Now;
            await _dataContext.ThongDiepChungs.AddAsync(_entity);
            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return _mp.Map<ThongDiepChungViewModel>(_entity);
            }
            else return null;
        }

        public async Task<bool> UpdateThongDiepChung(ThongDiepChungViewModel model)
        {
            var _entity = _mp.Map<ThongDiepChung>(model);
            _entity.ModifyDate = DateTime.Now;
            _dataContext.Update(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteThongDiepChung(string Id)
        {
            var entity = await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == Id);
            _dataContext.ThongDiepChungs.Remove(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepChungById(string Id)
        {
            IQueryable<ThongDiepChungViewModel> queryToKhai = from tdc in _dataContext.ThongDiepChungs
                                                              select new ThongDiepChungViewModel
                                                              {
                                                                  ThongDiepChungId = tdc.ThongDiepChungId,
                                                                  MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                                                                  MaThongDiep = tdc.MaThongDiep,
                                                                  TenLoaiThongDiep = ((MLTDiep)tdc.MaLoaiThongDiep).GetDescription(),
                                                                  MaNoiGui = tdc.MaNoiGui,
                                                                  MaNoiNhan = tdc.MaNoiNhan,
                                                                  MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                                                                  MaSoThue = tdc.MaSoThue,
                                                                  ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                                                                  HinhThuc = tdc.HinhThuc,
                                                                  TenHinhThuc = tdc.HinhThuc.HasValue ? ((HThuc)tdc.HinhThuc).GetDescription() : null,
                                                                  TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                                                                  TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                                                                  NgayGui = tdc.NgayGui ?? null,
                                                                  IdThamChieu = tdc.IdThamChieu,
                                                                  CreatedDate = tdc.CreatedDate,
                                                                  ModifyDate = tdc.ModifyDate
                                                              };

            IQueryable<ThongDiepChungViewModel> query = queryToKhai;
            return await query.FirstOrDefaultAsync(x => x.ThongDiepChungId == Id);
        }

        public List<DangKyUyNhiemViewModel> GetListTrungKyHieuTrongHeThong(List<DangKyUyNhiemViewModel> data)
        {
            return data.Where(x => _dataContext.BoKyHieuHoaDons.Any(o => o.KyHieuMauSoHoaDon == x.KHMSHDon && o.KyHieu == x.KHHDon)).ToList();
        }

        public async Task<List<ThongDiepChungViewModel>> GetAllThongDiepTraVe(string MaThongDiep)
        {
            return _mp.Map<List<ThongDiepChungViewModel>>(await _dataContext.ThongDiepChungs.Where(x => x.ThongDiepGuiDi == false && x.MaThongDiepThamChieu == MaThongDiep).OrderByDescending(x => x.CreatedDate).ToListAsync());
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepByThamChieu(string ThamChieuId)
        {
            var entity = await _dataContext.ThongDiepChungs.AsNoTracking().FirstOrDefaultAsync(x => x.IdThamChieu == ThamChieuId);
            var result = _mp.Map<ThongDiepChungViewModel>(entity);
            return result;
        }

        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep ConvertToThongDiepTiepNhan(string encodedContent)
        {
            return DataHelper.ConvertObjectFromStringContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(encodedContent);
        }

        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep ConvertToThongDiepKUNCQT(string encodedContent)
        {
            return DataHelper.ConvertObjectFromStringContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(encodedContent);
        }

        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep ConvertToThongDiepUNCQT(string encodedContent)
        {
            return DataHelper.ConvertObjectFromStringContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(encodedContent);
        }

        public List<LoaiThongDiep> GetListLoaiThongDiepNhan()
        {
            return TreeThongDiepNhan;
        }

        public List<LoaiThongDiep> GetListLoaiThongDiepGui()
        {
            return TreeThongDiepGui;
        }

        /// <summary>
        /// Thêm thông điệp nhận vào hệ thống
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<bool> InsertThongDiepNhanAsync(ThongDiepPhanHoiParams @params)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                List<HoaDonKhongHopLeViewModel> listHoaDongKhongHopLe = new List<HoaDonKhongHopLeViewModel>();

                // Save file
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                // Write to file
                string fileName = $"TD-{Guid.NewGuid()}.xml";
                string filePath = Path.Combine(fullFolderPath, fileName);
                File.WriteAllText(filePath, @params.DataXML);

                // Add To log
                await _dataContext.AddTransferLog(@params.DataXML, 2);

                // Update status
                var entityTD = await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == @params.ThongDiepId || x.MaThongDiep == @params.ThongDiepId);

                DLL.Entity.QuanLyHoaDon.ThongDiepGuiCQT thongDiepGuiCQT = null;
                if (entityTD != null && entityTD.MaLoaiThongDiep == 300)
                {
                    //đọc ra ThongDiepGuiCQT để biết xem thông báo 04 sẽ lập cho hóa đơn trong hay ngoài hệ thống
                    thongDiepGuiCQT = await _dataContext.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == entityTD.IdThamChieu);
                }

                switch (@params.MLTDiep)
                {
                    case (int)MLTDiep.TBTNToKhai: // 102
                        var tDiep102 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(@params.DataXML);
                        if (tDiep102.DLieu.TBao.DLTBao.THop == THop.TruongHop1 || tDiep102.DLieu.TBao.DLTBao.THop == THop.TruongHop3)
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.DaTiepNhan;
                        }
                        else
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.TuChoiTiepNhan;
                        };

                        entityTD.NgayThongBao = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.NTBao);
                        entityTD.MaThongDiepPhanHoi = tDiep102.TTChung.MTDiep;
                        _dataContext.ThongDiepChungs.Update(entityTD);

                        var tdc102 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep102.TTChung.PBan,
                            MaNoiGui = tDiep102.TTChung.MNGui,
                            MaNoiNhan = tDiep102.TTChung.MNNhan,
                            TrangThaiGui = (tDiep102.DLieu.TBao.DLTBao.THop == THop.TruongHop1 || tDiep102.DLieu.TBao.DLTBao.THop == THop.TruongHop3) ? (int)TrangThaiGuiThongDiep.DaTiepNhan : (int)TrangThaiGuiThongDiep.TuChoiTiepNhan,
                            MaLoaiThongDiep = int.Parse(tDiep102.TTChung.MLTDiep),
                            MaThongDiep = tDiep102.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep102.TTChung.MTDTChieu,
                            MaSoThue = tDiep102.TTChung.MST,
                            SoLuong = tDiep102.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            HinhThuc = 0,
                            NgayThongBao = DateTime.Now,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc102);
                        Tracert.WriteLog("tdc102");
                        break;
                    case (int)MLTDiep.TBCNToKhai: // 103
                        var tDiep103 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(@params.DataXML);
                        if (tDiep103.DLieu.TBao.DLTBao.TTXNCQT == TTXNCQT.ChapNhan)
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.ChapNhan;
                        }
                        else
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.KhongChapNhan;
                        };

                        entityTD.NgayThongBao = DateTime.Parse(tDiep103.DLieu.TBao.STBao.NTBao);
                        entityTD.MaThongDiepPhanHoi = tDiep103.TTChung.MTDiep;
                        _dataContext.ThongDiepChungs.Update(entityTD);

                        var tdc103 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep103.TTChung.PBan,
                            MaNoiGui = tDiep103.TTChung.MNGui,
                            MaNoiNhan = tDiep103.TTChung.MNNhan,
                            TrangThaiGui = tDiep103.DLieu.TBao.DLTBao.TTXNCQT == TTXNCQT.ChapNhan ? 5 : 6,
                            MaLoaiThongDiep = int.Parse(tDiep103.TTChung.MLTDiep),
                            MaThongDiep = tDiep103.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep103.TTChung.MTDTChieu,
                            MaSoThue = tDiep103.TTChung.MST,
                            SoLuong = tDiep103.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            HinhThuc = 0,
                            NgayThongBao = DateTime.Now,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc103);

                        await UpdateThongTinHoaDonTheoThongDiepAsync(entityTD, tDiep103.DLieu.TBao.DLTBao.TTXNCQT == TTXNCQT.ChapNhan);
                        break;
                    case (int)MLTDiep.TBCNToKhaiUN: // 104
                        var tDiep104 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(@params.DataXML);
                        if (!tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem.Any(x => x.DSLDKCNhan.Count > 0))
                        {
                            entityTD.TrangThaiGui = (int)(TrangThaiGuiThongDiep.ChapNhan);
                        }
                        else if (tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem.All(x => x.DSLDKCNhan.Count > 0))
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.KhongChapNhan;
                        }
                        else entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.CoUNCQTKhongChapNhan;

                        entityTD.NgayThongBao = DateTime.Parse(tDiep104.DLieu.TBao.STBao.NTBao);
                        entityTD.MaThongDiepPhanHoi = tDiep104.TTChung.MTDiep;
                        _dataContext.ThongDiepChungs.Update(entityTD);

                        var tdc104 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep104.TTChung.PBan,
                            MaNoiGui = tDiep104.TTChung.MNGui,
                            MaNoiNhan = tDiep104.TTChung.MNNhan,
                            MaLoaiThongDiep = int.Parse(tDiep104.TTChung.MLTDiep),
                            TrangThaiGui = tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem.All(x => x.DSLDKCNhan.Any()) ? (int)TrangThaiGuiThongDiep.KhongChapNhan :
                                                        !tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem.All(x => x.DSLDKCNhan.Any()) ? (int)TrangThaiGuiThongDiep.ChapNhan : (int)TrangThaiGuiThongDiep.CoUNCQTKhongChapNhan,
                            MaThongDiep = tDiep104.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep104.TTChung.MTDTChieu,
                            MaSoThue = tDiep104.TTChung.MST,
                            SoLuong = tDiep104.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            HinhThuc = 0,
                            NgayThongBao = DateTime.Now,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc104);
                        break;
                    case (int)MLTDiep.TBKQCMHDon: // 202
                        var tDiep202 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(@params.DataXML);
                        entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.CQTDaCapMa;
                        entityTD.NgayThongBao = DateTime.Now.Date;
                        entityTD.MaThongDiepPhanHoi = tDiep202.TTChung.MTDiep;
                        _dataContext.ThongDiepChungs.Update(entityTD);

                        ThongDiepChung tdc202 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep202.TTChung.PBan,
                            MaNoiGui = tDiep202.TTChung.MNGui,
                            MaNoiNhan = tDiep202.TTChung.MNNhan,
                            MaLoaiThongDiep = int.Parse(tDiep202.TTChung.MLTDiep),
                            MaThongDiep = tDiep202.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep202.TTChung.MTDTChieu,
                            MaSoThue = tDiep202.TTChung.MST,
                            SoLuong = tDiep202.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            TrangThaiGui = entityTD.TrangThaiGui,
                            HinhThuc = 0,
                            NgayThongBao = DateTime.Now,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc202);

                        // update trạng thái quy trình cho hóa đơn
                        await UpdateTrangThaiQuyTrinhHDDTAsync(entityTD, MLTDiep.TBKQCMHDon, false, @params.DataXML);
                        break;
                    case (int)MLTDiep.TDTBKQKTDLHDon: // 204
                        var tDiep204 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(@params.DataXML);

                        if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao1)
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.KhongDuDieuKienCapMa;
                        }
                        else
                        {
                            if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao2)
                            {
                                entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe;
                            }
                            else if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao3)
                            {
                                entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.CoHDKhongHopLe;

                                //thêm danh sách hóa đơn không hợp lệ
                                var lHDKMa = tDiep204.DLieu.TBao.DLTBao.LHDKMa;
                                if (lHDKMa != null)
                                {
                                    for (int i = 0; i < lHDKMa.DSHDon.Count; i++)
                                    {
                                        var item = lHDKMa.DSHDon[i];
                                        listHoaDongKhongHopLe.Add(new HoaDonKhongHopLeViewModel
                                        {
                                            MauHoaDon = item.KHMSHDon,
                                            KyHieuHoaDon = item.KHHDon,
                                            SoHoaDon = item.SHDon
                                        });
                                    }
                                }
                            }
                            else if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao9)
                            {
                                entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe;
                            }
                            else
                            {
                                entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                            }
                        }

                        if (tDiep204.DLieu.TBao.DLTBao.LBTHKXDau != null)
                        {
                            var plainContentThongDiepGoc = await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == entityTD.ThongDiepChungId);
                            var tDiep400 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(plainContentThongDiepGoc.Content);

                            if (tDiep204.DLieu.TBao.DLTBao.LTBao != LTBao.ThongBao4 && tDiep204.DLieu.TBao.DLTBao.LTBao != LTBao.ThongBao5)
                            {
                                var dsBTH = tDiep400.DLieu;
                                if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao2)
                                {
                                    foreach (var bth in dsBTH)
                                    {
                                        var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu;
                                        foreach (var hd in dshd)
                                        {
                                            var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.HoaDonHopLe);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var bth in dsBTH)
                                    {
                                        var dskhl = tDiep204.DLieu.TBao.DLTBao.LBTHKXDau.DSBTHop.SelectMany(x => x.DSLHDon.HDon).ToList();

                                        var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu.Where(x => !dskhl.Any(o => o.SHDon == x.SHDon.ToString() && o.KHMSHDon == x.KHMSHDon && o.KHHDon == o.KHHDon));
                                        foreach (var hd in dshd)
                                        {
                                            var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.HoaDonHopLe);
                                        }

                                        var dshdLoi = bth.DLBTHop.NDBTHDLieu.DSDLieu.Where(x => dskhl.Any(o => o.SHDon == x.SHDon.ToString() && o.KHMSHDon == x.KHMSHDon && o.KHHDon == o.KHHDon));
                                        foreach (var hd in dshdLoi)
                                        {
                                            var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.GuiLoi);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var dsBTH = tDiep400.DLieu;
                                foreach (var bth in dsBTH)
                                {
                                    var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu;
                                    foreach (var hd in dshd)
                                    {
                                        var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                        await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.GuiLoi);
                                    }
                                }
                            }
                        }

                        if (tDiep204.DLieu.TBao.DLTBao.LBTHXDau != null)
                        {
                            var plainContentThongDiepGoc = await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == entityTD.ThongDiepChungId);
                            var tDiep400 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(plainContentThongDiepGoc.Content);

                            if (tDiep204.DLieu.TBao.DLTBao.LTBao != LTBao.ThongBao4 && tDiep204.DLieu.TBao.DLTBao.LTBao != LTBao.ThongBao5)
                            {
                                var dsBTH = tDiep400.DLieu;
                                if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao2)
                                {
                                    foreach (var bth in dsBTH)
                                    {
                                        var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu;
                                        foreach (var hd in dshd)
                                        {
                                            var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.HoaDonHopLe);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var bth in dsBTH)
                                    {
                                        var dskhl = tDiep204.DLieu.TBao.DLTBao.LBTHXDau.DSBTHop.SelectMany(x => x.DSLMHang).ToList();

                                        var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu.Where(x => !dskhl.Any(o => o.MHHoa == x.MHHoa.ToString() && o.THHDVu == x.THHDVu)).DistinctBy(x => new { x.KHHDon, x.KHMSHDon, x.SHDon });
                                        foreach (var hd in dshd)
                                        {
                                            var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.HoaDonHopLe);
                                        }

                                        var dshdLoi = bth.DLBTHop.NDBTHDLieu.DSDLieu.Where(x => dskhl.Any(o => o.MHHoa == x.MHHoa.ToString() && o.THHDVu == x.THHDVu)).DistinctBy(x => new { x.KHHDon, x.KHMSHDon, x.SHDon });

                                        foreach (var hd in dshdLoi)
                                        {
                                            var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.GuiLoi);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                var dsBTH = tDiep400.DLieu;
                                foreach (var bth in dsBTH)
                                {
                                    var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu;
                                    foreach (var hd in dshd)
                                    {
                                        var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                        await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.GuiLoi);
                                    }
                                }
                            }
                        }

                        if (entityTD.MaLoaiThongDiep == (int)MLTDiep.TDCBTHDLHDDDTDCQThue && tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao2)
                        {
                            var plainContentThongDiepGoc = await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == entityTD.ThongDiepChungId);
                            var tDiep400 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(plainContentThongDiepGoc.Content);
                            var dsBTH = tDiep400.DLieu;
                            foreach (var bth in dsBTH)
                            {
                                var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu;
                                foreach (var hd in dshd)
                                {
                                    var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                    await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.HoaDonHopLe);
                                }
                            }
                        }

                        ThongDiepChung tdc204 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep204.TTChung.PBan,
                            MaNoiGui = tDiep204.TTChung.MNGui,
                            MaNoiNhan = tDiep204.TTChung.MNNhan,
                            MaLoaiThongDiep = int.Parse(tDiep204.TTChung.MLTDiep),
                            MaThongDiep = tDiep204.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep204.TTChung.MTDTChieu,
                            MaSoThue = tDiep204.TTChung.MST,
                            SoLuong = tDiep204.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            TrangThaiGui = entityTD.TrangThaiGui,
                            HinhThuc = 0,
                            NgayThongBao = DateTime.Now,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc204);

                        // update trạng thái quy trình cho hóa đơn
                        await UpdateTrangThaiQuyTrinhHDDTAsync(entityTD, MLTDiep.TDTBKQKTDLHDon, tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao1 || tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao3);
                        break;
                    case (int)MLTDiep.TDCDLTVANUQCTQThue: // 999
                        var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(@params.DataXML);
                        if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi)
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiKhongLoi;
                        }
                        else
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                        }

                        if (entityTD.MaLoaiThongDiep == 400)
                        {
                            var plainContentThongDiepGoc = await _dataContext.FileDatas.FirstOrDefaultAsync(x => x.RefId == entityTD.ThongDiepChungId);
                            var tDiep400 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(plainContentThongDiepGoc.Content);

                            if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.CoLoi)
                            {
                                var dsBTH = tDiep400.DLieu;
                                foreach (var bth in dsBTH)
                                {
                                    var dshd = bth.DLBTHop.NDBTHDLieu.DSDLieu;
                                    foreach (var hd in dshd)
                                    {
                                        var objHDDT = await _hoaDonDienTuService.GetByIdAsync(hd.SHDon.Value, hd.KHHDon, hd.KHMSHDon);
                                        await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(objHDDT.HoaDonDienTuId, TrangThaiQuyTrinh.GuiTCTNLoi);
                                    }
                                }
                            }
                        }

                        entityTD.NgayThongBao = DateTime.Now.Date;
                        entityTD.MaThongDiepPhanHoi = tDiep999.TTChung.MTDiep;
                        _dataContext.ThongDiepChungs.Update(entityTD);

                        ThongDiepChung tdc999 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep999.TTChung.PBan,
                            MaNoiGui = tDiep999.TTChung.MNGui,
                            MaNoiNhan = tDiep999.TTChung.MNNhan,
                            MaLoaiThongDiep = int.Parse(tDiep999.TTChung.MLTDiep),
                            MaThongDiep = tDiep999.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep999.TTChung.MTDTChieu,
                            MaSoThue = tDiep999.TTChung.MST,
                            SoLuong = tDiep999.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            TrangThaiGui = tDiep999.DLieu.TBao.TTTNhan == (int)TTTNhan.KhongLoi ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi,
                            HinhThuc = 0,
                            NgayThongBao = DateTime.Now,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc999);

                        // update trạng thái quy trình cho hóa đơn
                        await UpdateTrangThaiQuyTrinhHDDTAsync(entityTD, MLTDiep.TDCDLTVANUQCTQThue, tDiep999.DLieu.TBao.TTTNhan == TTTNhan.CoLoi);
                        break;
                    case (int)MLTDiep.TBTNVKQXLHDDTSSot: // 301
                        var tDiep301 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot.TDiep>(@params.DataXML);
                        ThongDiepChung tdc301 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep301.TTChung.PBan,
                            MaNoiGui = tDiep301.TTChung.MNGui,
                            MaNoiNhan = tDiep301.TTChung.MNNhan,
                            MaLoaiThongDiep = tDiep301.TTChung.MLTDiep,
                            MaThongDiep = tDiep301.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep301.TTChung.MTDTChieu,
                            MaSoThue = tDiep301.TTChung.MST,
                            SoLuong = tDiep301.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            HinhThuc = (int)HThuc.ChinhThuc,
                            NgayThongBao = DateTime.Now,
                            TrangThaiGui = (tDiep301.DLieu.TBao.DLTBao.DSHDon.Count(x => x.TTTNCCQT == 2) > 0) ? (int)TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan : (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon,
                            FileXML = fileName
                        };

                        //update lại trạng thái thông điệp 300
                        entityTD.NgayThongBao = DateTime.Now.Date;
                        entityTD.MaThongDiepPhanHoi = tDiep301.TTChung.MTDiep;
                        entityTD.TrangThaiGui = tdc301.TrangThaiGui;
                        _dataContext.ThongDiepChungs.Update(entityTD);

                        //thêm 1 thông điệp chung 301
                        await _dataContext.ThongDiepChungs.AddAsync(tdc301);

                        //thêm danh sách hóa đơn không hợp lệ
                        if (tdc301.TrangThaiGui == (int)TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan)
                        {
                            var dSHDon = tDiep301.DLieu.TBao.DLTBao.DSHDon;
                            if (dSHDon != null)
                            {
                                for (int i = 0; i < dSHDon.Count; i++)
                                {
                                    var item = dSHDon[i];
                                    listHoaDongKhongHopLe.Add(new HoaDonKhongHopLeViewModel
                                    {
                                        MauHoaDon = item.KHMSHDon,
                                        KyHieuHoaDon = item.KHHDon,
                                        SoHoaDon = item.SHDon
                                    });
                                }
                            }
                        }

                        break;
                    case (int)MLTDiep.TDTBHDDTCRSoat: // 302
                        var tDiep302 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep>(@params.DataXML);
                        ThongDiepChung tdc302 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep302.TTChung.PBan,
                            MaNoiGui = tDiep302.TTChung.MNGui,
                            MaNoiNhan = tDiep302.TTChung.MNNhan,
                            MaLoaiThongDiep = tDiep302.TTChung.MLTDiep,
                            MaThongDiep = tDiep302.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep302.TTChung.MTDTChieu,
                            MaSoThue = tDiep302.TTChung.MST,
                            SoLuong = tDiep302.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            HinhThuc = (int)HThuc.ChinhThuc,
                            NgayThongBao = DateTime.Now,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc302);
                        await _thongDiepGuiNhanCQTService.ThemThongBaoHoaDonRaSoat(tDiep302);
                        break;
                    default:
                        break;
                }

                var fileData = new FileData
                {
                    RefId = id,
                    Type = 1,
                    DateTime = DateTime.Now,
                    Content = @params.DataXML,
                    Binary = Encoding.ASCII.GetBytes(@params.DataXML),
                    IsSigned = true,
                    FileName = fileName
                };
                await _dataContext.FileDatas.AddAsync(fileData);

                //đánh dấu trạng thái gửi hóa đơn đã lập thông báo 04
                if (entityTD.MaLoaiThongDiep == 300)
                {
                    await CapNhatTrangThaiGui04ChoCacHoaDon(entityTD.IdThamChieu, entityTD.TrangThaiGui.GetValueOrDefault(), listHoaDongKhongHopLe, thongDiepGuiCQT);
                }

                var result = await _dataContext.SaveChangesAsync();

                return result > 0;

            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return false;
        }

        private async Task UpdateTrangThaiQuyTrinhHDDTAsync(ThongDiepChung ttChung, MLTDiep mLTDiepPhanHoi, bool hasError, string dataXML = null)
        {
            if ((ttChung.MaLoaiThongDiep == (int)MLTDiep.TDGHDDTTCQTCapMa) || (ttChung.MaLoaiThongDiep == (int)MLTDiep.TDCDLHDKMDCQThue))
            {
                var ddghddt = await _dataContext.DuLieuGuiHDDTs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == ttChung.IdThamChieu);

                if (ddghddt != null)
                {
                    var hddt = await _dataContext.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == ddghddt.HoaDonDienTuId);

                    switch (mLTDiepPhanHoi)
                    {
                        case MLTDiep.TDCDLTVANUQCTQThue:
                            if (hddt.TrangThaiQuyTrinh != ((int)TrangThaiQuyTrinh.CQTDaCapMa) && hddt.TrangThaiQuyTrinh != ((int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa))
                            {
                                hddt.TrangThaiQuyTrinh = hasError ? ((int)TrangThaiQuyTrinh.GuiLoi) : ((int)TrangThaiQuyTrinh.GuiKhongLoi);
                            }
                            break;
                        case MLTDiep.TDTBKQKTDLHDon:
                            if (hasError)
                            {
                                if (ttChung.TrangThaiGui == (int)TrangThaiGuiThongDiep.KhongDuDieuKienCapMa)
                                {
                                    hddt.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa;
                                }
                                else if (ttChung.TrangThaiGui == (int)TrangThaiGuiThongDiep.CoHDKhongHopLe)
                                {
                                    hddt.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonKhongHopLe;
                                }
                            }
                            else
                            {
                                hddt.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonHopLe;
                            }
                            break;
                        case MLTDiep.TBKQCMHDon:
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(dataXML);
                            XmlNode node = doc.SelectSingleNode("/TDiep/DLieu/HDon/MCCQT");

                            var hddtViewModel = await _hoaDonDienTuService.GetByIdAsync(hddt.HoaDonDienTuId);
                            hddtViewModel.IsCapMa = true;
                            hddtViewModel.MaCuaCQT = node.InnerText;
                            hddtViewModel.DataXML = dataXML;
                            await _hoaDonDienTuService.ConvertHoaDonToFilePDF(hddtViewModel);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// GetAllThongDiepTraVeV2 trả về danh sách các thông điệp nhận (của thông điệp 300)
        /// </summary>
        /// <param name="giaTriTimKiem"></param>
        /// <param name="phanLoai"></param>
        /// <returns></returns>
        public async Task<ThongDiepChiTiet> GetAllThongDiepTraVeV2(string giaTriTimKiem, string phanLoai)
        {
            //phanLoai = byId: tìm kiếm theo id; phanLoai = byMaThongDiep: tìm kiếm theo mã thông điệp
            //giaTriTimKiem: giá trị để tìm kiếm
            var listThongDiepChiTiet1 = new List<ThongDiepChiTiet1>();
            var listThongDiepChiTiet2 = new List<ThongDiepChiTiet2>();
            var result = new ThongDiepChiTiet();
            var moTaLoi = "";

            //đọc ra danh sách các thông điệp trả về
            var listThongDiepTraVe = await (from thongDiep in _dataContext.ThongDiepChungs
                                            join fileData in _dataContext.FileDatas on thongDiep.ThongDiepChungId equals fileData.RefId
                                            where thongDiep.ThongDiepGuiDi == false &&
                                            ((thongDiep.MaThongDiepThamChieu == giaTriTimKiem && phanLoai == "byMaThongDiep") ||
                                            (thongDiep.ThongDiepChungId == giaTriTimKiem && phanLoai == "byId")) &&
                                            (thongDiep.MaLoaiThongDiep == (int)MLTDiep.TDTBKQKTDLHDon ||  //204
                                            thongDiep.MaLoaiThongDiep == (int)MLTDiep.TBTNVKQXLHDDTSSot || //301
                                            thongDiep.MaLoaiThongDiep == (int)MLTDiep.TDCDLTVANUQCTQThue //999
                                            )
                                            select new NoiDungXMLCuaThongDiep
                                            {
                                                Content = fileData.Content,
                                                ThongDiepChungId = thongDiep.ThongDiepChungId,
                                                MaLoaiThongDiep = thongDiep.MaLoaiThongDiep,
                                                MaThongDiepThamChieu = thongDiep.MaThongDiepThamChieu
                                            }).ToListAsync();

            //đọc ra thông điệp chứa hóa đơn gốc lúc gửi đi của thông điệp 300
            //mục đích chỉ để đọc ra ngày lập hóa đơn, vì thông điệp 204 ko có ngày lập hóa đơn
            var maThongDiep = "";
            if (phanLoai == "byMaThongDiep")
            {
                maThongDiep = giaTriTimKiem;
            }
            if (phanLoai == "byId")
            {
                maThongDiep = listThongDiepTraVe.FirstOrDefault()?.MaThongDiepThamChieu;
            }
            var thongDiep300GocDaGui = await (from thongDiep in _dataContext.ThongDiepChungs.
                                           Where(x => x.MaThongDiep == maThongDiep &&
                                           x.ThongDiepGuiDi && x.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDLSSot)
                                              join fileData in _dataContext.FileDatas on thongDiep.ThongDiepChungId equals fileData.RefId
                                              select new NoiDungXMLCuaThongDiep { Content = fileData.Content }
                                           ).FirstOrDefaultAsync();

            List<ViewModels.XML.ThongDiepGuiNhanCQT.HDon> listHoaDonGocCuaThongDiep300 = new List<ViewModels.XML.ThongDiepGuiNhanCQT.HDon>();
            if (thongDiep300GocDaGui != null)
            {
                var listHoaDonGocContent = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiep>(thongDiep300GocDaGui.Content);
                listHoaDonGocCuaThongDiep300 = listHoaDonGocContent.DLieu.TBao.DLTBao.DSHDon;
            }

            foreach (var item in listThongDiepTraVe)
            {
                if (item.MaLoaiThongDiep == (int)MLTDiep.TDTBKQKTDLHDon) //204
                {
                    var tDiep204 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(item.Content);

                    //trường hợp 204 có LCMa (LTBao = 1)
                    if (tDiep204.DLieu.TBao.DLTBao.LCMa != null)
                    {
                        var lCMa = tDiep204.DLieu.TBao.DLTBao.LCMa;

                        lCMa.DSLDo = lCMa.DSLDo ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo>();
                        for (int i = 0; i < lCMa.DSLDo.Count; i++)
                        {
                            var dSLDoItem = lCMa.DSLDo[i];
                            moTaLoi += $"- {i + 1}. Mã lỗi: {dSLDoItem.MLoi}; Mô tả: {dSLDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {dSLDoItem.HDXLy}; Ghi chú (nếu có): {dSLDoItem.GChu}\n";
                        }
                    }

                    //trường hợp 204 cho 300 có LHDKMa (LTBao = 3)
                    if (tDiep204.DLieu.TBao.DLTBao.LHDKMa != null)
                    {
                        moTaLoi = "";
                        var lHDKMa = tDiep204.DLieu.TBao.DLTBao.LHDKMa;
                        var dsLyDo = lHDKMa.DSHDon.Where(x => x.DSLDo != null).ToList();

                        for (int i = 0; i < dsLyDo.Count; i++)
                        {
                            for (int j = 0; j < dsLyDo[i].DSLDo.Count; j++)
                            {
                                var lyDoItem = dsLyDo[i].DSLDo[j];
                                moTaLoi += $"- {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}\n";
                            }
                        }
                    }

                    //trường hợp 204 cho 300 có KHLKhac (LTBao = 9) 
                    if (tDiep204.DLieu.TBao.DLTBao.KHLKhac != null)
                    {
                        moTaLoi = "";
                        var dsLyDo = tDiep204.DLieu.TBao.DLTBao.KHLKhac.DSLDo;

                        for (int j = 0; j < dsLyDo.Count; j++)
                        {
                            var lyDoItem = dsLyDo[j];
                            moTaLoi += $"- {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}\n";
                        }
                    }

                    var thongDiepChiTiet1 = new ThongDiepChiTiet1
                    {
                        PhienBan = tDiep204.TTChung.PBan, //vì ko có thẻ PBan trong tDiep204.DLieu.TBao.DLTBao
                        MaNoiGui = tDiep204.TTChung.MNGui,
                        MaNoiNhan = tDiep204.TTChung.MNNhan,
                        MauSo = tDiep204.DLieu.TBao.DLTBao.MSo,
                        MaLoaiThongDiep = item.MaLoaiThongDiep,
                        MaThongDiep = tDiep204.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep204.TTChung.MTDTChieu,
                        TenThongBao = tDiep204.DLieu.TBao.DLTBao.Ten,
                        SoThongBao = tDiep204.DLieu.TBao.DLTBao.So,
                        NgayThongBao = DateTime.Parse(tDiep204.DLieu.TBao.DLTBao.NTBao),
                        DiaDanh = tDiep204.DLieu.TBao.DLTBao.DDanh,
                        MaSoThue = tDiep204.DLieu.TBao.DLTBao.MST,
                        TenNguoiNopThue = tDiep204.DLieu.TBao.DLTBao.TNNT,
                        MaDonViQuanHeNganSach = tDiep204.DLieu.TBao.DLTBao.MDVQHNSach,
                        ThoiGianGui = tDiep204.DLieu.TBao.DLTBao.TGGui.ConvertStringToDate(),
                        ThoiGianNhan = tDiep204.DLieu.TBao.DLTBao.TGGui.ConvertStringToDate(),//default = ThoiGianGui
                        LoaiThongBao = ((int)tDiep204.DLieu.TBao.DLTBao.LTBao).ToString(),
                        CanCu = tDiep204.DLieu.TBao.DLTBao.CCu,
                        SoLuong = tDiep204.DLieu.TBao.DLTBao.SLuong,
                        MoTaLoi = moTaLoi
                    };

                    listThongDiepChiTiet1.Add(thongDiepChiTiet1);

                    if (!string.IsNullOrWhiteSpace(moTaLoi) && ((int)tDiep204.DLieu.TBao.DLTBao.LTBao) == 3 &&
                        tDiep204.DLieu.TBao.DLTBao.LHDKMa != null)
                    {
                        foreach (var hoaDon in tDiep204.DLieu.TBao.DLTBao.LHDKMa.DSHDon)
                        {
                            //listThongDiepChiTiet2 là danh sách các hóa đơn không hợp lệ (ứng với 204 loại 3)
                            listThongDiepChiTiet2.Add(new ThongDiepChiTiet2
                            {
                                KyHieuMauSoHoaDon = hoaDon.KHMSHDon ?? string.Empty,
                                KyHieuHoaDon = hoaDon.KHHDon ?? string.Empty,
                                SoHoaDon = hoaDon.SHDon ?? string.Empty,
                                NgayLap = (
                                    listHoaDonGocCuaThongDiep300?.FirstOrDefault(x => (x.KHMSHDon ?? string.Empty) == (hoaDon.KHMSHDon ?? string.Empty) && (x.KHHDon ?? string.Empty) == (hoaDon.KHHDon ?? string.Empty) && (x.SHDon ?? string.Empty) == (hoaDon.SHDon ?? string.Empty)
                                )?.Ngay.ConvertStringToDate()),
                                MoTaLoi = moTaLoi,
                                MaLoaiThongDiep = item.MaLoaiThongDiep
                            });
                        }
                    }
                }
                else if (item.MaLoaiThongDiep == (int)MLTDiep.TDCDLTVANUQCTQThue) //999
                {
                    var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(item.Content);

                    moTaLoi = "";
                    if (tDiep999.DLieu.TBao.DSLDo != null)
                    {
                        for (int j = 0; j < tDiep999.DLieu.TBao.DSLDo.Count; j++)
                        {
                            var dSLDKTNhanItem = tDiep999.DLieu.TBao.DSLDo[j];
                            moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}\n";
                        }

                    }
                    var thongDiepChiTiet1 = new ThongDiepChiTiet1
                    {
                        PhienBan = tDiep999.TTChung.PBan,
                        TrangThaiXacNhanCuaCQT = tDiep999.DLieu.TBao.TTTNhan.GetDescription(),
                        MaLoaiThongDiep = item.MaLoaiThongDiep,
                        MaThongDiep = tDiep999.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep999.TTChung.MTDTChieu,
                        MaNoiGui = tDiep999.TTChung.MNGui,
                        MaNoiNhan = tDiep999.TTChung.MNNhan,
                        MoTaLoi = moTaLoi,
                        ThoiGianNhan = tDiep999.DLieu.TBao.NNhan.ConvertStringToDate()
                    };

                    listThongDiepChiTiet1.Add(thongDiepChiTiet1);
                }
                else if (item.MaLoaiThongDiep == (int)MLTDiep.TBTNVKQXLHDDTSSot) //301
                {
                    var tDiep301 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot.TDiep>(item.Content);

                    var dSLDKTNhan = tDiep301.DLieu.TBao.DLTBao.DSLDKTNhan;
                    if (dSLDKTNhan.Count > 0)
                    {
                        moTaLoi = "Lỗi của thông điệp<br>";
                    }
                    for (int i = 0; i < dSLDKTNhan.Count; i++)
                    {
                        var lyDoItem = dSLDKTNhan[i];
                        moTaLoi += $"- {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br>";
                    }

                    //danh sách chi tiết lý do trong các hóa đơn
                    var listLyDoTrongHoaDon = tDiep301.DLieu.TBao.DLTBao.DSHDon.Where(x => x.DSLDKTNhan.Count() > 0);

                    if (dSLDKTNhan.Count > 0 && listLyDoTrongHoaDon.Count() > 0)
                    {
                        moTaLoi += "<br>Lỗi chi tiết trong danh sách hóa đơn không được tiếp nhận:<br>";
                    }

                    for (int i = 0; i < listLyDoTrongHoaDon.Count(); i++)
                    {
                        var hoaDonItem = tDiep301.DLieu.TBao.DLTBao.DSHDon[i];
                        for (int j = 0; j < hoaDonItem.DSLDKTNhan.Count; j++)
                        {
                            var lyDoItem = hoaDonItem.DSLDKTNhan[j];
                            moTaLoi += "Ký hiệu mẫu số hóa đơn <b>" + (hoaDonItem.KHMSHDon ?? "") + "</b> Ký hiệu hóa đơn <b>" + (hoaDonItem.KHHDon ?? "") + "</b> Số <b>" + hoaDonItem.SHDon + "</b> Ngày hóa đơn <b>" + hoaDonItem.NLap.ConvertStringToDate()?.ToString("dd/MM/yyyy") + "</b><br>";
                            moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTa}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br>";
                        }
                    }

                    var thongDiepChiTiet1 = new ThongDiepChiTiet1
                    {
                        PhienBan = tDiep301.DLieu.TBao.DLTBao.PBan,
                        MauSo = tDiep301.DLieu.TBao.DLTBao.MSo,
                        TenThongBao = tDiep301.DLieu.TBao.DLTBao.Ten,
                        SoThongBao = tDiep301.DLieu.TBao.STBao.So,
                        NgayThongBao = tDiep301.DLieu.TBao.STBao.NTBao.ConvertStringToDate(),
                        DiaDanh = tDiep301.DLieu.TBao.DLTBao.DDanh,
                        TenCQTCapTren = tDiep301.DLieu.TBao.DLTBao.TCQTCTren,
                        TenCQTRaThongBao = tDiep301.DLieu.TBao.DLTBao.TCQT,
                        TenNguoiNopThue = tDiep301.DLieu.TBao.DLTBao.TNNT,
                        MaSoThue = tDiep301.DLieu.TBao.DLTBao.MST,
                        MaDonViQuanHeNganSach = tDiep301.DLieu.TBao.DLTBao.MDVQHNSach,
                        ThoiGianGui = null,
                        LoaiThongBao = null,
                        CanCu = null,
                        SoLuong = null,
                        MaGiaoDichDienTu = tDiep301.DLieu.TBao.DLTBao.MGDDTu,
                        ThoiGianNhan = tDiep301.DLieu.TBao.DLTBao.TGNhan.ConvertStringToDate(),
                        SoThuTuThe = tDiep301.DLieu.TBao.DLTBao.STTThe,
                        MoTaLoi = moTaLoi,
                        MaLoaiThongDiep = item.MaLoaiThongDiep
                    };

                    listThongDiepChiTiet1.Add(thongDiepChiTiet1);

                    if (tDiep301.DLieu.TBao.DLTBao.DSHDon != null && string.IsNullOrWhiteSpace(moTaLoi) == false)
                    {
                        var dSHDon = tDiep301.DLieu.TBao.DLTBao.DSHDon;
                        for (int i = 0; i < dSHDon.Count; i++)
                        {
                            moTaLoi = "";
                            var dSHDonItem = dSHDon[i];
                            for (int j = 0; j < dSHDonItem.DSLDKTNhan.Count; j++)
                            {
                                var dSLDKTNhanItem = dSHDonItem.DSLDKTNhan[j];
                                moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}\n";
                            }

                            listThongDiepChiTiet2.Add(new ThongDiepChiTiet2
                            {
                                STT = dSHDonItem.STT,
                                MaCQTCap = dSHDonItem.MCQTCap,
                                KyHieuMauSoHoaDon = dSHDonItem.KHMSHDon,
                                KyHieuHoaDon = dSHDonItem.KHHDon,
                                SoHoaDon = dSHDonItem.SHDon,
                                NgayLap = dSHDonItem.NLap.ConvertStringToDate(),
                                LoaiHoaDonDienTuApDung = dSHDonItem.LADHDDT.ToString(),
                                TinhChatThongBao = dSHDonItem.TCTBao.ToString(),
                                TrangThaiTiepNhanCuaCQT = dSHDonItem.TTTNCCQT.ToString(),
                                MoTaLoi = moTaLoi,
                                MaLoaiThongDiep = item.MaLoaiThongDiep
                            });
                        }
                    }
                }
            }

            result.ThongDiepChiTiet1s = listThongDiepChiTiet1.OrderByDescending(x => x.ThoiGianNhan).ToList();
            result.ThongDiepChiTiet2s = listThongDiepChiTiet2;

            return result;
        }

        /// <summary>
        /// Lấy nội dung thông điệp từ file xml
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ThongDiepChiTiet> ShowThongDiepFromFileByIdAsync(string id)
        {
            try
            {
                ThongDiepChiTiet result = new ThongDiepChiTiet
                {
                    ThongDiepChiTiet1s = new List<ThongDiepChiTiet1>(),
                    ThongDiepChiTiet2s = new List<ThongDiepChiTiet2>()
                };

                var entity = await _dataContext.ThongDiepChungs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ThongDiepChungId == id);

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

                string folderPath = folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{entity.FileXML}";
                if (entity.MaLoaiThongDiep != (int)MLTDiep.TDTBHDDLSSot)
                {
                    folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{entity.FileXML}";
                }
                else
                {
                    var entityTC = _dataContext.ThongDiepGuiCQTs.FirstOrDefault(x => x.Id == entity.IdThamChieu);
                    if (entityTC.DaKyGuiCQT == true)
                    {
                        folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{entityTC.FileXMLDaKy}";
                    }
                    else
                    {
                        var files = entityTC.FileDinhKem.Split(';');
                        var file = files.FirstOrDefault(x => x.Contains(".xml"));
                        folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}/{file}";
                    }
                }
                string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);

                string moTaLoi = string.Empty;
                int length = 0;

                var plainContent = await _dataContext.FileDatas.Where(x => x.RefId == id).Select(x => x.Content).FirstOrDefaultAsync();
                switch (entity.MaLoaiThongDiep)
                {
                    case (int)MLTDiep.TBTNToKhai: // 102
                        var tDiep102 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(plainContent);

                        var lstLoi102 = tDiep102.DLieu.TBao.DLTBao.DSLDKCNhan ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>();
                        length = lstLoi102.Count;
                        for (int i = 0; i < length; i++)
                        {
                            var item = lstLoi102[i];
                            moTaLoi += $"- {i + 1}. Mã lỗi: {item.MLoi}; Mô tả: {item.MTa}\n";
                        }

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep102.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep102.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep102.DLieu.TBao.DLTBao.Ten,
                            SoThongBao = tDiep102.DLieu.TBao.DLTBao.So,
                            MaLoaiThongDiep = int.Parse(tDiep102.TTChung.MLTDiep),
                            NgayThongBao = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.NTBao),
                            DiaDanh = tDiep102.DLieu.TBao.DLTBao.DDanh,
                            MaSoThue = tDiep102.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep102.DLieu.TBao.DLTBao.TNNT,
                            TenToKhai = tDiep102.DLieu.TBao.DLTBao.TTKhai,
                            MaGiaoDichDienTu = tDiep102.DLieu.TBao.DLTBao.MGDDTu,
                            ThoiGianGui = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.TGGui),
                            ThoiGianNhan = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.TGNhan),
                            TruongHop = tDiep102.DLieu.TBao.DLTBao.THop.GetDescription(),
                            MoTaLoi = moTaLoi
                        });
                        break;
                    case (int)MLTDiep.TBCNToKhai: // 103
                        var tDiep103 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(plainContent);

                        var lstLoi103 = tDiep103.DLieu.TBao.DLTBao.DSLDKCNhan ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>();
                        length = lstLoi103.Count;
                        for (int i = 0; i < length; i++)
                        {
                            var item = lstLoi103[i];
                            moTaLoi += $"- {i + 1}. Mã lỗi: {item.MLoi}; Mô tả: {item.MTa}\n";
                        }

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep103.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep103.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep103.DLieu.TBao.DLTBao.Ten,
                            MaLoaiThongDiep = int.Parse(tDiep103.TTChung.MLTDiep),
                            DiaDanh = tDiep103.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep103.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep103.DLieu.TBao.DLTBao.TCQT,
                            MaSoThue = tDiep103.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep103.DLieu.TBao.DLTBao.TNNT,
                            NgayDangKy_ThayDoi = DateTime.Parse(tDiep103.DLieu.TBao.DLTBao.Ngay),
                            HinhThucDanhKy_ThayDoi = tDiep103.DLieu.TBao.DLTBao.HTDKy.GetDescription(),
                            TrangThaiXacNhanCuaCQT = tDiep103.DLieu.TBao.DLTBao.TTXNCQT.GetDescription(),
                            MoTaLoi = moTaLoi
                        });
                        break;
                    case (int)MLTDiep.TBCNToKhaiUN: // 104
                        var tDiep104 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(plainContent);

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep104.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep104.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep104.DLieu.TBao.DLTBao.Ten,
                            SoThongBao = tDiep104.DLieu.TBao.DLTBao.So,
                            NgayThongBao = DateTime.Parse(tDiep104.DLieu.TBao.DLTBao.Ngay),
                            MaLoaiThongDiep = int.Parse(tDiep104.TTChung.MLTDiep),
                            DiaDanh = tDiep104.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep104.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep104.DLieu.TBao.DLTBao.TCQT,
                            MaSoThue = tDiep104.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep104.DLieu.TBao.DLTBao.TNNT,
                            NgayDangKy_ThayDoi = DateTime.Parse(tDiep104.DLieu.TBao.DLTBao.Ngay),
                            LoaiUyNhiem = tDiep104.DLieu.TBao.DLTBao.LUNhiem.GetDescription(),
                            MaGiaoDichDienTu = tDiep104.DLieu.TBao.DLTBao.MGDDTu,
                            ThoiGianNhan = DateTime.Parse(tDiep104.DLieu.TBao.DLTBao.TGNhan),
                        });

                        var dSTTUNhiem = tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TTUNhiem>();
                        for (int i = 0; i < dSTTUNhiem.Count; i++)
                        {
                            var dSTTUNhiemItem = dSTTUNhiem[i];
                            dSTTUNhiemItem.DSHDUNhiem = dSTTUNhiemItem.DSHDUNhiem ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.HDUNhiem>();

                            for (int j = 0; j < dSTTUNhiemItem.DSLDKCNhan.Count; j++)
                            {
                                var dSLDKCNhanItem = dSTTUNhiemItem.DSLDKCNhan[i];
                                moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKCNhanItem.MLoi}; Mô tả: {dSLDKCNhanItem.MTa}\n";
                            }

                            for (int k = 0; k < dSTTUNhiemItem.DSHDUNhiem.Count; k++)
                            {
                                var dSHDUNhiemItem = dSTTUNhiemItem.DSHDUNhiem[k];
                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = i + 1,
                                    MaSoThue = dSTTUNhiemItem.MST,
                                    TenToChuc = dSTTUNhiemItem.TTChuc,
                                    NgayTiepNhan = !string.IsNullOrEmpty(dSTTUNhiemItem.NTNhan) ? DateTime.Parse(dSTTUNhiemItem.NTNhan) : (DateTime?)null,
                                    TenLoaiHoaDon = dSHDUNhiemItem.TLHDon,
                                    KyHieuMauSoHoaDon = dSHDUNhiemItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDUNhiemItem.KHHDon,
                                    MucDich = dSHDUNhiemItem.MDich,
                                    TuNgay = DateTime.Parse(dSHDUNhiemItem.TNgay),
                                    DenNgay = DateTime.Parse(dSHDUNhiemItem.DNgay),
                                    MoTaLoi = moTaLoi
                                });
                            }
                        }
                        break;
                    case (int)MLTDiep.TDCDLHDKMDCQThue: // 203
                        var tDiep203 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep>(plainContent);
                        foreach (var item1 in tDiep203.DLieu)
                        {
                            result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                            {
                                PhienBan = item1.DLHDon.TTChung.PBan,
                                MaCuaCoQuanThue = item1.MCCQT,
                                TenHoaDon = item1.DLHDon.TTChung.THDon,
                                KyHieuMauSoHoaDon = item1.DLHDon.TTChung.KHMSHDon,
                                KyHieuHoaDon = item1.DLHDon.TTChung.KHHDon,
                                SoHoaDon = item1.DLHDon.TTChung.SHDon,
                                MaHoSo = item1.DLHDon.TTChung.MHSo,
                                NgayLap = DateTime.Parse(item1.DLHDon.TTChung.NLap),
                                SoBangKe = item1.DLHDon.TTChung.SBKe,
                                NgayBangKe = !string.IsNullOrEmpty(item1.DLHDon.TTChung.NBKe) ? DateTime.Parse(item1.DLHDon.TTChung.NBKe) : (DateTime?)null,
                                DonViTienTe = item1.DLHDon.TTChung.DVTTe,
                                TyGia = item1.DLHDon.TTChung.TGia,
                                HinhThucThanhToan = item1.DLHDon.TTChung.HTTToan,
                                MaSoThueDVNUNLHD = item1.DLHDon.TTChung.MSTDVNUNLHDon,
                                TenDVNhanUNLHD = item1.DLHDon.TTChung.TDVNUNLHDon,
                                DiaChiDVNhanUNLHD = item1.DLHDon.TTChung.DCDVNUNLHDon,
                                TinhChatHoaDon = item1.DLHDon.TTChung.TTHDLQuan?.TCHDon.GetDescription(),
                                LoaiHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.LHDCLQuan.GetDescription(),
                                KyHieuMauSoHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.KHMSHDCLQuan,
                                KyHieuHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.KHHDCLQuan,
                                SoHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.SHDCLQuan,
                                NgayLapHoaDonCoLienQuan = !string.IsNullOrEmpty(item1.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) ? DateTime.Parse(item1.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) : (DateTime?)null,
                                TenNguoiBan = item1.DLHDon.NDHDon.NBan.Ten,
                                MaSoThueNguoiBan = item1.DLHDon.NDHDon.NBan.MST,
                                DiaChiNguoiBan = item1.DLHDon.NDHDon.NBan.DChi,
                                TenNguoiMua = item1.DLHDon.NDHDon.NMua.Ten,
                                MaSoThueNguoiMua = item1.DLHDon.NDHDon.NMua.MST,
                                DiaChiNguoiMua = item1.DLHDon.NDHDon.NMua.DChi,
                                TongTienChuaThue = item1.DLHDon.NDHDon.TToan.TgTCThue,
                                TongTienThue = item1.DLHDon.NDHDon.TToan.TgTThue,
                                LoaiPhis = item1.DLHDon.NDHDon.TToan.DSLPhi
                                .Select(x => new LoaiPhi
                                {
                                    TenLoaiPhi = x.TLPhi,
                                    TienPhi = x.TPhi
                                }).ToList(),
                                TongTienChietKhauThuongMai = item1.DLHDon.NDHDon.TToan.TTCKTMai,
                                TongTienThanhToan = item1.DLHDon.NDHDon.TToan.TgTTTBSo
                            });
                        }

                        break;
                    case (int)MLTDiep.TDGHDDTTCQTCapMa: // 200
                        var tDiep200 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(plainContent);
                        if (tDiep200 != null)
                        {
                            var item2 = tDiep200.DLieu.HDon;
                            result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                            {
                                PhienBan = item2.DLHDon.TTChung.PBan,
                                MaCuaCoQuanThue = item2.MCCQT,
                                TenHoaDon = item2.DLHDon.TTChung.THDon,
                                KyHieuMauSoHoaDon = item2.DLHDon.TTChung.KHMSHDon,
                                KyHieuHoaDon = item2.DLHDon.TTChung.KHHDon,
                                SoHoaDon = item2.DLHDon.TTChung.SHDon,
                                MaHoSo = item2.DLHDon.TTChung.MHSo,
                                NgayLap = DateTime.Parse(item2.DLHDon.TTChung.NLap),
                                SoBangKe = item2.DLHDon.TTChung.SBKe,
                                NgayBangKe = !string.IsNullOrEmpty(item2.DLHDon.TTChung.NBKe) ? DateTime.Parse(item2.DLHDon.TTChung.NBKe) : (DateTime?)null,
                                DonViTienTe = item2.DLHDon.TTChung.DVTTe,
                                TyGia = item2.DLHDon.TTChung.TGia,
                                HinhThucThanhToan = item2.DLHDon.TTChung.HTTToan,
                                MaSoThueDVNUNLHD = item2.DLHDon.TTChung.MSTDVNUNLHDon,
                                TenDVNhanUNLHD = item2.DLHDon.TTChung.TDVNUNLHDon,
                                DiaChiDVNhanUNLHD = item2.DLHDon.TTChung.DCDVNUNLHDon,
                                TinhChatHoaDon = item2.DLHDon.TTChung.TTHDLQuan?.TCHDon.GetDescription(),
                                LoaiHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.LHDCLQuan.GetDescription(),
                                KyHieuMauSoHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.KHMSHDCLQuan,
                                KyHieuHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.KHHDCLQuan,
                                SoHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.SHDCLQuan,
                                NgayLapHoaDonCoLienQuan = !string.IsNullOrEmpty(item2.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) ? DateTime.Parse(item2.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) : (DateTime?)null,
                                TenNguoiBan = item2.DLHDon.NDHDon.NBan.Ten,
                                MaSoThueNguoiBan = item2.DLHDon.NDHDon.NBan.MST,
                                DiaChiNguoiBan = item2.DLHDon.NDHDon.NBan.DChi,
                                TenNguoiMua = item2.DLHDon.NDHDon.NMua.Ten,
                                MaSoThueNguoiMua = item2.DLHDon.NDHDon.NMua.MST,
                                DiaChiNguoiMua = item2.DLHDon.NDHDon.NMua.DChi,
                                TongTienChuaThue = item2.DLHDon.NDHDon.TToan.TgTCThue,
                                TongTienThue = item2.DLHDon.NDHDon.TToan.TgTThue,
                                LoaiPhis = item2.DLHDon.NDHDon.TToan.DSLPhi
                                .Select(x => new LoaiPhi
                                {
                                    TenLoaiPhi = x.TLPhi,
                                    TienPhi = x.TPhi
                                }).ToList(),
                                TongTienChietKhauThuongMai = item2.DLHDon.NDHDon.TToan.TTCKTMai,
                                TongTienThanhToan = item2.DLHDon.NDHDon.TToan.TgTTTBSo
                            });
                        }

                        break;
                    case (int)MLTDiep.TBKQCMHDon: // 202
                        var tDiep202 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(plainContent);

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep202.DLieu.HDon.DLHDon.TTChung.PBan,
                            MaNoiGui = tDiep202.TTChung.MNGui,
                            MaNoiNhan = tDiep202.TTChung.MNNhan,
                            MaLoaiThongDiep = int.Parse(tDiep202.TTChung.MLTDiep),
                            MaThongDiep = tDiep202.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep202.TTChung.MTDTChieu,

                            MaCuaCoQuanThue = tDiep202.DLieu.HDon.MCCQT,
                            TenHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.THDon,
                            KyHieuMauSoHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.KHMSHDon,
                            KyHieuHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.KHHDon,
                            SoHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.SHDon,
                            MaHoSo = tDiep202.DLieu.HDon.DLHDon.TTChung.MHSo,
                            NgayLap = DateTime.Parse(tDiep202.DLieu.HDon.DLHDon.TTChung.NLap),
                            SoBangKe = tDiep202.DLieu.HDon.DLHDon.TTChung.SBKe,
                            NgayBangKe = !string.IsNullOrEmpty(tDiep202.DLieu.HDon.DLHDon.TTChung.NBKe) ? DateTime.Parse(tDiep202.DLieu.HDon.DLHDon.TTChung.NBKe) : (DateTime?)null,
                            DonViTienTe = tDiep202.DLieu.HDon.DLHDon.TTChung.DVTTe,
                            TyGia = tDiep202.DLieu.HDon.DLHDon.TTChung.TGia,
                            HinhThucThanhToan = tDiep202.DLieu.HDon.DLHDon.TTChung.HTTToan,
                            MaSoThueDVNUNLHD = tDiep202.DLieu.HDon.DLHDon.TTChung.MSTDVNUNLHDon,
                            TenDVNhanUNLHD = tDiep202.DLieu.HDon.DLHDon.TTChung.TDVNUNLHDon,
                            DiaChiDVNhanUNLHD = tDiep202.DLieu.HDon.DLHDon.TTChung.DCDVNUNLHDon,
                            TinhChatHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.TCHDon.GetDescription(),
                            LoaiHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.LHDCLQuan.GetDescription(),
                            KyHieuMauSoHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.KHMSHDCLQuan,
                            KyHieuHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.KHHDCLQuan,
                            SoHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.SHDCLQuan,
                            NgayLapHoaDonCoLienQuan = !string.IsNullOrEmpty(tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) ? DateTime.Parse(tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) : (DateTime?)null,
                            TenNguoiBan = tDiep202.DLieu.HDon.DLHDon.NDHDon.NBan.Ten,
                            MaSoThueNguoiBan = tDiep202.DLieu.HDon.DLHDon.NDHDon.NBan.MST,
                            DiaChiNguoiBan = tDiep202.DLieu.HDon.DLHDon.NDHDon.NBan.DChi,
                            TenNguoiMua = tDiep202.DLieu.HDon.DLHDon.NDHDon.NMua.Ten,
                            MaSoThueNguoiMua = tDiep202.DLieu.HDon.DLHDon.NDHDon.NMua.MST,
                            DiaChiNguoiMua = tDiep202.DLieu.HDon.DLHDon.NDHDon.NMua.DChi,
                            TongTienChuaThue = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TgTCThue,
                            TongTienThue = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TgTThue,
                            LoaiPhis = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.DSLPhi
                                .Select(x => new LoaiPhi
                                {
                                    TenLoaiPhi = x.TLPhi,
                                    TienPhi = x.TPhi
                                }).ToList(),
                            TongTienChietKhauThuongMai = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TTCKTMai,
                            TongTienThanhToan = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TgTTTBSo
                        });
                        break;
                    case (int)MLTDiep.TDTBKQKTDLHDon: // 204
                        var tDiep204 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(plainContent);

                        if (tDiep204.DLieu.TBao.DLTBao.LCMa != null)
                        {
                            var lCMa = tDiep204.DLieu.TBao.DLTBao.LCMa;

                            lCMa.DSLDo = lCMa.DSLDo ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo>();
                            for (int i = 0; i < lCMa.DSLDo.Count; i++)
                            {
                                var dSLDoItem = lCMa.DSLDo[i];
                                moTaLoi += $"- {i + 1}. Mã lỗi: {dSLDoItem.MLoi}; Mô tả: {dSLDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {dSLDoItem.HDXLy}; Ghi chú (nếu có): {dSLDoItem.GChu}\n";
                            }
                        }

                        //trường hợp 204 cho 300 có LHDKMa
                        if (tDiep204.DLieu.TBao.DLTBao.LHDKMa != null)
                        {
                            moTaLoi = "";
                            var lHDKMa = tDiep204.DLieu.TBao.DLTBao.LHDKMa;
                            var dsLyDo = lHDKMa.DSHDon.Where(x => x.DSLDo != null).ToList();

                            for (int i = 0; i < dsLyDo.Count; i++)
                            {
                                for (int j = 0; j < dsLyDo[i].DSLDo.Count; j++)
                                {
                                    var lyDoItem = dsLyDo[i].DSLDo[j];
                                    moTaLoi += $"- {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}\n";
                                }
                            }
                        }

                        //trường hợp 204 cho 300 có KHLKhac
                        if (tDiep204.DLieu.TBao.DLTBao.KHLKhac != null)
                        {
                            moTaLoi = "";
                            var dsLyDo = tDiep204.DLieu.TBao.DLTBao.KHLKhac.DSLDo;

                            for (int j = 0; j < dsLyDo.Count; j++)
                            {
                                var lyDoItem = dsLyDo[j];
                                moTaLoi += $"- {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}\n";
                            }
                        }

                        //trường hợp thông điệp 400 có loại hàng hóa khác xăng dầu
                        if (tDiep204.DLieu.TBao.DLTBao.LBTHKXDau != null)
                        {
                            moTaLoi = "";

                            var dsBTH = tDiep204.DLieu.TBao.DLTBao.LBTHKXDau.DSBTHop;
                            for (int i = 0; i < dsBTH.Count; i++)
                            {
                                var dsLyDo = dsBTH[i].DSLDTTChung;

                                for (int j = 0; j < dsLyDo.Count; j++)
                                {
                                    var lyDoItem = dsLyDo[j];
                                    moTaLoi += $"- {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}\n";
                                }
                            }
                        }

                        //trường hợp thông điệp 400 có loại hàng hóa là xăng dầu
                        if (tDiep204.DLieu.TBao.DLTBao.LBTHXDau != null)
                        {
                            moTaLoi = "";

                            var dsBTH = tDiep204.DLieu.TBao.DLTBao.LBTHXDau.DSBTHop;
                            for (int i = 0; i < dsBTH.Count; i++)
                            {
                                var dsLyDo = dsBTH[i].DSLDTTChung;

                                for (int j = 0; j < dsLyDo.Count; j++)
                                {
                                    var lyDoItem = dsLyDo[j];
                                    moTaLoi += $"- {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}\n";
                                }
                            }
                        }

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep204.DLieu.TBao.DLTBao.PBan,
                            MaNoiGui = tDiep204.TTChung.MNGui,
                            MaNoiNhan = tDiep204.TTChung.MNNhan,
                            MauSo = tDiep204.DLieu.TBao.DLTBao.MSo,
                            MaLoaiThongDiep = int.Parse(tDiep204.TTChung.MLTDiep),
                            MaThongDiep = tDiep204.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep204.TTChung.MTDTChieu,
                            TenThongBao = tDiep204.DLieu.TBao.DLTBao.Ten,
                            SoThongBao = tDiep204.DLieu.TBao.DLTBao.So,
                            NgayThongBao = DateTime.Parse(tDiep204.DLieu.TBao.DLTBao.NTBao),
                            DiaDanh = tDiep204.DLieu.TBao.DLTBao.DDanh,
                            MaSoThue = tDiep204.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep204.DLieu.TBao.DLTBao.TNNT,
                            MaDonViQuanHeNganSach = tDiep204.DLieu.TBao.DLTBao.MDVQHNSach,
                            ThoiGianGui = !string.IsNullOrEmpty(tDiep204.DLieu.TBao.DLTBao.TGGui) ? DateTime.Parse(tDiep204.DLieu.TBao.DLTBao.TGGui) : (DateTime?)null,
                            LoaiThongBao = tDiep204.DLieu.TBao.DLTBao.LTBao.GetDescription(),
                            CanCu = tDiep204.DLieu.TBao.DLTBao.CCu,
                            SoLuong = tDiep204.DLieu.TBao.DLTBao.SLuong,
                            MoTaLoi = moTaLoi
                        });

                        if (!string.IsNullOrEmpty(moTaLoi))
                        {
                            var hoaDonDienTu = await (from hddt in _dataContext.HoaDonDienTus
                                                      join bkhhd in _dataContext.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                      join dlghd in _dataContext.DuLieuGuiHDDTs on hddt.HoaDonDienTuId equals dlghd.HoaDonDienTuId
                                                      join tlg in _dataContext.ThongDiepChungs on dlghd.DuLieuGuiHDDTId equals tlg.IdThamChieu
                                                      select new HoaDonDienTuViewModel
                                                      {
                                                          HoaDonDienTuId = hddt.HoaDonDienTuId,
                                                          MauSo = bkhhd.KyHieuMauSoHoaDon + "",
                                                          KyHieu = bkhhd.KyHieuHoaDon,
                                                          SoHoaDon = hddt.SoHoaDon,
                                                          NgayHoaDon = hddt.NgayHoaDon
                                                      })
                                                    .GroupBy(x => x.HoaDonDienTuId)
                                                    .Select(x => new HoaDonDienTuViewModel
                                                    {
                                                        HoaDonDienTuId = x.Key,
                                                        MauSo = x.First().MauSo,
                                                        KyHieu = x.First().KyHieu,
                                                        SoHoaDon = x.First().SoHoaDon,
                                                        NgayHoaDon = x.First().NgayHoaDon
                                                    })
                                                    .FirstOrDefaultAsync();

                            if (hoaDonDienTu != null)
                            {
                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    KyHieuMauSoHoaDon = hoaDonDienTu.MauSo ?? string.Empty,
                                    KyHieuHoaDon = hoaDonDienTu.KyHieu ?? string.Empty,
                                    SoHoaDon = hoaDonDienTu.SoHoaDon,
                                    NgayLap = hoaDonDienTu.NgayHoaDon,
                                    MoTaLoi = moTaLoi
                                });
                            }
                        }

                        break;
                    case (int)MLTDiep.TBTNVKQXLHDDTSSot: // 301
                        var tDiep301 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot.TDiep>(plainContent);

                        var dSLDKTNhan = tDiep301.DLieu.TBao.DLTBao.DSLDKTNhan;
                        if (dSLDKTNhan.Count > 0)
                        {
                            moTaLoi = "Lỗi của thông điệp<br>";
                        }
                        for (int i = 0; i < dSLDKTNhan.Count; i++)
                        {
                            var lyDoItem = dSLDKTNhan[i];
                            moTaLoi += $"- {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br>";
                        }

                        //danh sách chi tiết lý do trong các hóa đơn
                        var listLyDoTrongHoaDon = tDiep301.DLieu.TBao.DLTBao.DSHDon.Where(x => x.DSLDKTNhan.Count() > 0);

                        if (dSLDKTNhan.Count > 0 && listLyDoTrongHoaDon.Count() > 0)
                        {
                            moTaLoi += "<br>Lỗi chi tiết trong danh sách hóa đơn không được tiếp nhận:<br>";
                        }

                        for (int i = 0; i < listLyDoTrongHoaDon.Count(); i++)
                        {
                            var hoaDonItem = tDiep301.DLieu.TBao.DLTBao.DSHDon[i];
                            for (int j = 0; j < hoaDonItem.DSLDKTNhan.Count; j++)
                            {
                                var lyDoItem = hoaDonItem.DSLDKTNhan[j];
                                moTaLoi += "Ký hiệu mẫu số hóa đơn <b>" + (hoaDonItem.KHMSHDon ?? "") + "</b> Ký hiệu hóa đơn <b>" + (hoaDonItem.KHHDon ?? "") + "</b> Số <b>" + hoaDonItem.SHDon + "</b> Ngày hóa đơn <b>" + hoaDonItem.NLap.ConvertStringToDate()?.ToString("dd/MM/yyyy") + "</b><br>";
                                moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTa}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br>";
                            }
                        }

                        var thongDiepChiTiet1 = new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep301.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep301.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep301.DLieu.TBao.DLTBao.Ten,
                            SoThongBao = tDiep301.DLieu.TBao.STBao.So,
                            NgayThongBao = tDiep301.DLieu.TBao.STBao.NTBao.ConvertStringToDate(),
                            DiaDanh = tDiep301.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep301.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep301.DLieu.TBao.DLTBao.TCQT,
                            TenNguoiNopThue = tDiep301.DLieu.TBao.DLTBao.TNNT,
                            MaSoThue = tDiep301.DLieu.TBao.DLTBao.MST,
                            MaDonViQuanHeNganSach = tDiep301.DLieu.TBao.DLTBao.MDVQHNSach,
                            ThoiGianGui = null,
                            LoaiThongBao = null,
                            CanCu = null,
                            SoLuong = null,
                            MaGiaoDichDienTu = tDiep301.DLieu.TBao.DLTBao.MGDDTu,
                            ThoiGianNhan = tDiep301.DLieu.TBao.DLTBao.TGNhan.ConvertStringToDate(),
                            SoThuTuThe = tDiep301.DLieu.TBao.DLTBao.STTThe,
                            MoTaLoi = moTaLoi,
                            MaLoaiThongDiep = (int)MLTDiep.TBTNVKQXLHDDTSSot
                        };

                        result.ThongDiepChiTiet1s.Add(thongDiepChiTiet1);

                        if (tDiep301.DLieu.TBao.DLTBao.DSHDon != null && string.IsNullOrWhiteSpace(moTaLoi) == false)
                        {
                            var dSHDon = tDiep301.DLieu.TBao.DLTBao.DSHDon;
                            for (int i = 0; i < dSHDon.Count; i++)
                            {
                                moTaLoi = "";
                                var dSHDonItem = dSHDon[i];
                                for (int j = 0; j < dSHDonItem.DSLDKTNhan.Count; j++)
                                {
                                    var dSLDKTNhanItem = dSHDonItem.DSLDKTNhan[j];
                                    moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}\n";
                                }

                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = dSHDonItem.STT,
                                    MaCQTCap = dSHDonItem.MCQTCap,
                                    KyHieuMauSoHoaDon = dSHDonItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDonItem.KHHDon,
                                    SoHoaDon = dSHDonItem.SHDon,
                                    NgayLap = dSHDonItem.NLap.ConvertStringToDate(),
                                    LoaiHoaDonDienTuApDung = dSHDonItem.LADHDDT.ToString(),
                                    TinhChatThongBao = dSHDonItem.TCTBao.ToString(),
                                    TrangThaiTiepNhanCuaCQT = dSHDonItem.TTTNCCQT.ToString(),
                                    MoTaLoi = moTaLoi,
                                    MaLoaiThongDiep = (int)MLTDiep.TBTNVKQXLHDDTSSot
                                });
                            }
                        }
                        break;
                    case (int)MLTDiep.TDTBHDDTCRSoat: // 302
                        var tDiep302 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._6.TDiep>(plainContent);

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep302.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep302.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep302.DLieu.TBao.DLTBao.Ten,
                            DiaDanh = tDiep302.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep302.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep302.DLieu.TBao.DLTBao.TCQT,
                            TenNguoiNopThue = tDiep302.DLieu.TBao.DLTBao.TNNT,
                            MaSoThue = tDiep302.DLieu.TBao.DLTBao.MST,
                            MaDonViQuanHeNganSach = tDiep302.DLieu.TBao.DLTBao.MDVQHNSach,
                            DiaChiNguoiNopThue = tDiep302.DLieu.TBao.DLTBao.DCNNT,
                            DiaChiThuDienTu = tDiep302.DLieu.TBao.DLTBao.DCTDTu,
                            ThoiHan = tDiep302.DLieu.TBao.DLTBao.THan,
                            Lan = tDiep302.DLieu.TBao.DLTBao.Lan,
                        });

                        if (tDiep302.DLieu.TBao.DLTBao.DSHDon != null)
                        {
                            var dSHDon = tDiep302.DLieu.TBao.DLTBao.DSHDon;
                            for (int i = 0; i < length; i++)
                            {
                                var dSHDonItem = dSHDon[i];

                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = i + 1,
                                    KyHieuMauSoHoaDon = dSHDonItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDonItem.KHHDon,
                                    SoHoaDon = dSHDonItem.SHDon,
                                    NgayLap = DateTime.Parse(dSHDonItem.NLap),
                                    LoaiHoaDonDienTuApDung = dSHDonItem.LADHDDT.GetDescription(),
                                    LyDoCanRaSoat = dSHDonItem.LDo
                                });
                            }
                        }
                        break;
                    case (int)MLTDiep.TDTBHDDLSSot: //300
                        var tDiep300 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiep>(plainContent);
                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep300.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep300.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep300.DLieu.TBao.DLTBao.Ten,
                            LoaiThongBao = tDiep300.DLieu.TBao.DLTBao.Loai == 1 ? "Thông báo hủy/giải trình của NNT" : "Thông báo hủy/giải trình của NNT theo thông báo của CQT",
                            SoThongBao = tDiep300.DLieu.TBao.DLTBao.So,
                            NgayThongBaoCuaCQT = !string.IsNullOrEmpty(tDiep300.DLieu.TBao.DLTBao.NTBCCQT) ? DateTime.Parse(tDiep300.DLieu.TBao.DLTBao.NTBCCQT) : (DateTime?)null,
                            MaCuaCoQuanThue = tDiep300.DLieu.TBao.DLTBao.MCQT,
                            DiaDanh = tDiep300.DLieu.TBao.DLTBao.DDanh,
                            TenCQTRaThongBao = tDiep300.DLieu.TBao.DLTBao.TCQT,
                            TenNguoiNopThue = tDiep300.DLieu.TBao.DLTBao.TNNT,
                            MaSoThue = tDiep300.DLieu.TBao.DLTBao.MST,
                            NgayThongBao = !string.IsNullOrEmpty(tDiep300.DLieu.TBao.DLTBao.NTBao) ? DateTime.Parse(tDiep300.DLieu.TBao.DLTBao.NTBao) : (DateTime?)null,
                            MaDonViQuanHeNganSach = tDiep300.DLieu.TBao.DLTBao.MDVQHNSach,
                            ThoiGianGui = entity.NgayGui,
                            NgayCapNhat = entity.ModifyDate
                        });

                        if (tDiep300.DLieu.TBao.DLTBao.DSHDon != null)
                        {
                            var dSHDon = tDiep300.DLieu.TBao.DLTBao.DSHDon;
                            for (int i = 0; i < dSHDon.Count; i++)
                            {
                                var dSHDonItem = dSHDon[i];

                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = i + 1,
                                    KyHieuMauSoHoaDon = dSHDonItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDonItem.KHHDon,
                                    MaCQTCap = dSHDonItem.MCQTCap,
                                    SoHoaDon = dSHDonItem.SHDon,
                                    NgayLap = DateTime.Parse(dSHDonItem.Ngay),
                                    TinhChatThongBao = ((TCTBao)dSHDonItem.TCTBao).GetDescription(),
                                    LoaiHoaDonDienTuApDung = ((LADHDDT)dSHDonItem.LADHDDT).GetDescription(),
                                    LyDoCanRaSoat = dSHDonItem.LDo
                                });
                            }
                        }
                        break;
                    case (int)MLTDiep.TDCBTHDLHDDDTDCQThue:
                        var tDiep400 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(plainContent);
                        foreach (var it in tDiep400.DLieu)
                        {
                            result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                            {
                                PhienBan = tDiep400.TTChung.PBan,
                                MauSo = it.DLBTHop.TTChung.MSo,
                                TenThongBao = it.DLBTHop.TTChung.Ten,
                                SoThongBao = it.DLBTHop.TTChung.SBTHDLieu.ToString(),
                                LoaiKyDuLieu = it.DLBTHop.TTChung.LKDLieu,
                                KyDuLieu = it.DLBTHop.TTChung.KDLieu,
                                LanDau = it.DLBTHop.TTChung.LDau == LDau.LanDau,
                                BoSungLanThu = it.DLBTHop.TTChung.LDau == LDau.BoSung ? it.DLBTHop.TTChung.BSLThu : (int?)null,
                                NgayLap = !string.IsNullOrEmpty(it.DLBTHop.TTChung.NLap) ? DateTime.Parse(it.DLBTHop.TTChung.NLap) : (DateTime?)null,
                                TenNguoiNopThue = it.DLBTHop.TTChung.TNNT,
                                MaSoThue = it.DLBTHop.TTChung.MST,
                                HoaDonDatIn = it.DLBTHop.TTChung.HDDIn != HDDIn.HoaDonDienTu,
                                LoaiHangHoa = it.DLBTHop.TTChung.LHHoa.GetDescription(),
                                ThoiGianGui = entity.NgayGui,
                                NgayCapNhat = entity.ModifyDate
                            });
                        }

                        break;
                    case (int)MLTDiep.TDCDLTVANUQCTQThue:
                        var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(plainContent);

                        moTaLoi = string.Empty;
                        if (tDiep999.DLieu.TBao.DSLDo != null)
                        {
                            for (int j = 0; j < tDiep999.DLieu.TBao.DSLDo.Count; j++)
                            {
                                var dSLDKTNhanItem = tDiep999.DLieu.TBao.DSLDo[j];
                                moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}\n";
                            }

                        }
                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep999.TTChung.PBan,
                            TrangThaiXacNhanCuaCQT = tDiep999.DLieu.TBao.TTTNhan.GetDescription(),
                            MaLoaiThongDiep = int.Parse(tDiep999.TTChung.MLTDiep),
                            MaThongDiep = tDiep999.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep999.TTChung.MTDTChieu,
                            MaNoiGui = tDiep999.TTChung.MNGui,
                            MaNoiNhan = tDiep999.TTChung.MNNhan,
                            MoTaLoi = moTaLoi,
                            ThoiGianGui = entity.NgayGui,
                            NgayCapNhat = entity.ModifyDate,
                            ThoiGianNhan = DateTime.Parse(tDiep999.DLieu.TBao.NNhan)
                        });

                        break;
                    default:
                        break;
                }

                if ((entity.MaLoaiThongDiep == (int)MLTDiep.TDCDLHDKMDCQThue) ||
                    (entity.MaLoaiThongDiep == (int)MLTDiep.TDGHDDTTCQTCapMa))
                {
                    var thongDiep999s = await _dataContext.ThongDiepChungs.Where(x => x.MaThongDiepThamChieu == entity.MaThongDiep && x.MaLoaiThongDiep == 999)
                            .OrderBy(x => x.NgayThongBao)
                            .Select(x => x.ThongDiepChungId)
                            .ToListAsync();

                    var fileData999s = await _dataContext.FileDatas.Where(x => thongDiep999s.Contains(x.RefId)).ToListAsync();
                    foreach (var thongDiep999Id in thongDiep999s)
                    {
                        var fileDataItem = fileData999s.FirstOrDefault(x => x.RefId == thongDiep999Id);
                        if (fileDataItem != null)
                        {
                            var tDiepPHKT = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(fileDataItem.Content);

                            for (int i = 0; i < tDiepPHKT.DLieu.TBao.DSLDo.Count; i++)
                            {
                                var dSLDoItem = tDiepPHKT.DLieu.TBao.DSLDo[i];
                                moTaLoi += $"- {i + 1}. Mã lỗi: {dSLDoItem.MLoi}; Mô tả: {dSLDoItem.MTa}\n";
                            }

                            result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                            {
                                MaThongDiep = tDiepPHKT.TTChung.MTDiep,
                                MaNoiGui = tDiepPHKT.TTChung.MNGui,
                                NgayTiepNhan = DateTime.Parse(tDiepPHKT.DLieu.TBao.NNhan),
                                TrangThaiTiepNhanCuaCQT = tDiepPHKT.DLieu.TBao.TTTNhan.GetDescription(),
                                MoTaLoi = moTaLoi
                            });
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<FileReturn> ExportBangKeAsync(ThongDiepChungParams @params)
        {
            @params.PageSize = -1;
            var getAllPaging = await GetPagingThongDiepChungAsync(@params);
            var list = getAllPaging.Items;

            var hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/ThongDiep/BANG_KE_THONG_DIEP.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = list.Count;
                int begin_row = 7;

                worksheet.Cells[1, 1].Value = hoSoHDDTVM.TenDonVi;
                worksheet.Cells[2, 1].Value = hoSoHDDTVM.DiaChi;
                worksheet.Cells[4, 1].Value = @params.IsThongDiepGui == true ? "BẢNG KÊ THÔNG ĐIỆP GỬI" : "BẢNG KÊ THÔNG ĐIỆP NHẬN";

                // Add Row
                if (totalRows != 0)
                {
                    worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                }
                // Fill data
                int idx = begin_row + (totalRows == 0 ? 1 : 0);
                for (int i = 0; i < list.Count; i++)
                {
                    var _it = list[i];

                    worksheet.Cells[idx, 1].Value = i + 1;
                    worksheet.Cells[idx, 2].Value = _it.PhienBan;
                    worksheet.Cells[idx, 3].Value = _it.MaNoiGui;
                    worksheet.Cells[idx, 4].Value = _it.MaNoiNhan;
                    worksheet.Cells[idx, 5].Value = _it.MaLoaiThongDiep;
                    worksheet.Cells[idx, 6].Value = _it.MaThongDiep;
                    worksheet.Cells[idx, 7].Value = _it.MaThongDiepThamChieu;
                    worksheet.Cells[idx, 8].Value = _it.MaSoThue;
                    worksheet.Cells[idx, 9].Value = _it.SoLuong;

                    idx += 1;
                }

                worksheet.Cells[idx, 2].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"BANG_KE_THONG_DIEP_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string filePath = Path.Combine(destPath, fileName);
                package.SaveAs(new FileInfo(filePath));
                byte[] fileByte;

                if (@params.IsPrint == true)
                {
                    string pdfPath = Path.Combine(destPath, $"print-{DateTime.Now:yyyyMMddHHmmss}.pdf");
                    FileHelper.ConvertExcelToPDF(_hostingEnvironment.WebRootPath, filePath, pdfPath);
                    File.Delete(filePath);
                    fileByte = File.ReadAllBytes(pdfPath);
                    filePath = pdfPath;
                    File.Delete(filePath);
                }
                else
                {
                    fileByte = File.ReadAllBytes(filePath);
                    File.Delete(filePath);
                }

                return new FileReturn
                {
                    Bytes = fileByte,
                    ContentType = MimeTypes.GetMimeType(filePath),
                    FileName = Path.GetFileName(filePath)
                };
            }
        }

        public async Task<List<ToKhaiForBoKyHieuHoaDonViewModel>> GetListToKhaiFromBoKyHieuHoaDonAsync(ToKhaiParams toKhaiParams)
        {
            List<ToKhaiForBoKyHieuHoaDonViewModel> lstResult = new List<ToKhaiForBoKyHieuHoaDonViewModel>();

            var query = from tk in _dataContext.ToKhaiDangKyThongTins
                        join tdg in _dataContext.ThongDiepChungs on tk.Id equals tdg.IdThamChieu
                        where tk.NhanUyNhiem == (toKhaiParams.UyNhiemLapHoaDon == UyNhiemLapHoaDon.DangKy) &&
                        tdg.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
                        orderby tdg.NgayGui descending
                        select new ToKhaiForBoKyHieuHoaDonViewModel
                        {
                            ToKhaiId = tk.Id,
                            ThongDiepId = tdg.ThongDiepChungId,
                            MaThongDiepGui = tdg.MaThongDiep,
                            ThoiGianGui = tdg.NgayGui,
                            MaThongDiepNhan = tdg.MaThongDiepPhanHoi,
                            TrangThaiGui = tdg.TrangThaiGui,
                            TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdg.TrangThaiGui).GetDescription(),
                            ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_dataContext.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                            ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_dataContext.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                        };

            if (toKhaiParams.UyNhiemLapHoaDon == UyNhiemLapHoaDon.KhongDangKy)
            {
                query = query.Where(x => x.ToKhaiKhongUyNhiem != null && (toKhaiParams.HinhThucHoaDon == (HinhThucHoaDon)x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.HTHDon.CMa));

                switch (toKhaiParams.LoaiHoaDon)
                {
                    case LoaiHoaDon.HoaDonGTGT:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanHang:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanTaiSanCong:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1);
                        break;
                    case LoaiHoaDon.CacLoaiHoaDonKhac:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1);
                        break;
                    case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.CTu == 1);
                        break;
                    default:
                        break;
                }
            }

            var listToKhai = await query.ToListAsync();

            foreach (var item in listToKhai)
            {
                if (item.ToKhaiUyNhiem != null)
                {
                    var dkun = item.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSDKUNhiem.FirstOrDefault(x => x.MST == toKhaiParams.MaSoThueBenUyNhiem && x.KHMSHDon == toKhaiParams.kyHieuMauSoHoaDon && x.KHHDon == toKhaiParams.KyHieuHoaDon);
                    if (dkun != null)
                    {
                        item.STT = dkun.STT;
                        item.TenLoaiHoaDonUyNhiem = dkun.TLHDon;
                        item.KyHieuMauHoaDon = dkun.KHMSHDon;
                        item.KyHieuHoaDonUyNhiem = dkun.KHHDon;
                        item.TenToChucDuocUyNhiem = dkun.TTChuc;
                        item.MucDichUyNhiem = dkun.MDich;
                        item.ThoiGianUyNhiem = DateTime.Parse(dkun.DNgay);
                        item.PhuongThucThanhToan = (HTTToan)dkun.PThuc;
                        item.TenPhuongThucThanhToan = (((HTTToan)dkun.PThuc).GetDescription());
                    }
                }
            }

            var toKhaiMoiNhat = await query.OrderByDescending(o => o.ThoiDiemChapNhan).FirstOrDefaultAsync();
            if (toKhaiMoiNhat != null)
            {
                lstResult.Add(toKhaiMoiNhat);
            }

            return lstResult;
        }

        /// <summary>
        /// ThongKeSoLuongThongDiepAsync thống kê số lượng thông điệp chưa gửi theo điều kiện
        /// </summary>
        /// <param name="trangThaiGuiThongDiep"></param>
        /// <param name="coThongKeSoLuong"></param>
        /// <returns></returns>
        public async Task<ThongKeSoLuongThongDiepViewModel> ThongKeSoLuongThongDiepAsync(int trangThaiGuiThongDiep, byte coThongKeSoLuong)
        {
            var tuyChonKyKeKhai = (await _dataContext.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            DateTime fromDate = DateTime.Parse("2021-11-21");
            DateTime toDate = DateTime.Now;

            if (tuyChonKyKeKhai == "Thang") //ngày cuối cùng của tháng
            {
                toDate = DateTime.Now.GetLastDayOfMonth();
            }
            else if (tuyChonKyKeKhai == "Quy") //ngày cuối cùng của quý
            {
                int thang = DateTime.Now.Month;
                int nam = DateTime.Now.Year;
                if (thang <= 3)
                {
                    toDate = new DateTime(nam, 3, 1).GetLastDayOfMonth();
                }
                else if (thang > 3 && thang <= 6)
                {
                    toDate = new DateTime(nam, 6, 1).GetLastDayOfMonth();
                }
                else if (thang > 6 && thang <= 9)
                {
                    toDate = new DateTime(nam, 9, 1).GetLastDayOfMonth();
                }
                else if (thang > 9 && thang <= 12)
                {
                    toDate = new DateTime(nam, 12, 1).GetLastDayOfMonth();
                }
            }

            int thongKeSoLuong = 0;
            if (coThongKeSoLuong == 1)
            {
                thongKeSoLuong = await _dataContext.ThongDiepChungs.CountAsync(x => x.TrangThaiGui == trangThaiGuiThongDiep);
            }

            return new ThongKeSoLuongThongDiepViewModel
            {
                TuNgay = fromDate.ToString("yyyy-MM-dd"),
                DenNgay = toDate.ToString("yyyy-MM-dd"),
                SoLuong = thongKeSoLuong,
                TrangThaiGuiThongDiep = trangThaiGuiThongDiep
            };
        }

        //Method này để đánh dấu trạng thái gửi thông báo cho CQT của các hóa đơn đã lập thông báo 04/300
        private async Task CapNhatTrangThaiGui04ChoCacHoaDon(string thongDiepGuiCQTId, int trangThaiGuiCQT, List<HoaDonKhongHopLeViewModel> listHoaDonKhongHopLe, DLL.Entity.QuanLyHoaDon.ThongDiepGuiCQT thongDiepGuiCQT)
        {
            if (thongDiepGuiCQT != null)
            {
                if (thongDiepGuiCQT.IsTBaoHuyGiaiTrinhKhacCuaNNT.GetValueOrDefault())
                {
                    //nếu thông báo 04 được gửi cho hóa đơn được nhập từ pm khác
                    var listHoaDonCanDanhDau = await (from hoaDon in _dataContext.ThongTinHoaDons.AsNoTracking()
                                                      join hoaDonChiTiet in _dataContext.ThongDiepChiTietGuiCQTs.AsNoTracking() on hoaDon.Id equals hoaDonChiTiet.HoaDonDienTuId
                                                      where hoaDonChiTiet.ThongDiepGuiCQTId == thongDiepGuiCQTId
                                                      select new HoaDonDaLapThongBao04ViewModel
                                                      {
                                                          HoaDonNgoaiHeThong = hoaDon,
                                                          MauHoaDon = hoaDonChiTiet.MauHoaDon,
                                                          KyHieuHoaDon = hoaDonChiTiet.KyHieuHoaDon,
                                                          SoHoaDon = hoaDonChiTiet.SoHoaDon
                                                      }).ToListAsync();

                    if (listHoaDonCanDanhDau.Count > 0)
                    {
                        foreach (var item in listHoaDonCanDanhDau)
                        {
                            //kiểm tra hóa đơn không hợp lệ nếu có
                            if (listHoaDonKhongHopLe.Count > 0)
                            {
                                if (listHoaDonKhongHopLe.Count(x => x.MauHoaDon.TrimToUpper() == item.MauHoaDon.TrimToUpper() && x.KyHieuHoaDon.TrimToUpper() == item.KyHieuHoaDon.TrimToUpper() && x.SoHoaDon.TrimToUpper() == item.SoHoaDon.TrimToUpper()) > 0)
                                {
                                    item.HoaDonNgoaiHeThong.TrangThaiGui04 = trangThaiGuiCQT; //là hóa đơn không hợp lệ; lúc này trangThaiGuiCQT là trạng thái không hợp lệ
                                }
                                else
                                {
                                    item.HoaDonNgoaiHeThong.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon; //là hóa đơn hợp lệ
                                }
                            }
                            else
                            {
                                item.HoaDonNgoaiHeThong.TrangThaiGui04 = trangThaiGuiCQT;
                            }
                        }
                        _dataContext.ThongTinHoaDons.UpdateRange(listHoaDonCanDanhDau.Select(x => x.HoaDonNgoaiHeThong));
                    }
                }
                else
                {
                    //nếu thông báo 04 được gửi cho hóa đơn được nhập trong pm hdbk
                    var listHoaDonCanDanhDau = await (from hoaDon in _dataContext.HoaDonDienTus.AsNoTracking()
                                                      join hoaDonChiTiet in _dataContext.ThongDiepChiTietGuiCQTs.AsNoTracking() on hoaDon.HoaDonDienTuId equals hoaDonChiTiet.HoaDonDienTuId
                                                      where hoaDonChiTiet.ThongDiepGuiCQTId == thongDiepGuiCQTId
                                                      select new HoaDonDaLapThongBao04ViewModel
                                                      {
                                                          HoaDon = hoaDon,
                                                          MauHoaDon = hoaDonChiTiet.MauHoaDon,
                                                          KyHieuHoaDon = hoaDonChiTiet.KyHieuHoaDon,
                                                          SoHoaDon = hoaDonChiTiet.SoHoaDon
                                                      }).ToListAsync();

                    if (listHoaDonCanDanhDau.Count > 0)
                    {
                        foreach (var item in listHoaDonCanDanhDau)
                        {
                            //kiểm tra hóa đơn không hợp lệ nếu có
                            if (listHoaDonKhongHopLe.Count > 0)
                            {
                                if (listHoaDonKhongHopLe.Count(x => x.MauHoaDon.TrimToUpper() == item.MauHoaDon.TrimToUpper() && x.KyHieuHoaDon.TrimToUpper() == item.KyHieuHoaDon.TrimToUpper() && x.SoHoaDon.TrimToUpper() == item.SoHoaDon.TrimToUpper()) > 0)
                                {
                                    item.HoaDon.TrangThaiGui04 = trangThaiGuiCQT; //là hóa đơn không hợp lệ; lúc này trangThaiGuiCQT là trạng thái không hợp lệ
                                }
                                else
                                {
                                    item.HoaDon.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon; //là hóa đơn hợp lệ
                                }
                            }
                            else
                            {
                                item.HoaDon.TrangThaiGui04 = trangThaiGuiCQT;
                            }
                        }
                        _dataContext.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau.Select(x => x.HoaDon));
                    }
                }
            }
        }

        public async Task<string> GetXmlContentThongDiepAsync(string maThongDiep)
        {
            var thongDiep = await _dataContext.ThongDiepChungs
                .FirstOrDefaultAsync(x => x.MaThongDiep == maThongDiep);

            if (thongDiep == null)
            {
                return string.Empty;
            }

            var fileData = await _dataContext.FileDatas
                .FirstOrDefaultAsync(x => x.RefId == thongDiep.ThongDiepChungId);

            var result = fileData?.Content ?? string.Empty;
            return _xmlInvoiceService.PrintXML(result);
        }

        public Task<int> UpdateNgayThongBaoToKhaiAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method này đọc tên người ký và thời gian ký
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="loaiChuKy"></param>
        /// <returns></returns>
        private ThongTinChuKySoViewModel GetThongTinNguoiKy(XmlDocument xmlDoc, string loaiChuKy)
        {
            //loaiChuKy: là các thẻ chữ ký số trong thẻ <DSCKS>. Ở trong file này: dùng 2 loại là TTCQT và CQT

            var signature = xmlDoc.SelectNodes("/TDiep/DLieu/TBao/DSCKS/" + loaiChuKy);
            if (signature != null)
            {
                if (signature.Count > 0)
                {
                    var tenNguoiKy = "";
                    var ngayKy = "";

                    //đọc ra tên người ký
                    var node_Signature = signature[0]["Signature"];
                    var node_KeyInfo = node_Signature["KeyInfo"];
                    var node_X509Data = node_KeyInfo["X509Data"];
                    var node_X509SubjectName = node_X509Data["X509SubjectName"];
                    var chuoiTenNguoiKy = node_X509SubjectName.InnerText;
                    var tachChuoiTenNguoiKy = chuoiTenNguoiKy.Split(",");
                    var tachTenNguoiKy = tachChuoiTenNguoiKy.FirstOrDefault(x => x.Trim().StartsWith("CN="));
                    var tachTenNguoiKy2 = tachTenNguoiKy.Split("=");
                    if (tachTenNguoiKy2.Length > 0)
                    {
                        tenNguoiKy = tachTenNguoiKy2[1]; //lấy index = 1 vì tên người ký đứng sau chữ CN=
                    }

                    //đọc ra thời gian ký
                    var node_Object = node_Signature["Object"];
                    var node_SignatureProperties = node_Object["SignatureProperties"];
                    var node_SignatureProperty = node_SignatureProperties["SignatureProperty"];
                    var node_SigningTime = node_SignatureProperty["SigningTime"];
                    ngayKy = node_SigningTime.InnerText;

                    return new ThongTinChuKySoViewModel
                    {
                        TenNguoiKy = tenNguoiKy,
                        NgayKy = ngayKy
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Update thông tin hóa đơn theo thông điệp phản hồi từ cqt
        /// </summary>
        /// <param name="thongDiepGui"></param>
        /// <param name="isChapNhan"></param>
        /// <returns></returns>
        private async Task UpdateThongTinHoaDonTheoThongDiepAsync(ThongDiepChung thongDiepGui, bool isChapNhan)
        {
            if (isChapNhan) // tờ khai được chấp nhận
            {
                // get xml thông tin gửi 100
                var xmlThongDiepGui = await _dataContext.FileDatas
                    .Where(x => x.RefId == thongDiepGui.IdThamChieu)
                    .Select(x => x.Content)
                    .FirstOrDefaultAsync();

                var ngayThongBao = thongDiepGui.NgayThongBao.Value;

                // convert xml to model
                var tDiep100 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(xmlThongDiepGui);

                // get thông tin loại hóa đơn
                var thongTinLoaiHoaDons = await _dataContext.QuanLyThongTinHoaDons.ToListAsync();

                // list add sub thông tin
                var listAddSubThongTinHoaDon = new List<QuanLyThongTinHoaDon>();

                // declare hình thức hóa đơn hoặc loại hóa đơn ngừng sử dụng
                var hinhThucHoaDonNgungSuDung = HinhThucHoaDon.TatCa;
                var listLoaiHoaDonNgungSuDung = new List<LoaiHoaDon>();

                var hasChange = false;

                // update trạng thái
                foreach (var item in thongTinLoaiHoaDons)
                {
                    switch (item.TrangThaiSuDung)
                    {
                        case TrangThaiSuDung2.KhongSuDung: // Trường hợp không sử dụng mà tờ khai có đăng ký sử dụng hóa đơn => trạng thái sử dụng: Đang sử dụng + ngày bắt đầu sử dụng: NTBao
                            if ((item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CoMaCuaCoQuanThue && tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue && tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGT && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHang && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanTaiSanCong && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacLoaiHoaDonKhac && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon && tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1))
                            {
                                item.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                item.NgayBatDauSuDung = ngayThongBao;
                                hasChange = true;
                            }
                            break;
                        case TrangThaiSuDung2.DangSuDung: // Trường hợp đang sử dụng mà tờ khai không đăng ký sử dụng hóa đơn => trạng thái sử dụng: ngừng sử dụng + ngày ngừng sử dụng: NTBao
                            if ((item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CoMaCuaCoQuanThue && tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 0) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue && tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 0) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGT && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 0) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHang && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 0) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanTaiSanCong && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 0) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 0) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacLoaiHoaDonKhac && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 0) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon && tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 0))
                            {
                                item.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                item.NgayNgungSuDung = ngayThongBao;
                                hasChange = true;

                                // lưu hình thức hóa đơn hoặc loại hóa đơn ngừng sử dụng
                                switch (item.LoaiThongTinChiTiet)
                                {
                                    case LoaiThongTinChiTiet.CoMaCuaCoQuanThue:
                                        hinhThucHoaDonNgungSuDung = HinhThucHoaDon.CoMa;
                                        break;
                                    case LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue:
                                        hinhThucHoaDonNgungSuDung = HinhThucHoaDon.KhongCoMa;
                                        break;
                                    case LoaiThongTinChiTiet.HoaDonGTGT:
                                        listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.HoaDonGTGT);
                                        break;
                                    case LoaiThongTinChiTiet.HoaDonBanHang:
                                        listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.HoaDonBanHang);
                                        break;
                                    case LoaiThongTinChiTiet.HoaDonBanTaiSanCong:
                                        listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.HoaDonBanTaiSanCong);
                                        break;
                                    case LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia:
                                        listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.HoaDonBanHangDuTruQuocGia);
                                        break;
                                    case LoaiThongTinChiTiet.CacLoaiHoaDonKhac:
                                        listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.CacLoaiHoaDonKhac);
                                        break;
                                    case LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon:
                                        listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case TrangThaiSuDung2.NgungSuDung: // Trường hợp ngừng sử dụng mà tờ khai đăng ký sử dụng hóa đơn => trạng thái sử dụng: đang sử dụng + ngày ngừng sử dụng: NULL
                            if ((item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CoMaCuaCoQuanThue && tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue && tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGT && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHang && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanTaiSanCong && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacLoaiHoaDonKhac && tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1) ||
                                (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon && tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1))
                            {
                                AddThongTinHoaDonChild(thongTinLoaiHoaDons, item, listAddSubThongTinHoaDon, ngayThongBao);

                                item.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                item.NgayNgungSuDung = null;

                                hasChange = true;
                            }
                            break;
                        default:
                            break;
                    }
                }

                // add to thông tin hóa đơn
                await _dataContext.QuanLyThongTinHoaDons.AddRangeAsync(listAddSubThongTinHoaDon);

                // declare list add nhat ky xac thuc
                var listAddedNhatKyXacThuc = new List<NhatKyXacThucBoKyHieu>();

                // has change information
                if (hasChange)
                {
                    // add to nhật ký xác thực
                    var boKyHieuHoaDonNgungSuDungs = await _dataContext.BoKyHieuHoaDons
                        .Include(x => x.MauHoaDon)
                        .Where(x => x.TrangThaiSuDung != TrangThaiSuDung.HetHieuLuc && (x.HinhThucHoaDon == hinhThucHoaDonNgungSuDung || listLoaiHoaDonNgungSuDung.Contains(x.LoaiHoaDon)))
                        .ToListAsync();

                    foreach (var bkhhd in boKyHieuHoaDonNgungSuDungs)
                    {
                        // set ngừng sử dụng
                        bkhhd.TrangThaiSuDung = TrangThaiSuDung.NgungSuDung;

                        string tenLoaiNgungSuDung;

                        // Nếu ngừng sử dụng ký hiệu do ngừng sử dụng Hình thức hóa đơn và Loại hóa đơn
                        if (bkhhd.HinhThucHoaDon == hinhThucHoaDonNgungSuDung && listLoaiHoaDonNgungSuDung.Contains(bkhhd.LoaiHoaDon))
                        {
                            tenLoaiNgungSuDung = "Hình thức hóa đơn và Loại hóa đơn";
                        }
                        else
                        {
                            // Nếu ngừng sử dụng ký hiệu do ngừng sử dụng Hình thức hóa đơn
                            if (bkhhd.HinhThucHoaDon == hinhThucHoaDonNgungSuDung)
                            {
                                tenLoaiNgungSuDung = "Hình thức hóa đơn";
                            }
                            // Nếu ngừng sử dụng ký hiệu do ngừng sử dụng Loại hóa đơn
                            else
                            {
                                tenLoaiNgungSuDung = "Loại hóa đơn";
                            }
                        }

                        // save mau hoa don xac thuc to db
                        List<MauHoaDonXacThuc> mauHoaDonXacThucs = new List<MauHoaDonXacThuc>();
                        var listMauHoaDon = await _mauHoaDonService.GetListMauHoaDonXacThucAsync(bkhhd.MauHoaDonId);
                        foreach (var item in listMauHoaDon)
                        {
                            mauHoaDonXacThucs.Add(new MauHoaDonXacThuc
                            {
                                FileByte = item.FileByte,
                                FileType = item.FileType
                            });
                        }

                        listAddedNhatKyXacThuc.Add(new NhatKyXacThucBoKyHieu
                        {
                            TrangThaiSuDung = TrangThaiSuDung.NgungSuDung,
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            MauHoaDonId = bkhhd.MauHoaDonId,
                            ThongDiepId = thongDiepGui.ThongDiepChungId,
                            ThoiGianXacThuc = DateTime.Now,
                            ThoiDiemChapNhan = thongDiepGui.NgayThongBao,
                            MaThongDiepGui = thongDiepGui.MaThongDiep,
                            TenMauHoaDon = bkhhd.MauHoaDon.Ten,
                            NoiDung = tenLoaiNgungSuDung,
                            MauHoaDonXacThucs = mauHoaDonXacThucs
                        });
                    }
                }
                else
                {
                    var hinhThucHoaDon = tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 1 ? HinhThucHoaDon.CoMa : HinhThucHoaDon.KhongCoMa;

                    var listLoaiHoaDon = new List<LoaiHoaDon>();
                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonGTGT);
                    }

                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonBanHang);
                    }

                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonBanTaiSanCong);
                    }

                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonBanHangDuTruQuocGia);
                    }

                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.CacLoaiHoaDonKhac);
                    }

                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD);
                    }

                    // add to nhật ký xác thực
                    var boKyHieuHoaDaXacThucs = await _dataContext.BoKyHieuHoaDons
                        .Include(x => x.MauHoaDon)
                        .Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung) && x.HinhThucHoaDon == hinhThucHoaDon && listLoaiHoaDon.Contains(x.LoaiHoaDon))
                        .ToListAsync();

                    foreach (var bkhhd in boKyHieuHoaDaXacThucs)
                    {
                        bkhhd.ThongDiepId = thongDiepGui.ThongDiepChungId;

                        listAddedNhatKyXacThuc.Add(new NhatKyXacThucBoKyHieu
                        {
                            TrangThaiSuDung = TrangThaiSuDung.DaXacThuc,
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            MauHoaDonId = bkhhd.MauHoaDonId,
                            ThongDiepId = thongDiepGui.ThongDiepChungId,
                            ThoiGianXacThuc = DateTime.Now,
                            ThoiDiemChapNhan = thongDiepGui.NgayThongBao,
                            MaThongDiepGui = thongDiepGui.MaThongDiep,
                            TenMauHoaDon = bkhhd.MauHoaDon.Ten,
                        });
                    }
                }

                // add to nhật ký xác thực
                await _dataContext.NhatKyXacThucBoKyHieus.AddRangeAsync(listAddedNhatKyXacThuc);
            }
        }

        /// <summary>
        /// Add thông tin con
        /// </summary>
        /// <param name="listAll"></param>
        /// <param name="parentItem"></param>
        /// <param name="listCon"></param>
        private async void AddThongTinHoaDonChild(List<QuanLyThongTinHoaDon> listAll, QuanLyThongTinHoaDon parentItem, List<QuanLyThongTinHoaDon> listCon, DateTime ngayThongBao)
        {
            // get next stt
            var maxSTT = listAll
               .Where(x => ((int)x.STT) == parentItem.STT && x.LoaiThongTin == parentItem.LoaiThongTin && x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.TamNgungSuDung)
               .Select(x => x.STT)
               .DefaultIfEmpty(parentItem.STT)
               .Max(x => x);

            var nextSTT = 0D;
            if (maxSTT % 1 == 0) // so nguyen
            {
                nextSTT = maxSTT + 0.1;
            }
            else
            {
                var trunc = Math.Truncate(maxSTT);
                var dec = int.Parse(maxSTT.ToString().Replace(".", ",").Split(",")[1]);
                dec += 1;

                nextSTT = double.Parse($"{trunc},{dec}", NumberStyles.Float, CultureInfo.CreateSpecificCulture("es-ES"));
            }

            // add sub
            listCon.Add(new QuanLyThongTinHoaDon
            {
                STT = nextSTT,
                LoaiThongTin = parentItem.LoaiThongTin,
                LoaiThongTinChiTiet = LoaiThongTinChiTiet.TamNgungSuDung,
                TrangThaiSuDung = TrangThaiSuDung2.None,
                TuNgayTamNgungSuDung = parentItem.NgayNgungSuDung,
                DenNgayTamNgungSuDung = ngayThongBao
            });
        }

        /// <summary>
        /// Trả về đường dẫn file pdf thông điệp 102,103
        /// </summary>
        /// <param name="td"></param>
        /// <returns></returns>
        public async Task<KetQuaConvertPDF> ConvertThongDiepToFilePDF(ThongDiepChungViewModel td)
        {
            try
            {
                var thgChung = await GetThongDiepChungById(td.ThongDiepChungId);
                var path = string.Empty;
                var pathXML = string.Empty;
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string pdfFileName = string.Empty;
                string webRootPath = _hostingEnvironment.WebRootPath;
                string docPath = "";
                //get xml của thông điệp gửi về
                var chuKyCua_TTCQT = new ThongTinChuKySoViewModel();
                var chuKyCua_CQT = new ThongTinChuKySoViewModel();
                if (thgChung != null)
                {
                    string maThongDiep = thgChung.MaThongDiep;
                    string mst = thgChung.MaSoThue;
                    string dDanh = "";
                    string tCQTCTren = "";
                    string tCQT = "";
                    string sTBao = "";
                    string TNNT = "";
                    string CDanh = "";
                    string NTBao = "";
                    string lydo = "";
                    string THop = "";
                    //string nnt = _dataContext.HoSoHDDTs.FirstOrDefault(x => x.MaSoThue == mst).TenDonVi;
                    var strxml = _dataContext.TransferLogs.FirstOrDefault(x => x.MTDiep == maThongDiep).XMLData;
                    if (!string.IsNullOrEmpty(strxml))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(strxml);
                        chuKyCua_TTCQT = GetThongTinNguoiKy(xmlDoc, "TTCQT");
                        chuKyCua_CQT = GetThongTinNguoiKy(xmlDoc, "CQT");

                        XDocument docx = XDocument.Parse(strxml);
                        dDanh = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/DDanh") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/DDanh").Value : "";
                        NTBao = docx.XPathSelectElement("/TDiep/DLieu/TBao/STBao/NTBao") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/STBao/NTBao").Value : "";
                        tCQTCTren = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TCQTCTren") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TCQTCTren").Value : "";
                        tCQT = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TCQT") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TCQT").Value : "";
                        TNNT = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TNNT") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TNNT").Value : "";
                        CDanh = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/CDanh") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/CDanh").Value : "";

                        if (td.MaLoaiThongDiep == 102)
                        {
                            //THop = 1; // tiếp nhận tờ khai đăng ký
                            //THop = 2; // ko tiếp nhận tờ khai đăng ký
                            //THop = 3; // tiếp nhận tờ khai thay đổi thông tin
                            //THop = 4; // ko tiếp nhận tờ khai thay đổi thông tin
                            sTBao = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/So") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/So").Value : "";
                            THop = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/THop") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/THop").Value : "";
                            if (THop == "1" || THop == "3")
                            {
                                docPath = Path.Combine(webRootPath, $"docs/ThongDiep/TiepNhanToKhai.docx");
                                pdfFileName = string.Format("ThongBaoTiepNhan-102-{0}-{1}{2}", mst, maThongDiep, ".pdf");
                            }
                            else
                            {

                                lydo = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/DSLDKCNhan/LDo/MTa") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/DSLDKCNhan/LDo/MTa").Value : "";
                                docPath = Path.Combine(webRootPath, $"docs/ThongDiep/KhongTiepNhanToKhai.docx");
                                pdfFileName = string.Format("ThongBaoKhongTiepNhan-102-{0}-{1}{2}", mst, maThongDiep, ".pdf");
                            }


                        }
                        else if (td.MaLoaiThongDiep == 103)
                        {
                            sTBao = docx.XPathSelectElement("/TDiep/DLieu/TBao/STBao/So") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/STBao/So").Value : "";

                            docPath = Path.Combine(webRootPath, $"docs/ThongDiep/ChapNhanToKhai.docx");
                            pdfFileName = string.Format("ThongBaoChapNhan-103-{0}-{1}-{2}", mst, maThongDiep, ".pdf");

                        }

                        Document doc = new Document();
                        doc.LoadFromFile(docPath, Spire.Doc.FileFormat.Docx);

                        doc.Replace("<hh>", td.NgayThongBao.Value.Hour.ToString() ?? DateTime.Now.Hour.ToString(), true, true);
                        doc.Replace("<mm>", td.NgayThongBao.Value.Minute.ToString() ?? DateTime.Now.Minute.ToString(), true, true);
                        doc.Replace("<dd>", td.NgayThongBao.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                        doc.Replace("<MM>", td.NgayThongBao.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                        doc.Replace("<yyyy>", td.NgayThongBao.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);

                        doc.Replace("<dh>", dDanh ?? string.Empty, true, true);
                        doc.Replace("<nnt>", TNNT ?? string.Empty, true, true);
                        doc.Replace("<mst>", mst ?? string.Empty, true, true);
                        doc.Replace("<TCQTCTren>", tCQTCTren ?? string.Empty, true, true);
                        doc.Replace("<so>", sTBao ?? string.Empty, true, true);
                        doc.Replace("<TCQT>", tCQT ?? string.Empty, true, true);
                        doc.Replace("<mgdt>", maThongDiep ?? string.Empty, true, true);
                        doc.Replace("<lydo>", lydo ?? string.Empty, true, true);
                        //doc.Replace("<chucvu>", CDanh.ToUpper() ?? string.Empty, true, true);
                        //if (chuKyCua_TTCQT != null) ImageHelper.AddSignatureImageToDoc(doc, chuKyCua_TTCQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_TTCQT.NgayKy));
                        //if (chuKyCua_CQT != null) ImageHelper.AddSignatureImageToDoc_Buyer(doc, chuKyCua_CQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_CQT.NgayKy));

                        if (chuKyCua_TTCQT != null)
                        {
                            ImageHelper.CreateSignatureBox(doc, chuKyCua_TTCQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_TTCQT.NgayKy));
                        }

                        if (chuKyCua_CQT != null)
                        {
                            ImageHelper.CreateSignatureBox(doc, chuKyCua_TTCQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_TTCQT.NgayKy), true);
                        }

                        string fullPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}");
                        #region create folder
                        if (!Directory.Exists(fullPdfFolder))
                        {
                            Directory.CreateDirectory(fullPdfFolder);
                        }
                        else
                        {

                            string oldFilePath = Path.Combine(fullPdfFolder, pdfFileName);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                        }
                        string fullPdfFilePath = Path.Combine(fullPdfFolder, pdfFileName);
                        //doc.SaveToFile(fullPdfFilePath, Spire.Doc.FileFormat.PDF);
                        doc.SaveToPDF(fullPdfFilePath, _hostingEnvironment, LoaiNgonNgu.TiengViet);

                        path = $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}/{pdfFileName}";
                        #endregion

                        return new KetQuaConvertPDF()
                        {
                            FilePDF = path,
                            FileXML = pathXML,
                            PdfName = pdfFileName,
                            PDFBase64 = fullPdfFilePath.EncodeFile()
                        };
                    }

                }
                else
                {
                    return new KetQuaConvertPDF()
                    {
                        FilePDF = string.Empty,
                        PdfName = string.Empty,
                    };
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return null;
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepChungByMaThongDiep(string maThongDiep)
        {
            IQueryable<ThongDiepChungViewModel> queryToKhai = from tdc in _dataContext.ThongDiepChungs
                                                              select new ThongDiepChungViewModel
                                                              {
                                                                  ThongDiepChungId = tdc.ThongDiepChungId,
                                                                  MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                                                                  MaThongDiep = tdc.MaThongDiep,
                                                                  TenLoaiThongDiep = ((MLTDiep)tdc.MaLoaiThongDiep).GetDescription(),
                                                                  MaNoiGui = tdc.MaNoiGui,
                                                                  MaNoiNhan = tdc.MaNoiNhan,
                                                                  MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                                                                  MaSoThue = tdc.MaSoThue,
                                                                  ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                                                                  HinhThuc = tdc.HinhThuc,
                                                                  TenHinhThuc = tdc.HinhThuc.HasValue ? ((HThuc)tdc.HinhThuc).GetDescription() : null,
                                                                  TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                                                                  TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                                                                  NgayGui = tdc.NgayGui ?? null,
                                                                  IdThamChieu = tdc.IdThamChieu,
                                                                  CreatedDate = tdc.CreatedDate,
                                                                  ModifyDate = tdc.ModifyDate
                                                              };

            IQueryable<ThongDiepChungViewModel> query = queryToKhai;
            return await query.FirstOrDefaultAsync(x => x.MaThongDiep == maThongDiep);
        }
    }
}
