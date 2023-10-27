using Microsoft.AspNetCore.Identity;

namespace App_FDark.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FavorsLinksIds { get; set; }
        public string Tag {  get; set; }
        public string CharacterName { get; set; }
        public string ServerName { get; set; }

    }
}
