using System.ComponentModel.DataAnnotations;

namespace Demo.WebApp.Models
{
    public class ContactInfo
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
