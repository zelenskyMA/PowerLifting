using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportAssistant.Domain.Models.UserData;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SportAssistant.Application.UserData.Auth
{
    public class JwtManager
    {
        public string CreateToken(IConfiguration configuration, UserModel user)
            => CreateTokenData(configuration, user, 30);

        public string CreateRefreshToken(IConfiguration configuration, UserModel user)
            => CreateTokenData(configuration, user, 120);

        private string CreateTokenData(IConfiguration configuration, UserModel user, int minutesOfLife)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: configuration.GetSection("JWT:Issuer").Value,
                audience: configuration.GetSection("JWT:Audience").Value,
                claims: UserProvider.CreateClaims(user),
                expires: DateTime.Now.AddMinutes(minutesOfLife),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }
    }
}
