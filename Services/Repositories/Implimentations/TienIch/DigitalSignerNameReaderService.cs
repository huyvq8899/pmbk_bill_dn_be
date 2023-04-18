using AutoMapper;
using DLL;
using DLL.Entity;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AutoMapper.QueryableExtensions;
using Services.Repositories.Interfaces.TienIch;
using DLL.Entity.TienIch;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using DLL.Constants;
using Services.ViewModels.Params;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using DLL.Enums;
using Microsoft.Net.Http.Headers;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;

namespace Services.Repositories.Implimentations
{
    /// <summary>
    /// Lớp này thực hiện các nhiệm vụ:
    /// - Đọc thông tin người bán từ mẫu hóa đơn theo các trường hợp khác nhau.
    /// - Đọc ra tên người ký theo các điều kiện/trường hợp khác nhau.
    /// - Gửi lệnh socket để đọc thông tin người ký từ chữ ký số.
    /// </summary>
    public class DigitalSignerNameReaderService : IDigitalSignerNameReaderService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        // Địa chỉ URI của socket đang chạy
        private readonly string _webSocketURI = "ws://localhost:15872/bksoft";

        public DigitalSignerNameReaderService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHoSoHDDTService hoSoHDDTService)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hoSoHDDTService = hoSoHDDTService;
        }

        /// <summary>
        /// Đọc ra thông tin của người bán trên mẫu hóa đơn của hóa đơn điện tử.
        /// </summary>
        /// <param name="hoaDonDienTuIds">Là mảng các id của hóa đơn điện tử cần đọc thông tin.</param>
        /// <returns>Thông tin của người bán trên mẫu hóa đơn.</returns>
        public async Task<List<ThongTinNguoiBanTrenMauHoaDonViewModel>> GetThongTinNguoiBanTuHoaDonAsync(string[] hoaDonDienTuIds)
        {
            // Khai báo biến lưu kết quả trả về
            var listResult = new List<ThongTinNguoiBanTrenMauHoaDonViewModel>();

            // Tạo câu lệnh truy vấn và đọc dữ liệu từ database
            var queryMauHoaDonTuyChinh = await (from hoaDon in _dataContext.HoaDonDienTus.Where(x => hoaDonDienTuIds.Contains(x.HoaDonDienTuId)).AsNoTracking()

                                                join boKyHieuHoaDon in _dataContext.BoKyHieuHoaDons.AsNoTracking() on hoaDon.BoKyHieuHoaDonId equals boKyHieuHoaDon.BoKyHieuHoaDonId into boKyHieuHoaDonTemp
                                                from boKyHieuHoaDon in boKyHieuHoaDonTemp.DefaultIfEmpty()

                                                join mauHoaDon in _dataContext.MauHoaDons.AsNoTracking() on hoaDon.MauHoaDonId equals mauHoaDon.MauHoaDonId into mauHoaDonTemp
                                                from mauHoaDon in mauHoaDonTemp.DefaultIfEmpty()

                                                join mauHoaDonFile in _dataContext.MauHoaDonFiles.AsNoTracking() on hoaDon.MauHoaDonId equals mauHoaDonFile.MauHoaDonId into mauHoaDonFileTemp
                                                from mauHoaDonFile in mauHoaDonFileTemp.DefaultIfEmpty()

                                                join mauHoaDonTuyChinhChiTiet in _dataContext.MauHoaDonTuyChinhChiTiets.AsNoTracking() on mauHoaDon.MauHoaDonId equals mauHoaDonTuyChinhChiTiet.MauHoaDonId

                                                where (mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DienThoaiNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.FaxNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.WebsiteNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.EmailNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiBan) &&
                                                mauHoaDonTuyChinhChiTiet.LoaiContainer == LoaiContainerTuyChinh.NoiDung && mauHoaDonFile.Type == HinhThucMauHoaDon.HoaDonMauCoBan && mauHoaDonFile.Status == true

                                                select new
                                                {
                                                    boKyHieuHoaDon.KyHieu,
                                                    mauHoaDonTuyChinhChiTiet.LoaiChiTiet,
                                                    mauHoaDonTuyChinhChiTiet.GiaTri,
                                                    mauHoaDonFile.MaSoThueGiaiPhap,

                                                }).ToListAsync();

            // Xử lý dữ liệu đã đọc được
            var listDistinctBoKyHieu = queryMauHoaDonTuyChinh.Select(x => x.KyHieu).Distinct();
            foreach (var boKyHieu in listDistinctBoKyHieu)
            {
                var listThongTinNguoiBan = queryMauHoaDonTuyChinh.Where(x => x.KyHieu == boKyHieu).ToList();

                var result = new ThongTinNguoiBanTrenMauHoaDonViewModel
                {
                    TenBoKyHieu = boKyHieu,
                    TenDonViNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan) ? listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan).GiaTri : string.Empty,
                    MaSoThueNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan) ? listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan).GiaTri : string.Empty,
                    DiaChiNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiNguoiBan) ? listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiNguoiBan).GiaTri : string.Empty,
                    DienThoaiNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DienThoaiNguoiBan) ? listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DienThoaiNguoiBan).GiaTri : string.Empty,
                    FaxNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.FaxNguoiBan) ? listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.FaxNguoiBan).GiaTri : string.Empty,
                    WebsiteNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.WebsiteNguoiBan) ? listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.WebsiteNguoiBan).GiaTri : string.Empty,
                    EmailNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.EmailNguoiBan) ? listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.EmailNguoiBan).GiaTri : string.Empty,
                    SoTaiKhoanNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiBan) ? CutStringGetValue(listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiBan)?.GiaTri, 1) : string.Empty,
                    TenNganHangNguoiBan = listThongTinNguoiBan.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiBan) ? CutStringGetValue(listThongTinNguoiBan.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiBan)?.GiaTri, 2) : string.Empty,
                    MaSoThueGiaiPhap = listThongTinNguoiBan.Any(x => x.MaSoThueGiaiPhap != null) ? listThongTinNguoiBan.FirstOrDefault(x => x.MaSoThueGiaiPhap != null).MaSoThueGiaiPhap: string.Empty,

                };

                // Thêm thông tin vào danh sách kết quả trả về
                listResult.Add(result);
            }

            return listResult;
        }

        private string CutStringGetValue(string value,int  Type )
        {
            string result = "";
            string[] split = Regex.Split(value, @"(?=-)");
            switch (Type)
            {
                case 1:
                    if(split.Count() > 0)
                    {
                        result = split[0].Replace("-", "").Trim();
                    }

                    break;
                case 2:
                    if (split.Count() > 1)
                    {
                        result = split[1].Replace("-", "").Trim();
                    }
                    break;
                default:
                    break;
            }

            return result;
               
        }

        /// <summary>
        /// Đọc ra thông tin của người bán trên mẫu hóa đơn cụ thể.
        /// </summary>
        /// <param name="mauHoaDonId">Id của mẫu hóa đơn cần đọc thông tin.</param>
        /// <returns>Thông tin của người bán trên mẫu hóa đơn.</returns>
        public async Task<ThongTinNguoiBanTrenMauHoaDonViewModel> GetThongTinNguoiBanTuMauHoaDonAsync(string mauHoaDonId)
        {
            // Tạo câu lệnh truy vấn và đọc dữ liệu từ database
            var queryMauHoaDonTuyChinh = await (from mauHoaDon in _dataContext.MauHoaDons.Where(x => x.MauHoaDonId == mauHoaDonId).AsNoTracking()

                                                join mauHoaDonTuyChinhChiTiet in _dataContext.MauHoaDonTuyChinhChiTiets.AsNoTracking() on mauHoaDon.MauHoaDonId equals mauHoaDonTuyChinhChiTiet.MauHoaDonId

                                                where (mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan ||
                                                mauHoaDonTuyChinhChiTiet.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiNguoiBan) &&
                                                mauHoaDonTuyChinhChiTiet.LoaiContainer == LoaiContainerTuyChinh.NoiDung

                                                select new
                                                {
                                                    mauHoaDonTuyChinhChiTiet.LoaiChiTiet,
                                                    mauHoaDonTuyChinhChiTiet.GiaTri
                                                }).ToListAsync();

            // Xử lý dữ liệu đã đọc được
            // Khai báo biến lưu kết quả trả về
            var result = new ThongTinNguoiBanTrenMauHoaDonViewModel
            {
                TenDonViNguoiBan = queryMauHoaDonTuyChinh.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan)?.GiaTri,
                MaSoThueNguoiBan = queryMauHoaDonTuyChinh.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan)?.GiaTri,
                DiaChiNguoiBan = queryMauHoaDonTuyChinh.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiNguoiBan)?.GiaTri
            };

            return result;
        }

        /// <summary>
        /// Đọc ra tên người ký theo các điều kiện/trường hợp khác nhau.
        /// </summary>
        /// <param name="signerNameParams">Tham số điều kiện.</param>
        /// <returns>Tên người ký.</returns>
        public async Task<string> GetUnifiedSignerNameAsync(UnifiedSignerNameParams signerNameParams)
        {
            // Khai báo biến lưu tên của người ký điện tử
            var unifiedSignerName = string.Empty;

            // Đọc ra giá trị của tùy chọn cách thể hiện tên người nộp thuế thực hiện ký điện tử
            var cachTheHienTenNguoiKyDienTu = (await _dataContext.TuyChons.Where(x => x.Ma == "CachTheHienTenNguoiKyDienTu").FirstOrDefaultAsync())?.GiaTri;

            switch (cachTheHienTenNguoiKyDienTu)
            {
                case "TheoChungThuSo": // Theo chứng thư số
                    unifiedSignerName = await GetSignerNameByHttpHeaderAsync(signerNameParams);
                    break;

                case "TheoThongTinNguoiNopThue": // Theo thông tin người nộp thuế
                    var thongTinTuNguoiNopThue = await _hoSoHDDTService.GetDetailAsync();
                    unifiedSignerName = thongTinTuNguoiNopThue?.TenDonVi;
                    break;

                case "TheoThongTinNguoiBan": // Theo thông tin người bán hoặc bên A

                    // Nếu không có id thì trả về empty luôn
                    if (string.IsNullOrWhiteSpace(signerNameParams.Id))
                    {
                        return unifiedSignerName;
                    }

                    switch (signerNameParams.Type)
                    {
                        case "HoaDonDienTu": // Nếu là hóa đơn điện tử
                            var listThongTinTuHoaDon = await GetThongTinNguoiBanTuHoaDonAsync(new string[] { signerNameParams.Id });
                            var thongTinTuHoaDon = listThongTinTuHoaDon?.FirstOrDefault();
                            unifiedSignerName = thongTinTuHoaDon?.TenDonViNguoiBan;
                            break;

                        case "MauHoaDon": // Nếu là mẫu hóa đơn
                            var thongTinTuMauHoaDon = await GetThongTinNguoiBanTuMauHoaDonAsync(signerNameParams.Id);
                            unifiedSignerName = thongTinTuMauHoaDon?.TenDonViNguoiBan;
                            break;

                        case "BienBanHuy": // Nếu là biên bản hủy/xóa bỏ hóa đơn
                            var thongTinTuBienBanHuy = await _dataContext.BienBanXoaBos.Where(x => x.Id == signerNameParams.Id).AsNoTracking().FirstOrDefaultAsync();
                            unifiedSignerName = thongTinTuBienBanHuy?.TenCongTyBenA;
                            break;

                        case "BienBanDieuChinh": // Nếu là biên bản điều chỉnh hóa đơn
                            var thongTinTuBienBanDieuChinh = await _dataContext.BienBanDieuChinhs.Where(x => x.BienBanDieuChinhId == signerNameParams.Id).AsNoTracking().FirstOrDefaultAsync();
                            unifiedSignerName = thongTinTuBienBanDieuChinh?.TenDonViBenA;
                            break;
                    }
                    break;
            }

            return unifiedSignerName?.ToUpper();
        }

        /// <summary>
        /// Đọc ra tên người ký từ http-headers của client gửi lên.
        /// </summary>
        /// <param name="signerNameParams">Các tham số điều kiện.</param>
        /// <returns>Tên người ký của chữ ký số.</returns>
        public async Task<string> GetSignerNameByHttpHeaderAsync(UnifiedSignerNameParams signerNameParams)
        {
            // Khai báo biến lưu tên người ký trong chữ ký số
            var signerName = string.Empty;

            // Đọc chuỗi tên người ký từ http-headers của client gửi lên
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            var cacheContent = headers[HeaderNames.CacheControl].ToString();
            var listCacheItems = cacheContent.Split(',');
            // Lấy ra chuỗi tên người ký từ chuỗi bắt đầu với ký tự signerName=
            var signerNameFullString = listCacheItems.Where(x => x.Trim().StartsWith("signerName="))?.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(signerNameFullString))
            {
                // Cắt chuỗi để có tên người ký đúng (đã được mã hóa)
                var encodedSignerName = signerNameFullString.Trim().Substring("signerName=".Length);
                // Decode để có tên người ký đúng
                signerName = HttpUtility.UrlDecode(encodedSignerName);
            }
            else
            {
                // Nếu không tìm thấy signerName thì đọc từ nội dung XML của hóa đơn
                if (signerNameParams.Type == "HoaDonDienTu" && !string.IsNullOrWhiteSpace(signerNameParams.Id))
                {
                    signerName = await GetSignerNameFromXMLByIdAsync(signerNameParams.Id);
                }
            }

            return await Task.FromResult(signerName);
        }

        /// <summary>
        /// Đọc tên người ký từ nội dung XML (file XML của hóa đơn đã được ký duyệt)
        /// </summary>
        /// <param name="recordId">Id của bản ghi (hóa đơn,...)</param>
        /// <returns>Tên của người ký hóa đơn.</returns>
        public async Task<string> GetSignerNameFromXMLByIdAsync(string recordId)
        {
            // Khai báo biến lưu tên người ký trong chữ ký số
            var signerName = string.Empty;

            // Đọc ra nội dung của file XML theo ID bản ghi
            var fileData = await _dataContext.FileDatas.Where(x => x.RefId == recordId).FirstOrDefaultAsync();

            // Nếu không có bản ghi thì return empty luôn
            if (fileData == null)
            {
                return await Task.FromResult(signerName);
            }

            XmlDocument signedXMLDoc = new XmlDocument();
            var xmlContent = Encoding.UTF8.GetString(fileData.Binary);
            signedXMLDoc.LoadXml(xmlContent);

            // Phân tích nội dung XML để đọc ra tên người ký
            var signature = signedXMLDoc.SelectNodes("/TDiep/DLieu/HDon/DSCKS/NBan");

            if (signature != null)
            {
                if (signature.Count > 0)
                {
                    // Đọc ra tên người ký
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
                        signerName = tachTenNguoiKy2[1]; // Lấy index = 1 vì tên người ký đứng sau chữ CN=
                    }
                }
            }

            return await Task.FromResult(signerName);
        }

        /// <summary>
        /// Đọc ra tên của người ký chữ ký số từ chứng thư số.
        /// </summary>
        /// <returns>Tên của người ký chữ ký số.</returns>
        public async Task<string> GetSignerNameAsync()
        {
            // Khai báo mã số thuế cần đọc thông tin chữ ký số
            var maSoThue = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

            // Khai báo biến lưu tên người ký trong chữ ký số
            var signerName = string.Empty;

            try
            {
                // Khai báo socket để gửi và đọc dữ liệu
                using (ClientWebSocket webSocket = new ClientWebSocket())
                {
                    // Kết nối đến socket
                    await webSocket.ConnectAsync(new Uri(_webSocketURI), CancellationToken.None);

                    // Khai báo nội dung câu lệnh gửi đến socket
                    // Mã 050: là lệnh yêu cầu đọc thông tin về các chữ ký số đang có trong máy
                    var message = string.Format("{0}{1}{2}", "{'mLTDiep':'050','mst': '", maSoThue, "'}");

                    // Khai báo dữ liệu json để gửi vào socket
                    ArraySegment<byte> jsonDataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));

                    // Thực hiện gửi lệnh vào socket
                    webSocket.SendAsync(jsonDataToSend, WebSocketMessageType.Text, true, CancellationToken.None).Wait();

                    // Khai báo bộ nhớ tạm để lưu dữ liệu trả về từ socket
                    var buffer = new byte[1048576]; // 1MB
                    // Đọc dữ liệu trả về
                    var resultByWebSocket = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).Result;
                    // Chuyển kết quả sang chuỗi
                    var resultString = Encoding.UTF8.GetString(buffer, 0, resultByWebSocket.Count);

                    // Chuyển kết quả sang định dạng JSon
                    var resultJson = JsonConvert.DeserializeObject<ResultFromWebSocket>(resultString);

                    // Đọc ra phần tử DataJson của biến resultJson
                    var dataJsonInDetail = JsonConvert.DeserializeObject<DataJsonDetail>(resultJson.DataJson);

                    // Đọc ra phần tử Subject của biến dataJsonInDetail,
                    // vì phần tử Subject chứa tên của người ký,
                    // sau đó tách Subject thành mảng chuỗi bởi ký tự ","
                    var mangCacPhanTu = dataJsonInDetail.Subject.Split(",");

                    // Lọc ra phần tử nào trong mảng bắt đầu bởi chữ "CN=",
                    // vì tên người ký đứng sau chữ "CN="
                    // Ví dụ: "Subject": "OID.0.9.2342.19200300.100.1.1=MST:0200784873-999, CN=Bách Khoa - Kiểm thử HĐĐT có mã, S=THÀNH PHỐ HẢI PHÒNG, C=VN"
                    var signerNameItem = mangCacPhanTu.Where(x => x.Trim().StartsWith("CN="))?.FirstOrDefault();

                    // Tách ra tên người ký đầy đủ
                    signerName = signerNameItem.Trim().Substring("CN=".Length);
                }
            }
            catch (Exception)
            {
                signerName = "[Có lỗi đọc thông tin chữ ký số]";
                // Ví dụ: lỗi không chạy tool ChuKySo,...
            }

            return await Task.FromResult(signerName);
        }

        /// <summary>
        /// Lớp này thể hiện dữ liệu trả về từ socket.
        /// </summary>
        private class ResultFromWebSocket
        {
            /// <summary>
            /// Dữ liệu json dạng chuỗi.
            /// DataJson là một trong nhiều trường của dữ liệu trả về, nhưng mình chỉ cần lấy về trường DataJson.
            /// </summary>
            public string DataJson { get; set; }
        }

        /// <summary>
        /// Lớp này thể hiện dữ liệu chi tiết của DataJson trả về.
        /// </summary>
        private class DataJsonDetail
        {
            /// <summary>
            /// Một chuỗi chứa thông tin về người ký điện tử
            /// </summary>
            public string Subject { get; set; }
        }
    }
}
