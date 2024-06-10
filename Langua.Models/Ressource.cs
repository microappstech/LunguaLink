using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class Ressource
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Teacher Teacher { get; set; }
        public int? TeacherId { get; set; }
        public byte[]? ContentBytes { get; set; }
        public string? ContentFile { get; set; }
        public int RessourceType { get; set; }
        public string? Url { get; set; }
    }
    
}
