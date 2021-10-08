using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HeThong
{
    public class NhapKhauParams
    {
        public IList<IFormFile> Files { get; set; }
        public int ModeValue { get; set; } //chế độ nhập khẩu: 1: nhập khẩu thêm mới; 2: nhâp khẩu cập nhật
    }
}
