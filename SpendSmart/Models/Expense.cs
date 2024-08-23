using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? UploadedImage { get; set; }
    }
}
