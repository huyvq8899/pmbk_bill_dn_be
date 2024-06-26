﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using ImageMagick;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.DanhMuc;
using Services.Helper.Params.Filter;
using Services.Helper.VeThamQuan;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.TienIch;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class MauHoaDonService : IMauHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly IDigitalSignerNameReaderService _digitalSignerNameReaderService;

        public MauHoaDonService(Datacontext datacontext,
            IMapper mapper,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IHoSoHDDTService hoSoHDDTService,
            IDigitalSignerNameReaderService digitalSignerNameReaderService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _hoSoHDDTService = hoSoHDDTService;
            _digitalSignerNameReaderService = digitalSignerNameReaderService;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == id);

            // delelte files from docs folder
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");

            /*            foreach (var item in entity.MauHoaDonFiles)
                        {
                            var fullFilePath = Path.Combine(docFolderPath, item.FileName);
                            if (File.Exists(fullFilePath))
                            {
                                File.Delete(fullFilePath);
                            }
                        }*/

            _db.MauHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;

            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, _db);

            return result;
        }
        public async Task<bool> CheckPhatSinhAsync(string id)
        {
            return await _db.HoaDonDienTus.AnyAsync(x => x.MauHoaDonId == id)
                || await _db.BoKyHieuHoaDons.AnyAsync(x => x.MauHoaDonId == id);
        }
        public async Task<List<MauHoaDonViewModel>> GetAllAsync(MauHoaDonParams @params = null)
        {
            var query = _db.MauHoaDons.Select(x => new MauHoaDonViewModel
            {
                MauHoaDonId = x.MauHoaDonId,
                Ten = x.Ten,
                SoThuTu = x.SoThuTu,
                MauSo = x.MauSo,
                KyHieu = x.KyHieu,
                TenBoMau = x.TenBoMau,
                UyNhiemLapHoaDon = x.UyNhiemLapHoaDon,
                HinhThucHoaDon = x.HinhThucHoaDon,
                LoaiHoaDon = x.LoaiHoaDon,
                LoaiMauHoaDon = x.LoaiMauHoaDon,
                LoaiThueGTGT = x.LoaiThueGTGT,
                LoaiKhoGiay = x.LoaiKhoGiay,
                LoaiNgonNgu = x.LoaiNgonNgu,
                Status = x.Status,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                NgayKy = x.NgayKy,
            });

            if (@params != null)
            {
                if (!string.IsNullOrEmpty(@params.Keyword))
                {
                    string keyword = @params.Keyword.ToUpper().ToTrim();
                    //query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) ||
                    //                        x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()));
                }
                //if (@params.IsThongBaoPhatHanh == true)
                //{
                //    query = from q in query
                //            join tbphct in _db.ThongBaoPhatHanhChiTiets on q.MauHoaDonId equals tbphct.MauHoaDonId
                //            join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                //            where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
                //            group q by q.MauHoaDonId into g
                //            select new MauHoaDonViewModel
                //            {
                //                MauHoaDonId = g.Key,
                //                Ten = g.First().Ten,
                //                SoThuTu = g.First().SoThuTu,
                //                MauSo = g.First().MauSo,
                //                KyHieu = g.First().KyHieu,
                //                TenBoMau = g.First().TenBoMau,
                //                QuyDinhApDung = g.First().QuyDinhApDung,
                //                LoaiHoaDon = g.First().LoaiHoaDon,
                //                LoaiMauHoaDon = g.First().LoaiMauHoaDon,
                //                LoaiThueGTGT = g.First().LoaiThueGTGT,
                //                LoaiKhoGiay = g.First().LoaiKhoGiay,
                //                LoaiNgonNgu = g.First().LoaiNgonNgu,
                //                Status = true,
                //                CreatedBy = g.First().CreatedBy,
                //                CreatedDate = g.First().CreatedDate,
                //                NgayKy = g.First().NgayKy,
                //            };
                //}
            }

            var result = await query
                .OrderBy(x => x.MauSo)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<MauHoaDonViewModel>> GetAllPagingAsync(MauHoaDonParams @params)
        {
            var query = from mhd in _db.MauHoaDons
                            //where @params.MauHoaDonDuocPQ.Contains(mhd.MauHoaDonId) || @params.IsAdmin == true
                        orderby mhd.CreatedDate descending
                        select new MauHoaDonViewModel
                        {
                            MauHoaDonId = mhd.MauHoaDonId,
                            Ten = mhd.Ten,
                            NgayKy = mhd.NgayKy,
                            HinhThucHoaDon = mhd.HinhThucHoaDon,
                            LoaiHoaDon = mhd.LoaiHoaDon,
                            UyNhiemLapHoaDon = mhd.UyNhiemLapHoaDon,
                            TenBoMau = mhd.TenBoMau,
                            TenHinhThucHoaDon = mhd.HinhThucHoaDon.GetDescription(),
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            TenUyNhiemLapHoaDon = mhd.UyNhiemLapHoaDon.GetDescription(),
                            TenLoaiMau = mhd.LoaiMauHoaDon.GetDescription(),
                            TenLoaiThueGTGT = (mhd.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT || mhd.LoaiHoaDon == LoaiHoaDon.HoaDonGTGTCMTMTT) ? mhd.LoaiThueGTGT.GetDescription() : string.Empty,
                            ModifyDate = mhd.ModifyDate,
                        };

            // filter each col in table
            if (@params.FilterColumns != null && @params.FilterColumns.Any())
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                foreach (var filterCol in @params.FilterColumns)
                {
                    switch (filterCol.ColKey)
                    {
                        case nameof(@params.Filter.Ten):
                            query = GenericFilterColumn<MauHoaDonViewModel>.Query(query, x => x.Ten, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenLoaiMau):
                            query = GenericFilterColumn<MauHoaDonViewModel>.Query(query, x => x.KyHieu, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenHinhThucHoaDon):
                            query = GenericFilterColumn<MauHoaDonViewModel>.Query(query, x => x.TenUyNhiemLapHoaDon, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenLoaiHoaDon):
                            query = GenericFilterColumn<MauHoaDonViewModel>.Query(query, x => x.TenLoaiHoaDon, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenUyNhiemLapHoaDon):
                            query = GenericFilterColumn<MauHoaDonViewModel>.Query(query, x => x.TenUyNhiemLapHoaDon, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.ModifyDate):
                            query = GenericFilterColumn<MauHoaDonViewModel>.Query(query, x => x.ModifyDate.Value.ToString("dd/MM/yyyy"), filterCol, FilterValueType.String);
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.TimKiemTheo.Ten))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ten);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ten);
                    }
                }

                if (@params.SortKey == nameof(@params.TimKiemTheo.TenLoaiMau))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenLoaiMau);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenLoaiMau);
                    }
                }

                if (@params.SortKey == nameof(@params.TimKiemTheo.TenHinhThucHoaDon))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenHinhThucHoaDon);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenHinhThucHoaDon);
                    }
                }

                if (@params.SortKey == nameof(@params.TimKiemTheo.TenLoaiHoaDon))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenLoaiHoaDon);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenLoaiHoaDon);
                    }
                }

                if (@params.SortKey == nameof(@params.TimKiemTheo.TenLoaiThueGTGT))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenLoaiThueGTGT);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenLoaiThueGTGT);
                    }
                }

                if (@params.SortKey == nameof(@params.TimKiemTheo.TenUyNhiemLapHoaDon))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenUyNhiemLapHoaDon);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenUyNhiemLapHoaDon);
                    }
                }

                if (@params.SortKey == nameof(@params.TimKiemTheo.ModifyDate))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.ModifyDate);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.ModifyDate);
                    }
                }
            }

            if (@params.HinhThucHoaDon != HinhThucHoaDon.TatCa)
            {
                query = query.Where(x => x.HinhThucHoaDon == @params.HinhThucHoaDon);
            }

            if (@params.LoaiHoaDons.Any() && !@params.LoaiHoaDons.Any(x => x == LoaiHoaDon.TatCa))
            {
                query = query.Where(x => @params.LoaiHoaDons.Contains(x.LoaiHoaDon));
            }

            if (@params.UyNhiemLapHoaDon != UyNhiemLapHoaDon.TatCa)
            {
                query = query.Where(x => x.UyNhiemLapHoaDon == @params.UyNhiemLapHoaDon);
            }

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.Ten))
                {
                    var keyword = timKiemTheo.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenLoaiMau))
                {
                    var keyword = timKiemTheo.TenLoaiMau.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiMau.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenHinhThucHoaDon))
                {
                    var keyword = timKiemTheo.TenHinhThucHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenHinhThucHoaDon.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenLoaiHoaDon))
                {
                    var keyword = timKiemTheo.TenLoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDon.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenLoaiThueGTGT))
                {
                    var keyword = timKiemTheo.TenLoaiThueGTGT.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiThueGTGT.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenUyNhiemLapHoaDon))
                {
                    var keyword = timKiemTheo.TenUyNhiemLapHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenUyNhiemLapHoaDon.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NgayCapNhatFilter))
                {
                    var keyword = timKiemTheo.NgayCapNhatFilter.ToTrim();
                    query = query.Where(x => x.ModifyDate.HasValue && x.ModifyDate.Value.ToString("dd/MM/yyyy").Contains(keyword));
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<MauHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<MauHoaDonViewModel> GetByIdAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var attachPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.FILE_ATTACH}");
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");

            if (!Directory.Exists(attachPath))
            {
                Directory.CreateDirectory(attachPath);
            }

            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }

            // get file of mauhoadon
            var fileDatas = await _db.FileDatas.Where(x => x.RefId == id).AsNoTracking().ToListAsync();

            var query = from mhd in _db.MauHoaDons
                        join mhd_file in _db.MauHoaDonFiles on mhd.MauHoaDonId equals mhd_file.MauHoaDonId into mauHoaDonTmp
                        from mhd_file in mauHoaDonTmp.DefaultIfEmpty()
                        where mhd.MauHoaDonId == id && (mhd_file == null || mhd_file.Type == HinhThucMauHoaDon.HoaDonMauCoBan)
                        select new MauHoaDonViewModel
                        {
                            MauHoaDonId = mhd.MauHoaDonId,
                            Ten = mhd.Ten,
                            SoThuTu = mhd.SoThuTu,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            TenBoMau = mhd.TenBoMau,
                            NgayKy = mhd.NgayKy,
                            FilePath = mhd_file != null ? Path.Combine(docFolderPath, mhd_file.FileName) : string.Empty,
                            QuyDinhApDung = mhd.QuyDinhApDung,
                            UyNhiemLapHoaDon = mhd.UyNhiemLapHoaDon,
                            HinhThucHoaDon = mhd.HinhThucHoaDon,
                            LoaiHoaDon = mhd.LoaiHoaDon,
                            LoaiMauHoaDon = mhd.LoaiMauHoaDon,
                            LoaiThueGTGT = mhd.LoaiThueGTGT,
                            LoaiNgonNgu = mhd.LoaiNgonNgu,
                            LoaiKhoGiay = mhd.LoaiKhoGiay,
                            CreatedBy = mhd.CreatedBy,
                            CreatedDate = mhd.CreatedDate,
                            Status = mhd.Status,
                            MauHoaDonThietLapMacDinhs = (from tlmd in _db.MauHoaDonThietLapMacDinhs
                                                         where tlmd.MauHoaDonId == mhd.MauHoaDonId
                                                         select new MauHoaDonThietLapMacDinhViewModel
                                                         {
                                                             MauHoaDonThietLapMacDinhId = tlmd.MauHoaDonThietLapMacDinhId,
                                                             MauHoaDonId = tlmd.MauHoaDonId,
                                                             Loai = tlmd.Loai,
                                                             GiaTri = tlmd.GiaTri,
                                                             GiaTriBoSung = tlmd.GiaTriBoSung,
                                                             ImgBase64 = tlmd.GiaTri.GetBase64ImageMauHoaDon(tlmd.Loai, attachPath, fileDatas)
                                                         })
                                                         .ToList(),
                            MauHoaDonTuyChinhChiTiets = (from tcct in _db.MauHoaDonTuyChinhChiTiets
                                                         where tcct.MauHoaDonId == mhd.MauHoaDonId
                                                         group tcct by new { tcct.LoaiChiTiet, tcct.CustomKey } into g
                                                         select new MauHoaDonTuyChinhChiTietViewModel
                                                         {
                                                             MauHoaDonTuyChinhChiTietId = g.First(x => x.IsParent == true).MauHoaDonTuyChinhChiTietId,
                                                             MauHoaDonId = g.First(x => x.IsParent == true).MauHoaDonId,
                                                             GiaTri = g.First(x => x.IsParent == true).GiaTri,
                                                             KieuDuLieuThietLap = g.First(x => x.IsParent == true).KieuDuLieuThietLap,
                                                             Loai = g.First(x => x.IsParent == true).Loai,
                                                             LoaiChiTiet = g.Key.LoaiChiTiet,
                                                             IsParent = g.First(x => x.IsParent == true).IsParent,
                                                             Checked = g.First(x => x.IsParent == true).Checked,
                                                             Disabled = g.First(x => x.IsParent == true).Disabled,
                                                             CustomKey = g.First(x => x.IsParent == true).CustomKey,
                                                             STT = g.First(x => x.IsParent == true).STT,
                                                             Status = g.First(x => x.IsParent == true).Status,
                                                             Children = g.Where(x => x.IsParent != true)
                                                                .Select(x => new MauHoaDonTuyChinhChiTietViewModel
                                                                {
                                                                    MauHoaDonTuyChinhChiTietId = x.MauHoaDonTuyChinhChiTietId,
                                                                    MauHoaDonId = x.MauHoaDonId,
                                                                    GiaTri = x.GiaTri,
                                                                    TuyChonChiTiet = JsonConvert.DeserializeObject<TuyChinhChiTietModel>(x.TuyChinhChiTiet),
                                                                    TuyChinhChiTiet = x.TuyChinhChiTiet,
                                                                    GiaTriMacDinh = x.GiaTriMacDinh,
                                                                    KieuDuLieuThietLap = x.KieuDuLieuThietLap,
                                                                    Loai = x.Loai,
                                                                    LoaiChiTiet = x.LoaiChiTiet,
                                                                    LoaiContainer = x.LoaiContainer,
                                                                    IsParent = x.IsParent,
                                                                    Checked = x.Checked,
                                                                    Disabled = x.Disabled,
                                                                    CustomKey = x.CustomKey,
                                                                    STT = GetSTTTuyChonChiTietChildren(x.LoaiContainer),
                                                                    Status = x.Status,
                                                                })
                                                                .OrderBy(x => x.STT)
                                                                .ToList()
                                                         })
                                                         .OrderBy(x => x.STT)
                                                         .ToList()
                        };

            MauHoaDonViewModel result =  query.FirstOrDefault();

            return result;
        }

        public List<EnumModel> GetListLoaiHoaDon()
        {
            List<EnumModel> enums = ((LoaiHoaDon[])Enum.GetValues(typeof(LoaiHoaDon)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiKhoGiay()
        {
            List<EnumModel> enums = ((LoaiKhoGiay[])Enum.GetValues(typeof(LoaiKhoGiay)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiMau()
        {
            List<EnumModel> enums = ((LoaiMauHoaDon[])Enum.GetValues(typeof(LoaiMauHoaDon)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiNgonNgu()
        {
            List<EnumModel> enums = ((LoaiNgonNgu[])Enum.GetValues(typeof(LoaiNgonNgu)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiThueGTGT()
        {
            List<EnumModel> enums = ((LoaiThueGTGT[])Enum.GetValues(typeof(LoaiThueGTGT)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public async Task<List<MauHoaDonViewModel>> GetListMauDaDuocChapNhanAsync()
        {
            var query = from mhd in _db.MauHoaDons
                            //join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId
                            //join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                            //where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
                        group mhd by new { mhd.LoaiHoaDon, mhd.MauSo } into g
                        select new MauHoaDonViewModel
                        {
                            LoaiHoaDon = g.Key.LoaiHoaDon,
                            MauSo = g.Key.MauSo,
                            MauHoaDonIds = g.Select(x => x.MauHoaDonId).ToList(),
                            KyHieus = g.OrderBy(x => x.KyHieu).Select(x => x.KyHieu).ToList()
                        };

            var result = await query.ToListAsync();
            var mauHoaDonIds = result.SelectMany(x => x.MauHoaDonIds).ToList();

            //var thongBaoKetQuaHuyHDs = await _db.ThongBaoKetQuaHuyHoaDonChiTiets.Where(x => mauHoaDonIds.Contains(x.MauHoaDonId)).ToListAsync();
            var mauHoaDons = await _db.MauHoaDons.Where(x => mauHoaDonIds.Contains(x.MauHoaDonId)).ToListAsync();
            foreach (var group in result)
            {
                group.ThongTinChiTiets = new List<ThongTinChiTietKetQuaHuy>();

                foreach (var kyHieu in group.KyHieus)
                {
                    var mauHoaDon = mauHoaDons.FirstOrDefault(x => x.MauSo == group.MauSo && x.KyHieu == kyHieu);
                    int? tuSo = 1;
                    if (mauHoaDon != null)
                    {
                        //int? maxTuSo = thongBaoKetQuaHuyHDs.Where(x => x.MauHoaDonId == mauHoaDon.MauHoaDonId).Max(x => x.DenSo);
                        //if (maxTuSo.HasValue && maxTuSo > 0)
                        //{
                        //    tuSo = maxTuSo + 1;
                        //}
                    }

                    group.ThongTinChiTiets.Add(new ThongTinChiTietKetQuaHuy
                    {
                        KyHieu = kyHieu,
                        TuSo = tuSo
                    });
                }
            }

            return result;
        }

        public List<MauParam> GetListMauHoaDon(MauHoaDonParams @params)
        {
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");
            var list = new List<MauParam>().Deserialize(Path.Combine(jsonPath, "mau-hoa-don-anhbh.json")).ToList();

            list = list.Where(x => x.UyNhiemLapHoaDon == @params.UyNhiemLapHoaDon &&
                                    x.HinhThucHoaDon == @params.HinhThucHoaDon &&
                                    x.LoaiHoaDon == @params.LoaiHoaDon &&
                                    x.LoaiMauHoaDon == @params.LoaiMau &&
                                    (@params.LoaiHoaDon == 2 || @params.LoaiHoaDon == 7 || @params.LoaiHoaDon == 8 || @params.LoaiHoaDon == 10 || x.LoaiThueGTGT == @params.LoaiThueGTGT) &&
                                    x.LoaiNgonNgu == @params.LoaiNgonNgu &&
                                    x.LoaiKhoGiay == @params.LoaiKhoGiay)
                        .Select(x => new MauParam
                        {
                            Key = x.Key,
                            Code = x.Code,
                            File = x.File,
                            Stt = x.Stt,
                            UyNhiemLapHoaDon = x.UyNhiemLapHoaDon,
                            HinhThucHoaDon = x.HinhThucHoaDon,
                            LoaiHoaDon = x.LoaiHoaDon,
                            LoaiMauHoaDon = x.LoaiMauHoaDon,
                            LoaiThueGTGT = x.LoaiThueGTGT,
                            LoaiNgonNgu = x.LoaiNgonNgu,
                            LoaiKhoGiay = x.LoaiKhoGiay,
                            Avatar = $"/images/template/{x.Avatar}"
                        })
                        .ToList();

            return list;
        }

        public List<EnumModel> GetListQuyDinhApDung()
        {
            List<EnumModel> enums = ((QuyDinhApDung[])Enum.GetValues(typeof(QuyDinhApDung)))
               .Select(c => new EnumModel()
               {
                   Value = (int)c,
                   Name = c.GetDescription()
               }).ToList();
            return enums;
        }

        public List<ImageParam> GetMauHoaDonBackgrounds()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "template-background.json")).ToList();

            foreach (var item in list)
            {
                item.thumb = "/images/background-thumb/" + item.thumb;
                item.background = "/images/background/" + item.background;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public async Task<MauHoaDonViewModel> InsertAsync(MauHoaDonViewModel model)
        {
            // add mau hoa don
            var entity = _mp.Map<MauHoaDon>(model);
            await _db.MauHoaDons.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<MauHoaDonViewModel>(entity);

            return result;
        }

        public async Task<bool> UpdateAsync(MauHoaDonViewModel model)
        {
            var mauHoaDonThietLapMacDinhs = await _db.MauHoaDonThietLapMacDinhs
                .Where(x => x.MauHoaDonId == model.MauHoaDonId)
                .ToListAsync();
            _db.MauHoaDonThietLapMacDinhs.RemoveRange(mauHoaDonThietLapMacDinhs);

            var mauHoaDonTuyChinhChiTiets = await _db.MauHoaDonTuyChinhChiTiets
                .Where(x => x.MauHoaDonId == model.MauHoaDonId)
                .ToListAsync();
            _db.MauHoaDonTuyChinhChiTiets.RemoveRange(mauHoaDonTuyChinhChiTiets);

            var entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            entity.MauHoaDonThietLapMacDinhs = _mp.Map<List<MauHoaDonThietLapMacDinh>>(model.MauHoaDonThietLapMacDinhs);
            entity.MauHoaDonTuyChinhChiTiets = _mp.Map<List<MauHoaDonTuyChinhChiTiet>>(model.MauHoaDonTuyChinhChiTiets);
            entity.MauHoaDonFiles = _mp.Map<List<MauHoaDonFile>>(model.MauHoaDonFiles);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ChiTietMauHoaDon> GetChiTietByMauHoaDon(string mauHoaDonId)
        {
            var result = new ChiTietMauHoaDon();

            var mhd = _mp.Map<MauHoaDonViewModel>(await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == mauHoaDonId));
            var listBanMau = new List<BanMauHoaDon>();
            string jsonFolder = Path.Combine(_hostingEnvironment.WebRootPath, "jsons/mau-hoa-don.json");
            using (StreamReader r = new StreamReader(jsonFolder))
            {
                string json = r.ReadToEnd();
                listBanMau = JsonConvert.DeserializeObject<List<BanMauHoaDon>>(json);
            }

            var banMau = listBanMau.FirstOrDefault(x => x.TenBanMau.Contains(mhd.TenBoMau));
            if (banMau != null) result = banMau.ChiTiets.FirstOrDefault();

            return result;
        }

        public async Task<List<string>> GetAllMauSoHoaDon()
        {
            return await _db.MauHoaDons.Select(x => x.MauSo).ToListAsync();
        }

        public async Task<List<string>> GetAllKyHieuHoaDon(string ms = "")
        {
            return await _db.MauHoaDons.Where(x => string.IsNullOrEmpty(ms) || x.MauSo == ms).Select(x => x.KyHieu).ToListAsync();
        }

        /// <summary>
        /// Preview file by pdf
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<FileReturn> PreviewPdfAsync(MauHoaDonFileParams @params)
        {
            var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();
            var mauHoaDon = await GetByIdAsync(@params.MauHoaDonId);

            // get or generate 
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");
            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }

            // get file from db
            var mauHoaDonFile = await _db.MauHoaDonFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MauHoaDonId == @params.MauHoaDonId && x.Type == @params.Loai && x.Status == true);

            if (mauHoaDonFile == null) // if it's not in db then add to db
            {
                var addedFileListVM = await AddDocFilesAsync(mauHoaDon);

                string fileName = addedFileListVM.FirstOrDefault(x => x.MauHoaDonId == mauHoaDon.MauHoaDonId && x.Type == @params.Loai).FileName;
                mauHoaDon.FilePath = Path.Combine(docFolderPath, fileName);
            }
            else
            {
                mauHoaDon.FilePath = Path.Combine(docFolderPath, mauHoaDonFile.FileName);
                if (!File.Exists(mauHoaDon.FilePath)) // if physical file is not exist in server then generate it from byte
                {
                    await File.WriteAllBytesAsync(mauHoaDon.FilePath, mauHoaDonFile.Binary);
                }
            }

            if (!string.IsNullOrEmpty(@params.KyHieu))
            {
                mauHoaDon.KyHieu = @params.KyHieu;
            }

            var result = MauHoaDonHelper.PreviewFilePDF(mauHoaDon, @params.Loai, hoSoHDDT, _hostingEnvironment, _httpContextAccessor);
            return result;
        }

        public async Task<FileReturn> DownloadFileAsync(MauHoaDonFileParams @params)
        {
            var fileReturn = await PreviewPdfAsync(@params);
            if (@params.LoaiFile == DinhDangTepMau.PDF)
            {
                return fileReturn;
            }
            else
            {
                string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp/download_mau_hoa_don_{Guid.NewGuid()}");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string pdfPath = Path.Combine(folderPath, $"{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(pdfPath, fileReturn.Bytes);

                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.LoadFromFile(pdfPath);
                Image bmp = pdfDoc.SaveAsImage(0);
                Image emf = pdfDoc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap);
                Image zoomImg = new Bitmap(emf.Size.Width * 2, emf.Size.Height * 2);
                using (Graphics gg = Graphics.FromImage(zoomImg))
                {
                    gg.ScaleTransform(2.0f, 2.0f);
                    gg.DrawImage(emf, new Rectangle(new Point(0, 0), emf.Size), new Rectangle(new Point(0, 0), emf.Size), GraphicsUnit.Pixel);
                }

                Document docEmpty = new Document(Path.Combine(_hostingEnvironment.WebRootPath, "docs/MauHoaDon/Hoa_don_trang.docx"));
                DocPicture picture2 = docEmpty.Sections[0].Paragraphs[0].AppendPicture(bmp);
                picture2.Width = 580;
                picture2.Height = 800;
                string docPath = Path.Combine(folderPath, @params.Loai.GetTenFile() + (@params.LoaiFile == DinhDangTepMau.DOC ? ".doc" : ".docx"));
                docEmpty.SaveToFile(docPath, (@params.LoaiFile == DinhDangTepMau.DOC ? Spire.Doc.FileFormat.Doc : Spire.Doc.FileFormat.Docx));
                byte[] bytes = File.ReadAllBytes(docPath);
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
                return new FileReturn
                {
                    Bytes = bytes,
                    ContentType = MimeTypes.GetMimeType(docPath),
                    FileName = Path.GetFileName(docPath)
                };
            }
        }

        public async Task<bool> CheckTrungTenMauHoaDonAsync(MauHoaDonViewModel model)
        {
            bool result = await _db.MauHoaDons.AnyAsync(x => x.Ten == model.Ten);
            return result;
        }

        public async Task<string> CheckAllowUpdateAsync(MauHoaDonViewModel model)
        {
            //var check1 = await (from tbph in _db.ThongBaoPhatHanhs
            //                    join tbphct in _db.ThongBaoPhatHanhChiTiets on tbph.ThongBaoPhatHanhId equals tbphct.ThongBaoPhatHanhId
            //                    where tbphct.MauHoaDonId == model.MauHoaDonId && tbph.TrangThaiNop != TrangThaiNop.ChuaNop
            //                    select new
            //                    {
            //                        TenTrangThaiNop = tbph.TrangThaiNop.GetDescription()
            //                    })
            //                    .Select(x => x.TenTrangThaiNop).Distinct().ToListAsync();

            //if (check1.Any())
            //{
            //    return string.Join(", ", check1);
            //}

            //if (model.NgayKy.HasValue)
            //{
            //    return "signed";
            //}

            return null;
        }

        public async Task<FileReturn> ExportMauHoaDonAsync(ExportMauHoaDonParams @params)
        {
            var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();
            var mauHoaDon = await GetByIdAsync(@params.MauHoaDonId);

            List<string> filePaths = new List<string>();
            string folderName = $"temp/export_mau_hoa_don_{Guid.NewGuid()}";
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // get doc folder path
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");
            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }

            // get file from db
            var mauHoaDonFiles = await _db.MauHoaDonFiles
                .AsNoTracking()
                .Where(x => x.MauHoaDonId == @params.MauHoaDonId)
                .ToListAsync();

            if (!mauHoaDonFiles.Any()) // if it's not in db then generate it
            {
                var addedFileListVM = await AddDocFilesAsync(mauHoaDon);
                mauHoaDonFiles = _mp.Map<List<MauHoaDonFile>>(addedFileListVM);
            }

            foreach (var item in @params.HinhThucMauHoaDon)
            {
                var fileFromDB = mauHoaDonFiles.FirstOrDefault(x => x.MauHoaDonId == mauHoaDon.MauHoaDonId && x.Type == item);
                mauHoaDon.FilePath = Path.Combine(docFolderPath, fileFromDB.FileName);
                if (!File.Exists(mauHoaDon.FilePath)) // if it's not in server then generate it from byte
                {
                    File.WriteAllBytes(mauHoaDon.FilePath, fileFromDB.Binary);
                }

                var fileReturn = MauHoaDonHelper.PreviewFilePDF(mauHoaDon, item, hoSoHDDT, _hostingEnvironment, _httpContextAccessor);
                string pdfPath = Path.Combine(folderPath, $"{item.GetTenFile()}.pdf");
                File.WriteAllBytes(pdfPath, fileReturn.Bytes);
                filePaths.Add(pdfPath);
            }

            if (@params.DinhDangTepMau != 0)
            {
                Parallel.For(0, filePaths.Count, i =>
                {
                    string path = filePaths[i];

                    PdfDocument pdfDoc = new PdfDocument();
                    pdfDoc.LoadFromFile(path);
                    Image bmp = pdfDoc.SaveAsImage(0);
                    Image emf = pdfDoc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap);
                    Image zoomImg = new Bitmap(emf.Size.Width * 2, emf.Size.Height * 2);
                    using (Graphics gg = Graphics.FromImage(zoomImg))
                    {
                        gg.ScaleTransform(2.0f, 2.0f);
                        gg.DrawImage(emf, new Rectangle(new Point(0, 0), emf.Size), new Rectangle(new Point(0, 0), emf.Size), GraphicsUnit.Pixel);
                    }

                    Document docEmpty = new Document(Path.Combine(_hostingEnvironment.WebRootPath, "docs/MauHoaDon/Hoa_don_trang.docx"));
                    DocPicture picture2 = docEmpty.Sections[0].Paragraphs[0].AppendPicture(bmp);
                    picture2.Width = 580;
                    picture2.Height = 800;

                    filePaths[i] = path.Replace(".pdf", (@params.DinhDangTepMau == DinhDangTepMau.DOC) ? ".doc" : ".docx");
                    docEmpty.SaveToFile(filePaths[i], (@params.DinhDangTepMau == DinhDangTepMau.DOC) ? Spire.Doc.FileFormat.Doc : Spire.Doc.FileFormat.Docx);
                });
            }

            if (filePaths.Count() > 1)
            {
                using (var zipFileMemoryStream = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Create, leaveOpen: true))
                    {
                        foreach (var botFilePath in filePaths)
                        {
                            var botFileName = Path.GetFileName(botFilePath);
                            var entry = archive.CreateEntry(botFileName, CompressionLevel.Fastest);
                            using (var entryStream = entry.Open())
                            using (var fileStream = File.OpenRead(botFilePath))
                            {
                                await fileStream.CopyToAsync(entryStream);
                            }
                        }
                    }
                    zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
                    // use stream as needed
                    byte[] zipBytes = zipFileMemoryStream.ToArray(); //get all flushed data
                    string zipPath = Path.Combine(folderPath, "compressed.zip");
                    File.WriteAllBytes(Path.Combine(folderPath, "compressed.zip"), zipBytes);
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                    }

                    return new FileReturn
                    {
                        Bytes = zipBytes,
                        ContentType = MimeTypes.GetMimeType(zipPath),
                        FileName = Path.GetFileName(zipPath)
                    };
                }
            }

            byte[] bytes = File.ReadAllBytes(filePaths[0]);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            return new FileReturn
            {
                Bytes = bytes,
                ContentType = MimeTypes.GetMimeType(filePaths[0]),
                FileName = Path.GetFileName(filePaths[0])
            };
        }

        public async Task<bool> UpdateNgayKyAsync(MauHoaDonViewModel model)
        {
            MauHoaDon entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId);
            entity.NgayKy = model.NgayKy;
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<MauHoaDonViewModel> GetNgayKyByIdAsync(string id)
        {
            MauHoaDonViewModel result = await _db.MauHoaDons
                .Where(x => x.MauHoaDonId == id)
                .Select(x => new MauHoaDonViewModel
                {
                    MauHoaDonId = x.MauHoaDonId,
                    NgayKy = x.NgayKy
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<NhatKyTruyCapViewModel>> GetListNhatKyHoaDonAsync(string id)
        {
            List<NhatKyTruyCapViewModel> result = await (from nktc in _db.NhatKyTruyCaps
                                                         join u in _db.Users on nktc.CreatedBy equals u.UserId
                                                         where nktc.RefId == id
                                                         orderby nktc.CreatedDate descending
                                                         select new NhatKyTruyCapViewModel
                                                         {
                                                             NhatKyTruyCapId = nktc.NhatKyTruyCapId,
                                                             CreatedBy = nktc.CreatedBy,
                                                             CreatedByUserName = u.UserName,
                                                             HanhDong = nktc.HanhDong,
                                                             CreatedDate = nktc.CreatedDate,
                                                             MoTaChiTiet = nktc.MoTaChiTiet,
                                                             MoTaChiTietLimit = nktc.MoTaChiTiet.LimitLine(2),
                                                             IsOverLimitContent = nktc.MoTaChiTiet.IsOverLimit(2),
                                                             DiaChiIP = nktc.DiaChiIP,
                                                             DoiTuongThaoTac = nktc.DoiTuongThaoTac,
                                                             ThamChieu = nktc.ThamChieu,
                                                             TenMayTinh = nktc.TenMayTinh,
                                                             RefFile = nktc.RefFile,
                                                             RefId = nktc.RefId,
                                                             RefType = nktc.RefType,
                                                         })
                                                         .ToListAsync();

            return result;
        }

        public List<ImageParam> GetBackgrounds()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "hinh-nen-mau-hoa-don.json")).ToList();

            foreach (var item in list)
            {
                item.background = "/images/background/" + item.Value;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public List<ImageParam> GetBorders()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "khung-vien-mau-hoa-don.json")).ToList();

            foreach (var item in list)
            {
                item.background = "/images/border/" + item.Value;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public List<ImageParam> GetBackgroundA5s()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "hinh-nen-mau-hoa-don-A5.json")).ToList();

            foreach (var item in list)
            {
                item.background = "/images/background/" + item.Value;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public List<ImageParam> GetBorderA5s()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "khung-vien-mau-hoa-don-A5.json")).ToList();

            foreach (var item in list)
            {
                item.background = "/images/borderA5/" + item.Value;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public async Task<List<MauHoaDonTuyChinhChiTietViewModel>> GetTruongMoRongByLoaiHoaDonAsync(LoaiHoaDon loaiHoaDon)
        {
            var result = await (from tmr in _db.ThietLapTruongDuLieus
                                where tmr.LoaiHoaDon == loaiHoaDon
                                orderby tmr.STT
                                select new MauHoaDonTuyChinhChiTietViewModel
                                {
                                    ///
                                })
                                .ToListAsync();

            return result;
        }

        public async Task<List<MauHoaDonViewModel>> GetListFromBoKyHieuHoaDonAsync(MauHoaDonParams @params)
        {
            var result = await _db.MauHoaDons
                .Where(x => x.UyNhiemLapHoaDon == @params.UyNhiemLapHoaDon && x.HinhThucHoaDon == @params.HinhThucHoaDon && x.LoaiHoaDon == (LoaiHoaDon)@params.LoaiHoaDon)
                .Select(x => new MauHoaDonViewModel
                {
                    MauHoaDonId = x.MauHoaDonId,
                    Ten = x.Ten,
                    NgayKy = x.NgayKy,
                    LoaiThueGTGT = x.LoaiThueGTGT,
                    TenLoaiThueGTGT = x.LoaiThueGTGT.GetDescription()
                })
                .OrderBy(x => x.Ten)
                .ToListAsync();

            return result;
        }

        public string GetFileToSign()
        {
            string xml = $@"<TDiep>
                            <DLieu>
                                <HDon>
                                    <DLHDon Id=""SigningData"">
                                        <TTChung>
                                            <NLap>{DateTime.Now:yyyy-MM-dd}</NLap>
                                        </TTChung>
                                    </DLHDon>
                                    <DSCKS>
                                        <NBan />
                                    </DSCKS>
                                </HDon>
                            </DLieu>
                        </TDiep>";

            var result = DataHelper.EncodeString(xml);
            return result;
        }

        public async Task<int> UpdateMauTuyChonChiTietBanHangAsync()
        {
            var mauHoaDonIds = await (from mhd in _db.MauHoaDons
                                      join mct in _db.MauHoaDonTuyChinhChiTiets.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan)
                                      on mhd.MauHoaDonId equals mct.MauHoaDonId into tmpMauChiTiets
                                      from mct in tmpMauChiTiets.DefaultIfEmpty()
                                      where mhd.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && mct == null
                                      select mhd.MauHoaDonId)
                                      .ToListAsync();

            if (mauHoaDonIds.Any())
            {
                foreach (var mauHoaDonId in mauHoaDonIds)
                {
                    var congTienHangs = await _db.MauHoaDonTuyChinhChiTiets
                        .Where(x => x.MauHoaDonId == mauHoaDonId && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang)
                        .ToListAsync();

                    var addedList = new List<MauHoaDonTuyChinhChiTiet>();

                    foreach (var item in congTienHangs)
                    {
                        var newItem = new MauHoaDonTuyChinhChiTiet
                        {
                            MauHoaDonTuyChinhChiTietId = Guid.NewGuid().ToString(),
                            MauHoaDonId = item.MauHoaDonId,
                            GiaTri = item.GiaTri,
                            TuyChinhChiTiet = item.TuyChinhChiTiet,
                            TenTiengAnh = item.TenTiengAnh,
                            GiaTriMacDinh = item.GiaTriMacDinh,
                            DoRong = item.DoRong,
                            KieuDuLieuThietLap = item.KieuDuLieuThietLap,
                            Loai = item.Loai,
                            LoaiChiTiet = LoaiChiTietTuyChonNoiDung.TongTienThanhToan,
                            LoaiContainer = item.LoaiContainer,
                            IsParent = item.IsParent,
                            Checked = item.Checked,
                            Disabled = item.Disabled,
                            CustomKey = item.CustomKey,
                            STT = item.STT,
                            Status = item.Status,
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate,
                            ModifyBy = item.ModifyBy,
                            ModifyDate = item.ModifyDate
                        };

                        if (item.IsParent != true)
                        {
                            if (item.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                            {
                                newItem.GiaTri = "Tổng tiền thanh toán";
                                item.GiaTri = "Cộng tiền hàng";
                            }
                        }
                        else
                        {
                            newItem.STT = 4;
                        }


                        addedList.Add(newItem);
                    }

                    await _db.MauHoaDonTuyChinhChiTiets.AddRangeAsync(addedList);

                    var thongTinTongTienHangs = await _db.MauHoaDonTuyChinhChiTiets
                        .Where(x => x.MauHoaDonId == mauHoaDonId && x.Loai == LoaiTuyChinhChiTiet.ThongTinVeTongGiaTriHHDV && x.IsParent == true)
                        .ToListAsync();

                    foreach (var item in thongTinTongTienHangs)
                    {
                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang)
                        {
                            item.STT = 1;
                        }
                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyLeChietKhau)
                        {
                            item.STT = 2;
                        }
                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienChietKhau)
                        {
                            item.STT = 3;
                        }
                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu)
                        {
                            item.STT = 5;
                        }
                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.GhiChuHHDV)
                        {
                            item.STT = 6;
                        }
                    }
                }

                var result = await _db.SaveChangesAsync();
                return result;
            }

            return 0;
        }

        private int GetSTTTuyChonChiTietChildren(LoaiContainerTuyChinh loai)
        {
            int stt = 1;

            switch (loai)
            {
                case LoaiContainerTuyChinh.TieuDe:
                    stt = 1;
                    break;
                case LoaiContainerTuyChinh.NoiDung:
                    stt = 3;
                    break;
                case LoaiContainerTuyChinh.KyHieuCot:
                    stt = 4;
                    break;
                case LoaiContainerTuyChinh.TieuDeSongNgu:
                    stt = 2;
                    break;
                default:
                    break;
            }

            return stt;
        }

        public async Task<bool> CheckXoaKyDienTuAsync(string mauHoaDonId)
        {
            var result = await _db.BoKyHieuHoaDons
                .AnyAsync(x => x.MauHoaDonId == mauHoaDonId);

            if (result)
            {
                return await _db.BoKyHieuHoaDons
                   .AnyAsync(x => x.MauHoaDonId == mauHoaDonId && (x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung));
            }

            return false;
        }

        public async Task<MauHoaDonViewModel> GetByIdBasicAsync(string id)
        {
            var entity = await _db.MauHoaDons.AsNoTracking().FirstOrDefaultAsync(x => x.MauHoaDonId == id);
            var result = _mp.Map<MauHoaDonViewModel>(entity);
            return result;
        }

        /// <summary>
        /// get file xem mau hoa don by pdf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <summary>
        /// get file xem mau hoa don by pdf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MauHoaDonXacThuc>> GetListMauHoaDonXacThucAsync(string id)
        {
            List<MauHoaDonXacThuc> result = new List<MauHoaDonXacThuc>();

            var hoSoHDDT = new HoSoHDDTViewModel();
            // Khai báo biến có kiểu HoSoHDDTViewModel để truyền vào phương thức tĩnh (static) là
            // MauHoaDonHelper.PreviewFilePDF; vì phương thức tĩnh đó đã được dùng ở nhiều chỗ code
            // nên phải khai báo biến <hoSoHDDT> và gọi đến API GetUnifiedSignerNameAsync
            // để đọc tên người ký rồi gán vào biến <hoSoHDDT>

            // Gọi API đọc tên người ký
            var unifiedSignerName = await _digitalSignerNameReaderService.GetUnifiedSignerNameAsync(
                new UnifiedSignerNameParams
                {
                    Id = id,
                    Type = "MauHoaDon"
                });
            hoSoHDDT.TenDonVi = unifiedSignerName;

            // get or generate 
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");
            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }

            var loaiTheHienHoaDons = new List<HinhThucMauHoaDon>()
            {
                HinhThucMauHoaDon.HoaDonMauCoBan,
                HinhThucMauHoaDon.HoaDonMauDangChuyenDoi,
                HinhThucMauHoaDon.HoaDonMauCoChietKhau,
                HinhThucMauHoaDon.HoaDonMauNgoaiTe
            };

            // get or generate 
            var tempFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            var mauHoaDonIds = id.Split(";");
            // get file from db
            /*            var allMauHoaDonFiles = await _db.MauHoaDonFiles
                           .AsNoTracking()
                           .Where(x => mauHoaDonIds.Contains(x.MauHoaDonId) && loaiTheHienHoaDons.Contains(x.Type))
                           .ProjectTo<MauHoaDonFileViewModel>(_mp.ConfigurationProvider)
                           .ToListAsync();*/

            foreach (var mauHoaDonId in mauHoaDonIds)
            {
                var mauHoaDon = await GetByIdAsync(mauHoaDonId);
                // loop to convert docx to pdf
                var mauHoaDonFiles = await AddDocFilesAsync(mauHoaDon);
               
                foreach (var item in mauHoaDonFiles)
                {
                    mauHoaDon.FilePath = Path.Combine(docFolderPath, item.FileName);
                    if (!File.Exists(mauHoaDon.FilePath)) // if physical file is not exist in server then generate it from byte
                    {
                        await File.WriteAllBytesAsync(mauHoaDon.FilePath, item.Binary);
                    }

                    var resultPDF = MauHoaDonHelper.PreviewFilePDF(mauHoaDon, item.Type, hoSoHDDT, _hostingEnvironment, _httpContextAccessor);
                    result.Add(new MauHoaDonXacThuc
                    {
                        MauHoaDonId = mauHoaDonId,
                        FileByte = resultPDF.Bytes,
                        FileType = item.Type,
                    });
                }
            }

            return result;
        }


        public async Task<bool> XacThucMauHoaDonWhenChangeHopDong()
        {
            bool result = false;
            try
            {
                string PhatHanhBoi = await GetPhatHanhBoiFromHopDong();

                List<MauHoaDonViewModel> listMauHoaDon = await GetDetailMauHoaDonForUpdateWhwnChangeHopDong();
                foreach (var item in listMauHoaDon)
                {
                    // get or add doc folder
                    string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                    var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");
                    if (!Directory.Exists(docFolderPath))
                    {
                        Directory.CreateDirectory(docFolderPath);
                    }

                    // if is update then remove old files
                    if (!string.IsNullOrEmpty(item.MauHoaDonId))
                    {
                        var oldFiles = await _db.MauHoaDonFiles.Where(x => x.MauHoaDonId == item.MauHoaDonId && x.Status == true).ToListAsync();

                        // remove files in server
                        foreach (var element in oldFiles)
                        {
                            var oldFilePath = Path.Combine(docFolderPath, element.FileName);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                            element.Status = false;
                            element.NgayNgungSuDung = DateTime.Now;

                        }
                        /// Kiem tra neu doi nha giai phap thi chuyen lai chan trang
                        if (oldFiles.Count() > 0)
                        {
                            if (oldFiles[0].MaSoThueGiaiPhap != PhatHanhBoi)
                            {
                                string stringPhatHanhBoiPMBK = String.Format("Phát hành bởi Hóa đơn Bách Khoa - {0} - MST: {1}", "Công ty Cổ phần Phát triển và Ứng dụng phần mềm Bách Khoa", "0202029650");
                                string stringPhatHanhBoiDVBK = String.Format("Phát hành bởi Hóa đơn Bách Khoa - {0} - MST: {1}", "Công ty Cổ phần Thiết bị điện - điện tử Bách Khoa", "0200784873");
                                var MauHoaDonTuyChinhChiTiet = await _db.MauHoaDonTuyChinhChiTiets.FirstOrDefaultAsync(x => x.MauHoaDonId == item.MauHoaDonId && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhatHanhBoi && x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                var MauHoaDonTuyChinhChiTietParent = await _db.MauHoaDonTuyChinhChiTiets.FirstOrDefaultAsync(x => x.MauHoaDonId == item.MauHoaDonId && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhatHanhBoi && x.IsParent == true);
                                var MauHoaDonTuyChinhChiTietInModel = item.MauHoaDonTuyChinhChiTiets.FirstOrDefault(x => x.MauHoaDonTuyChinhChiTietId == MauHoaDonTuyChinhChiTietParent.MauHoaDonTuyChinhChiTietId).Children;
                                if (PhatHanhBoi == "0202029650")
                                {
                                    MauHoaDonTuyChinhChiTiet.GiaTri = stringPhatHanhBoiPMBK;
                                }
                                else
                                {
                                    MauHoaDonTuyChinhChiTiet.GiaTri = stringPhatHanhBoiDVBK;
                                }
                                if (MauHoaDonTuyChinhChiTietInModel != null)
                                {
                                    foreach (var elemet in MauHoaDonTuyChinhChiTietInModel)
                                    {
                                        if (elemet.MauHoaDonTuyChinhChiTietId == MauHoaDonTuyChinhChiTiet.MauHoaDonTuyChinhChiTietId)
                                        {
                                            elemet.GiaTri = MauHoaDonTuyChinhChiTiet.GiaTri;
                                        }
                                    }
                                    item.MauHoaDonTuyChinhChiTiets.FirstOrDefault(x => x.MauHoaDonTuyChinhChiTietId == MauHoaDonTuyChinhChiTietParent.MauHoaDonTuyChinhChiTietId).Children = MauHoaDonTuyChinhChiTietInModel;
                                }
                                _db.MauHoaDonTuyChinhChiTiets.Update(MauHoaDonTuyChinhChiTiet);


                            }
                        }
                        // remove files in db
                        //   _db.MauHoaDonFiles.RemoveRange(oldFiles);
                        await _db.SaveChangesAsync();

                    }


                    var listFilesToAdd = new List<MauHoaDonFile>();
                    var typeOfMauHoaDons = Enum.GetValues(typeof(HinhThucMauHoaDon)).Cast<HinhThucMauHoaDon>()/*.Where(x => x == HinhThucMauHoaDon.HoaDonMauNgoaiTe).ToList()*/;

                    List<Task<(Document, HinhThucMauHoaDon)>> tasks = new List<Task<(Document, HinhThucMauHoaDon)>>();

                    foreach (var type in typeOfMauHoaDons)
                    {
                        tasks.Add(CreateMauAsync(item, type, _hostingEnvironment, _httpContextAccessor));
                    }

                    var listDocument = await Task.WhenAll(tasks);
                    foreach (var element in listDocument)
                    {
                        var docFileName = $"{Guid.NewGuid()}.docx";
                        var docPath = Path.Combine(docFolderPath, docFileName);
                        element.Item1.SaveToFile(docPath, Spire.Doc.FileFormat.Docx);

                        listFilesToAdd.Add(new MauHoaDonFile
                        {
                            MauHoaDonId = item.MauHoaDonId,
                            MauHoaDonFileId = Guid.NewGuid().ToString(),
                            Type = element.Item2,
                            FileName = docFileName,
                            NgayXacThuc = DateTime.Now,
                            MaSoThueGiaiPhap = PhatHanhBoi,
                            Status = true,
                            Binary = File.ReadAllBytes(docPath)
                        });
                    }
                    // await UpdateAllHoaDonChuaKyWhenChangeHopDong(item.MauHoaDonId, listFilesToAdd);
                    await _db.MauHoaDonFiles.AddRangeAsync(listFilesToAdd);
                }

                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                Tracert.WriteLog("Bug When Change " + ex.Message);
            }
            return result;
        }

        public async Task<List<MauHoaDonViewModel>> GetDetailMauHoaDonForUpdateWhwnChangeHopDong()
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var attachPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.FILE_ATTACH}");
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");

            if (!Directory.Exists(attachPath))
            {
                Directory.CreateDirectory(attachPath);
            }

            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }
            var query = from mhd in _db.MauHoaDons
                        select new MauHoaDonViewModel
                        {
                            MauHoaDonId = mhd.MauHoaDonId,
                            Ten = mhd.Ten,
                            SoThuTu = mhd.SoThuTu,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            TenBoMau = mhd.TenBoMau,
                            NgayKy = mhd.NgayKy,
                            QuyDinhApDung = mhd.QuyDinhApDung,
                            UyNhiemLapHoaDon = mhd.UyNhiemLapHoaDon,
                            HinhThucHoaDon = mhd.HinhThucHoaDon,
                            LoaiHoaDon = mhd.LoaiHoaDon,
                            LoaiMauHoaDon = mhd.LoaiMauHoaDon,
                            LoaiThueGTGT = mhd.LoaiThueGTGT,
                            LoaiNgonNgu = mhd.LoaiNgonNgu,
                            LoaiKhoGiay = mhd.LoaiKhoGiay,
                            CreatedBy = mhd.CreatedBy,
                            CreatedDate = mhd.CreatedDate,
                            Status = mhd.Status,
                            MauHoaDonThietLapMacDinhs = (from tlmd in _db.MauHoaDonThietLapMacDinhs
                                                         where tlmd.MauHoaDonId == mhd.MauHoaDonId
                                                         select new MauHoaDonThietLapMacDinhViewModel
                                                         {
                                                             MauHoaDonThietLapMacDinhId = tlmd.MauHoaDonThietLapMacDinhId,
                                                             MauHoaDonId = tlmd.MauHoaDonId,
                                                             Loai = tlmd.Loai,
                                                             GiaTri = tlmd.GiaTri,
                                                             GiaTriBoSung = tlmd.GiaTriBoSung,
                                                         })
                                                         .ToList(),
                            MauHoaDonTuyChinhChiTiets = (from tcct in _db.MauHoaDonTuyChinhChiTiets
                                                         where tcct.MauHoaDonId == mhd.MauHoaDonId
                                                         group tcct by new { tcct.LoaiChiTiet, tcct.CustomKey } into g
                                                         select new MauHoaDonTuyChinhChiTietViewModel
                                                         {
                                                             MauHoaDonTuyChinhChiTietId = g.First(x => x.IsParent == true).MauHoaDonTuyChinhChiTietId,
                                                             MauHoaDonId = g.First(x => x.IsParent == true).MauHoaDonId,
                                                             GiaTri = g.First(x => x.IsParent == true).GiaTri,
                                                             KieuDuLieuThietLap = g.First(x => x.IsParent == true).KieuDuLieuThietLap,
                                                             Loai = g.First(x => x.IsParent == true).Loai,
                                                             LoaiChiTiet = g.Key.LoaiChiTiet,
                                                             IsParent = g.First(x => x.IsParent == true).IsParent,
                                                             Checked = g.First(x => x.IsParent == true).Checked,
                                                             Disabled = g.First(x => x.IsParent == true).Disabled,
                                                             CustomKey = g.First(x => x.IsParent == true).CustomKey,
                                                             STT = g.First(x => x.IsParent == true).STT,
                                                             Status = g.First(x => x.IsParent == true).Status,
                                                             Children = g.Where(x => x.IsParent != true)
                                                                .Select(x => new MauHoaDonTuyChinhChiTietViewModel
                                                                {
                                                                    MauHoaDonTuyChinhChiTietId = x.MauHoaDonTuyChinhChiTietId,
                                                                    MauHoaDonId = x.MauHoaDonId,
                                                                    GiaTri = x.GiaTri,
                                                                    TuyChonChiTiet = JsonConvert.DeserializeObject<TuyChinhChiTietModel>(x.TuyChinhChiTiet),
                                                                    TuyChinhChiTiet = x.TuyChinhChiTiet,
                                                                    GiaTriMacDinh = x.GiaTriMacDinh,
                                                                    KieuDuLieuThietLap = x.KieuDuLieuThietLap,
                                                                    Loai = x.Loai,
                                                                    LoaiChiTiet = x.LoaiChiTiet,
                                                                    LoaiContainer = x.LoaiContainer,
                                                                    IsParent = x.IsParent,
                                                                    Checked = x.Checked,
                                                                    Disabled = x.Disabled,
                                                                    CustomKey = x.CustomKey,
                                                                    STT = GetSTTTuyChonChiTietChildren(x.LoaiContainer),
                                                                    Status = x.Status,
                                                                })
                                                                .OrderBy(x => x.STT)
                                                                .ToList()
                                                         })
                                                         .OrderBy(x => x.STT)
                                                         .ToList()
                        };

            List<MauHoaDonViewModel> result = await query.ToListAsync();

            return result;
        }


        public async Task<FileReturn> PreviewPdfOfXacThucAsync(MauHoaDonFileParams @params)
        {
            // get mau hoa don from db
            var mauHoaDonXacThuc = await _db.MauHoaDonXacThucs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.NhatKyXacThucBoKyHieuId == @params.NhatKyXacThucBoKyHieuId && ((int)x.FileType == (int)@params.Loai));

            if (mauHoaDonXacThuc == null)
            {
                return null;
            }

            string fileName = $"mau-hoa-don-{Guid.NewGuid()}.pdf";
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            // replace text
            string fullName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.FULL_NAME)?.Value;

            PdfDocument pdfDocument = new PdfDocument(mauHoaDonXacThuc.FileByte);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("<convertor>", fullName);
            MauHoaDonHelper.FintTextInPDFAndReplaceIt(pdfDocument, dictionary);
            pdfDocument.SaveToFile(filePath);

            var pdfBytes = File.ReadAllBytes(filePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return new FileReturn
            {
                Bytes = pdfBytes,
                ContentType = MimeTypes.GetMimeType(filePath),
                FileName = Path.GetFileName(filePath)
            };
        }

        /// <summary>
        /// get document for invoice to view pdf
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="hasReason">có lý do thay thế/điều chỉnh không</param>
        /// <returns>tuple</returns>
        public async Task<(Document, int)> GetDocForInvoiceAsync(MauHoaDonViewModel model, HinhThucMauHoaDon type, bool hasReason, DateTime? NgayThemHoaDon)
        {
            Document document = new Document();
            int beginRow = 1;
            bool resetFile = false;
            if(NgayThemHoaDon == null)
            {
                NgayThemHoaDon = DateTime.Now;
                resetFile = true;
            }
            // get doc folder path
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");
            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }

            // get file from db
            /*            var mauHoaDonFile1 = await _db.MauHoaDonFiles
                           .AsNoTracking().Where(x => x.HoaDonDienTuId == HoaDonDienTuId)
                           .ToListAsync();*/


            /*            var mauHoaDonFile = (from mhd_file in _db.MauHoaDonFiles
                                             join hd in _db.MauFile_HoaDons on mhd_file.MauHoaDonFileId equals hd.MauHoaDonFileId into mauHoaDonTmp
                                             from hd in mauHoaDonTmp.DefaultIfEmpty()
                                             where hd.HoaDonDienTuId == HoaDonDienTuId && (mhd_file.MauHoaDonId == model.MauHoaDonId && mhd_file.Type == type)
                                             select mhd_file).FirstOrDefault();*/

            var mauHoaDonFile = new MauHoaDonFile();
            // get file from db

            var mauHoaDonFileActive = await _db.MauHoaDonFiles.AsNoTracking().Where(x => x.MauHoaDonId == model.MauHoaDonId && x.Type == type && x.Status == true).OrderByDescending(x => x.NgayXacThuc).FirstOrDefaultAsync();
            if (mauHoaDonFileActive != null)
            {
                if (NgayThemHoaDon >= mauHoaDonFileActive.NgayXacThuc)
                {

                    mauHoaDonFile = mauHoaDonFileActive;
                }
                else
                {
                    mauHoaDonFile = await _db.MauHoaDonFiles.AsNoTracking().Where(x => x.MauHoaDonId == model.MauHoaDonId && x.Type == type &&
                    x.Status == false && x.NgayNgungSuDung >= NgayThemHoaDon && x.NgayXacThuc <= NgayThemHoaDon).OrderByDescending(x => x.NgayXacThuc).FirstOrDefaultAsync();
                    if (mauHoaDonFile == null)
                    {
                        mauHoaDonFile = mauHoaDonFileActive;
                    }
                }
                if (mauHoaDonFile == null) // if it's not in db then generate it
                {
                    var addedFileListVM = await AddDocFilesAsync(model);

                    string fileName = addedFileListVM.FirstOrDefault(x => x.MauHoaDonId == model.MauHoaDonId && x.Type == type).FileName;
                    model.FilePath = Path.Combine(docFolderPath, fileName);
                }
                else
                {
                    model.FilePath = Path.Combine(docFolderPath, mauHoaDonFile.FileName);

                }
            }
            else
            {
                var addedFileListVM = await AddDocFilesAsync(model);

                string fileName = addedFileListVM.FirstOrDefault(x => x.MauHoaDonId == model.MauHoaDonId && x.Type == type).FileName;
                model.FilePath = Path.Combine(docFolderPath, fileName);

            }


            if (!File.Exists(model.FilePath) || resetFile == true) // if physical file is not exist in server then generate it from byte
            {
                await File.WriteAllBytesAsync(model.FilePath, mauHoaDonFile.Binary);
            }

            // load from file path
            document.LoadFromFile(model.FilePath);

            // add reason tag for thay thế/điều chỉnh
            if (hasReason == true)
            {
                var coChu = int.Parse(model.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.CoChu).GiaTri);
                var kieuChu = model.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.KieuChu).GiaTri;
                var mauChu = model.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.MauChu).GiaTri;

                Section section = document.Sections[0];

                Paragraph replacePar = section.AddParagraph();
                TextRange trExchange = replacePar.AppendText("<reason>");
                trExchange.CharacterFormat.FontSize = 10 + coChu;
                trExchange.CharacterFormat.FontName = kieuChu;
                trExchange.CharacterFormat.TextColor = ColorTranslator.FromHtml(mauChu);
                section.Paragraphs.Insert(0, section.Paragraphs[section.Paragraphs.Count - 1]);
            }

            // get begin row
            beginRow = model.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.ThietLapDongKyHieuCot).GiaTri == "true" ? 2 : 1;

            if (model.MauHoaDonTuyChinhChiTiets.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoLuongNhapXuat && x.Checked == true))
            {
                beginRow += 1;
            }

            return (document, beginRow);
        }

        /// <summary>
        /// add doc files to server and db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<MauHoaDonFileViewModel>> AddDocFilesAsync(MauHoaDonViewModel model)
        {
            string PhatHanhBoi = await GetPhatHanhBoiFromHopDong();
            // get or add doc folder
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");
            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }

            // if is update then remove old files
            if (!string.IsNullOrEmpty(model.MauHoaDonId))
            {
                var oldFiles = await _db.MauHoaDonFiles.Where(x => x.MauHoaDonId == model.MauHoaDonId && x.Status == true).ToListAsync();

                // remove files in server
                foreach (var item in oldFiles)
                {
                    var oldFilePath = Path.Combine(docFolderPath, item.FileName);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                    item.Status = false;
                    item.NgayNgungSuDung = DateTime.Now;

                }
                /// Kiem tra neu doi nha giai phap thi chuyen lai chan trang
                if (oldFiles.Count() > 0)
                {
                    if (oldFiles[0].MaSoThueGiaiPhap != PhatHanhBoi)
                    {
                        string stringPhatHanhBoiPMBK = String.Format("Phát hành bởi Hóa đơn Bách Khoa - {0} - MST: {1}", "Công ty Cổ phần Phát triển và Ứng dụng phần mềm Bách Khoa", "0202029650");
                        string stringPhatHanhBoiDVBK = String.Format("Phát hành bởi Hóa đơn Bách Khoa - {0} - MST: {1}", "Công ty Cổ phần Thiết bị điện - điện tử Bách Khoa", "0200784873");
                        var MauHoaDonTuyChinhChiTiet = await _db.MauHoaDonTuyChinhChiTiets.FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhatHanhBoi && x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                        var MauHoaDonTuyChinhChiTietParent = await _db.MauHoaDonTuyChinhChiTiets.FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhatHanhBoi && x.IsParent == true);
                        var MauHoaDonTuyChinhChiTietInModel = model.MauHoaDonTuyChinhChiTiets.FirstOrDefault(x => x.MauHoaDonTuyChinhChiTietId == MauHoaDonTuyChinhChiTietParent.MauHoaDonTuyChinhChiTietId).Children;
                        if (PhatHanhBoi == "0202029650")
                        {
                            MauHoaDonTuyChinhChiTiet.GiaTri = stringPhatHanhBoiPMBK;
                        }
                        else
                        {
                            MauHoaDonTuyChinhChiTiet.GiaTri = stringPhatHanhBoiDVBK;
                        }
                        if (MauHoaDonTuyChinhChiTietInModel != null)
                        {
                            foreach (var item in MauHoaDonTuyChinhChiTietInModel)
                            {
                                if (item.MauHoaDonTuyChinhChiTietId == MauHoaDonTuyChinhChiTiet.MauHoaDonTuyChinhChiTietId)
                                {
                                    item.GiaTri = MauHoaDonTuyChinhChiTiet.GiaTri;
                                }
                            }
                            model.MauHoaDonTuyChinhChiTiets.FirstOrDefault(x => x.MauHoaDonTuyChinhChiTietId == MauHoaDonTuyChinhChiTietParent.MauHoaDonTuyChinhChiTietId).Children = MauHoaDonTuyChinhChiTietInModel;
                        }
                        _db.MauHoaDonTuyChinhChiTiets.Update(MauHoaDonTuyChinhChiTiet);


                    }
                }
                // remove files in db
                //   _db.MauHoaDonFiles.RemoveRange(oldFiles);
                await _db.SaveChangesAsync();

            }


            var listFilesToAdd = new List<MauHoaDonFile>();
            //Debug faster
        //    var typeOfMauHoaDons = Enum.GetValues(typeof(HinhThucMauHoaDon)).Cast<HinhThucMauHoaDon>().Where(x => x == HinhThucMauHoaDon.HoaDonMauCoBan).ToList();
            var typeOfMauHoaDons = Enum.GetValues(typeof(HinhThucMauHoaDon)).Cast<HinhThucMauHoaDon>().ToList();

            List<Task<(Document, HinhThucMauHoaDon)>> tasks = new List<Task<(Document, HinhThucMauHoaDon)>>();

            foreach (var type in typeOfMauHoaDons)
            {
                tasks.Add(CreateMauAsync(model, type, _hostingEnvironment, _httpContextAccessor));
            }

            var listDocument = await Task.WhenAll(tasks);
            foreach (var item in listDocument)
            {
                var docFileName = $"{Guid.NewGuid()}.docx";
                var docPath = Path.Combine(docFolderPath, docFileName);
                item.Item1.SaveToFile(docPath, Spire.Doc.FileFormat.Docx);

                listFilesToAdd.Add(new MauHoaDonFile
                {
                    MauHoaDonId = model.MauHoaDonId,
                    Type = item.Item2,
                    FileName = docFileName,
                    NgayXacThuc = DateTime.Now,
                    MaSoThueGiaiPhap = PhatHanhBoi,
                    Status = true,
                    Binary = File.ReadAllBytes(docPath)
                });
                //await UpdateAllHoaDonChuaKyWhenXacThuc(model.MauHoaDonId);
                ;
            }

            await _db.MauHoaDonFiles.AddRangeAsync(listFilesToAdd);
            await _db.SaveChangesAsync();

            var result = _mp.Map<List<MauHoaDonFileViewModel>>(listFilesToAdd);
            return result;
        }

        private async Task<(Document, HinhThucMauHoaDon)> CreateMauAsync(MauHoaDonViewModel model, HinhThucMauHoaDon type, IHostingEnvironment env, IHttpContextAccessor context)
        {
            return await Task.Run(() =>
            {   
                if(model.LoaiHoaDon == LoaiHoaDon.TemVeGTGT)
                {
                    Document doc = MauVeHelper.TaoMauVEDoc(model, type, env, context, out _);
                    return (doc, type);
                } else
                {
                    Document doc = MauHoaDonHelper.TaoMauHoaDonDoc(model, type, env, context, out _);
                    return (doc, type);
                }

            });
        }

        public async Task<bool> AddDongTienThanhToanVaTyGiaChiTietAsync()
        {
            var mauHoaDons = await (from mhd in _db.MauHoaDons
                                    join mct in _db.MauHoaDonTuyChinhChiTiets.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DongTienThanhToan || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyGiaHHDV)
                                    on mhd.MauHoaDonId equals mct.MauHoaDonId into tmpMauChiTiets
                                    from mct in tmpMauChiTiets.DefaultIfEmpty()
                                    where mhd.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT && mct == null
                                    select new MauHoaDonViewModel
                                    {
                                        MauHoaDonId = mhd.MauHoaDonId,
                                        LoaiThueGTGT = mhd.LoaiThueGTGT
                                    })
                                    .GroupBy(x => x.MauHoaDonId)
                                    .Select(x => new MauHoaDonViewModel
                                    {
                                        MauHoaDonId = x.Key,
                                        LoaiThueGTGT = x.First().LoaiThueGTGT,
                                        MauHoaDonTuyChinhChiTiets = (from tcct in _db.MauHoaDonTuyChinhChiTiets
                                                                     where tcct.MauHoaDonId == x.Key && (tcct.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua || tcct.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TienThueHHDV)
                                                                     orderby tcct.LoaiContainer
                                                                     select new MauHoaDonTuyChinhChiTietViewModel
                                                                     {
                                                                         MauHoaDonId = x.Key,
                                                                         GiaTri = tcct.GiaTri,
                                                                         TuyChinhChiTiet = tcct.TuyChinhChiTiet,
                                                                         TenTiengAnh = tcct.TenTiengAnh,
                                                                         GiaTriMacDinh = tcct.GiaTriMacDinh,
                                                                         DoRong = tcct.DoRong,
                                                                         KieuDuLieuThietLap = tcct.KieuDuLieuThietLap,
                                                                         Loai = tcct.Loai,
                                                                         LoaiChiTiet = tcct.LoaiChiTiet,
                                                                         LoaiContainer = tcct.LoaiContainer,
                                                                         IsParent = tcct.IsParent,
                                                                         Checked = tcct.Checked,
                                                                         Disabled = tcct.Disabled,
                                                                         CustomKey = tcct.CustomKey
                                                                     })
                                                                     .ToList()
                                    })
                                    .ToListAsync();

            var mauHoaDonTuyChonMoRongs = await _db.MauHoaDonTuyChinhChiTiets
                .Where(x => mauHoaDons.Where(y => y.LoaiThueGTGT == LoaiThueGTGT.MauNhieuThueSuat).Select(y => y.MauHoaDonId).Contains(x.MauHoaDonId) && x.IsParent == true && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.GhiChuHHDV || (x.LoaiChiTiet >= LoaiChiTietTuyChonNoiDung.TruongMoRongChiTiet1 && x.LoaiChiTiet <= LoaiChiTietTuyChonNoiDung.TruongMoRongChiTiet10)))
                .OrderBy(x => x.LoaiChiTiet)
                .ToListAsync();

            if (mauHoaDons.Any())
            {
                var addedList = new List<MauHoaDonTuyChinhChiTiet>();

                foreach (var item in mauHoaDons)
                {
                    foreach (var child in item.MauHoaDonTuyChinhChiTiets)
                    {
                        if (child.IsParent == true)
                        {
                            child.Checked = false;
                            child.Disabled = false;
                            child.STT = child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua ? 7 : 15;
                        }
                        if (child.LoaiContainer == 0 || child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                        {
                            child.GiaTri = child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua ? "Đồng tiền thanh toán" : "Tỷ giá";
                        }
                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                        {
                            child.GiaTri = child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua ? "(Payment Currency)" : "(Exchange Rate)";
                        }

                        if ((child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua) || (item.LoaiThueGTGT == LoaiThueGTGT.MauNhieuThueSuat && child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TienThueHHDV))
                        {
                            child.LoaiChiTiet = child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua ? LoaiChiTietTuyChonNoiDung.DongTienThanhToan : LoaiChiTietTuyChonNoiDung.TyGiaHHDV;
                            var mapper = _mp.Map<MauHoaDonTuyChinhChiTiet>(child);
                            addedList.Add(mapper);
                        }
                    }

                    if (item.LoaiThueGTGT == LoaiThueGTGT.MauNhieuThueSuat)
                    {
                        var tuyChinhChiTiets = mauHoaDonTuyChonMoRongs.Where(x => x.MauHoaDonId == item.MauHoaDonId).ToList();
                        for (int i = 0; i < tuyChinhChiTiets.Count; i++)
                        {
                            tuyChinhChiTiets[i].STT = i + 16;
                        }
                    }
                }

                await _db.MauHoaDonTuyChinhChiTiets.AddRangeAsync(addedList);
                var result = await _db.SaveChangesAsync();
                return result > 0;
            }

            return true;
        }

        public async Task<bool> ClearOldFormatMauHoaDonsAsync()
        {
            // get or add doc folder
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");

            //var oldFiles = await _db.MauHoaDonFiles.Where(x => x.Type == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi ||
            //                                                    x.Type == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau ||
            //                                                    x.Type == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
            //                                                    x.Type == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All)
            //                                        .ToListAsync();

            var oldFiles = await (from mf in _db.MauHoaDonFiles
                                  join mhd in _db.MauHoaDons on mf.MauHoaDonId equals mhd.MauHoaDonId
                                  // where mhd.LoaiHoaDon == LoaiHoaDon.PXKKiemVanChuyenNoiBo || mhd.LoaiHoaDon == LoaiHoaDon.PXKHangGuiBanDaiLy
                                  select mf).ToListAsync();

            foreach (var item in oldFiles)
            {
                var oldFilePath = Path.Combine(docFolderPath, item.FileName);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }
            // remove files in db
            _db.MauHoaDonFiles.RemoveRange(oldFiles);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }


        /// <summary>
        /// Check tồn tại mẫu hóa đơn theo id không
        /// </summary>
        /// <param name="mauHoaDonId"></param>
        /// <returns></returns>
        public async Task<(bool, List<string>)> CheckExistAsync(string mauHoaDonId)
        {
            var mauHoaDonIds = mauHoaDonId.Split(";");
            var mauHoaDons = await _db.MauHoaDons.Where(x => mauHoaDonIds.Contains(x.MauHoaDonId)).ToListAsync();

            bool result = mauHoaDons.Any();
            var listIdNotExist = mauHoaDonIds.Where(x => !mauHoaDons.Any(y => y.MauHoaDonId == x)).ToList();

            return (result, listIdNotExist);
        }

        public async Task<List<MauHoaDonFileViewModel>> AddDocFilesTemplateAsync(MauHoaDonViewModel model)
        {
            // get or add doc folder
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var docFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.DOC}");
            if (!Directory.Exists(docFolderPath))
            {
                Directory.CreateDirectory(docFolderPath);
            }

            // if is update then remove old files
            if (!string.IsNullOrEmpty(model.MauHoaDonId))
            {
                var oldFiles = await _db.MauHoaDonFiles.Where(x => x.MauHoaDonId == model.MauHoaDonId).ToListAsync();

                // remove files in server
                foreach (var item in oldFiles)
                {
                    var oldFilePath = Path.Combine(docFolderPath, item.FileName);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                // remove files in db
                _db.MauHoaDonFiles.RemoveRange(oldFiles);
                await _db.SaveChangesAsync();
            }

            var listFilesToAdd = new List<MauHoaDonFile>();
            var typeOfMauHoaDons = Enum.GetValues(typeof(HinhThucMauHoaDon)).Cast<HinhThucMauHoaDon>()/*.Where(x => x == HinhThucMauHoaDon.HoaDonMauNgoaiTe).ToList()*/;

            List<Task<(Document, HinhThucMauHoaDon)>> tasks = new List<Task<(Document, HinhThucMauHoaDon)>>();

            foreach (var type in typeOfMauHoaDons)
            {
                tasks.Add(CreateMauAsync(model, type, _hostingEnvironment, _httpContextAccessor));
            }

            var listDocument = await Task.WhenAll(tasks);
            foreach (var item in listDocument)
            {
                var docFileName = $"{Guid.NewGuid()}.docx";
                var docPath = Path.Combine(docFolderPath, docFileName);
                item.Item1.SaveToFile(docPath, Spire.Doc.FileFormat.Docx);

                listFilesToAdd.Add(new MauHoaDonFile
                {
                    MauHoaDonId = model.MauHoaDonId,
                    Type = item.Item2,
                    FileName = docFileName,
                    Binary = File.ReadAllBytes(docPath)
                });
            }

            await _db.MauHoaDonFiles.AddRangeAsync(listFilesToAdd);
            await _db.SaveChangesAsync();

            var result = _mp.Map<List<MauHoaDonFileViewModel>>(listFilesToAdd);
            return result;
        }
        /// <summary>
        /// Let 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<MauHoaDonFile>> GetListMauHoaDonFileActive(string MauHoaDonId, string HoaDonDienTuId)
        {
            List<MauHoaDonFile> result = new List<MauHoaDonFile>();
            try
            {
                result = await (from mhd_file in _db.MauHoaDonFiles
                                where mhd_file.MauHoaDonId == MauHoaDonId && mhd_file.Status == true
                                select mhd_file).ToListAsync();

                if (result.Count() > 0)
                {
                    List<MauFile_HoaDon> mauFile_HoaDons = new List<MauFile_HoaDon>();

                    foreach (var item in result)
                    {
                        MauFile_HoaDon mauFile_HoaDon = new MauFile_HoaDon();
                        mauFile_HoaDon.MauFile_HoaDonId = Guid.NewGuid().ToString();
                        mauFile_HoaDon.STT = (int)item.Type;
                        mauFile_HoaDon.HoaDonDienTuId = HoaDonDienTuId;
                        mauFile_HoaDon.MauHoaDonFileId = item.MauHoaDonFileId;
                        mauFile_HoaDon.CreatedDate = DateTime.Now;
                        mauFile_HoaDons.Add(mauFile_HoaDon);

                    }
                    await _db.MauFile_HoaDons.AddRangeAsync(mauFile_HoaDons);
                    await _db.SaveChangesAsync();
                }


            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        /// <summary>
        ///  Delete Old Template and Update
        /// </summary>
        /// <param name="MauHoaDonId"></param>
        /// <param name="HoaDonDienTuId"></param>
        /// <returns></returns>
        public async Task<List<MauHoaDonFile>> UpdateHoaDonWhenChangMauHoaDon(string MauHoaDonId, string HoaDonDienTuId)
        {
            List<MauHoaDonFile> result = new List<MauHoaDonFile>();
            try
            {
                List<MauFile_HoaDon> listmauFilesOld = await _db.MauFile_HoaDons.Where(x => x.HoaDonDienTuId == HoaDonDienTuId).ToListAsync();
                if (listmauFilesOld.Count() > 0)
                {
                    _db.MauFile_HoaDons.RemoveRange(listmauFilesOld);
                }
                result = await (from mhd_file in _db.MauHoaDonFiles
                                where mhd_file.MauHoaDonId == MauHoaDonId && mhd_file.Status == true
                                select mhd_file).ToListAsync();

                if (result.Count() > 0)
                {
                    List<MauFile_HoaDon> mauFile_HoaDons = new List<MauFile_HoaDon>();

                    foreach (var item in result)
                    {
                        MauFile_HoaDon mauFile_HoaDon = new MauFile_HoaDon();
                        mauFile_HoaDon.MauFile_HoaDonId = Guid.NewGuid().ToString();
                        mauFile_HoaDon.STT = (int)item.Type;
                        mauFile_HoaDon.HoaDonDienTuId = HoaDonDienTuId;
                        mauFile_HoaDon.MauHoaDonFileId = item.MauHoaDonFileId;
                        mauFile_HoaDon.CreatedDate = DateTime.Now;
                        mauFile_HoaDons.Add(mauFile_HoaDon);

                    }
                    await _db.MauFile_HoaDons.AddRangeAsync(mauFile_HoaDons);
                    await _db.SaveChangesAsync();
                }


            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public async Task<List<MauHoaDonFile>> UpdateAllHoaDonChuaKyWhenXacThuc(string MauHoaDonId)
        {
            List<MauHoaDonFile> result = new List<MauHoaDonFile>();
            try
            {
                List<HoaDonDienTu> listHoaDonChuaKy = await _db.HoaDonDienTus.Where(x => x.MauHoaDonId == MauHoaDonId && (x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaKyDienTu || x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaPhatHanh)).ToListAsync();
                if (listHoaDonChuaKy.Count() > 0)
                {
                    foreach (var element in listHoaDonChuaKy)
                    {
                        List<MauFile_HoaDon> listmauFilesOld = await _db.MauFile_HoaDons.Where(x => x.HoaDonDienTuId == element.HoaDonDienTuId).ToListAsync();
                        if (listmauFilesOld.Count() > 0)
                        {
                            _db.MauFile_HoaDons.RemoveRange(listmauFilesOld);
                        }
                        result = await (from mhd_file in _db.MauHoaDonFiles
                                        where mhd_file.MauHoaDonId == MauHoaDonId && mhd_file.Status == true
                                        select mhd_file).ToListAsync();

                        if (result.Count() > 0)
                        {
                            List<MauFile_HoaDon> mauFile_HoaDons = new List<MauFile_HoaDon>();

                            foreach (var item in result)
                            {
                                MauFile_HoaDon mauFile_HoaDon = new MauFile_HoaDon();
                                mauFile_HoaDon.MauFile_HoaDonId = Guid.NewGuid().ToString();
                                mauFile_HoaDon.STT = (int)item.Type;
                                mauFile_HoaDon.HoaDonDienTuId = element.HoaDonDienTuId;
                                mauFile_HoaDon.MauHoaDonFileId = item.MauHoaDonFileId;
                                mauFile_HoaDon.CreatedDate = DateTime.Now;
                                mauFile_HoaDons.Add(mauFile_HoaDon);

                            }
                            await _db.MauFile_HoaDons.AddRangeAsync(mauFile_HoaDons);
                        }
                    }
                    await _db.SaveChangesAsync();

                }


            }
            catch (Exception ex)
            {

                Tracert.WriteLog("Bug Update Hoa Don Chua Ky or Phat Hanh");
            }
            return result;

        }
        /// <summary>
        /// Update MauHoaDonId of all hóa đơn chưa ký
        /// </summary>
        /// <param name="MauHoaDonId"></param>
        /// <param name="MauHoaDonIdOld"></param>
        /// <returns></returns>
        public async Task<List<MauHoaDonFile>> UpdateAllHoaDonChuaKyWhenXacThuBytime(string MauHoaDonId, string MauHoaDonIdOld)
        {
            List<MauHoaDonFile> result = new List<MauHoaDonFile>();
            try
            {
                List<HoaDonDienTu> listHoaDonChuaKy = await _db.HoaDonDienTus.Where(x => x.MauHoaDonId == MauHoaDonIdOld && (x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaKyDienTu || x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaPhatHanh)).ToListAsync();
                if (listHoaDonChuaKy.Count() > 0)
                {
                    foreach (var element in listHoaDonChuaKy)
                    {
                        element.MauHoaDonId = MauHoaDonId;
                    }
                    await _db.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {

                Tracert.WriteLog("Bug Update Hoa Don Chua Ky or Phat Hanh");
            }
            return result;

        }

        public async Task<List<MauHoaDonFile>> UpdateAllHoaDonChuaKyWhenChangeHopDong(string MauHoaDonId, List<MauHoaDonFile> listFileAddNew)
        {
            List<MauHoaDonFile> result = new List<MauHoaDonFile>();
            try
            {
                List<HoaDonDienTu> listHoaDonChuaKy = await _db.HoaDonDienTus.Where(x => x.MauHoaDonId == MauHoaDonId && (x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaKyDienTu || x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaPhatHanh)).ToListAsync();
                if (listHoaDonChuaKy.Count() > 0)
                {
                    foreach (var element in listHoaDonChuaKy)
                    {
                        List<MauFile_HoaDon> listmauFilesOld = await _db.MauFile_HoaDons.Where(x => x.HoaDonDienTuId == element.HoaDonDienTuId).ToListAsync();
                        if (listmauFilesOld.Count() > 0)
                        {
                            _db.MauFile_HoaDons.RemoveRange(listmauFilesOld);
                        }
                        if (listFileAddNew.Count() > 0)
                        {
                            List<MauFile_HoaDon> mauFile_HoaDons = new List<MauFile_HoaDon>();

                            foreach (var item in listFileAddNew)
                            {
                                MauFile_HoaDon mauFile_HoaDon = new MauFile_HoaDon();
                                mauFile_HoaDon.MauFile_HoaDonId = Guid.NewGuid().ToString();
                                mauFile_HoaDon.STT = (int)item.Type;
                                mauFile_HoaDon.HoaDonDienTuId = element.HoaDonDienTuId;
                                mauFile_HoaDon.MauHoaDonFileId = item.MauHoaDonFileId;
                                mauFile_HoaDon.CreatedDate = DateTime.Now;
                                mauFile_HoaDons.Add(mauFile_HoaDon);

                            }
                            await _db.MauFile_HoaDons.AddRangeAsync(mauFile_HoaDons);
                        }
                    }
                    await _db.SaveChangesAsync();

                }


            }
            catch (Exception ex)
            {

                Tracert.WriteLog("Bug Update Hoa Don Chua Ky or Phat Hanh");
            }
            return result;

        }




        public async Task<string> GetPhatHanhBoiFromHopDong()
        {
            string result = "0202029650";
            try
            {
                List<HopDongHoaDon> listHopDongMoiNhat = await _db.HopDongHoaDons.Where(x => x.TrangThaiHopDong == 2 && x.NgayDuyet != null).OrderByDescending(x => x.NgayDuyet).ToListAsync();
                if (listHopDongMoiNhat.Count() > 0)
                {
                    result = listHopDongMoiNhat[0].MstGp;
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog("Error Phat Hanh Boi From Hop Dpng" + ex.Message);
            }
            return result;


        }

    }

}