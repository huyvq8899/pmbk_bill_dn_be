using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuanLy;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.Filter;
using Services.Helper.Params.HoaDon;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.Pos;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.Pos;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.TienIch;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using TDiep103 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep;

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
        private readonly INhatKyThaoTacLoiService _nhatKyThaoTacLoiService;
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;
        private readonly IPosTransferService _posTransferService;

        private readonly List<LoaiThongDiep> TreeThongDiepNhan = new List<LoaiThongDiep>()
        {
            new LoaiThongDiep(){ LoaiThongDiepId = -99, MaLoaiThongDiep = -99, Ten = "Tất cả", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
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
            new LoaiThongDiep(){ LoaiThongDiepId = 14, MaLoaiThongDiep = -1, Ten = "-1 - Thông điệp phản hồi sai định dạng", LoaiThongDiepChaId = 9, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 15, MaLoaiThongDiep = 5, Ten = "Nhóm thông điệp chuyển dữ liệu hóa đơn điện tử do TCTN uỷ quyền cấp mã đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true  },
            new LoaiThongDiep(){ LoaiThongDiepId = 16, MaLoaiThongDiep = 505, Ten = "505 - Thông điệp cung cấp MST có thay đổi thông tin trong ngày", LoaiThongDiepChaId = 5, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 17, MaLoaiThongDiep = 506, Ten = "506 - Thông điệp cung cấp quyết định ngừng/tiếp tục sử dụng hóa đơn", LoaiThongDiepChaId = 5, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 18, MaLoaiThongDiep = 507, Ten = "507 - Thông điệp cung cấp thông tin đăng ký sử dụng hóa đơn điện tử", LoaiThongDiepChaId = 5, Level = 1 },

        };

        private readonly List<LoaiThongDiep> TreeThongDiepGui = new List<LoaiThongDiep>()
        {
            new LoaiThongDiep(){ LoaiThongDiepId = -1, MaLoaiThongDiep = -99, Ten = "Tất cả", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 0, MaLoaiThongDiep = 1, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ đăng ký, thay đổi thông tin sử dụng hóa đơn điện tử, đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 1, MaLoaiThongDiep = 100, Ten = "100 - Thông điệp gửi tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 2, MaLoaiThongDiep = 101, Ten = "101 - Thông điệp gửi tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 3, MaLoaiThongDiep = 106, Ten = "106 - Thông điệp gửi Đơn đề nghị cấp hóa đơn điện tử có mã của CQT theo từng lần phát sinh", LoaiThongDiepChaId = 1, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 4, MaLoaiThongDiep = 2, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ lập và gửi hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 5, MaLoaiThongDiep = 200, Ten = "200 - Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 6, MaLoaiThongDiep = 201, Ten = "201 - Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã theo từng lần phát sinh", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 7, MaLoaiThongDiep = 203, Ten = "203 - Thông điệp chuyển dữ liệu hóa đơn điện tử không mã đến cơ quan thuế", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 8, MaLoaiThongDiep = 206, Ten = "206 - Thông điệp gửi hóa đơn khởi tạo từ máy tính tiền đã cấp mã tới cơ quan thuế", LoaiThongDiepChaId = 2, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 9, MaLoaiThongDiep = 3, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ xử lý hóa đơn có sai sót", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 10, MaLoaiThongDiep = 300, Ten = "300 - Thông điệp thông báo về hóa đơn điện tử đã lập có sai sót", LoaiThongDiepChaId = 3, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 11, MaLoaiThongDiep = 303, Ten = "303 - Thông điệp thông báo về hóa đơn điện tử đã lập có sai sót từ máy tính tiền", LoaiThongDiepChaId = 3, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 12, MaLoaiThongDiep = 4, Ten = "Nhóm thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 13, MaLoaiThongDiep = 400, Ten = "400 - Thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = 4, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 14, MaLoaiThongDiep = 5, Ten = "Nhóm thông điệp chuyển dữ liệu hóa đơn điện tử do TCTN ủy quyền cấp mã đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 15, MaLoaiThongDiep = 500, Ten = "500 - Thông điệp chuyển dữ liệu hóa đơn điện tử do TCTN ủy quyền cấp mã đến cơ quan thuế", LoaiThongDiepChaId = 5, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 16, MaLoaiThongDiep = 9, Ten = "Nhóm thông điệp khác", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 17, MaLoaiThongDiep = 999, Ten = "999 - Thông điệp phản hồi kỹ thuật", LoaiThongDiepChaId = 9, Level = 1 },
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
            IMauHoaDonService mauHoaDonService,
            INhatKyThaoTacLoiService nhatKyThaoTacLoiService,
            INhatKyTruyCapService nhatKyTruyCapService,
            IPosTransferService posTransferService
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
            _posTransferService = posTransferService;

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
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiTCTNLoi, Name = TrangThaiGuiThongDiep.GuiTCTNLoi.GetDescription() });

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
                case (int)MLTDiep.TBHTGHDDT:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.HetThoiGianSuDungCMaMienPhi, Name = TrangThaiGuiThongDiep.HetThoiGianSuDungCMaMienPhi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.KhongConThuocTruongHopSuDungHoaDonKCMa, Name = TrangThaiGuiThongDiep.KhongConThuocTruongHopSuDungHoaDonKCMa.GetDescription() });
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
                case (int)MLTDiep.TDTBHDDTCRSoat:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh, Name = TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.QuaHanVaChuaGiaiTrinh, Name = TrangThaiGuiThongDiep.QuaHanVaChuaGiaiTrinh.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.DaGiaiTrinhKhiTrongHan, Name = TrangThaiGuiThongDiep.DaGiaiTrinhKhiTrongHan.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.DaGiaiTrinhKhiQuaHan, Name = TrangThaiGuiThongDiep.DaGiaiTrinhKhiQuaHan.GetDescription() });
                        break;
                    }
                case (int)MLTDiep.TDCBTHDLHDDDTDCQThue:
                case (int)MLTDiep.TDCDLHDKMDCQThue:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChuaGui, Name = TrangThaiGuiThongDiep.ChuaGui.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChoPhanHoi, Name = TrangThaiGuiThongDiep.ChoPhanHoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiKhongLoi, Name = TrangThaiGuiThongDiep.GuiKhongLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiLoi, Name = TrangThaiGuiThongDiep.GuiLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoHDKhongHopLe, Name = TrangThaiGuiThongDiep.CoHDKhongHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuHopLe.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe, Name = TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe.GetDescription() });

                        if (maLoaiThongDiep == (int)MLTDiep.TDCBTHDLHDDDTDCQThue)
                        {
                            result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiTCTNLoi, Name = TrangThaiGuiThongDiep.GuiTCTNLoi.GetDescription() });
                        }
                        break;
                    }
                case (int)MLTDiep.TDGHDDTTCQTCapMa:
                case (int)MLTDiep.TDGHDKTTMTT:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiKhongLoi, Name = TrangThaiGuiThongDiep.GuiKhongLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.ChoPhanHoi, Name = TrangThaiGuiThongDiep.ChoPhanHoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CQTDaCapMa, Name = TrangThaiGuiThongDiep.CQTDaCapMa.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiTCTNLoi, Name = TrangThaiGuiThongDiep.GuiTCTNLoi.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.KhongDuDieuKienCapMa, Name = TrangThaiGuiThongDiep.KhongDuDieuKienCapMa.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.GuiLoi, Name = TrangThaiGuiThongDiep.GuiLoi.GetDescription() });
                        break;
                    }
                case (int)MLTDiep.TBKQCMHDon:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CQTDaCapMa, Name = TrangThaiGuiThongDiep.CQTDaCapMa.GetDescription() });
                        break;
                    }
                case (int)MLTDiep.TBTNVKQXLHDDTSSot:
                    {
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon, Name = TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon.GetDescription() });
                        result.Add(new EnumModel { Value = (int)TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan, Name = TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan.GetDescription() });
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
                        if (File.Exists(fullXMLPath))
                        {
                            File.Delete(fullXMLPath);
                        }

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
                        if (File.Exists(fullXMLPath))
                        {
                            File.Delete(fullXMLPath);
                        }

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
                        if (File.Exists(fullXMLPath))
                        {
                            File.Delete(fullXMLPath);
                        }

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
                        if (File.Exists(fullXMLPath))
                        {
                            File.Delete(fullXMLPath);
                        }

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
                                                            && (tdc.MaLoaiThongDiep != (int)MLTDiep.TDCBTHDLHDDDTDCQThue ||
                                                            (tdc.MaLoaiThongDiep == (int)MLTDiep.TDCBTHDLHDDDTDCQThue && tdc.TrangThaiGui != (int)TrangThaiGuiThongDiep.ChuaGui))
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
                                                                ThoiHan = tdc.ThoiHan,
                                                                IdThamChieu = tdc.IdThamChieu,
                                                                CreatedDate = tdc.CreatedDate,
                                                                ModifyDate = tdc.ModifyDate,

                                                            };

                if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
                {
                    DateTime fromDate = DateTime.ParseExact(@params.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(@params.ToDate + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    query = query.Where(x => (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) >= fromDate &&
                                            (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) <= toDate);
                }

                // thông điệp nhận
                if (@params.IsThongDiepGui != true && @params.LoaiThongDiep != -99)
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

                if (@params.IsThongDiepGui == true && @params.LoaiThongDiep != -99)
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

                if (@params.TrangThaiGui != -99 && @params.TrangThaiGui != null)
                {
                    query = query.Where(x => x.TrangThaiGui == (TrangThaiGuiThongDiep)@params.TrangThaiGui);
                }

                if (@params.LocThongBaoHoaDonCanRaSoat == true)
                {
                    query = query.Where(x => x.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDTCRSoat && (x.TrangThaiGui == TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh || x.TrangThaiGui == TrangThaiGuiThongDiep.QuaHanVaChuaGiaiTrinh));
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

        /// <summary>
        /// Get thông điệp chung theo bảng TransLog
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungInTransLogsAsync(ThongDiepChungParams @params)
        {
            try
            {

                IQueryable<ThongDiepChungViewModel> query = from tdc in _dataContext.ThongDiepChungs
                                                            where tdc.ThongDiepGuiDi == @params.IsThongDiepGui
                                                            && (tdc.MaLoaiThongDiep != (int)MLTDiep.TDCBTHDLHDDDTDCQThue ||
                                                            (tdc.MaLoaiThongDiep == (int)MLTDiep.TDCBTHDLHDDDTDCQThue && tdc.TrangThaiGui != (int)TrangThaiGuiThongDiep.ChuaGui))
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
                                                                Status = tdc.Status,
                                                                SoTBaoPhanHoiCuaCQT = tdc.SoTBaoPhanHoiCuaCQT,
                                                                NgayThongBao = tdc.NgayThongBao,
                                                                ThoiHan = tdc.ThoiHan,
                                                                IdThamChieu = tdc.IdThamChieu,
                                                                CreatedDate = tdc.CreatedDate,
                                                                ModifyDate = tdc.ModifyDate,
                                                                //DataXML = tl.XMLData ?? (tl_tcn.XMLData ?? string.Empty),
                                                            };
                //if (@params.IsThongDiepGui != true)
                //{
                //    query = from q in query
                //            join tl in _dataContext.TransferLogs on q.MaThongDiep equals tl.MTDiep into tmpTrans
                //            from tl in tmpTrans.DefaultIfEmpty()
                //            join tl_tcn in _dataContext.TransferLogs on q.MaThongDiepThamChieu equals tl_tcn.MTDTChieu into tmpTrans_TCN
                //            from tl_tcn in tmpTrans_TCN.DefaultIfEmpty()
                //            select new ThongDiepChungViewModel
                //            {
                //                ThongDiepChungId = q.ThongDiepChungId,
                //                PhienBan = q.PhienBan,
                //                MaNoiGui = q.MaNoiGui,
                //                MaNoiNhan = q.MaNoiNhan,
                //                MaLoaiThongDiep = q.MaLoaiThongDiep,
                //                MaThongDiep = q.MaThongDiep,
                //                MaThongDiepThamChieu = q.MaThongDiepThamChieu,
                //                MaSoThue = q.MaSoThue,
                //                SoLuong = q.SoLuong,
                //                TenLoaiThongDiep = q.TenLoaiThongDiep,
                //                ThongDiepGuiDi = q.ThongDiepGuiDi,
                //                HinhThuc = q.HinhThuc,
                //                TenHinhThuc = q.TenHinhThuc,
                //                TrangThaiGui = q.TrangThaiGui,
                //                TenTrangThaiGui = q.TenTrangThaiGui,
                //                NgayGui = q.NgayGui,
                //                Status = q.Status,
                //                SoTBaoPhanHoiCuaCQT = q.SoTBaoPhanHoiCuaCQT,
                //                NgayThongBao = q.NgayThongBao,
                //                ThoiHan = q.ThoiHan,
                //                IdThamChieu = q.IdThamChieu,
                //                CreatedDate = q.CreatedDate,
                //                ModifyDate = q.ModifyDate,
                //                DataXML = tl.XMLData ?? (tl_tcn.XMLData ?? string.Empty),
                //            };

                //}

                if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
                {
                    DateTime fromDate = DateTime.ParseExact(@params.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(@params.ToDate + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    query = query.Where(x => (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) >= fromDate &&
                                            (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) <= toDate);
                }

                // thông điệp nhận
                if (@params.IsThongDiepGui != true && @params.LoaiThongDiep != -99)
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

                if (@params.IsThongDiepGui == true && @params.LoaiThongDiep != -99)
                {
                    var loaiThongDiep = TreeThongDiepGui.FirstOrDefault(x => x.MaLoaiThongDiep == @params.LoaiThongDiep);
                    if (loaiThongDiep.IsParent == true)
                    {
                        var maLoaiThongDieps = TreeThongDiepGui.Where(x => x.LoaiThongDiepChaId == @params.LoaiThongDiep).Select(x => x.MaLoaiThongDiep);
                        query = query.Where(x => maLoaiThongDieps.Contains(x.MaLoaiThongDiep));
                    }
                    else
                    {
                        query = query.Where(x => x.MaLoaiThongDiep == loaiThongDiep.MaLoaiThongDiep);
                    }
                }

                if (@params.TrangThaiGui != -99 && @params.TrangThaiGui != null)
                {
                    query = query.Where(x => x.TrangThaiGui == (TrangThaiGuiThongDiep)@params.TrangThaiGui);
                }

                if (@params.LocThongBaoHoaDonCanRaSoat == true)
                {
                    query = query.Where(x => x.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDTCRSoat && (x.TrangThaiGui == TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh || x.TrangThaiGui == TrangThaiGuiThongDiep.QuaHanVaChuaGiaiTrinh));
                }
                var ketQua = await query.ToListAsync();
                //if (@params.IsThongDiepGui != true)
                //{
                //    foreach (var item in ketQua)
                //    {
                //        /// Lấy dữ liệu từ XML
                //        var temp = TransferLogHelper.GetThongTinThongBao(item.DataXML, item.MaLoaiThongDiep);
                //        item.MGDDTu = temp.MGDDTu;
                //        item.SoTBao = temp.SoTBao;
                //        item.NgayTBao = temp.NgayTBao;
                //        item.MSoTBao = temp.MSoTBao;
                //    }
                //}

                if (@params.TimKiemTheo != null)
                {
                    var timKiemTheo = @params.TimKiemTheo;
                    if (!string.IsNullOrEmpty(timKiemTheo.PhienBan))
                    {
                        var keyword = timKiemTheo.PhienBan.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.PhienBan) && x.PhienBan.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaNoiGui))
                    {
                        var keyword = timKiemTheo.MaNoiGui.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.MaNoiGui) && x.MaNoiGui.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaNoiNhan))
                    {
                        var keyword = timKiemTheo.MaNoiNhan.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.MaNoiNhan) && x.MaNoiNhan.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (timKiemTheo.MaLoaiThongDiep != null)
                    {
                        var keyword = timKiemTheo.MaLoaiThongDiep;
                        ketQua = ketQua.Where(x => x.MaLoaiThongDiep == keyword.Value).ToList();
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiep))
                    {
                        var keyword = timKiemTheo.MaThongDiep.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.MaThongDiep) && x.MaThongDiep.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiepThamChieu))
                    {
                        var keyword = timKiemTheo.MaThongDiepThamChieu.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.MaThongDiepThamChieu) && x.MaThongDiepThamChieu.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                    {
                        var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.MaSoThue) && x.MaSoThue.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (timKiemTheo.SoLuong != null)
                    {
                        var keyword = timKiemTheo.SoLuong;
                        ketQua = ketQua.Where(x => x.SoLuong == keyword.Value).ToList();
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MGDDTu))
                    {
                        var keyword = timKiemTheo.MGDDTu.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.MGDDTu) && x.MGDDTu.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.SoTBao))
                    {
                        var keyword = timKiemTheo.SoTBao.ToUpper().ToTrim();
                        ketQua = ketQua.Where(x => !string.IsNullOrEmpty(x.SoTBao) && x.SoTBao.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                }

                if (@params.IsThongDiepGui != true)
                {
                    ketQua = ketQua.GroupBy(x => x.MaThongDiep).Select(x => x.First()).ToList();
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
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.PhienBan, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaNoiGui):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.MaNoiGui, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaNoiNhan):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.MaNoiNhan, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaLoaiThongDiep):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.MaLoaiThongDiep, filterCol, FilterValueType.Decimal);
                                break;
                            case nameof(@params.Filter.MaThongDiep):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.MaThongDiep, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaThongDiepThamChieu):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.MaThongDiepThamChieu, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MGDDTu):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.MGDDTu, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.SoTBao):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.SoTBao, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaSoThue):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.MaSoThue, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.SoLuong):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.SoLuong, filterCol, FilterValueType.Decimal);
                                break;
                            case nameof(@params.Filter.NgayGui):
                                ketQua = GenericFilterColumn<ThongDiepChungViewModel>.Query(ketQua, x => x.NgayGui, filterCol, FilterValueType.DateTime);
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
                        ketQua = ketQua.OrderBy(x => x.PhienBan).ToList();
                    }
                    if (@params.SortKey == "PhienBan" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.PhienBan).ToList();
                    }

                    if (@params.SortKey == "MaNoiGui" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.MaNoiGui).ToList();
                    }
                    if (@params.SortKey == "MaNoiGui" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.MaNoiGui).ToList();
                    }


                    if (@params.SortKey == "MaNoiNhan" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.MaNoiNhan).ToList();
                    }
                    if (@params.SortKey == "MaNoiNhan" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.MaNoiNhan).ToList();
                    }

                    if (@params.SortKey == "MaLoaiThongDiep" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.MaLoaiThongDiep).ToList();
                    }
                    if (@params.SortKey == "MaLoaiThongDiep" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.MaLoaiThongDiep).ToList();
                    }

                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.MaThongDiep).ToList();
                    }
                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.MaThongDiep).ToList();
                    }

                    if (@params.SortKey == "MaThongDiepThamChieu" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.MaThongDiepThamChieu).ToList();
                    }
                    if (@params.SortKey == "MaThongDiepThamChieu" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.MaThongDiepThamChieu).ToList();
                    }

                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.MaSoThue).ToList();
                    }
                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.MaSoThue).ToList();
                    }

                    if (@params.SortKey == "SoLuong" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.SoLuong).ToList();
                    }
                    if (@params.SortKey == "SoLuong" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.SoLuong).ToList();
                    }

                    if (@params.SortKey == "NgayGui" && @params.SortValue == "ascend")
                    {
                        ketQua = ketQua.OrderBy(x => x.NgayGui).ToList();
                    }
                    if (@params.SortKey == "NgayGui" && @params.SortValue == "descend")
                    {
                        ketQua = ketQua.OrderByDescending(x => x.NgayGui).ToList();
                    }

                }
                else
                {
                    ketQua = ketQua.OrderByDescending(x => x.CreatedDate).ToList();
                }
                #endregion




                //var list = await ketQua.ToListAsync();

                if (@params.PageSize == -1)
                {
                    @params.PageSize = ketQua.Count();
                }

                var paged = PagedList<ThongDiepChungViewModel>
                     .CreateWithList(ketQua, @params.PageNumber, @params.PageSize);

                if (@params.IsThongDiepGui != true)
                {
                    foreach (var item in paged.Items)
                    {
                        item.DataXML = _dataContext.TransferLogs.Where(x => x.MLTDiep == item.MaLoaiThongDiep).Select(x => x.XMLData).FirstOrDefault()
                                           ?? ((_dataContext.TransferLogs.Where(x => x.MTDTChieu == item.MaThongDiepThamChieu).Select(x => x.XMLData).FirstOrDefault())
                                           ?? string.Empty);
                        var temp = TransferLogHelper.GetThongTinThongBao(item.DataXML, item.MaLoaiThongDiep);
                        item.MGDDTu = temp.MGDDTu;
                        item.SoTBao = temp.SoTBao;
                        item.NgayTBao = temp.NgayTBao;
                        item.MSoTBao = temp.MSoTBao;
                    }
                }


                //if (paged != null)
                //{
                //    foreach (var item in paged.Items)
                //    {
                //        if (item.ThongDiepGuiDi == false && item.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDTCRSoat)
                //        {
                //            var timeExpired = item.ThoiHan.HasValue ? item.NgayThongBao.Value.AddHours(item.ThoiHan.Value) : item.NgayThongBao.Value.AddDays(2);
                //            var tDiepPhanHoi = (from tdc in _dataContext.ThongDiepChungs
                //                                where tdc.MaThongDiepThamChieu == item.MaThongDiep && tdc.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDLSSot
                //                                select tdc)
                //                                .FirstOrDefault();
                //            if (tDiepPhanHoi == null || !tDiepPhanHoi.NgayGui.HasValue)
                //            {
                //                if (DateTime.Now <= timeExpired)
                //                {
                //                    item.TrangThaiGui = TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh;
                //                }
                //                else
                //                {
                //                    item.TrangThaiGui = TrangThaiGuiThongDiep.QuaHanVaChuaGiaiTrinh;
                //                }
                //            }
                //            else
                //            {
                //                if (tDiepPhanHoi.NgayGui <= timeExpired)
                //                {
                //                    item.TrangThaiGui = TrangThaiGuiThongDiep.DaGiaiTrinhKhiTrongHan;
                //                }
                //                else
                //                {
                //                    item.TrangThaiGui = TrangThaiGuiThongDiep.DaGiaiTrinhKhiQuaHan;
                //                }
                //            }

                //            await UpdateThongDiepChung(item);

                //            item.TenTrangThaiGui = item.TrangThaiGui.GetDescription();
                //        }
                //    }
                //}
                return paged;
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
                        /// (Loại thông báo là “7- Thông báo kết quả đối chiếu sơ bộ thông tin gói dữ liệu hóa đơn khởi tạo từ máy tính tiền không hợp lệ”)
                        else if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao7)
                        {
                            return (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe;
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
            model.Status = model.Status.HasValue ? model.Status : true;
            var entity = _mp.Map<ThongDiepChung>(model);
            entity.ModifyDate = DateTime.Now;
            _dataContext.Entry<ThongDiepChung>(entity).State = EntityState.Detached;
            _dataContext.ThongDiepChungs.Update(entity);
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
            if (entity != null)
            {
                var result = _mp.Map<ThongDiepChungViewModel>(entity);
                result.TenTrangThaiGui = result.TrangThaiGui.GetDescription();
                return result;
            }

            return null;
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
                var entityTD = await _dataContext.ThongDiepChungs.AsNoTracking().FirstOrDefaultAsync(x => x.ThongDiepChungId == @params.ThongDiepId || x.MaThongDiep == @params.ThongDiepId);
                var entityTDXml = string.Empty;
                DLL.Entity.QuanLyHoaDon.ThongDiepGuiCQT thongDiepGuiCQT = null;
                if (entityTD != null && (entityTD.MaLoaiThongDiep == 300 || entityTD.MaLoaiThongDiep == 303))
                {
                    //đọc ra ThongDiepGuiCQT để biết xem thông báo 04 sẽ lập cho hóa đơn trong hay ngoài hệ thống
                    thongDiepGuiCQT = await _dataContext.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == entityTD.IdThamChieu);
                }
                else if (entityTD != null)
                {
                    entityTDXml = await _dataContext.FileDatas.Where(x => x.RefId == entityTD.ThongDiepChungId && x.Type == 1).Select(x => x.Content).FirstOrDefaultAsync();
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
                            NgayThongBao = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.NTBao),
                            SoTBaoPhanHoiCuaCQT = tDiep102.DLieu.TBao.DLTBao.So,
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
                            if (tDiep103.DLieu.TBao.DLTBao.MCCQT != null)
                            {
                                await _hoSoHDDTService.InsertMCCQTToKhaiAsync(tDiep103.DLieu, entityTD.MaThongDiep);
                            }
                        }
                        else
                        {
                            entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.KhongChapNhan;
                        }

                        entityTD.NgayThongBao = DateTime.Parse(tDiep103.DLieu.TBao.STBao.NTBao);
                        entityTD.MaThongDiepPhanHoi = tDiep103.TTChung.MTDiep;
                        _dataContext.ThongDiepChungs.Update(entityTD);

                        await _dataContext.SaveChangesAsync();

                        var tdc103 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep103.TTChung.PBan,
                            MaNoiGui = tDiep103.TTChung.MNGui,
                            MaNoiNhan = tDiep103.TTChung.MNNhan,
                            TrangThaiGui = (tDiep103.DLieu.TBao.DLTBao.TTXNCQT == TTXNCQT.ChapNhan ? 5 : 6),
                            MaLoaiThongDiep = int.Parse(tDiep103.TTChung.MLTDiep),
                            MaThongDiep = tDiep103.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep103.TTChung.MTDTChieu,
                            MaSoThue = tDiep103.TTChung.MST,
                            SoLuong = tDiep103.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            HinhThuc = 0,
                            SoTBaoPhanHoiCuaCQT = tDiep103.DLieu.TBao.STBao.So,
                            NgayThongBao = DateTime.Parse(tDiep103.DLieu.TBao.STBao.NTBao),
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
                            NgayThongBao = DateTime.Parse(tDiep104.DLieu.TBao.STBao.NTBao),
                            SoTBaoPhanHoiCuaCQT = tDiep104.DLieu.TBao.STBao.So,
                            FileXML = fileName
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc104);
                        break;
                    case (int)MLTDiep.TBHTGHDDT:
                        var tDiep105 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._13.TDiep>(@params.DataXML);

                        var tdc105 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep105.TTChung.PBan,
                            MaNoiGui = tDiep105.TTChung.MNGui,
                            MaNoiNhan = tDiep105.TTChung.MNNhan,
                            MaLoaiThongDiep = int.Parse(tDiep105.TTChung.MLTDiep),
                            TrangThaiGui = tDiep105.DLieu.TBao.DLTBao.THop == (int)THopHetHanHDDT.TruongHop1 ? (int)TrangThaiGuiThongDiep.HetThoiGianSuDungCMaMienPhi : (int)TrangThaiGuiThongDiep.KhongConThuocTruongHopSuDungHoaDonKCMa,
                            MaThongDiep = tDiep105.TTChung.MTDiep,
                            MaThongDiepThamChieu = tDiep105.TTChung.MTDTChieu,
                            MaSoThue = tDiep105.TTChung.MST,
                            SoLuong = tDiep105.TTChung.SLuong,
                            ThongDiepGuiDi = false,
                            HinhThuc = 0,
                            NgayThongBao = DateTime.Parse(tDiep105.DLieu.TBao.STBao.NTBao),
                            SoTBaoPhanHoiCuaCQT = tDiep105.DLieu.TBao.STBao.So
                        };

                        await _dataContext.ThongDiepChungs.AddAsync(tdc105);

                        if (tDiep105.DLieu.TBao.DLTBao.THop == (int)THopHetHanHDDT.TruongHop1)
                        {
                            var boKyHieuCoMas = await _dataContext.BoKyHieuHoaDons.Where(x => x.HinhThucHoaDon == HinhThucHoaDon.CoMa).ToListAsync();
                            foreach (var item in boKyHieuCoMas)
                            {
                                if (item.TrangThaiSuDung != TrangThaiSuDung.HetHieuLuc)
                                {
                                    item.TrangThaiSuDung = TrangThaiSuDung.NgungSuDung;
                                }
                                else
                                {
                                    item.TrangThaiSuDung = TrangThaiSuDung.HetHieuLuc;
                                }

                                _dataContext.BoKyHieuHoaDons.Update(item);

                                await _dataContext.NhatKyXacThucBoKyHieus.AddAsync(new NhatKyXacThucBoKyHieu
                                {
                                    BoKyHieuHoaDonId = item.BoKyHieuHoaDonId,
                                    CreatedDate = DateTime.Parse(tDiep105.DLieu.TBao.STBao.NTBao),
                                    ThoiGianXacThuc = DateTime.Now,
                                    TrangThaiSuDung = item.TrangThaiSuDung,
                                    LoaiHetHieuLuc = LoaiHetHieuLuc.HetThoiGianSuDung,
                                    MauHoaDonId = item.MauHoaDonId,

                                    TenNguoiXacThuc = "Hệ thống",
                                    MaThongDiepGui = tDiep105.TTChung.MTDiep,
                                    ThoiDiemChapNhan = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi)
                                });
                            }

                            var thongTinHoaDons = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CoMaCuaCoQuanThue).FirstOrDefaultAsync();
                            if (thongTinHoaDons.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                thongTinHoaDons.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                thongTinHoaDons.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(thongTinHoaDons);
                            }

                            var loaiHoaDonGTGT = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGT).FirstOrDefaultAsync();
                            if (loaiHoaDonGTGT.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonGTGT.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonGTGT.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonGTGT);
                            }

                            var loaiHoaDonBanHang = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHang).FirstOrDefaultAsync();
                            if (loaiHoaDonBanHang.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonBanHang.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonBanHang.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonGTGT);
                            }

                            var loaiHoaDonBanTaiSanCong = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanTaiSanCong).FirstOrDefaultAsync();
                            if (loaiHoaDonBanTaiSanCong.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonBanTaiSanCong.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonBanTaiSanCong.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonBanTaiSanCong);
                            }

                            var loaiHoaDonBanHangDuTruQG = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia).FirstOrDefaultAsync();
                            if (loaiHoaDonBanHangDuTruQG.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonBanHangDuTruQG.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonBanHangDuTruQG.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonBanHangDuTruQG);
                            }

                            var loaiHoaDonKhac = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacLoaiHoaDonKhac).FirstOrDefaultAsync();
                            if (loaiHoaDonKhac.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonKhac.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonKhac.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonKhac);
                            }

                            var loaiChungTuDcSuDungNhuHoaDon = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon).FirstOrDefaultAsync();
                            if (loaiChungTuDcSuDungNhuHoaDon.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiChungTuDcSuDungNhuHoaDon.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiChungTuDcSuDungNhuHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiChungTuDcSuDungNhuHoaDon);
                            }

                            await _dataContext.SaveChangesAsync();
                        }
                        else
                        {
                            var boKyHieuKoMas = await _dataContext.BoKyHieuHoaDons.Where(x => x.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa).ToListAsync();
                            foreach (var item in boKyHieuKoMas)
                            {
                                if (item.TrangThaiSuDung != TrangThaiSuDung.HetHieuLuc)
                                {
                                    item.TrangThaiSuDung = TrangThaiSuDung.NgungSuDung;
                                }
                                else
                                {
                                    item.TrangThaiSuDung = TrangThaiSuDung.HetHieuLuc;
                                }

                                _dataContext.BoKyHieuHoaDons.Update(item);

                                await _dataContext.NhatKyXacThucBoKyHieus.AddAsync(new NhatKyXacThucBoKyHieu
                                {
                                    BoKyHieuHoaDonId = item.BoKyHieuHoaDonId,
                                    CreatedDate = DateTime.Now,
                                    ThoiGianXacThuc = DateTime.Now,
                                    TrangThaiSuDung = item.TrangThaiSuDung,
                                    LoaiHetHieuLuc = LoaiHetHieuLuc.HetThoiGianSuDung,
                                    MauHoaDonId = item.MauHoaDonId,
                                    TenNguoiXacThuc = "Hệ thống",
                                    MaThongDiepGui = tDiep105.TTChung.MTDiep,
                                    ThoiDiemChapNhan = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi)
                                });


                                await _dataContext.SaveChangesAsync();
                            }

                            var thongTinHoaDons = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CoMaCuaCoQuanThue).FirstOrDefaultAsync();
                            if (thongTinHoaDons.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                thongTinHoaDons.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                thongTinHoaDons.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(thongTinHoaDons);
                            }

                            var loaiHoaDonGTGT = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGT).FirstOrDefaultAsync();
                            if (loaiHoaDonGTGT.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonGTGT.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonGTGT.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonGTGT);
                            }

                            var loaiHoaDonBanHang = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHang).FirstOrDefaultAsync();
                            if (loaiHoaDonBanHang.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonBanHang.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonBanHang.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonGTGT);
                            }

                            var loaiHoaDonBanTaiSanCong = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanTaiSanCong).FirstOrDefaultAsync();
                            if (loaiHoaDonBanTaiSanCong.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonBanTaiSanCong.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonBanTaiSanCong.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonBanTaiSanCong);
                            }

                            var loaiHoaDonBanHangDuTruQG = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia).FirstOrDefaultAsync();
                            if (loaiHoaDonBanHangDuTruQG.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonBanHangDuTruQG.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonBanHangDuTruQG.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonBanHangDuTruQG);
                            }

                            var loaiHoaDonKhac = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacLoaiHoaDonKhac).FirstOrDefaultAsync();
                            if (loaiHoaDonKhac.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiHoaDonKhac.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiHoaDonKhac.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiHoaDonKhac);
                            }

                            var loaiChungTuDcSuDungNhuHoaDon = await _dataContext.QuanLyThongTinHoaDons.Where(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon).FirstOrDefaultAsync();
                            if (loaiChungTuDcSuDungNhuHoaDon.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung)
                            {
                                loaiChungTuDcSuDungNhuHoaDon.NgayNgungSuDung = DateTime.Parse(tDiep105.DLieu.TBao.DLTBao.NYCCDoi);
                                loaiChungTuDcSuDungNhuHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                _dataContext.QuanLyThongTinHoaDons.Update(loaiChungTuDcSuDungNhuHoaDon);
                            }

                            await _dataContext.SaveChangesAsync();
                        }

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
                        await UpdateTrangThaiQuyTrinhHDDTAsync(@params, entityTD, MLTDiep.TBKQCMHDon, false, @params.DataXML);
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
                            else if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao7)
                            {
                                var dshdon = await (from hd in _dataContext.HoaDonDienTus
                                                    join dlhddt in _dataContext.DuLieuGuiHDDTs on hd.HoaDonDienTuId equals dlhddt.HoaDonDienTuId
                                                    join tdc in _dataContext.ThongDiepChungs on dlhddt.ThongDiepChungId equals tdc.ThongDiepChungId
                                                    where tdc.ThongDiepChungId == entityTD.ThongDiepChungId
                                                    select hd).ToListAsync();

                                if (dshdon.Count == tDiep204.DLieu.TBao.DLTBao.LHDMTTien.DSLDo.Count)
                                    entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe;
                                else
                                    entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.CoHDKhongHopLe;
                            }
                            else if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao9)
                            {
                                entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe;
                            }
                            else
                            {
                                entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe;
                            }
                        }

                        if (entityTD.MaLoaiThongDiep == (int)MLTDiep.TDCBTHDLHDDDTDCQThue)
                        {
                            var dsBTH = await GetBangTongHopByThongDiepChungId(entityTD.ThongDiepChungId);
                            if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao2)
                            {
                                foreach (var bth in dsBTH)
                                {
                                    bth.TrangThaiQuyTrinh = TrangThaiQuyTrinh_BangTongHop.BangTongHopHopLe;
                                    bth.ActionUser = @params.ActionUser;
                                    await UpdateBangTongHopAsync(bth);
                                }
                            }
                            else
                            {
                                if (tDiep204.DLieu.TBao.DLTBao.LBTHKXDau != null)
                                {
                                    if (tDiep204.DLieu.TBao.DLTBao.LTBao != LTBao.ThongBao4)
                                    {
                                        foreach (var item in tDiep204.DLieu.TBao.DLTBao.LBTHKXDau.DSBTHop)
                                        {
                                            var bth = dsBTH.Where(x => x.SoBTHDLieu == item.SBTHDLieu && item.KDLieu == x.KyDuLieu && x.LanDau == (item.LDau == LDau.LanDau) && item.BSLThu == x.BoSungLanThu).FirstOrDefault();
                                            var soHoaDonLoi = item.DSLHDon.DistinctBy(x => new { x.KHMSHDon, x.KHHDon, x.SHDon, x.Ngay }).Count();
                                            var soHoaDon = bth.ChiTiets.DistinctBy(x => new { x.MauSo, x.KyHieu, x.SoHoaDon, x.NgayHoaDon }).Count();
                                            if (soHoaDonLoi < soHoaDon)
                                            {
                                                bth.TrangThaiQuyTrinh = TrangThaiQuyTrinh_BangTongHop.BangTongHopCoHoaDonKhongHopLe;
                                            }
                                            else bth.TrangThaiQuyTrinh = TrangThaiQuyTrinh_BangTongHop.BangTongHopKhongHopLe;
                                            bth.ActionUser = @params.ActionUser;
                                            await UpdateBangTongHopAsync(bth);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in tDiep204.DLieu.TBao.DLTBao.LBTHKXDau.DSBTHop)
                                        {
                                            var bth = dsBTH.Where(x => x.SoBTHDLieu == item.SBTHDLieu && item.KDLieu == x.KyDuLieu && x.LanDau == (item.LDau == LDau.LanDau) && item.BSLThu == x.BoSungLanThu).FirstOrDefault();
                                            bth.TrangThaiQuyTrinh = TrangThaiQuyTrinh_BangTongHop.BangTongHopKhongHopLe;
                                            bth.ActionUser = @params.ActionUser;
                                            await UpdateBangTongHopAsync(bth);
                                        }
                                    }
                                }

                                if (tDiep204.DLieu.TBao.DLTBao.LBTHXDau != null)
                                {
                                    if (tDiep204.DLieu.TBao.DLTBao.LTBao != LTBao.ThongBao5)
                                    {
                                        foreach (var item in tDiep204.DLieu.TBao.DLTBao.LBTHXDau.DSBTHop)
                                        {
                                            var bth = dsBTH.Where(x => x.SoBTHDLieu == item.SBTHDLieu && item.KDLieu == x.KyDuLieu && x.LanDau == (item.LDau == LDau.LanDau) && item.BSLThu == x.BoSungLanThu).FirstOrDefault();
                                            var matHangLois = item.DSLMHang.DistinctBy(x => new { x.MHHoa, x.THHDVu }).ToList();
                                            var matHang = bth.ChiTiets.DistinctBy(x => new { x.MauSo, x.KyHieu, x.SoHoaDon, x.NgayHoaDon, x.MaHang, x.TenHang }).ToList();
                                            var soHoaDonLoi = matHang.Where(x => matHangLois.Any(o => o.MHHoa == x.MaHang && o.THHDVu == x.TenHang)).DistinctBy(x => new { x.MauSo, x.KyHieu, x.SoHoaDon, x.NgayHoaDon }).Count();
                                            var soHoaDon = bth.ChiTiets.DistinctBy(x => new { x.MauSo, x.KyHieu, x.SoHoaDon, x.NgayHoaDon }).Count();

                                            if (soHoaDonLoi < soHoaDon)
                                            {
                                                bth.TrangThaiQuyTrinh = TrangThaiQuyTrinh_BangTongHop.BangTongHopCoHoaDonKhongHopLe;
                                            }
                                            else bth.TrangThaiQuyTrinh = TrangThaiQuyTrinh_BangTongHop.BangTongHopKhongHopLe;
                                            bth.ActionUser = @params.ActionUser;
                                            await UpdateBangTongHopAsync(bth);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in tDiep204.DLieu.TBao.DLTBao.LBTHXDau.DSBTHop)
                                        {
                                            var bth = dsBTH.Where(x => x.SoBTHDLieu == item.SBTHDLieu && item.KDLieu == x.KyDuLieu && x.LanDau == (item.LDau == LDau.LanDau) && item.BSLThu == x.BoSungLanThu).FirstOrDefault();
                                            bth.TrangThaiQuyTrinh = TrangThaiQuyTrinh_BangTongHop.BangTongHopKhongHopLe;
                                            bth.ActionUser = @params.ActionUser;
                                            await UpdateBangTongHopAsync(bth);
                                        }
                                    }
                                }
                            }
                        }

                        if (entityTD.MaLoaiThongDiep == (int)MLTDiep.TDGHDKTTMTT)
                        {
                            List<ResultHoaDonMTTViewModels> ListHoaDonToPost = new List<ResultHoaDonMTTViewModels>();
                            var dshdon = await (from hd in _dataContext.HoaDonDienTus
                                                join dlhddt in _dataContext.DuLieuGuiHDDTs on hd.HoaDonDienTuId equals dlhddt.HoaDonDienTuId
                                                join tdc in _dataContext.ThongDiepChungs on dlhddt.ThongDiepChungId equals tdc.ThongDiepChungId
                                                where tdc.ThongDiepChungId == entityTD.ThongDiepChungId
                                                select hd).ToListAsync();

                            if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao2)
                            {
                                ResultHoaDonMTTViewModels HoaDonToPost = new ResultHoaDonMTTViewModels();
                                foreach (var item in dshdon)
                                {
                                    item.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonHopLe;
                                    HoaDonToPost.MaTraCuu = item.HoaDonDienTuId;
                                    HoaDonToPost.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonHopLe;
                                    HoaDonToPost.SoHoaDon = (long)item.SoHoaDon;
                                    HoaDonToPost.TrangThaiHoaDon = item.TrangThai;
                                    HoaDonToPost.BienLaiId = item.BienLaiId;
                                    HoaDonToPost.PosCustomerURL = item.PosCustomerURL;
                                    HoaDonToPost.Error = "";
                                    ListHoaDonToPost.Add(HoaDonToPost);

                                    //thêm bản ghi vào bảng xóa bỏ hóa đơn đối với cấp mã cho hóa đơn thay thế
                                    if (!string.IsNullOrWhiteSpace(item.ThayTheChoHoaDonId) && item.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaPhatHanh)
                                    {
                                        var _objHDDTBiThayThe = await _hoaDonDienTuService.GetByIdAsync(item.ThayTheChoHoaDonId);
                                        if (_objHDDTBiThayThe != null)
                                        {
                                            if (_objHDDTBiThayThe.HinhThucXoabo == null && _objHDDTBiThayThe.TrangThai != 2)
                                            {
                                                var lydoxoabo = string.IsNullOrEmpty(item.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(item.LyDoThayThe);

                                                _objHDDTBiThayThe.NgayXoaBo = DateTime.Now;
                                                _objHDDTBiThayThe.LyDoXoaBo = lydoxoabo.LyDo;
                                                _objHDDTBiThayThe.IsNotCreateThayThe = null;
                                                if (_objHDDTBiThayThe.TrangThai == 1) //hóa đơn gốc
                                                {
                                                    _objHDDTBiThayThe.HinhThucXoabo = 2;
                                                }
                                                else if (_objHDDTBiThayThe.TrangThai == 3) //hóa đơn thay thế
                                                {
                                                    _objHDDTBiThayThe.HinhThucXoabo = 5;
                                                }
                                                else
                                                {
                                                    _objHDDTBiThayThe.HinhThucXoabo = 2;
                                                }
                                                _objHDDTBiThayThe.BackUpTrangThai = _objHDDTBiThayThe.TrangThai;
                                                _objHDDTBiThayThe.SoCTXoaBo = "XHD-" + (_objHDDTBiThayThe.MauSo ?? "") + "-" + (_objHDDTBiThayThe.KyHieu ?? "") + "-" + (_objHDDTBiThayThe.SoHoaDon + "") + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                                                //nếu nó chưa bị xóa bỏ
                                                //thì thực hiện xóa bỏ hóa đơn cho nó
                                                ParamXoaBoHoaDon paramXoaBoHoaDon = new ParamXoaBoHoaDon
                                                {
                                                    HoaDon = _objHDDTBiThayThe,
                                                    OptionalSend = 1
                                                };

                                                await _hoaDonDienTuService.XoaBoHoaDon(paramXoaBoHoaDon);
                                            }
                                        }
                                    }

                                }

                            }
                            else
                            {
                                if (tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao7)
                                {
                                    var dsLyDo = tDiep204.DLieu.TBao.DLTBao.LHDMTTien.DSLDo;
                                    List<BoKyHieuHoaDon> boKyHieuHoaDons = await _dataContext.BoKyHieuHoaDons.ToListAsync();
                                    List<ReadMTLoiXML204> readMTLoiXML204 = this.GetMoTaLoiFrom204(dsLyDo);

                                    if (dshdon.Count == dsLyDo.Count)
                                    {
                                        ResultHoaDonMTTViewModels HoaDonToPost = new ResultHoaDonMTTViewModels();
                                        for (int i = 0; i < dshdon.Count(); i++)
                                        {
                                            var item = dshdon[i];
                                            BoKyHieuHoaDon boKyHieuHoaDon = boKyHieuHoaDons.FirstOrDefault(x => x.BoKyHieuHoaDonId == item.BoKyHieuHoaDonId);

                                            item.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonKhongHopLe;
                                            HoaDonToPost.MaTraCuu = item.HoaDonDienTuId;
                                            HoaDonToPost.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonKhongHopLe;
                                            HoaDonToPost.SoHoaDon = (long)item.SoHoaDon;
                                            HoaDonToPost.TrangThaiHoaDon = item.TrangThai;
                                            HoaDonToPost.BienLaiId = item.BienLaiId;
                                            HoaDonToPost.PosCustomerURL = item.PosCustomerURL;
                                            HoaDonToPost.Error = readMTLoiXML204.FirstOrDefault(x => long.Parse(x.SoHoaDon) == item.SoHoaDon && x.KyHieu == boKyHieuHoaDon.KyHieuHoaDon).MoTaLoi;
                                            ListHoaDonToPost.Add(HoaDonToPost);
                                        }
                                    }
                                    else
                                    {
                                        var mtLois = dsLyDo.Select(x => x.MTLoi).ToList();
                                        var positionKHL = new List<int>();
                                        ResultHoaDonMTTViewModels HoaDonToPost = new ResultHoaDonMTTViewModels();
                                        foreach (var mt in mtLois)
                                        {
                                            var mts = mt.Split(";");
                                            var bkh = await _dataContext.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.KyHieuMauSoHoaDon == int.Parse(mts[0]) && x.KyHieuHoaDon == mts[1]);
                                            var hoaDon = dshdon.FirstOrDefault(x => x.BoKyHieuHoaDonId == bkh.BoKyHieuHoaDonId && x.SoHoaDon == long.Parse(mts[2]));
                                            if (hoaDon != null)
                                            {
                                                dshdon[dshdon.IndexOf(hoaDon)].TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonKhongHopLe;
                                                positionKHL.Add(dshdon.IndexOf(hoaDon));
                                                HoaDonToPost.MaTraCuu = hoaDon.HoaDonDienTuId;
                                                HoaDonToPost.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonKhongHopLe;
                                                HoaDonToPost.SoHoaDon = (long)hoaDon.SoHoaDon;
                                                HoaDonToPost.TrangThaiHoaDon = hoaDon.TrangThai;
                                                HoaDonToPost.BienLaiId = hoaDon.BienLaiId;
                                                HoaDonToPost.Error = "";
                                                ListHoaDonToPost.Add(HoaDonToPost);
                                            }
                                        }

                                        var hoaDonHopLes = dshdon.Where(x => !positionKHL.Contains(dshdon.IndexOf(x))).ToList();
                                        if (hoaDonHopLes.Any())
                                        {
                                            foreach (var item in hoaDonHopLes)
                                            {
                                                dshdon[dshdon.IndexOf(item)].TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonHopLe;
                                                HoaDonToPost.MaTraCuu = item.HoaDonDienTuId;
                                                HoaDonToPost.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonHopLe;
                                                HoaDonToPost.SoHoaDon = (long)item.SoHoaDon;
                                                HoaDonToPost.TrangThaiHoaDon = item.TrangThai;
                                                HoaDonToPost.BienLaiId = item.BienLaiId;
                                                HoaDonToPost.PosCustomerURL = item.PosCustomerURL;
                                                HoaDonToPost.Error = "";
                                                ListHoaDonToPost.Add(HoaDonToPost);

                                                //thêm bản ghi vào bảng xóa bỏ hóa đơn đối với cấp mã cho hóa đơn thay thế
                                                if (!string.IsNullOrWhiteSpace(item.ThayTheChoHoaDonId) && item.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaPhatHanh)
                                                {
                                                    var _objHDDTBiThayThe = await _hoaDonDienTuService.GetByIdAsync(item.ThayTheChoHoaDonId);
                                                    if (_objHDDTBiThayThe != null)
                                                    {
                                                        if (_objHDDTBiThayThe.HinhThucXoabo == null && _objHDDTBiThayThe.TrangThai != 2)
                                                        {
                                                            var lydoxoabo = string.IsNullOrEmpty(item.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(item.LyDoThayThe);

                                                            _objHDDTBiThayThe.NgayXoaBo = DateTime.Now;
                                                            _objHDDTBiThayThe.LyDoXoaBo = lydoxoabo.LyDo;
                                                            _objHDDTBiThayThe.IsNotCreateThayThe = null;
                                                            if (_objHDDTBiThayThe.TrangThai == 1) //hóa đơn gốc
                                                            {
                                                                _objHDDTBiThayThe.HinhThucXoabo = 2;
                                                            }
                                                            else if (_objHDDTBiThayThe.TrangThai == 3) //hóa đơn thay thế
                                                            {
                                                                _objHDDTBiThayThe.HinhThucXoabo = 5;
                                                            }
                                                            else
                                                            {
                                                                _objHDDTBiThayThe.HinhThucXoabo = 2;
                                                            }
                                                            _objHDDTBiThayThe.BackUpTrangThai = _objHDDTBiThayThe.TrangThai;
                                                            _objHDDTBiThayThe.SoCTXoaBo = "XHD-" + (_objHDDTBiThayThe.MauSo ?? "") + "-" + (_objHDDTBiThayThe.KyHieu ?? "") + "-" + (_objHDDTBiThayThe.SoHoaDon + "") + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                                                            //nếu nó chưa bị xóa bỏ
                                                            //thì thực hiện xóa bỏ hóa đơn cho nó
                                                            ParamXoaBoHoaDon paramXoaBoHoaDon = new ParamXoaBoHoaDon
                                                            {
                                                                HoaDon = _objHDDTBiThayThe,
                                                                OptionalSend = 1
                                                            };

                                                            await _hoaDonDienTuService.XoaBoHoaDon(paramXoaBoHoaDon);
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }

                            _dataContext.HoaDonDienTus.UpdateRange(dshdon);
                            var rs = await _dataContext.SaveChangesAsync();
                            if (rs > 0 && ListHoaDonToPost.Count() > 0)
                            {
                               await _posTransferService.SendResponseTCTToPos(ListHoaDonToPost);
                            }
                        }

                        entityTD.NgayThongBao = DateTime.Now.Date;
                        entityTD.MaThongDiepPhanHoi = tDiep204.TTChung.MTDiep;
                        _dataContext.ThongDiepChungs.Update(entityTD);

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
                            FileXML = fileName,
                            MauSoTBaoPhanHoiCuaCQT = tDiep204.DLieu.TBao.DLTBao.MSo,
                            SoTBaoPhanHoiCuaCQT = tDiep204.DLieu.TBao.DLTBao.So,
                            NgayTBaoPhanHoiCuaCQT = DateTime.Parse(tDiep204.DLieu.TBao.DLTBao.NTBao)
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc204);

                        // update trạng thái quy trình cho hóa đơn
                        if (entityTD.MaLoaiThongDiep != 400) await UpdateTrangThaiQuyTrinhHDDTAsync(@params, entityTD, MLTDiep.TDTBKQKTDLHDon, tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao1 || tDiep204.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao3);
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
                        await UpdateTrangThaiQuyTrinhHDDTAsync(@params, entityTD, MLTDiep.TDCDLTVANUQCTQThue, tDiep999.DLieu.TBao.TTTNhan == TTTNhan.CoLoi);
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
                            FileXML = fileName,
                            MauSoTBaoPhanHoiCuaCQT = tDiep301.DLieu.TBao.DLTBao.MSo,
                            SoTBaoPhanHoiCuaCQT = tDiep301.DLieu.TBao.STBao.So,
                            NgayTBaoPhanHoiCuaCQT = DateTime.Parse(tDiep301.DLieu.TBao.STBao.NTBao)
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
                                        SoHoaDon = item.SHDon,
                                        IsTBaoHuyKhongDuocChapNhan = true
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
                            TrangThaiGui = (int)TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh,
                            HinhThuc = (int)HThuc.ChinhThuc,
                            ThoiHan = tDiep302.DLieu.TBao.DLTBao.THan,
                            NgayThongBao = DateTime.Parse(tDiep302.DLieu.TBao.STBao.NTBao),
                            FileXML = fileName,
                            MauSoTBaoPhanHoiCuaCQT = tDiep302.DLieu.TBao.DLTBao.MSo,
                            SoTBaoPhanHoiCuaCQT = tDiep302.DLieu.TBao.STBao.So,
                            NgayTBaoPhanHoiCuaCQT = DateTime.Parse(tDiep302.DLieu.TBao.STBao.NTBao)
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc302);
                        await _thongDiepGuiNhanCQTService.ThemThongBaoHoaDonRaSoat(tDiep302);
                        break;
                    case (int)MLTDiep.TDCCMSTCTDTTTNgay: // 505
                        var tDiep505 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VII._1.TDiep>(@params.DataXML);
                        ThongDiepChung tdc505 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep505.TTChung.PBan,
                            MaNoiGui = tDiep505.TTChung.MNGui,
                            MaNoiNhan = tDiep505.TTChung.MNNhan,
                            MaLoaiThongDiep = Int32.Parse(tDiep505.TTChung.MLTDiep),
                            MaThongDiep = tDiep505.TTChung.MTDiep,
                            MaSoThue = tDiep505.TTChung.MST,
                            Status = true, // tạm thời
                            ThongDiepGuiDi = false,
                            TrangThaiGui = (int)TrangThaiGuiThongDiep.ChapNhan,
                            NgayThongBao = tDiep505.DLieu.NCNhat,
                            FileXML = fileName,
                            NgayTBaoPhanHoiCuaCQT = tDiep505.DLieu.NCNhat
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc505);
                        break;
                    case (int)MLTDiep.TDCCQDNTTSDHDDTu: // 506
                        var tDiep506 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VII._2.TDiep>(@params.DataXML);
                        ThongDiepChung tdc506 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep506.TTChung.PBan,
                            MaNoiGui = tDiep506.TTChung.MNGui,
                            MaLoaiThongDiep = Int32.Parse(tDiep506.TTChung.MLTDiep),
                            MaThongDiep = tDiep506.TTChung.MTDiep,
                            MaSoThue = tDiep506.TTChung.MST,
                            Status = true, // tạm thời
                            ThongDiepGuiDi = false,
                            TrangThaiGui = (int)TrangThaiGuiThongDiep.ChapNhan,
                            NgayThongBao = tDiep506.DLieu.NCNhat,
                            FileXML = fileName,
                            NgayTBaoPhanHoiCuaCQT = tDiep506.DLieu.NCNhat,
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc506);
                        break;
                    case (int)MLTDiep.TDCCTTDKSDHDDTu: // 507
                        var tDiep507 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VII._3.TDiep>(@params.DataXML);
                        ThongDiepChung tdc507 = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiep507.TTChung.PBan,
                            MaNoiGui = tDiep507.TTChung.MNGui,
                            MaNoiNhan = tDiep507.TTChung.MNNhan,
                            MaLoaiThongDiep = Int32.Parse(tDiep507.TTChung.MLTDiep),
                            MaThongDiep = tDiep507.TTChung.MTDiep,
                            MaSoThue = tDiep507.TTChung.MST,
                            Status = true, // tạm thời
                            ThongDiepGuiDi = false,
                            NgayThongBao = tDiep507.DLieu.NCNhat,
                            FileXML = fileName,
                            NgayTBaoPhanHoiCuaCQT = tDiep507.DLieu.NCNhat,
                        };
                        await _dataContext.ThongDiepChungs.AddAsync(tdc507);
                        break;
                    case (int)MLTDiep.TDPHSDDang: //-1
                        var tDiepSDDang = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VI._3.TDiep>(@params.DataXML);
                        ThongDiepChung tdcSDDang = new ThongDiepChung
                        {
                            ThongDiepChungId = id,
                            PhienBan = tDiepSDDang.TTChung.PBan,
                            MaNoiGui = tDiepSDDang.TTChung.MNGui,
                            MaNoiNhan = tDiepSDDang.TTChung.MNNhan,
                            MaLoaiThongDiep = Int32.Parse(tDiepSDDang.TTChung.MLTDiep),
                            MaThongDiep = tDiepSDDang.TTChung.MTDiep,
                            MaThongDiepThamChieu = string.IsNullOrEmpty(tDiepSDDang.TTChung.MTDTChieu) ? tDiepSDDang.DLieu.TBao.MTDiep : tDiepSDDang.TTChung.MTDTChieu,
                            SoLuong = 1,
                            Status = true, // tạm thời
                            ThongDiepGuiDi = false,
                            FileXML = fileName,
                        };

                        entityTD.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                        _dataContext.ThongDiepChungs.Update(entityTD);
                        await _dataContext.ThongDiepChungs.AddAsync(tdcSDDang);
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
                if (result > 0 && @params.MLTDiep == (int)MLTDiep.TBKQCMHDon || @params.MLTDiep == (int)MLTDiep.TDCDLHDKMDCQThue)
                {
                    await AutoSendEmail(@params, entityTD);
                }
                return result > 0;

            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return false;
        }

        private async Task UpdateTrangThaiQuyTrinhHDDTAsync(ThongDiepPhanHoiParams @params, ThongDiepChung ttChung, MLTDiep mLTDiepPhanHoi, bool hasError, string dataXML = null)
        {
            if ((ttChung.MaLoaiThongDiep == (int)MLTDiep.TDGHDDTTCQTCapMa) || (ttChung.MaLoaiThongDiep == (int)MLTDiep.TDCDLHDKMDCQThue))
            {
                var ddghddt = await _dataContext.DuLieuGuiHDDTs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == ttChung.IdThamChieu);

                if (ddghddt != null)
                {
                    var hddt = await _dataContext.HoaDonDienTus.AsNoTracking().FirstOrDefaultAsync(x => x.HoaDonDienTuId == ddghddt.HoaDonDienTuId);
                    if (hddt != null)
                    {
                        switch (mLTDiepPhanHoi)
                        {
                            case MLTDiep.TDCDLTVANUQCTQThue:
                                if (hddt.TrangThaiQuyTrinh != ((int)TrangThaiQuyTrinh.CQTDaCapMa) && hddt.TrangThaiQuyTrinh != ((int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa))
                                {
                                    hddt.TrangThaiQuyTrinh = hasError ? ((int)TrangThaiQuyTrinh.GuiLoi) : ((int)TrangThaiQuyTrinh.GuiKhongLoi);
                                }

                                _dataContext.HoaDonDienTus.Update(hddt);
                                break;
                            case MLTDiep.TDTBKQKTDLHDon:
                                if (hasError)
                                {
                                    if ((ttChung.TrangThaiGui == (int)TrangThaiGuiThongDiep.KhongDuDieuKienCapMa) && (hddt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.CQTDaCapMa))
                                    {
                                        hddt.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa;
                                    }
                                    else if ((ttChung.TrangThaiGui == (int)TrangThaiGuiThongDiep.CoHDKhongHopLe) && (hddt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.HoaDonHopLe))
                                    {
                                        hddt.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonKhongHopLe;
                                    }
                                }
                                else
                                {
                                    hddt.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.HoaDonHopLe;
                                }

                                _dataContext.HoaDonDienTus.Update(hddt);
                                break;
                            case MLTDiep.TBKQCMHDon:
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(dataXML);
                                XmlNode node = doc.SelectSingleNode("/TDiep/DLieu/HDon/MCCQT");
                                hddt.MaCuaCQT = node.InnerText;
                                hddt.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.CQTDaCapMa;
                                hddt.ModifyDate = DateTime.Now;
                                _dataContext.HoaDonDienTus.Update(hddt);

                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        private async Task AutoSendEmail(ThongDiepPhanHoiParams @params, ThongDiepChung ttChung)
        {
            if ((ttChung.MaLoaiThongDiep == (int)MLTDiep.TDGHDDTTCQTCapMa) || (ttChung.MaLoaiThongDiep == (int)MLTDiep.TDCDLHDKMDCQThue))
            {
                var ddghddt = await _dataContext.DuLieuGuiHDDTs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == ttChung.IdThamChieu);

                if (ddghddt != null)
                {
                    // send email
                    var hddtViewModel = await _hoaDonDienTuService.GetByIdAsync(ddghddt.HoaDonDienTuId);
                    if (hddtViewModel.IsKemGuiEmail == true)
                    {
                        // cấp mã
                        string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                        var paramSendEmail = new ParamsSendMail
                        {
                            HoaDon = hddtViewModel,
                            TenNguoiNhan = hddtViewModel.TenNguoiNhanKemTheo,
                            ToMail = TextHelper.GetEmailWithType(hddtViewModel.EmailNhanKemTheo, 1),
                            CC = TextHelper.GetEmailWithType(hddtViewModel.EmailNhanKemTheo, 2),
                            BCC = TextHelper.GetEmailWithType(hddtViewModel.EmailNhanKemTheo, 3),
                            LoaiEmail = (int)LoaiEmail.ThongBaoPhatHanhHoaDon,
                            Link = host,
                            LinkTraCuu = host + "/tra-cuu-hoa-don"
                        };

                        var result = await _hoaDonDienTuService.SendEmailAsync(paramSendEmail);
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
                            moTaLoi += $"&bull; {i + 1}. Mã lỗi: {dSLDoItem.MLoi}; Mô tả: {dSLDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {dSLDoItem.HDXLy}; Ghi chú (nếu có): {dSLDoItem.GChu}<br/>";
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
                                moTaLoi += $"&bull; {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
                            }
                        }
                    }

                    //trường hợp 204 cho 300 có LHDMTTien (LTBao = 9) 
                    if (tDiep204.DLieu.TBao.DLTBao.LHDMTTien != null)
                    {
                        moTaLoi = "";
                        var dsLyDo = tDiep204.DLieu.TBao.DLTBao.LHDMTTien.DSLDo;

                        for (int j = 0; j < dsLyDo.Count; j++)
                        {
                            var lyDoItem = dsLyDo[j];
                            moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
                        }
                    }
                    //// 204 cho 300 cps LTBao = 7  danh sách các lý do không hợp lệ của hóa đơn khởi tạo từ máy tính tiền 
                    if (tDiep204.DLieu.TBao.DLTBao.KHLKhac != null)
                    {
                        moTaLoi = "";
                        var dsLyDo = tDiep204.DLieu.TBao.DLTBao.KHLKhac.DSLDo;

                        for (int j = 0; j < dsLyDo.Count; j++)
                        {
                            var lyDoItem = dsLyDo[j];
                            moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
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
                            moTaLoi += $"&bull; {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}\n";
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
                        moTaLoi = "Lỗi của thông điệp<br/>";
                    }
                    for (int i = 0; i < dSLDKTNhan.Count; i++)
                    {
                        var lyDoItem = dSLDKTNhan[i];
                        moTaLoi += $"&bull; {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
                    }

                    //danh sách chi tiết lý do trong các hóa đơn
                    var listLyDoTrongHoaDon = tDiep301.DLieu.TBao.DLTBao.DSHDon.Where(x => x.DSLDKTNhan.Count() > 0);

                    if (dSLDKTNhan.Count > 0 && listLyDoTrongHoaDon.Count() > 0)
                    {
                        moTaLoi += "<br/>Lỗi chi tiết trong danh sách hóa đơn không được tiếp nhận:<br/>";
                    }

                    for (int i = 0; i < listLyDoTrongHoaDon.Count(); i++)
                    {
                        var hoaDonItem = tDiep301.DLieu.TBao.DLTBao.DSHDon[i];
                        for (int j = 0; j < hoaDonItem.DSLDKTNhan.Count; j++)
                        {
                            var lyDoItem = hoaDonItem.DSLDKTNhan[j];
                            moTaLoi += "Ký hiệu mẫu số hóa đơn <b>" + (hoaDonItem.KHMSHDon ?? "") + "</b> Ký hiệu hóa đơn <b>" + (hoaDonItem.KHHDon ?? "") + "</b> Số <b>" + hoaDonItem.SHDon + "</b> Ngày hóa đơn <b>" + hoaDonItem.NLap.ConvertStringToDate()?.ToString("dd/MM/yyyy") + "</b> <br/>";
                            moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTa}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu} <br/>";
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
                                moTaLoi += $"&bull; {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}<br/>";
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
                            moTaLoi += $"&bull; {i + 1}. Mã lỗi: {item.MLoi}; Mô tả: {item.MTa}<br/>";
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
                            moTaLoi += $"&bull; {i + 1}. Mã lỗi: {item.MLoi}; Mô tả: {item.MTa}<br/>";
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
                                moTaLoi += $"&bull; {j + 1}. Mã lỗi: {dSLDKCNhanItem.MLoi}; Mô tả: {dSLDKCNhanItem.MTa}<br/>";
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
                                moTaLoi += $"&bull; {i + 1}. Mã lỗi: {dSLDoItem.MLoi}; Mô tả: {dSLDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {dSLDoItem.HDXLy}; Ghi chú (nếu có): {dSLDoItem.GChu}<br/>";
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
                                    moTaLoi += $"&bull; {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
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
                                moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
                            }
                        }

                        //trường hợp 204 cho 300 có KHLKhac
                        if (tDiep204.DLieu.TBao.DLTBao.LHDMTTien != null)
                        {
                            moTaLoi = "";
                            var dsLyDo = tDiep204.DLieu.TBao.DLTBao.LHDMTTien.DSLDo;

                            for (int j = 0; j < dsLyDo.Count; j++)
                            {
                                var lyDoItem = dsLyDo[j];
                                moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
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
                                    moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
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
                                    moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
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
                            //var hoaDonDienTu = await (from hddt in _dataContext.HoaDonDienTus
                            //                          join bkhhd in _dataContext.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                            //                          join dlghd in _dataContext.DuLieuGuiHDDTs on hddt.HoaDonDienTuId equals dlghd.HoaDonDienTuId
                            //                          join tlg in _dataContext.ThongDiepChungs on dlghd.DuLieuGuiHDDTId equals tlg.IdThamChieu
                            //                          select new HoaDonDienTuViewModel
                            //                          {
                            //                              HoaDonDienTuId = hddt.HoaDonDienTuId,
                            //                              MauSo = bkhhd.KyHieuMauSoHoaDon + "",
                            //                              KyHieu = bkhhd.KyHieuHoaDon,
                            //                              SoHoaDon = hddt.SoHoaDon,
                            //                              NgayHoaDon = hddt.NgayHoaDon
                            //                          })
                            //                        .GroupBy(x => x.HoaDonDienTuId)
                            //                        .Select(x => new HoaDonDienTuViewModel
                            //                        {
                            //                            HoaDonDienTuId = x.Key,
                            //                            MauSo = x.First().MauSo,
                            //                            KyHieu = x.First().KyHieu,
                            //                            SoHoaDon = x.First().SoHoaDon,
                            //                            NgayHoaDon = x.First().NgayHoaDon
                            //                        })
                            //                        .FirstOrDefaultAsync();

                            //if (hoaDonDienTu != null)
                            //{
                            //    result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                            //    {
                            //        KyHieuMauSoHoaDon = hoaDonDienTu.MauSo ?? string.Empty,
                            //        KyHieuHoaDon = hoaDonDienTu.KyHieu ?? string.Empty,
                            //        SoHoaDon = hoaDonDienTu.SoHoaDon,
                            //        NgayLap = hoaDonDienTu.NgayHoaDon,
                            //        MoTaLoi = moTaLoi
                            //    });
                            //}
                        }

                        break;
                    case (int)MLTDiep.TBTNVKQXLHDDTSSot: // 301
                        var tDiep301 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot.TDiep>(plainContent);

                        var dSLDKTNhan = tDiep301.DLieu.TBao.DLTBao.DSLDKTNhan;
                        if (dSLDKTNhan.Count > 0)
                        {
                            moTaLoi = "Lỗi của thông điệp<br/>";
                        }
                        for (int i = 0; i < dSLDKTNhan.Count; i++)
                        {
                            var lyDoItem = dSLDKTNhan[i];
                            moTaLoi += $"&bull; {i + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTLoi}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
                        }

                        //danh sách chi tiết lý do trong các hóa đơn
                        var listLyDoTrongHoaDon = tDiep301.DLieu.TBao.DLTBao.DSHDon.Where(x => x.DSLDKTNhan.Count() > 0);

                        if (dSLDKTNhan.Count > 0 && listLyDoTrongHoaDon.Count() > 0)
                        {
                            moTaLoi += "<br/>Lỗi chi tiết trong danh sách hóa đơn không được tiếp nhận:<br/>";
                        }

                        for (int i = 0; i < listLyDoTrongHoaDon.Count(); i++)
                        {
                            var hoaDonItem = tDiep301.DLieu.TBao.DLTBao.DSHDon[i];
                            for (int j = 0; j < hoaDonItem.DSLDKTNhan.Count; j++)
                            {
                                var lyDoItem = hoaDonItem.DSLDKTNhan[j];
                                moTaLoi += "Ký hiệu mẫu số hóa đơn <b>" + (hoaDonItem.KHMSHDon ?? "") + "</b> Ký hiệu hóa đơn <b>" + (hoaDonItem.KHHDon ?? "") + "</b> Số <b>" + hoaDonItem.SHDon + "</b> Ngày hóa đơn <b>" + hoaDonItem.NLap.ConvertStringToDate()?.ToString("dd/MM/yyyy") + "</b><br/>";
                                moTaLoi += $"&bull; {j + 1}. Mã lỗi: {lyDoItem.MLoi}; Mô tả: {lyDoItem.MTa}; Hướng dẫn xử lý (nếu có): {lyDoItem.HDXLy}; Ghi chú (nếu có): {lyDoItem.GChu}<br/>";
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
                                    moTaLoi += $"&bull; {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}<br/>";
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
                                moTaLoi += $"&bull; {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}<br/>";
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
                                moTaLoi += $"&bull; {i + 1}. Mã lỗi: {dSLDoItem.MLoi}; Mô tả: {dSLDoItem.MTa}<br/>";
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
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    fileByte = File.ReadAllBytes(pdfPath);
                    filePath = pdfPath;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    fileByte = File.ReadAllBytes(filePath);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
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

            //var query = from tk in _dataContext.ToKhaiDangKyThongTins
            //            join tdg in _dataContext.ThongDiepChungs on tk.Id equals tdg.IdThamChieu
            //            where tk.NhanUyNhiem == (toKhaiParams.UyNhiemLapHoaDon == UyNhiemLapHoaDon.DangKy) &&
            //            tdg.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
            //            orderby tdg.NgayGui descending
            //            select new ToKhaiForBoKyHieuHoaDonViewModel
            //            {
            //                ToKhaiId = tk.Id,
            //                ThongDiepId = tdg.ThongDiepChungId,
            //                MaThongDiepGui = tdg.MaThongDiep,
            //                ThoiGianGui = tdg.NgayGui,
            //                MaThongDiepNhan = tdg.MaThongDiepPhanHoi,
            //                TrangThaiGui = tdg.TrangThaiGui,
            //                TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdg.TrangThaiGui).GetDescription(),
            //                ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_dataContext.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
            //                ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_dataContext.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
            //            };

            //if (toKhaiParams.UyNhiemLapHoaDon == UyNhiemLapHoaDon.KhongDangKy)
            //{
            //    query = query.Where(x => x.ToKhaiKhongUyNhiem != null && (toKhaiParams.HinhThucHoaDon == (HinhThucHoaDon)x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.HTHDon.CMa));

            //    switch (toKhaiParams.LoaiHoaDon)
            //    {
            //        case LoaiHoaDon.HoaDonGTGT:
            //            query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1);
            //            break;
            //        case LoaiHoaDon.HoaDonBanHang:
            //            query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1);
            //            break;
            //        case LoaiHoaDon.HoaDonBanTaiSanCong:
            //            query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1);
            //            break;
            //        case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
            //            query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1);
            //            break;
            //        case LoaiHoaDon.CacLoaiHoaDonKhac:
            //            query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1);
            //            break;
            //        case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
            //            query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.CTu == 1);
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //var listToKhai = await query.ToListAsync();

            //foreach (var item in listToKhai)
            //{
            //    if (item.ToKhaiUyNhiem != null)
            //    {
            //        var dkun = item.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSDKUNhiem.FirstOrDefault(x => x.MST == toKhaiParams.MaSoThueBenUyNhiem && x.KHMSHDon == toKhaiParams.kyHieuMauSoHoaDon && x.KHHDon == toKhaiParams.KyHieuHoaDon);
            //        if (dkun != null)
            //        {
            //            item.STT = dkun.STT;
            //            item.TenLoaiHoaDonUyNhiem = dkun.TLHDon;
            //            item.KyHieuMauHoaDon = dkun.KHMSHDon;
            //            item.KyHieuHoaDonUyNhiem = dkun.KHHDon;
            //            item.TenToChucDuocUyNhiem = dkun.TTChuc;
            //            item.MucDichUyNhiem = dkun.MDich;
            //            item.ThoiGianUyNhiem = DateTime.Parse(dkun.DNgay);
            //            item.PhuongThucThanhToan = (HTTToan)dkun.PThuc;
            //            item.TenPhuongThucThanhToan = (((HTTToan)dkun.PThuc).GetDescription());
            //        }
            //    }
            //}

            //var toKhaiMoiNhat = await query.OrderByDescending(o => o.ThoiDiemChapNhan).FirstOrDefaultAsync();
            //if (toKhaiMoiNhat != null)
            //{
            //    lstResult.Add(toKhaiMoiNhat);
            //}

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

            DateTime? fromDate = DateTime.Parse("2021-11-21");
            DateTime? toDate = DateTime.Now;

            if (!fromDate.HasValue || !toDate.HasValue)
            {
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
            }
            else
            {
                toDate = toDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            int thongKeSoLuong = 0;
            if (coThongKeSoLuong == 1)
            {
                if (trangThaiGuiThongDiep == (int)TrangThaiGuiThongDiep.ChuaGui)
                {
                    thongKeSoLuong = await _dataContext.ThongDiepChungs.Where(x => x.MaLoaiThongDiep != (int)MLTDiep.TDCBTHDLHDDDTDCQThue && x.TrangThaiGui == trangThaiGuiThongDiep && x.CreatedDate >= fromDate && x.CreatedDate <= toDate).CountAsync();
                }
                else thongKeSoLuong = await _dataContext.ThongDiepChungs.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate).CountAsync(x => x.TrangThaiGui == trangThaiGuiThongDiep);
            }

            return new ThongKeSoLuongThongDiepViewModel
            {
                TuNgay = fromDate.Value.ToString("yyyy-MM-dd"),
                DenNgay = toDate.Value.ToString("yyyy-MM-dd"),
                SoLuong = thongKeSoLuong,
                TrangThaiGuiThongDiep = trangThaiGuiThongDiep
            };
        }

        /// <summary>
        /// ThongKeSoLuongThongDiepAsync thống kê số lượng thông điệp hóa đơn cần rà soát (302) theo điều kiện
        /// </summary>
        /// <param name="trangThaiGuiThongDiep"></param>
        /// <param name="coThongKeSoLuong"></param>
        /// <returns></returns>
        public async Task<ThongKeSoLuongThongDiepViewModel> ThongKeSoLuongThongDiepRaSoatAsync(byte coThongKeSoLuong)
        {
            var tuyChonKyKeKhai = (await _dataContext.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            DateTime? fromDate = DateTime.Parse("2021-11-21");
            DateTime? toDate = DateTime.Now;

            if (!fromDate.HasValue || !toDate.HasValue)
            {
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
            }
            else
            {
                toDate = toDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            int thongKeSoLuong = 0;
            if (coThongKeSoLuong == 1)
            {
                thongKeSoLuong = await _dataContext.ThongDiepChungs.Where(x => x.NgayThongBao >= fromDate && x.NgayThongBao <= toDate && x.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDTCRSoat)
                                                    .CountAsync(x => x.TrangThaiGui == (int)TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh || x.TrangThaiGui == (int)TrangThaiGuiThongDiep.QuaHanVaChuaGiaiTrinh);
            }

            return new ThongKeSoLuongThongDiepViewModel
            {
                TuNgay = fromDate.Value.ToString("yyyy-MM-dd"),
                DenNgay = toDate.Value.ToString("yyyy-MM-dd"),
                SoLuong = thongKeSoLuong
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
                            if (item.HoaDon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon && item.HoaDon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe)
                            {
                                //kiểm tra hóa đơn không hợp lệ nếu có
                                if (listHoaDonKhongHopLe.Count > 0)
                                {
                                    if (listHoaDonKhongHopLe.Count(x => x.MauHoaDon.TrimToUpper() == item.MauHoaDon.TrimToUpper() && x.KyHieuHoaDon.TrimToUpper() == item.KyHieuHoaDon.TrimToUpper() && x.SoHoaDon.TrimToUpper() == item.SoHoaDon.TrimToUpper()) > 0)
                                    {
                                        item.HoaDon.TrangThaiGui04 = trangThaiGuiCQT; //là hóa đơn không hợp lệ; lúc này trangThaiGuiCQT là trạng thái không hợp lệ
                                        item.HoaDon.IsTBaoHuyKhongDuocChapNhan = listHoaDonKhongHopLe.Where(x => x.MauHoaDon.TrimToUpper() == item.MauHoaDon.TrimToUpper() && x.KyHieuHoaDon.TrimToUpper() == item.KyHieuHoaDon.TrimToUpper() && x.SoHoaDon.TrimToUpper() == item.SoHoaDon.TrimToUpper()).OrderByDescending(x => x.IsTBaoHuyKhongDuocChapNhan).Select(x => x.IsTBaoHuyKhongDuocChapNhan).FirstOrDefault();
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
            try
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
                                    (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon && tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1) ||
                                    (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGTCMTMTTien && tDiep100.DLTKhai.NDTKhai.HTHDon.CMTMTTien == 1))
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
                                    (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon && tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 0) ||
                                            (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGTCMTMTTien && tDiep100.DLTKhai.NDTKhai.HTHDon.CMTMTTien == 0) ||
                                    (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangCMTMTTien && tDiep100.DLTKhai.NDTKhai.HTHDon.CMTMTTien == 0))
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
                                            listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.PXKKiemVanChuyenNoiBo);
                                            listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.PXKHangGuiBanDaiLy);
                                            break;
                                        case LoaiThongTinChiTiet.HoaDonGTGTCMTMTTien:
                                            listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.HoaDonBanHangCMTMTT);
                                            listLoaiHoaDonNgungSuDung.Add(LoaiHoaDon.HoaDonGTGTCMTMTT);
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
                                    (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon && tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1) ||
                                             (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGTCMTMTTien && tDiep100.DLTKhai.NDTKhai.HTHDon.CMTMTTien == 1) ||
                                    (item.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangCMTMTTien && tDiep100.DLTKhai.NDTKhai.HTHDon.CMTMTTien == 1))
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
                            .Where(x => x.TrangThaiSuDung != TrangThaiSuDung.HetHieuLuc && (x.HinhThucHoaDon == hinhThucHoaDonNgungSuDung || listLoaiHoaDonNgungSuDung.Contains(x.LoaiHoaDon)))
                            .ToListAsync();

                        var listMauHoaDonIdFromBKH = boKyHieuHoaDonNgungSuDungs
                            .Select(x => x.MauHoaDonId.Split(";").ToList())
                            .SelectMany(x => x)
                            .Distinct()
                            .ToList();

                        var mauHoaDons = await _dataContext.MauHoaDons
                            .Where(x => listMauHoaDonIdFromBKH.Contains(x.MauHoaDonId))
                            .ToDictionaryAsync(x => x.MauHoaDonId);

                        foreach (var bkhhd in boKyHieuHoaDonNgungSuDungs)
                        {
                            var tenMauHoaDons = new List<string>();
                            var mauHoaDonIds = bkhhd.MauHoaDonId.Split(";").ToList();
                            foreach (var mauHoaDonId in mauHoaDonIds)
                            {
                                tenMauHoaDons.Add(mauHoaDons[mauHoaDonId].Ten);
                            }

                            // Nếu khác ngừng sử dụng thì mới vào nhật ký xác thực
                            if (bkhhd.TrangThaiSuDung != TrangThaiSuDung.NgungSuDung)
                            {
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
                                        MauHoaDonId = item.MauHoaDonId,
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
                                    TenMauHoaDon = string.Join(";", tenMauHoaDons),
                                    NoiDung = tenLoaiNgungSuDung,
                                    MauHoaDonXacThucs = mauHoaDonXacThucs
                                });
                            }

                            // set ngừng sử dụng
                            bkhhd.TrangThaiSuDung = TrangThaiSuDung.NgungSuDung;
                        }
                    }

                    // update tờ khai mới nhất cho bộ ký hiệu
                    #region update tờ khai mới nhất cho bộ ký hiệu đang dùng
                    var hinhThucHoaDon = tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 1 ? HinhThucHoaDon.CoMa : HinhThucHoaDon.KhongCoMa;
                    var IsCoMaTuMayTinhTien = tDiep100.DLTKhai.NDTKhai.HTHDon.CMTMTTien == 1;
                    var listLoaiHoaDon = new List<LoaiHoaDon>();

                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonGTGT);
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonGTGTCMTMTT);
                    }

                    if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1)
                    {
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonBanHang);
                        listLoaiHoaDon.Add(LoaiHoaDon.HoaDonBanHangCMTMTT);
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
                        listLoaiHoaDon.Add(LoaiHoaDon.PXKKiemVanChuyenNoiBo);
                        listLoaiHoaDon.Add(LoaiHoaDon.PXKHangGuiBanDaiLy);
                    }

                    var isChuyenDayDuNoiDungTungHoaDon = tDiep100.DLTKhai.NDTKhai.PThuc.CDDu == 1;
                    var isChuyenBangTonghop = tDiep100.DLTKhai.NDTKhai.PThuc.CBTHop == 1;

                    // add to nhật ký xác thực
                    var boKyHieuHoaDaXacThucs = await _dataContext.BoKyHieuHoaDons
                        .Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung) && x.HinhThucHoaDon == hinhThucHoaDon && listLoaiHoaDon.Contains(x.LoaiHoaDon))
                        .ToListAsync();
                    if (IsCoMaTuMayTinhTien)
                    {
                        var boKyHieuHoaDaXacThucsMTT = await _dataContext.BoKyHieuHoaDons
                             .Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung) && x.HinhThucHoaDon == HinhThucHoaDon.CoMaTuMayTinhTien && listLoaiHoaDon.Contains(x.LoaiHoaDon))
                              .ToListAsync();
                        if (boKyHieuHoaDaXacThucsMTT.Count() > 0)
                        {
                            boKyHieuHoaDaXacThucs = boKyHieuHoaDaXacThucsMTT.Union(boKyHieuHoaDaXacThucs).ToList();
                        }
                    }


                    var listMauHoaDonIdFromBKHXacThuc = boKyHieuHoaDaXacThucs
                        .Select(x => x.MauHoaDonId.Split(";").ToList())
                        .SelectMany(x => x)
                        .Distinct()
                        .ToList();

                    var mauHoaDonForBKHXacThucs = await _dataContext.MauHoaDons
                            .Where(x => listMauHoaDonIdFromBKHXacThuc.Contains(x.MauHoaDonId))
                            .ToDictionaryAsync(x => x.MauHoaDonId);

                    foreach (var bkhhd in boKyHieuHoaDaXacThucs)
                    {
                        bkhhd.ThongDiepId = thongDiepGui.ThongDiepChungId;
                        bkhhd.ThongDiepMoiNhatId = thongDiepGui.ThongDiepChungId;

                        var tenMauHoaDons = new List<string>();
                        var mauHoaDonIds = bkhhd.MauHoaDonId.Split(";").ToList();
                        foreach (var mauHoaDonId in mauHoaDonIds)
                        {
                            tenMauHoaDons.Add(mauHoaDonForBKHXacThucs[mauHoaDonId].Ten);
                        }

                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 1)
                        {
                            // Nếu tờ khai mới nhất không cùng phương thức với bộ ký hiệu thì update phương thức tờ khai cho bộ ký hiệu
                            if (!((bkhhd.PhuongThucChuyenDL == PhuongThucChuyenDL.CDDu && isChuyenDayDuNoiDungTungHoaDon) || (bkhhd.PhuongThucChuyenDL == PhuongThucChuyenDL.CBTHop && isChuyenBangTonghop)))
                            {
                                bkhhd.PhuongThucChuyenDL = tDiep100.DLTKhai.NDTKhai.PThuc.CDDu == 1 ? PhuongThucChuyenDL.CDDu : PhuongThucChuyenDL.CBTHop;
                            }
                        }
                        else
                        {
                            bkhhd.PhuongThucChuyenDL = PhuongThucChuyenDL.CDDu;
                        }

                        listAddedNhatKyXacThuc.Add(new NhatKyXacThucBoKyHieu
                        {
                            TrangThaiSuDung = TrangThaiSuDung.DaXacThuc,
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            MauHoaDonId = bkhhd.MauHoaDonId,
                            ThongDiepId = thongDiepGui.ThongDiepChungId,
                            ThoiGianXacThuc = DateTime.Now,
                            ThoiDiemChapNhan = thongDiepGui.NgayThongBao,
                            MaThongDiepGui = thongDiepGui.MaThongDiep,
                            TenMauHoaDon = string.Join(";", tenMauHoaDons),
                            PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL
                        });
                    }
                    #endregion

                    // add to nhật ký xác thực
                    await _dataContext.NhatKyXacThucBoKyHieus.AddRangeAsync(listAddedNhatKyXacThuc);


                }
            }
            catch (Exception ex)
            {

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
                    string MGDDTu = "";
                    //string nnt = _dataContext.HoSoHDDTs.FirstOrDefault(x => x.MaSoThue == mst).TenDonVi;
                    var strxml = _dataContext.TransferLogs.FirstOrDefault(x => x.MTDiep == maThongDiep && x.MTDTChieu == td.MaThongDiepThamChieu).XMLData;
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
                        tCQT = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TCQT") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TCQT").Value : "cơ quan thuế quản lý";
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
                            var tTXNCQT = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TTXNCQT") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/TTXNCQT").Value : "";
                            sTBao = docx.XPathSelectElement("/TDiep/DLieu/TBao/STBao/So") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/STBao/So").Value : "";
                            if (tTXNCQT == "1")
                            {
                                docPath = Path.Combine(webRootPath, $"docs/ThongDiep/ChapNhanToKhai.docx");
                                pdfFileName = string.Format("ThongBaoChapNhan-103-{0}-{1}-{2}", mst, maThongDiep, ".pdf");
                            }
                            else
                            {
                                //không chập nhận tờ khai
                                lydo = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/DSLDKCNhan/LDo/MTa") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/DSLDKCNhan/LDo/MTa").Value : "";

                                docPath = Path.Combine(webRootPath, $"docs/ThongDiep/KhongChapNhanToKhai.docx");
                                pdfFileName = string.Format("ThongBaoChapNhan-103-{0}-{1}-{2}", mst, maThongDiep, ".pdf");
                            }


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

                        /// Sửa lại mã giao dịch điện tử trên thông điệp 102
                        if (td.MaLoaiThongDiep == 102)
                        {
                            MGDDTu = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/MGDDTu") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/MGDDTu").Value : "";
                            doc.Replace("<mgdt>", MGDDTu ?? string.Empty, true, true);
                        }
                        else
                        {
                            doc.Replace("<mgdt>", maThongDiep ?? string.Empty, true, true);
                        }
                        doc.Replace("<lydo>", lydo ?? string.Empty, true, true);
                        doc.Replace("<chucvu>", tCQT.ToUpper() ?? string.Empty, true, true);
                        //if (chuKyCua_TTCQT != null) ImageHelper.AddSignatureImageToDoc(doc, chuKyCua_TTCQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_TTCQT.NgayKy));
                        //if (chuKyCua_CQT != null) ImageHelper.AddSignatureImageToDoc_Buyer(doc, chuKyCua_CQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_CQT.NgayKy));

                        if (chuKyCua_TTCQT != null)
                        {
                            ImageHelper.CreateSignatureBox(doc, chuKyCua_TTCQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_TTCQT.NgayKy));
                        }

                        if (chuKyCua_CQT != null)
                        {
                            ImageHelper.CreateSignatureBox(doc, chuKyCua_CQT.TenNguoiKy, LoaiNgonNgu.TiengViet, DateTime.Parse(chuKyCua_CQT.NgayKy), true);
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
                        /// set font cho file xuất ra
                        doc.FormatBeforeSavePdf("Times New Roman", 0);
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

        private async Task<List<BangTongHopDuLieuHoaDonViewModel>> GetBangTongHopByThongDiepChungId(string thongDiepChungId)
        {
            IQueryable<BangTongHopDuLieuHoaDonViewModel> query = from bth in _dataContext.BangTongHopDuLieuHoaDons
                                                                 where bth.ThongDiepChungId == thongDiepChungId
                                                                 select new BangTongHopDuLieuHoaDonViewModel
                                                                 {
                                                                     Id = bth.Id,
                                                                     PhienBan = bth.PhienBan,
                                                                     MauSo = bth.MauSo,
                                                                     Ten = bth.Ten,
                                                                     SoBTHDLieu = bth.SoBTHDLieu,
                                                                     LoaiKyDuLieu = bth.LoaiKyDuLieu,
                                                                     KyDuLieu = bth.KyDuLieu,
                                                                     NamDuLieu = bth.NamDuLieu,
                                                                     ThangDuLieu = bth.ThangDuLieu,
                                                                     QuyDuLieu = bth.QuyDuLieu,
                                                                     NgayDuLieu = bth.NgayDuLieu,
                                                                     LanDau = bth.LanDau,
                                                                     BoSungLanThu = bth.BoSungLanThu,
                                                                     NgayLap = bth.NgayLap,
                                                                     TenNNT = bth.TenNNT,
                                                                     MaSoThue = bth.MaSoThue,
                                                                     HDDIn = bth.HDDIn,
                                                                     LHHoa = bth.LHHoa,
                                                                     ThoiHanGui = bth.ThoiHanGui,
                                                                     NNT = bth.NNT,
                                                                     CreatedDate = bth.CreatedDate,
                                                                     CreatedBy = bth.CreatedBy,
                                                                     ChiTiets = (from ct in _dataContext.BangTongHopDuLieuHoaDonChiTiets
                                                                                 where ct.BangTongHopDuLieuHoaDonId == bth.Id
                                                                                 select new BangTongHopDuLieuHoaDonChiTietViewModel
                                                                                 {
                                                                                     Id = ct.Id,
                                                                                     BangTongHopDuLieuHoaDonId = ct.BangTongHopDuLieuHoaDonId,
                                                                                     MauSo = ct.MauSo,
                                                                                     KyHieu = ct.KyHieu,
                                                                                     SoHoaDon = ct.SoHoaDon,
                                                                                     NgayHoaDon = ct.NgayHoaDon,
                                                                                     MaSoThue = ct.MaSoThue,
                                                                                     MaKhachHang = ct.MaKhachHang,
                                                                                     TenKhachHang = ct.TenKhachHang,
                                                                                     DiaChi = ct.DiaChi,
                                                                                     HoTenNguoiMuaHang = ct.HoTenNguoiMuaHang,
                                                                                     MaHang = ct.MaHang,
                                                                                     TenHang = ct.TenHang,
                                                                                     SoLuong = ct.SoLuong,
                                                                                     DonViTinh = ct.DonViTinh,
                                                                                     ThanhTien = ct.ThanhTien,
                                                                                     ThueGTGT = ct.ThueGTGT,
                                                                                     TienThueGTGT = ct.TienThueGTGT,
                                                                                     TongTienThanhToan = ct.TongTienThanhToan,
                                                                                     TrangThaiHoaDon = ct.TrangThaiHoaDon,
                                                                                     TenTrangThaiHoaDon = ((TrangThaiHoaDon)ct.TrangThaiHoaDon).GetDescription(),
                                                                                     LoaiHoaDonLienQuan = ct.LoaiHoaDonLienQuan,
                                                                                     MauSoHoaDonLienQuan = ct.MauSoHoaDonLienQuan,
                                                                                     KyHieuHoaDonLienQuan = ct.KyHieuHoaDonLienQuan,
                                                                                     SoHoaDonLienQuan = ct.SoHoaDonLienQuan,
                                                                                     NgayHoaDonLienQuan = ct.NgayHoaDonLienQuan,
                                                                                     LKDLDChinh = ct.LKDLDChinh,
                                                                                     KDLDChinh = ct.KDLDChinh,
                                                                                     STBao = ct.STBao,
                                                                                     NTBao = ct.NTBao,
                                                                                     GhiChu = ct.GhiChu
                                                                                 }).ToList()
                                                                 };
            return await query.ToListAsync();
        }

        private async Task<bool> UpdateBangTongHopAsync(BangTongHopDuLieuHoaDonViewModel model)
        {
            var entityChiTiets = await _dataContext.BangTongHopDuLieuHoaDonChiTiets.Where(x => x.BangTongHopDuLieuHoaDonId == model.Id).ToListAsync();
            if (entityChiTiets.Any())
            {
                _dataContext.BangTongHopDuLieuHoaDonChiTiets.RemoveRange(entityChiTiets);
            }

            var chiTiets = model.ChiTiets;
            model.ChiTiets = null;

            foreach (var item in chiTiets)
            {
                item.BangTongHopDuLieuHoaDonId = model.Id;
            }

            var entitiesChiTiet = _mp.Map<List<BangTongHopDuLieuHoaDonChiTiet>>(chiTiets);
            await _dataContext.BangTongHopDuLieuHoaDonChiTiets.AddRangeAsync(entitiesChiTiet);

            model.ModifyBy = model.ActionUser.UserId;
            model.ModifyDate = DateTime.Now;
            var entity = _mp.Map<BangTongHopDuLieuHoaDon>(model);
            _dataContext.BangTongHopDuLieuHoaDons.Update(entity);

            return await _dataContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Check chưa lập tờ khai 01
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckChuaLapToKhaiAsync()
        {
            var result = await _dataContext.ThongDiepChungs
                .AnyAsync(x => x.MaLoaiThongDiep == 100 && ((x.HinhThuc == (int)HThuc.DangKyMoi) || ((x.HinhThuc == (int)HThuc.ThayDoiThongTin) && (x.TrangThaiGui != (int)TrangThaiGuiThongDiep.ChapNhan))));

            return !result;
        }

        /// <summary>
        /// Get Trạng Thái 
        /// </summary>
        /// <param name="ThongDiepGuiCQTId"></param>
        /// <returns></returns>
        public async Task<int> GetTrangThaiGuiToKhaiDangKyThayDoi(string ThongDiepGuiCQTId)
        {
            var result = -1;
            ThongDiepChung thongDiepTuongUng = await _dataContext.ThongDiepChungs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdThamChieu == ThongDiepGuiCQTId);

            if (thongDiepTuongUng != null)
            {
                result = thongDiepTuongUng.TrangThaiGui ?? -1;
            }
            return result;
        }
        private List<ReadMTLoiXML204> GetMoTaLoiFrom204(List<Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo> list)
        {
            List<ReadMTLoiXML204> result = new List<ReadMTLoiXML204>();
            foreach (var item in list)
            {
                ReadMTLoiXML204 result204 = new ReadMTLoiXML204();
                string mta = item.MTLoi;
                string firstSemicolon = item.MTLoi.Substring(0, mta.IndexOf(";"));
                result204.STT = firstSemicolon;
                mta = mta.Substring(firstSemicolon.Length + 1);
                string secondSemicolon = mta.Substring(0, mta.IndexOf(";"));
                result204.KyHieu = secondSemicolon;
                mta = mta.Substring(secondSemicolon.Length + 1);
                string thirdSemicolon = mta.Substring(0, mta.IndexOf(";"));
                result204.SoHoaDon = thirdSemicolon;
                mta = mta.Substring(thirdSemicolon.Length + 1);
                result204.MoTaLoi = mta;
                result.Add(result204);
            }

            return result;
        }

        public class ReadMTLoiXML204
        {
            public string STT { get; set; }
            public string KyHieu { get; set; }
            public string SoHoaDon { get; set; }
            public string MoTaLoi { get; set; }
        }
    }

}
