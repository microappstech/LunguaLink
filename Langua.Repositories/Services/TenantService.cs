using Langua.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public class TenantService:ITenantService
    {
        private int _tenantId ;
        public void SetTenant(int tenant)
        {
            _tenantId = tenant;
        }
        public int GetTenant() => _tenantId;
    }
}
