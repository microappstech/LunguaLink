using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class ContentGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Groups Group { get; set; }
        public int GroupId { get; set; }
        public Ressource Ressource { get; set; }
        public int RessourceId { get; set; }
        public DateOnly HideOn { get; set; }= DateOnly.MaxValue;

        public DateTime? PublishedAt { get; set; } = DateTime.Now;
    }
}
