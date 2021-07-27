using System;
using System.ComponentModel;
using System.Reflection;

namespace Services.Helper.LogHelper
{
    public class ChangeLogModel
    {
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DetailType DetailType { get; set; }
    }

    public enum DetailType
    {
        [Description("Thêm dòng chi tiết")]
        Create,
        [Description("Sửa dòng chi tiết")]
        Update,
        [Description("Xóa dòng chi tiết")]
        Delete
    }

    public class ChangeLogDetails
    {
        public string Id { get; set; }
        public string DetailKey { get; set; }
        public object Entry { get; set; }
        public PropertyInfo[] Properties { get; set; }
    }
}
