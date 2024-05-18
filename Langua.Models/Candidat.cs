using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class Candidat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        //[Required]
        public string? Photo { get; set; }
        [Required]
        [Phone(ErrorMessage ="Please enter a valid Phone number")]
        [MinLength(10, ErrorMessage ="Phone number is not valid")]
        [MaxLength(14)]
        public string? Phone { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string? Email { get; set; }
        
        //[Required] 
        [NotMapped] 
        [DataType(DataType.Password, ErrorMessage ="Please enter a strong password")]
        [MinLength(8)]
        public string? Password { get; set; }
        public Subject? Subject { get; set; }
        public int SubjectId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsConnected { get; set; }
        public List<MessageGroup> MessageGroups { get; set; }
        public Groups? Group { get; set; }
        public int? GroupId { get; set; }
        public ApplicationUser User { get; set; }
        [NotMapped] public bool ConfirmedMail { get; set; }
        
    }
}
