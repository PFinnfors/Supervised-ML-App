using System.ComponentModel.DataAnnotations;

namespace BayesApp.Models
{
    public class TennisModel
    {
        [Required]
        public string Outlook { get; set; }

        [Required]
        public string Temperature { get; set; }

        [Required]
        public string Humidity { get; set; }

        [Required]
        public string Wind { get; set; }
    }
}