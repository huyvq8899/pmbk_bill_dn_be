using Services.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface ITruongDuLieuMoRongService
    {
        Task<bool> InsertRangeAsync(List<TruongDuLieuMoRongViewModel> range);
        Task<bool> UpdateRangeAsync(List<TruongDuLieuMoRongViewModel> range);

    }
}
