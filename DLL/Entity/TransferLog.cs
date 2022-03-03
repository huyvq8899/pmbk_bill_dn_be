using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace DLL.Entity
{
    public class TransferLog
    {
        public string TransferLogId { set; get; }

        /// <summary>
        /// Thời gian ghi nhận
        /// </summary>
        public DateTime DateTime { set; get; }

        public string MNGui { set; get; }

        public string MNNhan { set; get; }

        /// <summary>
        /// 3 - Phản hồi
        /// 2 - Nhận
        /// 1 - Truyền
        /// </summary>
        public int Type { set; get; }
                
        public int MLTDiep {set; get;}

        public string MTDiep { set; get; }

        public string MTDTChieu { set; get; }

        public string XMLData { set; get; }

        /// <summary>
        /// XML thông điệp
        /// </summary>
        [Required]
        public string DataXML { get; set; }

        /// <summary>
        /// XmlWrapper
        /// </summary>
        public XElement XmlWrapper
        {
            get { return XElement.Parse(DataXML); }
            set { DataXML = value.ToString(); }
        }
    }
}
