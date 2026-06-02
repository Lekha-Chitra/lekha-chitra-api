using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Helpers.JwtHelper
{
    public interface IJwtService
    {
        Task<string> GenerateNewJsonWebToken(List<Claim> claims);
    }
}
