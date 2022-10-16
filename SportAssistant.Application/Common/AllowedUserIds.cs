using SportAssistant.Application.UserData.Auth.Interfaces;
using SportAssistant.Domain.Interfaces.Common;

namespace SportAssistant.Application.Common
{
    public class AllowedUserIds : IAllowedUserIds
    {
        private readonly IUserProvider _user;

        public AllowedUserIds(IUserProvider user)
        {
            _user = user;
        }

        public int?[] MyAndCommon => new int?[] { null, 0, _user.Id };

        public int?[] MyOnly => new int?[] { _user.Id };

        public int?[] CommonOnly => new int?[] { null, 0};

    }
}