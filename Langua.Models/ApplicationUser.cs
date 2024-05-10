using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Langua.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Code { get; set; }
        public Candidat? Candidate { get; set; }
        [NotMapped] public string Password { get; set; }
        [NotMapped] public IList<string> Roles { get; set; }
    }

}
