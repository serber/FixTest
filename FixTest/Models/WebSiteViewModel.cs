using System.ComponentModel.DataAnnotations;

namespace FixTest.Models
{
    public class WebSiteViewModel
    {
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Адрес сайта")]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Интервал проверки (сек.)")]
        public long CheckInterval { get; set; }
    }
}