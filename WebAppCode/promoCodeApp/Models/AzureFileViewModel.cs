using System.ComponentModel.DataAnnotations;

namespace promoCodeApp.Models
{
    public class AzureFileViewModel
    {
        [Display(Name = "Attached Files")]
        public string FileName { get; set; }
        public string AzureUrl { get; set; }
    }
}
