using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Helpers.TenantService
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetTenantId =>
        Guid.Parse(
            _httpContextAccessor.HttpContext?.User
                .FindFirst("tenantId")?.Value
            ?? throw new UnauthorizedAccessException("Tenant claim not found."));
    }
}
