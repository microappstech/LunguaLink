using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class Session:ITenantEntity
    {
        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string? ETag
        {
            get;
            set;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string? Name { get; set; }
        public Groups? Group { get; set; }
        public int GroupId { get; set; }
        public Teacher? Teacher { get; set; }
        public int TeacherId { get; set; }
        public DateTime Start {  get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now.AddHours(1);
    }
}
