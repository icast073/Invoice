using System;
namespace Invoice.Repository.Filter
{
    public class BaseFilter
    {
        public string Criteria { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
