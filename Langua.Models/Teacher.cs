using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class Teacher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        //[DataType(DataType.EmailAddress)] 
        public string? Email { get; set; }
        //[DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        [NotMapped]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        // [DataType(DataType.Password)] 
        public string? ConfirmPassword { get; set; }
        public string? Photo { get;set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser User { get; set; }
    }
}
