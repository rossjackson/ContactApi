using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace ContactApi.Web.Common.Security
{
    public class JwtService
    {
        private readonly string _secretKey = ConfigurationManager.AppSettings["jwtSecretKey"];
        private readonly string _domainConsumer;

        public JwtService(string domainConsumer)
        {
            _domainConsumer = domainConsumer;
        }

        public string GenerateToken(int expiresInMinutes)
        {
            var dateTimeNow = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);

            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsIdentity = GenerateClaims();

            var signingCredentials =
                new SigningCredentials(GenerateSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature);

            var token = tokenHandler.CreateJwtSecurityToken(issuer: _domainConsumer,
                audience: _domainConsumer, subject: claimsIdentity, notBefore: dateTimeNow, expires: expires,
                signingCredentials: signingCredentials);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity GenerateClaims()
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Ross Jackson"),
                new Claim(ClaimTypes.Role, "SuperAdmin")
            });
        }

        private SymmetricSecurityKey GenerateSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.Default.GetBytes(_secretKey));
        }

        public ClaimsPrincipal GetClaimsFromToken(string token, out SecurityToken validatedToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = _domainConsumer,
                ValidIssuer = _domainConsumer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = LifetimeValidator,
                IssuerSigningKey = GenerateSymmetricSecurityKey()
            };

            return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }


        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}
