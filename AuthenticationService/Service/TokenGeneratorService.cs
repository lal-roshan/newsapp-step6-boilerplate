using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Service
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public string GenerateToken(string userId)
        {
            var claims = new[]
            {
                new Claim("userId", userId)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("$eCuRiTeE_KeY_4_AuTh"));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "AuthAPI",
                audience: "AuthClient",
                expires: DateTime.UtcNow.AddMinutes(10),
                claims: claims,
                signingCredentials: signature
            );

            var tokenResponse = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return JsonConvert.SerializeObject(tokenResponse);
        }
    }
}
