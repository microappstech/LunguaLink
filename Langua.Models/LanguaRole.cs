using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class LanguaRole:IdentityRole,ITenantEntity
    {
        public int? TenantId { get; set; }
    }
}
