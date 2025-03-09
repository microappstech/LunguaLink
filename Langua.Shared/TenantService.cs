using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Shared
{
    public interface ITenantService
    {
        public int GetTenant();
        public void SetTenant(int tenant);
    }
    public class TenantService:ITenantService
    {
        private int _tenantId;
        public void SetTenant(int tenant)
        {
            _tenantId = tenant;
        }
        public int GetTenant() => _tenantId;
    }
}
