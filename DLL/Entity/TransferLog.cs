using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity
{
    public class TransferLog
    {
        public string TransferLogId { set; get; }

        /// <summary>
        /// Thời gian ghi nhận
        /// </summary>
        public DateTime DateTime { set; get; }

        /// <summary>
        /// 2 - Nhận
        /// 1 - Truyền
        /// </summary>
        public int Type { set; get; }
                
        public int MLTDiep {set; get;}

        public string MTDiep { set; get; }

        public string MTDTChieu { set; get; }

        public string XMLData { set; get; }
    }
}
