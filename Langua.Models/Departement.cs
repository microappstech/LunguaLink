using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public string Location { get; set; }
        //public string ManagerName { get; set; }
        public Manager Manager { get; set; }
        public int ManagerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Candidat> Candidates { get; set; }
    }
}
