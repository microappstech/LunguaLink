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
        [NotMapped]
        public string? RessourceTypeStr { 
            get 
            {
                if (this.RessourceType == (int)Langua.Models.RessourceType.QCM)
                    return "QCM";
                if (this.RessourceType == (int)Langua.Models.RessourceType.File)
                    return "File";
                if (this.RessourceType == (int)Langua.Models.RessourceType.URL)
                    return "Url internet";
                if (this.RessourceType == (int)Langua.Models.RessourceType.VEDIO)
                    return "Url vedio";
                else
                    return null;
            }
        }
        public ICollection<ContentGroup> GroupRessources { get; set; } = null!;

        [NotMapped] public string GroupsName { get; set; } = null!;
    }
    
}
