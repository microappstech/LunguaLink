using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class Groups
    { 
        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Photo { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        #nullable enable
        public ICollection<Candidat> Candidats { get; set; }
        #nullable disable
        public ICollection<MessageGroup> GroupeMessages { get; set; }
        //public int SubjectId { get; set; }
        //public Subject Subject { get; set; }
        [NotMapped] public int NbCandidate { get; set; }
    }
}
