// Models/LogsViewModel.cs
using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class LogsViewModel
    {
        public IEnumerable<Log> Logs { get; set; } = new List<Log>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string Search { get; set; } = string.Empty;
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}