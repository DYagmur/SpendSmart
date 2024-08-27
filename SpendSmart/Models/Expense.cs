using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;

        public string SmallImagePath { get; set; } = string.Empty;
        public string MediumImagePath { get; set; } = string.Empty;
        public string LargeImagePath { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? UploadedImage { get; set; }
    }
}
