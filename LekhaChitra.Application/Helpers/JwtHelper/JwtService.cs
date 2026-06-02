using LekhaChitra.Application.Exceptions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Helpers.JwtHelper
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
          );

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    authSecret,
                    SecurityAlgorithms.HmacSha256
                )
            );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            if (string.IsNullOrWhiteSpace(token))
            {
              throw new JwtGenerationFailedException();
            }
            return token;
        }
    }
}
