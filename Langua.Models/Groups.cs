using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class Groups
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        #nullable enable
        public List<Candidat> Candidats { get; set; }
        #nullable disable
        public List<MessageGroup> GroupeMessages { get; set; }
        [NotMapped] public int NbCandidate { get; set; }
    }
}
