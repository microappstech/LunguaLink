using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Langua.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, ITenantEntity
    {
        public int? TenantId { get; set; }

        public string? FullName { get; set; }
        public string? Code { get; set; }
        public Candidat? Candidate { get; set; }
        public Teacher? Teacher { get; set; }
        public ICollection<MessageGroup> MessagesGroup { get; set; }
        [NotMapped] public string Password { get; set; }
        [NotMapped] public IList<string> Roles { get; set; }
    }

}
