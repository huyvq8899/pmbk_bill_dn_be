using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT
{
    public partial class TTChung
    {
        /// <summary>
        /// Phiên bản XML. Trong khuyến nghị này có giá trị là 2.0.0
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string PBan { set; get; } = "2.0.0";

        
        public string MNGui { set; get; }

       
        public string MNNhan { set; get; }

        
        public string MLTDiep { set; get; }

        
        public string MTDiep { set; get; }

       
        public string MTDTChieu { set; get; }

        public string MST { set; get; }

        public int SLuong { set; get; }


    }
}
