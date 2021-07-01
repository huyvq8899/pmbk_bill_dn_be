using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.Repositories.Interfaces.DanhMuc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class MauHoaDonService : IMauHoaDonService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MauHoaDonService(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
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
    }
}
