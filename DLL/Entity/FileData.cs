using System;

namespace DLL.Entity
{
    public class FileData
    {
        public string FileDataId { set; get; }

        /// <summary>
        /// 1 - XML
        /// 2 - PDF
        /// 3 - DOC
        /// 4 - OTHER (attach, vv...)
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
        /// Byte
        /// </summary>
        public byte[] Binary { set; get; }

        /// <summary>
        /// Tên file -> sau này restore sẽ theo tên này
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Trạng thái đã ký hay chưa để còn phân thư mục
        /// </summary>
        public bool? IsSigned { get; set; }
    }
}
