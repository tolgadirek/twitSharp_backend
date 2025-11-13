using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken<T>(T user)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "1440");

            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("JWT:Key is missing in configuration.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 🧩 Reflection ile property’leri bul
            var userType = typeof(T);
            var idProp = userType.GetProperty("UserId") ?? userType.GetProperty("Id");
            var emailProp = userType.GetProperty("Email");
            var firstNameProp = userType.GetProperty("FirstName");
            var lastNameProp = userType.GetProperty("LastName");

            var id = idProp?.GetValue(user)?.ToString() ?? "0";
            var email = emailProp?.GetValue(user)?.ToString() ?? "";
            var name = $"{firstNameProp?.GetValue(user)} {lastNameProp?.GetValue(user)}".Trim();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
