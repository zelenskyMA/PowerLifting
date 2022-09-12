using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PowerLifting.Domain.Models.UserData;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PowerLifting.Application.UserData.Auth
{
    public static class JwtManager
    {
        public static string CreateToken(IConfiguration configuration, User user, UserInfo info = null)
        {
            var fullname = info == null ? "Unknown user" : string.Join(" ", new string?[] { info.Surname, info.FirstName, info.Patronimic });

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: configuration.GetSection("JWT:Issuer").Value,
                audience: configuration.GetSection("JWT:Audience").Value,
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()), //Id
                    new Claim(ClaimTypes.Email, user.Email), //Login
                    new Claim(ClaimTypes.Name, fullname), //Name
                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

    }
}
