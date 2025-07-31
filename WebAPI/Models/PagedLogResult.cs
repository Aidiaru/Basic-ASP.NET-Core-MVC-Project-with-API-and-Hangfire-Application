namespace WebAPI.Models
{
    public class PagedLogResult
    {
        public List<Log> Logs { get; set; }
        public int TotalCount { get; set; }
    }
}
