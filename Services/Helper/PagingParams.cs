namespace ManagementServices.Helper
{
    public class PagingParams
    {
        public string Date { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string SortValue { get; set; }
        public string SortKey { get; set; }
        public string KeywordCol { get; set; }
        public string ColName { get; set; }
    }
}
