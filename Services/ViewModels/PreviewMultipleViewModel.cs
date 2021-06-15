using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class PreviewMultipleViewModel
    {
        public List<string> listIds { get; set; }
        public UserViewModel ActionUser { get; set; }
        public int? Type { get; set; }
        public bool IsTienMat { get; set; } //Phần in quỹ
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
