using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string? Exception { get; set; }
    }
}