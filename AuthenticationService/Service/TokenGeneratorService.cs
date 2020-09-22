using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Service
{
    /// <summary>
    /// Class for generating JWT token
    /// </summary>
    public class TokenGeneratorService : ITokenGeneratorService
    {
        /// <summary>
        /// Method for generating token
        /// </summary>
        /// <param name="userId">The user for whom the token is to be generated</param>
        /// <returns>The generated token string</returns>
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
