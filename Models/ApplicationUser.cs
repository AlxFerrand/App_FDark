using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaybeNull]
        public string FavorsLinksIds { get; set; }
        public string Tag {  get; set; }
        public string CharacterName { get; set; }
        public string ServerName { get; set; }

    }
}
