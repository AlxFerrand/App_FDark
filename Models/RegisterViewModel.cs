using System.ComponentModel.DataAnnotations;

namespace App_FDark.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        public string Tag { get; set; }
        public string CharacterName { get; set; }
        public string ServerName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
