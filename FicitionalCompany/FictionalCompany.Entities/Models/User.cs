using System.ComponentModel.DataAnnotations;

namespace FictionalCompany.Entities.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }= string.Empty;

        [Required]
        [EmailAddress]
        public string Mail { get; set; }=string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Skillsets { get; set; } = string.Empty;
        public string Hobby { get; set; } = string.Empty;
    }
}
