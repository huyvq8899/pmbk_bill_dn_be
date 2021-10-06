﻿using AutoMapper;
using DLL;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.TienIch;
using Spire.Doc;
using Spire.Doc.Fields;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class MauHoaDonService : IMauHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public MauHoaDonService(Datacontext datacontext,
            IMapper mapper,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == id);
            _db.MauHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;

            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, RefType.MauHoaDon, _db);

            return result;
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
                QuyDinhApDung = x.QuyDinhApDung,
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
                if (@params.IsThongBaoPhatHanh == true)
                {
                    query = from q in query
                            join tbphct in _db.ThongBaoPhatHanhChiTiets on q.MauHoaDonId equals tbphct.MauHoaDonId
                            join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                            where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
                            group q by q.MauHoaDonId into g
                            select new MauHoaDonViewModel
                            {
                                MauHoaDonId = g.Key,
                                Ten = g.First().Ten,
                                SoThuTu = g.First().SoThuTu,
                                MauSo = g.First().MauSo,
                                KyHieu = g.First().KyHieu,
                                TenBoMau = g.First().TenBoMau,
                                QuyDinhApDung = g.First().QuyDinhApDung,
                                LoaiHoaDon = g.First().LoaiHoaDon,
                                LoaiMauHoaDon = g.First().LoaiMauHoaDon,
                                LoaiThueGTGT = g.First().LoaiThueGTGT,
                                LoaiKhoGiay = g.First().LoaiKhoGiay,
                                LoaiNgonNgu = g.First().LoaiNgonNgu,
                                Status = true,
                                CreatedBy = g.First().CreatedBy,
                                CreatedDate = g.First().CreatedDate,
                                NgayKy = g.First().NgayKy,
                            };
                }
            }

            var result = await query
                .OrderBy(x => x.MauSo)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<MauHoaDonViewModel>> GetAllPagingAsync(MauHoaDonParams @params)
        {
            var query = (from mhd in _db.MauHoaDons
                         join u in _db.Users on mhd.CreatedBy equals u.UserId into tmpUsers
                         join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId into tmpTBPHCTs
                         from tbphct in tmpTBPHCTs.DefaultIfEmpty()
                         from u in tmpUsers.DefaultIfEmpty()
                         where @params.MauHoaDonDuocPQ.Contains(mhd.MauHoaDonId) || @params.IsAdmin == true
                         orderby mhd.CreatedDate descending
                         select new MauHoaDonViewModel
                         {
                             MauHoaDonId = mhd.MauHoaDonId,
                             Ten = mhd.Ten,
                             SoThuTu = mhd.SoThuTu,
                             MauSo = mhd.MauSo,
                             KyHieu = mhd.KyHieu,
                             TenBoMau = mhd.TenBoMau,
                             Status = mhd.Status,
                             LoaiHoaDon = mhd.LoaiHoaDon,
                             TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                             TenQuyDinhApDung = mhd.QuyDinhApDung.GetDescription(),
                             Username = u != null ? u.UserName : string.Empty,
                             ModifyDate = mhd.ModifyDate,
                             NgayKy = mhd.NgayKy,
                             IsDaThongBaoPhatHanh = tbphct != null
                         })
                         .GroupBy(x => x.MauHoaDonId)
                         .Select(x => new MauHoaDonViewModel
                         {
                             MauHoaDonId = x.Key,
                             Ten = x.First().Ten,
                             SoThuTu = x.First().SoThuTu,
                             MauSo = x.First().MauSo,
                             KyHieu = x.First().KyHieu,
                             TenBoMau = x.First().TenBoMau,
                             Status = x.First().Status,
                             LoaiHoaDon = x.First().LoaiHoaDon,
                             TenLoaiHoaDon = x.First().TenLoaiHoaDon,
                             TenQuyDinhApDung = x.First().TenQuyDinhApDung,
                             Username = x.First().Username,
                             ModifyDate = x.First().ModifyDate,
                             NgayKy = x.First().NgayKy,
                             IsDaThongBaoPhatHanh = x.First().IsDaThongBaoPhatHanh,
                             TenTrangThaiTBPH = x.First().IsDaThongBaoPhatHanh == true ? "Đã thông báo phát hành" : "Chưa thông báo phát hành"
                         });

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.TenQuyDinhApDung))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenQuyDinhApDung);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenQuyDinhApDung);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.Ten))
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

                if (@params.SortKey == nameof(@params.Filter.Ten))
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

                if (@params.SortKey == nameof(@params.Filter.TenLoaiHoaDon))
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

                if (@params.SortKey == nameof(@params.Filter.SoThuTu))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoThuTu);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoThuTu);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.MauSo))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MauSo);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MauSo);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.KyHieu))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.KyHieu);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.KyHieu);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.Username))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Username);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Username);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.ModifyDate))
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

                if (@params.SortKey == nameof(@params.Filter.TenTrangThaiTBPH))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenTrangThaiTBPH);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenTrangThaiTBPH);
                    }
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
            var query = from mhd in _db.MauHoaDons
                        where mhd.MauHoaDonId == id
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
                                                                    STT = x.STT,
                                                                    Status = x.Status,
                                                                })
                                                                .OrderBy(x => x.LoaiContainer)
                                                                .ToList()
                                                         })
                                                         .OrderBy(x => x.STT)
                                                         .ToList()
                        };

            MauHoaDonViewModel result = await query.FirstOrDefaultAsync();
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
                        join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId
                        join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                        where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
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

            var thongBaoKetQuaHuyHDs = await _db.ThongBaoKetQuaHuyHoaDonChiTiets.Where(x => mauHoaDonIds.Contains(x.MauHoaDonId)).ToListAsync();
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
                        int? maxTuSo = thongBaoKetQuaHuyHDs.Where(x => x.MauHoaDonId == mauHoaDon.MauHoaDonId).Max(x => x.DenSo);
                        if (maxTuSo.HasValue && maxTuSo > 0)
                        {
                            tuSo = maxTuSo + 1;
                        }
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
            list = list.Where(x => x.LoaiHoaDon == @params.LoaiHoaDon && x.LoaiMauHoaDon == @params.LoaiMau && x.LoaiThueGTGT == @params.LoaiThueGTGT && x.LoaiNgonNgu == @params.LoaiNgonNgu && x.LoaiKhoGiay == @params.LoaiKhoGiay).ToList();
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

        public async Task<FileReturn> PreviewPdfAsync(MauHoaDonFileParams @params)
        {
            var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();
            var mauHoaDon = await GetByIdAsync(@params.MauHoaDonId);

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
                Directory.Delete(folderPath, true);
                return new FileReturn
                {
                    Bytes = bytes,
                    ContentType = MimeTypes.GetMimeType(docPath),
                    FileName = Path.GetFileName(docPath)
                };
            }
        }

        public async Task<bool> CheckTrungMauSoAsync(MauHoaDonViewModel model)
        {
            bool result = await _db.MauHoaDons.AnyAsync(x => x.MauSo == model.MauSo);
            return result;
        }

        public async Task<string> CheckAllowUpdateAsync(MauHoaDonViewModel model)
        {
            var check1 = await (from tbph in _db.ThongBaoPhatHanhs
                                join tbphct in _db.ThongBaoPhatHanhChiTiets on tbph.ThongBaoPhatHanhId equals tbphct.ThongBaoPhatHanhId
                                where tbphct.MauHoaDonId == model.MauHoaDonId && tbph.TrangThaiNop != TrangThaiNop.ChuaNop
                                select new
                                {
                                    TenTrangThaiNop = tbph.TrangThaiNop.GetDescription()
                                })
                                .Select(x => x.TenTrangThaiNop).Distinct().ToListAsync();

            if (check1.Any())
            {
                return string.Join(", ", check1);
            }

            if (model.NgayKy.HasValue)
            {
                return "signed";
            }

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

            foreach (var item in @params.HinhThucMauHoaDon)
            {
                var fileReturn = MauHoaDonHelper.PreviewFilePDF(mauHoaDon, item, hoSoHDDT, _hostingEnvironment, _httpContextAccessor);
                string pdfPath = Path.Combine(folderPath, $"{item.GetTenFile()}.pdf");
                File.WriteAllBytes(pdfPath, fileReturn.Bytes);
                filePaths.Add(pdfPath);
            }

            if (@params.DinhDangTepMau != 0)
            {
                for (int i = 0; i < filePaths.Count(); i++)
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
                }
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
                    Directory.Delete(folderPath, true);

                    return new FileReturn
                    {
                        Bytes = zipBytes,
                        ContentType = MimeTypes.GetMimeType(zipPath),
                        FileName = Path.GetFileName(zipPath)
                    };
                }
            }

            byte[] bytes = File.ReadAllBytes(filePaths[0]);
            Directory.Delete(folderPath, true);
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
                                                             MoTaChiTiet = nktc.MoTaChiTiet
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
    }
}
