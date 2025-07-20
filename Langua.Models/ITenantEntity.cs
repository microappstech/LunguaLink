using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    internal interface ITenantEntity
    {
        int? TenantId { get; set; }
    }
}
