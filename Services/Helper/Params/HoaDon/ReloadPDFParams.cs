﻿using Microsoft.AspNetCore.Http;
using System.Numerics;

namespace Services.Helper.Params.HoaDon
{
    public class ReloadPDFParams
    {
        public string Password { get; set; }
        public string KyHieu { get; set; }
        public long? SoHoaDon { get; set; }
        public string MaSoThue { get; set; }
    }

    public class ReloadPDFResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class ReloadXmlParams
    {
        public IFormFile File { get; set; }
        public string MaSoThue { get; set; }
    }

    public class ReloadXmlResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
