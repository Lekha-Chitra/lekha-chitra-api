using LekhaChitra.Domain.Interface.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Domain.Entities.Application.User
{
    public class ApplicationUser : IdentityUser, ITenantEntity
    {
        public Guid TenantId { get; set; }
    }
}
