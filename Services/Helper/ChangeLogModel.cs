namespace Services.Helper
{
    public class ChangeLogModel
    {
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
