using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class MessageGroup : BaseMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public byte[]? ContentMessage { get; set; }
    }
}
