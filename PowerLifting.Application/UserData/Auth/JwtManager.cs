using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PowerLifting.Domain.Models.UserData;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PowerLifting.Application.UserData.Auth
{
    public static class JwtManager
    {
        public static string CreateToken(IConfiguration configuration, UserModel user)
            => CreateTokenData(configuration, user, 30);

        public static string CreateRefreshToken(IConfiguration configuration, UserModel user)
            => CreateTokenData(configuration, user, 120);

        private static string CreateTokenData(IConfiguration configuration, UserModel user, int minutesOfLife)
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
