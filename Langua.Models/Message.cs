using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public abstract class BaseMessage: ITenantEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? TenantId { get; set; }
 
        [Required] public string? SenderId { get; set; }
        [Required] public string Content { get; set; }
        public DateTime SendAt { get; set; }

    }
}
