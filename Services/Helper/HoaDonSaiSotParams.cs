﻿using Services.Helper.Params.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class HoaDonSaiSotParams
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public byte LoaiHoaDon { get; set; }
        public byte HinhThucHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }

        public List<FilterColumn> FilterColumns { get; set; }
        public string SortKey { get; set; }
        public string SortValue { get; set; }
    }

    public class FileXMLThongDiepGuiParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public string DataXML { get; set; }
    }

    public class DuLieuXMLGuiCQTParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public string XMLFilePath { get; set; }
        public string MaSoThue { get; set; }
        public string MaThongDiep { get; set; }
    }
}
