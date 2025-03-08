using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Interfaces
{
    public interface ITenantService
    {
        public int GetTenant();
        public void SetTenant(int tenant);
    }
}
