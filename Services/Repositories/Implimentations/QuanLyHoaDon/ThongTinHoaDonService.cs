using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class ThongTinHoaDonService : IThongTinHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ThongTinHoaDonService(Datacontext datacontext, IMapper mapper, 
            IHostingEnvironment hostingEnvironment)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ThongTinHoaDon> InsertAsync(ThongTinHoaDon model)
        {
            model.Id = Guid.NewGuid().ToString();
            await _db.ThongTinHoaDons.AddAsync(model);
            var result = await _db.SaveChangesAsync() > 0;
            if (result)
            {
                return model;
            }
            else
            {
                return null;
            }
        }

        public async Task<ThongTinHoaDon> UpdateAsync(ThongTinHoaDon model)
        {
            var entity = await _db.ThongTinHoaDons.FirstOrDefaultAsync(x => x.Id == model.Id);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            if (result)
            {
                return model;
            }
            else
            {
                return null;
            }
        }
    }
}
