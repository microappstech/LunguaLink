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
        public string FullName { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Photo { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        
        [Required] 
        [NotMapped] 
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsConnected { get; set; }
        public List<MessageGroup> MessageGroups { get; set; }
        public Groups? Group { get; set; }
        public int? GroupId { get; set; }
        
    }
}
