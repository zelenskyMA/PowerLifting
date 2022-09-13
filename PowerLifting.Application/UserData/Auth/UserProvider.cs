using Microsoft.AspNetCore.Http;
using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.CustomExceptions;
using PowerLifting.Domain.Models.UserData;
using System.Security.Claims;

namespace PowerLifting.Application.UserData.Auth
{
    /// <inheritdoc />
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public int Id
        {
            get
            {
                var userIdString = _context.HttpContext?.User?.Claims?.FirstOrDefault(i => i.Type == ClaimTypes.Sid)?.Value;
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    throw new UnauthorizedException();
                }

                return userId;
            }
        }

        /// <inheritdoc />
        public string Email
        {
            get
            {
                var email = _context.HttpContext?.User?.Claims?.FirstOrDefault(i => i.Type == ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    throw new UnauthorizedException();
                }

                return email;
            }
        }

        public static List<Claim> CreateClaims(UserModel user)
        {
            return new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()), //Id
                    new Claim(ClaimTypes.Email, user.Email), //Login
                };
        }
    }
}