using System;
using System.ComponentModel;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Attributes
{
    public class CustomDataTypeAttribute : Attribute
    {
        public CustomDataTypeAttribute(CustomDataType dataType)
        {
        }
    }

    public enum CustomDataType
    {
        [Description("Chuỗi ký tự")]
        String,
        [Description("Số")]
        Number,
        [Description("Ngày")]
        Date,
        [Description("Ngày giờ")]
        DateTime
    }
}
