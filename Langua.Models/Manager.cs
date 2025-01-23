using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Langua.Models
{
    public class Manager
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        public string? Photo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        [NotMapped] public string? Password { get; set; }
        [NotMapped] public string? ConfirmPassword { get; set; }
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
