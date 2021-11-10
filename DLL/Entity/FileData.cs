using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity
{
    public class FileData
    {
        public string FileDataId { set; get; }

        /// <summary>
        /// 1 - XML
        /// 2 - PDF
        /// 3 - DOC
        /// 4 - OTHER
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        /// Thời gian ghi nhận
        /// </summary>
        public DateTime DateTime { set; get; }

        /// <summary>
        /// Nội dung file
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Binary { set; get; }
    }
}
