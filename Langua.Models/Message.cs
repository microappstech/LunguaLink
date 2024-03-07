using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public abstract class BaseMessage
    {
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }

    }
}
